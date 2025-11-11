using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces.Meet;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;
using static FATWA_GENERAL.Helper.Permissions;
using Microsoft.Extensions.DependencyInjection;
using static FATWA_DOMAIN.Enums.TaskEnums;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.TaskModels;
using System.Threading.Tasks;
using FATWA_DOMAIN.Interfaces.PatternNumber;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Interfaces.Communication;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_DOMAIN.Models.OrganizingCommittee;
using PdfSharp;
using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_INFRASTRUCTURE.Repository.MeetRepo
{
    public class MeetingRepository : IMeeting
    {
        private readonly DatabaseContext _dbContext;
        private List<MeetingVM> _MeetingVM;
        private Meeting meetingDetail = new Meeting();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CommunicationRepository _communicationRepo;
        private readonly ICMSCOMSInboxOutboxRequestPatternNumber _cMSCOMSInboxOutboxRequestPatternNumber;
        private readonly TempFileUploadRepository _tempFileUploadRepository;
        private readonly ICMSCaseRequest _iCMSCaseRequest;
        private readonly IRole _IRole;
        private readonly DmsDbContext _dmsDbContext;


        public List<MomAttendeeDecisionVM> momAttendeeDecisionVMs { get; set; } = new List<MomAttendeeDecisionVM>();
        private readonly SaveMeetingVM meetingVM = new SaveMeetingVM()
        {
            Meeting = new Meeting(),
            GeAttendee = new List<MeetingAttendeeVM>(),
            DeletedGeAttendeeIds = new List<Guid>(),
            LegislationAttendee = new List<FatwaAttendeeVM>(),
            DeletedLegislationAttendeeIds = new List<Guid>(),
            MeetingMom = new MeetingMom()
        };

        SaveMomVM meetingMomVM = new SaveMomVM()
        {
            Meeting = new Meeting(),
            GeAttendee = new List<MeetingAttendeeVM>(),
            LegislationAttendee = new List<FatwaAttendeeVM>()
        };
        private readonly
        MeetingCommunicationVM meetingCommunication = new MeetingCommunicationVM();

        public MeetingRepository(ICMSCaseRequest iCmsCaseRequest, DatabaseContext dbContext, CommunicationRepository communicationRepo, IServiceScopeFactory serviceScopeFactory, ICMSCOMSInboxOutboxRequestPatternNumber cMSCOMSInboxOutboxRequestPatternNumber, IRole iRole, TempFileUploadRepository tempFileUploadRepository, DmsDbContext dmsDbContext)
        {
            _iCMSCaseRequest = iCmsCaseRequest;
            _dbContext = dbContext;
            _communicationRepo = communicationRepo;
            _serviceScopeFactory = serviceScopeFactory;
            _cMSCOMSInboxOutboxRequestPatternNumber = cMSCOMSInboxOutboxRequestPatternNumber;
            _IRole = iRole;
            _tempFileUploadRepository = tempFileUploadRepository;
            _dmsDbContext = dmsDbContext;
        }
        public class NotificationParameter
        {
            public string? EntityName { get; set; }
            public string? FileNumber { get; set; }
        }
        public NotificationParameter notifPara { get; set; } = new NotificationParameter();
        #region GET

        public async Task<List<MeetingVM>> GetMeetingsList(string userName, int PageSize, int PageNumber)
        {
            try
            {
                string storedProc = $"exec pmeetinglist @UserName = N'{userName}',@PageNumber ='{PageNumber}',@PageSize ='{PageSize}'";
                var result = await _dbContext.MeetingVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task<List<MeetingVM>> GetMeetingsForMobileApp(string userName, int channelId)
        {
            try
            {
                string storedProc = $"exec pMeetingListMobileApp @UserName = N'{userName}'";
                var result = await _dbContext.MeetingVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SaveMeetingVM> GetMeetingById(Guid meetingId)
        {
            try
            {
                Meeting meeting = await _dbContext.Meetings.FirstOrDefaultAsync(x => x.MeetingId == meetingId);
                if (meeting != null)
                {
                    meetingVM.Meeting = meeting;
                }

                List<MeetingAttendeeVM> legisAattendees = await GetMeetingAttendeeByMeetingId(meetingId, (int)MeetingAttendeeTypeEnum.LegislationAttendee);
                if (legisAattendees != null)
                {
                    int atttendeeLegislationSerialNo = 1;
                    foreach (var attendee in legisAattendees)
                    {
                        attendee.AttendeeUserId = attendee.AttendeeUserId ?? string.Empty;
                        FatwaAttendeeVM newAttendee = new FatwaAttendeeVM()
                        {
                            Id = attendee.AttendeeUserId,
                            FirstNameEnglish = attendee.RepresentativeNameEn,
                            FirstNameArabic = attendee.RepresentativeNameAr,
                            DepartmentId = attendee.DepartmentId,
                            SerialNo = atttendeeLegislationSerialNo++,
                            AttendeeStatusId = attendee.AttendeeStatusId,
                            AttendeeStatusEn = attendee.AttendeeStatusEn,
                            AttendeeStatusAr = attendee.AttendeeStatusAr,
                        };
                        meetingVM.LegislationAttendee.Add(newAttendee);
                    }
                }
                List<CommunicationAttendee> geattendees = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == meeting.CommunicationId).ToListAsync();
                if (geattendees != null)
                {
                    int atttendeeGeSerialNo = 1;
                    foreach (var attendee in geattendees)
                    {
                        attendee.AttendeeUserId = attendee.AttendeeUserId ?? string.Empty;
                        MeetingAttendeeVM newAttendee = new MeetingAttendeeVM()
                        {
                            AttendeeUserId = attendee.AttendeeUserId,
                            RepresentativeNameEn = attendee.RepresentativeNameEn,
                            RepresentativeNameAr = attendee.RepresentativeNameAr,
                            DepartmentId = attendee.DepartmentId,
                            SerialNo = atttendeeGeSerialNo++,
                            //AttendeeStatusId = attendee.AttendeeStatusId,
                            //AttendeeStatusEn = attendee.AttendeeStatusEn,
                            //AttendeeStatusAr = attendee.AttendeeStatusAr,
                        };

                        var govt = _dbContext.GovernmentEntity.FirstOrDefault(x => x.EntityId == attendee.GovernmentEntityId);
                        if (govt is not null)
                        {
                            newAttendee.GovernmentEntityId = govt.EntityId;
                            newAttendee.GovernmentEntityNameEn = govt.Name_En;
                            newAttendee.GovernmentEntityNameAr = govt.Name_Ar;
                        }

                        var dept = _dbContext.GEDepartments.FirstOrDefault(x => x.Id == attendee.DepartmentId);
                        if (dept is not null)
                        {
                            newAttendee.DepartmentId = dept.Id;
                            newAttendee.DepartmentNameEn = dept.Name_En;
                            newAttendee.DepartmentNameAr = dept.Name_Ar;
                        }
                        meetingVM.GeAttendee.Add(newAttendee);
                    }
                    //meetingVM.GeAttendee = geAattendees;
                }
                // In This If statement if meeting initiated by fatwa ge attentee will be picked against meeting else from communication Id
                if (!meetingVM.Meeting.IsReplyForMeetingRequest)
                {
                    List<MeetingAttendeeVM> geAattendees = await GetMeetingAttendeeByMeetingId(meetingId, (int)MeetingAttendeeTypeEnum.GeAttendee);
                    if (geAattendees != null)
                    {
                        meetingVM.GeAttendee = geAattendees;
                    }
                }
                else
                {
                    int atttendeeGeSerialNo = 1;
                    var commMeetingAttendees = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == meetingVM.Meeting.CommunicationId).ToListAsync();
                    if (commMeetingAttendees.Count() != 0)
                    {
                        foreach (var attendee in commMeetingAttendees)
                        {
                            MeetingAttendeeVM newAttendee = new MeetingAttendeeVM();

                            var govt = await _dbContext.GovernmentEntity.FirstOrDefaultAsync(x => x.EntityId == attendee.GovernmentEntityId);
                            if (govt is not null)
                            {
                                newAttendee.GovernmentEntityId = govt.EntityId;
                                newAttendee.GovernmentEntityNameEn = govt.Name_En;
                                newAttendee.GovernmentEntityNameAr = govt.Name_Ar;
                            }

                            var dept = await _dbContext.GEDepartments.FirstOrDefaultAsync(x => x.Id == attendee.DepartmentId);
                            if (dept is not null)
                            {
                                newAttendee.DepartmentId = dept.Id;
                                newAttendee.DepartmentNameEn = dept.Name_En;
                                newAttendee.DepartmentNameAr = dept.Name_Ar;
                            }

                            newAttendee.RepresentativeNameEn = attendee.RepresentativeNameEn;
                            newAttendee.RepresentativeNameAr = attendee.RepresentativeNameAr;
                            newAttendee.RepresentativeId = attendee.RepresentativeId;
                            newAttendee.SerialNo = atttendeeGeSerialNo++;

                            meetingVM.GeAttendee.Add(newAttendee);
                        }
                    }


                }


                MeetingMom mom = await GetMeetingMOMByMeetingId(meetingId);
                if (mom != null)
                {
                    meetingVM.MeetingMom = mom;
                }

                return meetingVM;
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        public async Task<MeetingDecisionVM> GetMeetingDecisionDetailById(Guid meetingId, int sectorId)
        {
            try
            {
                string storedProc = $"exec pMeetingDecision @MeetingId = N'{meetingId}'";
                var result = await _dbContext.MeetingDecisionVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result != null)
                {
                    var onlyViceHosApproval = _dbContext.OperatingSectorType.Where(x => x.Id == sectorId).Select(y => y.IsOnlyViceHosApprovalRequired).FirstOrDefault();
                    var res = result.FirstOrDefault();
                    res.HosApprovalRequire = onlyViceHosApproval;

                    return res;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<List<MeetingAttendeeVM>> GetMeetingAttendeeByMeetingId(Guid meetingId, int meetingTypeId)
        {
            try
            {
                string storedProc = $"exec pMeetingAttendeeByMeetingId @MeetingId = N'{meetingId}', @MeetingAttendeeTypeId = '{meetingTypeId}'";
                return await _dbContext.MeetingAttendeeVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<MeetingMom> GetMeetingMOMByMeetingId(Guid meetingId)
        {
            try
            {
                return await _dbContext.MeetingMoms.FirstOrDefaultAsync(x => x.MeetingId == meetingId);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion

        #region Add/Update

        public async Task<MeetingCommunicationVM> AddMeeting(SaveMeetingVM meetingVM)
        {
            bool isSaved = true;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await SaveMeeting(meetingVM, _dbContext);

                        if (meetingVM.LegislationAttendee.Any())
                            isSaved = await SaveLegislationAttendee(meetingVM, _dbContext);

                        if (meetingVM.GeAttendee.Any())
                            isSaved = await SaveGeAttendee(meetingVM, _dbContext);

                        // update communication meeting status
                        if (meetingVM.CommunicationId != null && meetingVM.CommunicationId != Guid.Empty)
                        {
                            var comMeeting = _dbContext.CommunicationMeetings.Where(x => x.CommunicationId == meetingVM.CommunicationId).FirstOrDefault();
                            comMeeting.StatusId = meetingVM.Meeting.MeetingStatusId;
                            _dbContext.CommunicationMeetings.Update(comMeeting);


                            _dbContext.SaveChanges();
                        }
                        if (meetingVM.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meetingVM.Meeting.IsApproved == true && !meetingVM.isSaveAsDraft)
                        {
                            isSaved = await SaveCommunication(_dbContext, meetingVM.Meeting);
                        }
                        if (isSaved)
                        {
                            if (meetingVM.Meeting.ReferenceGuid != null)
                            {
                                var obj = CheckReferenceGuid((Guid)meetingVM.Meeting.ReferenceGuid);
                                meetingVM.NotificationParameter.FileNumber = obj.FileNumber;
                                meetingVM.NotificationParameter.Entity = obj.EntityName;
                                meetingVM.NotificationParameter.CreatedBy = meetingVM.Meeting.CreatedBy;
                                meetingVM.NotificationParameter.CreatedDate = meetingVM.Meeting.CreatedDate;
                            }
                            transaction.Commit();
                        }

                        if (meetingVM.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meetingVM.Meeting.IsApproved == true)
                        {
                            Meeting meeting = _dbContext.Meetings.FirstOrDefault(m => m.MeetingId == meetingVM.Meeting.MeetingId);
                            List<MeetingAttendee> meetingAttendees = _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).ToList();
                            meetingCommunication.Meeting = meeting;
                            meetingCommunication.MeetingAttendees = meetingAttendees;
                            return meetingCommunication;
                        }
                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                        throw;
                    }
                    return null;
                }
            }
        }
        public NotificationParameter CheckReferenceGuid(Guid fileId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var FileNumber = _DbContext.CaseFiles.Where(x => x.FileId == fileId).Select(y => y.FileNumber).FirstOrDefault();
                var entName = new CaseFile().GetType().Name;
                if (FileNumber == null)
                {
                    FileNumber = _DbContext.cmsRegisteredCases.Where(x => x.CaseId == fileId).Select(y => y.CaseNumber).FirstOrDefault();

                    entName = "Case";
                }
                if (FileNumber == null)
                {
                    FileNumber = _DbContext.ConsultationFiles.Where(x => x.FileId == fileId).Select(y => y.FileNumber).FirstOrDefault();
                    entName = new ConsultationFile().GetType().Name;

                }
                if (FileNumber == null)
                {
                    CommitteeDetailsVm committeeDetails = new CommitteeDetailsVm();
                    var storeProc = $"exec pOC_GetCommitteeNumber @CommitteeId='{fileId}'";
                    var result = _dbContext.CommitteeDetailsVms.FromSqlRaw(storeProc).ToList().FirstOrDefault();
                    if (result != null)
                    {
                        committeeDetails = result;
                        FileNumber = committeeDetails.CommitteeNumber;
                    }
                    entName = new Committee().GetType().Name;
                }
                notifPara.EntityName = entName;
                notifPara.FileNumber = FileNumber;

                return notifPara;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //public async Task<SaveMeetingVM> EditMeeting(SaveMeetingVM meetingVM)
        //{
        //    bool isSaved = false;
        //    using (_dbContext)
        //    {
        //        using (var transaction = _dbContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                isSaved = await SaveMeeting(meetingVM, true, _dbContext);

        //                if (meetingVM.LegislationAttendee.Any())
        //                    isSaved = await SaveLegislationAttendee(meetingVM, _dbContext);

        //                if (meetingVM.GeAttendee.Any())
        //                    isSaved = await SaveGeAttendee(meetingVM, _dbContext);

        //                if (isSaved)
        //                    transaction.Commit();

        //            }
        //            catch
        //            {
        //                isSaved = false;
        //                transaction.Rollback();
        //            }
        //            return isSaved;
        //        }
        //    }
        //}
        public async Task<MeetingCommunicationVM> EditsMeeting(SaveMeetingVM meetingVM)
        {
            var onlyViceHosApproval = _dbContext.OperatingSectorType.Where(x => x.Id == meetingVM.Meeting.SectorTypeId).Select(y => y.IsOnlyViceHosApprovalRequired).FirstOrDefault();
            Meeting meeting = await _dbContext.Meetings.Where(m => m.MeetingId == meetingVM.Meeting.MeetingId).FirstOrDefaultAsync();
            bool isSaved = true;

            if (onlyViceHosApproval || meetingVM.Meeting.IsApproved)
            {

                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if ((bool)meetingVM.IsCreateMeeting)
                            {
                                meetingVM.Meeting.IsApproved = true;
                            }
                            if (meetingVM.Meeting.IsApproved)
                            {
                                meetingVM.Meeting.ApprovalReqBy = Guid.Parse(meetingVM.LoggedInUser);
                            }
                            else
                            {
                                if ((bool)meetingVM.IsCreateMeeting)
                                {
                                    var hos = await _IRole.GetHOSBySectorId((int)meetingVM.Meeting.SectorTypeId);
                                    meetingVM.Meeting.ApprovalReqBy = Guid.Parse(hos.Id);
                                }
                            }
                            isSaved = await SaveMeeting(meetingVM, _dbContext);

                            if (meetingVM.LegislationAttendee.Any())
                                isSaved = await SaveLegislationAttendee(meetingVM, _dbContext);

                            if (meetingVM.GeAttendee.Any())
                                isSaved = await SaveGeAttendee(meetingVM, _dbContext);

                            // update communication meeting status
                            if (meetingVM.CommunicationId != null && meetingVM.CommunicationId != Guid.Empty)
                            {
                                var comMeeting = _dbContext.CommunicationMeetings.Where(x => x.CommunicationId == meetingVM.CommunicationId).FirstOrDefault();
                                comMeeting.StatusId = meetingVM.Meeting.MeetingStatusId;
                                _dbContext.CommunicationMeetings.Update(comMeeting);
                                _dbContext.SaveChanges();
                            }
                            if (meetingVM.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meetingVM.Meeting.IsApproved == true)
                            {

                                isSaved = await SaveCommunication(_dbContext, meetingVM.Meeting);

                            }
                            if (meetingVM.Meeting.IsApproved)
                            {
                                await _tempFileUploadRepository.CopyAttachmentsFromSourceToDestination(new List<CopyAttachmentVM>
                                 {
                                     new CopyAttachmentVM()
                                    {
                                         SourceId = meeting.MeetingId,
                                        DestinationId = (Guid)meeting.ReferenceGuid,
                                        CreatedBy = meetingVM.Meeting.CreatedBy
                                    }
                                 });
                            }
                            if (isSaved)
                            {
                                if (meetingVM.Meeting.ReferenceGuid != null)
                                {
                                    var obj = CheckReferenceGuid((Guid)meetingVM.Meeting.ReferenceGuid);
                                    meetingVM.NotificationParameter.FileNumber = obj.FileNumber;
                                    meetingVM.NotificationParameter.Entity = obj.EntityName;
                                    meetingVM.NotificationParameter.CreatedBy = meetingVM.Meeting.CreatedBy;
                                    meetingVM.NotificationParameter.CreatedDate = meetingVM.Meeting.CreatedDate;
                                }
                            }
                            transaction.Commit();

                            if (meetingVM.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meetingVM.Meeting.IsApproved == true)
                            {

                                using var scope = _serviceScopeFactory.CreateScope();
                                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                                Meeting meetingSend = _DbContext.Meetings.FirstOrDefault(m => m.MeetingId == meetingVM.Meeting.MeetingId);
                                List<MeetingAttendee> meetingAttendees = _DbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).ToList();
                                meetingCommunication.Meeting = meetingSend;
                                meetingCommunication.MeetingAttendees = meetingAttendees;
                                return meetingCommunication;
                            }
                        }

                        catch
                        {
                            isSaved = false;
                            transaction.Rollback();
                            throw;
                        }
                        return null;
                    }
                }
            }
            else
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        if ((bool)meetingVM.IsCreateMeeting)
                        {
                            var hos = await _IRole.GetHOSBySectorId((int)meetingVM.Meeting.SectorTypeId);
                            meetingVM.Meeting.ApprovalReqBy = Guid.Parse(hos.Id);
                        }

                        isSaved = await SaveMeeting(meetingVM, _dbContext);

                        if (meetingVM.LegislationAttendee.Any())
                            isSaved = await SaveLegislationAttendee(meetingVM, _dbContext);

                        if (meetingVM.GeAttendee.Any())
                            isSaved = await SaveGeAttendee(meetingVM, _dbContext);
                        if (isSaved)
                        {
                            if (meetingVM.Meeting.ReferenceGuid != null)
                            {
                                var obj = CheckReferenceGuid((Guid)meetingVM.Meeting.ReferenceGuid);
                                meetingVM.NotificationParameter.FileNumber = obj.FileNumber;
                                meetingVM.NotificationParameter.Entity = obj.EntityName;
                                meetingVM.NotificationParameter.CreatedBy = meetingVM.Meeting.CreatedBy;
                                meetingVM.NotificationParameter.CreatedDate = meetingVM.Meeting.CreatedDate;
                            }
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }

                    }
                }

                MeetingCommunicationVM meetings = new MeetingCommunicationVM()
                {
                    Meeting = new Meeting()

                };
                meetings.Meeting = meeting;
                meetings.Meeting.MeetingStatusId = meetingVM.Meeting.MeetingStatusId;
                if ((bool)meetingVM.IsCreateMeeting)
                {
                    meetings.Meeting.ApprovalReqBy = meetingVM.Meeting.ApprovalReqBy;

                    meetings.Meeting.IsSendToHOS = true;

                }

                return meetings;

            }
        }




        public async Task<MeetingCommunicationVM> UpdateMeetingDecision(MeetingDecisionVM meetingVM)
        {
            bool isSaved = true;

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var onlyViceHosApproval = _dbContext.OperatingSectorType.Where(x => x.Id == meetingVM.SectorTypeId).Select(y => y.IsOnlyViceHosApprovalRequired).FirstOrDefault();
                        Meeting meeting = await _dbContext.Meetings.Where(m => m.MeetingId == meetingVM.MeetingId).FirstOrDefaultAsync();
                        List<MeetingAttendee> meetingAttendees = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.MeetingId).ToListAsync();
                        if (onlyViceHosApproval == true || meetingVM.HOSUser == true)
                        {
                            if (meeting is not null)
                            {
                                meeting.MeetingStatusId = meetingVM.MeetingStatusId;
                                meeting.IsApproved = meetingVM.IsApproved;
                                if (!meetingVM.IsApproved)
                                {
                                    meeting.IsSendToHOS = false;
                                }
                                else
                                {
                                    meeting.ApprovalReqBy = meetingVM.LoggedInUser;
                                }
                                meeting.Comment = meetingVM.Comment;
                                meeting.ModifiedBy = meetingVM.ModifiedBy;
                                meeting.ModifiedDate = DateTime.Now;
                                _dbContext.Entry(meeting).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                                if (meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meeting.IsApproved == true)
                                {
                                    meeting.ReceivedBy = meetingVM.ReceivedBy;
                                    meeting.SentBy = meeting.CreatedBy;
                                    meeting.SectorTypeId = meetingVM.SectorTypeId;
                                    meeting.GovtEntityId = meetingVM.GovtEntityId;
                                    isSaved = await SaveCommunication(_dbContext, meeting);
                                    meetingCommunication.Meeting = meeting;
                                    meetingCommunication.MeetingAttendees = meetingAttendees;
                                }
                                if (meeting.IsApproved)
                                {
                                    await _tempFileUploadRepository.CopyAttachmentsFromSourceToDestination(new List<CopyAttachmentVM>
                                      {
                                          new CopyAttachmentVM()
                                         {
                                              SourceId = meeting.MeetingId,
                                             DestinationId = (Guid)meeting.ReferenceGuid,
                                             CreatedBy = meeting.CreatedBy
                                         }
                                      });
                                }
                                if (isSaved)
                                {
                                    if (meetingVM.ReferenceGuid != Guid.Empty)
                                    {
                                        var obj = CheckReferenceGuid((Guid)meetingVM.ReferenceGuid);
                                        meetingVM.NotificationParameter.FileNumber = obj.FileNumber;
                                        meetingVM.NotificationParameter.Entity = obj.EntityName;
                                    }
                                    transaction.Commit();
                                    return meetingCommunication;
                                }
                            }
                        }
                        else
                        {
                            //if(meeting.MeetingStatusId != (int)MeetingStatusEnum.ApprovedByViceHos || meeting.MeetingStatusId != (int)MeetingStatusEnum.RejectedByViceHos)
                            //meeting.MeetingStatusId = (int)MeetingStatusEnum.OnHold;
                            var hos = await _IRole.GetHOSBySectorId((int)meetingVM.SectorTypeId);
                            meeting.ApprovalReqBy = Guid.Parse(hos.Id);
                            meeting.IsApproved = false;
                            meeting.Comment = meetingVM.Comment;
                            meeting.ModifiedBy = meetingVM.ModifiedBy;
                            meeting.ModifiedDate = DateTime.Now;
                            meeting.MeetingStatusId = meetingVM.MeetingStatusId;
                            _dbContext.Entry(meeting).State = EntityState.Modified;
                            meetingCommunication.Meeting = meeting;
                            await _dbContext.SaveChangesAsync();
                            if (meetingVM.ReferenceGuid != Guid.Empty)
                            {
                                var obj = CheckReferenceGuid((Guid)meetingVM.ReferenceGuid);
                                meetingVM.NotificationParameter.FileNumber = obj.FileNumber;
                                meetingVM.NotificationParameter.Entity = obj.EntityName;
                            }
                            transaction.Commit();
                            return meetingCommunication;
                        }


                    }
                    catch (Exception ex)
                    {
                        isSaved = false;
                        transaction.Rollback();
                        return null;


                    }
                }
            }
            return null;
        }

        #endregion

        #region Save

        public async Task<bool> SaveMeeting(SaveMeetingVM meetingVM, DatabaseContext dbContext)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            bool isSaved;
            try
            {
                var resultMeeting = await _DbContext.Meetings.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).AsNoTracking().FirstOrDefaultAsync();
                if (resultMeeting != null)
                {
                    resultMeeting = meetingVM.Meeting;
                    _DbContext.Update(resultMeeting);
                }
                else
                {
                    await _DbContext.Meetings.AddAsync(meetingVM.Meeting);
                }
                await _DbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        #region Attendees

        public async Task<bool> SaveLegislationAttendee(SaveMeetingVM meetingVM, DatabaseContext _dbContext)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            bool isSaved;
            List<MeetingAttendee> attendeesList = new List<MeetingAttendee>();
            try
            {
                foreach (var attendee in meetingVM.LegislationAttendee)
                {
                    var attendeeExist = await _DbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId && x.AttendeeUserId == attendee.Id).FirstOrDefaultAsync();
                    if (attendeeExist is null)
                    {
                        int Status = 0;

                        if (attendee.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New && meetingVM.Meeting.MeetingStatusId != (int)MeetingStatusEnum.SaveAsDraft)
                        {
                            Status = (int)MeetingAttendeeStatusEnum.Pending;
                        }
                        else
                        {
                            Status = (int)attendee.AttendeeStatusId;

                        }
                        MeetingAttendee newAttendee = new MeetingAttendee()
                        {
                            MeetingAttendeeId = Guid.NewGuid(),
                            AttendeeUserId = attendee.Id,
                            RepresentativeNameEn = attendee.FirstNameEnglish,
                            RepresentativeNameAr = attendee.FirstNameArabic,
                            MeetingId = meetingVM.Meeting.MeetingId,
                            MeetingAttendeeTypeId = (int)MeetingAttendeeTypeEnum.LegislationAttendee,
                            AttendeeStatusId = Status,
                            IsPresent = false,
                            DepartmentId = attendee.DepartmentId,
                            CreatedBy = meetingVM.Meeting.CreatedBy,
                            CreatedDate = meetingVM.Meeting.CreatedDate,
                            IsDeleted = meetingVM.Meeting.IsDeleted,
                        };
                        attendeesList.Add(newAttendee);
                    }
                }
                await _DbContext.MeetingAttendees.AddRangeAsync(attendeesList);

                //Remove Attendees 
                var deleteAttendees = meetingVM.DeletedLegislationAttendeeIds;
                await DeleteAttendees(deleteAttendees, meetingVM, _dbContext);

                await _DbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> SaveGeAttendee(SaveMeetingVM meetingVM, DatabaseContext _dbContext)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            bool isSaved = true;
            List<MeetingAttendee> attendeesList = new List<MeetingAttendee>();
            try
            {
                foreach (var attendee in meetingVM.GeAttendee)
                {
                    var attendeeExist = _DbContext.MeetingAttendees.FirstOrDefault(x => x.MeetingId == meetingVM.Meeting.MeetingId && x.MeetingAttendeeId == attendee.MeetingAttendeeId);
                    if (attendeeExist is null)
                    {
                        MeetingAttendee newAttendee = new MeetingAttendee()
                        {
                            MeetingAttendeeId = Guid.NewGuid(),
                            GovernmentEntityId = attendee.GovernmentEntityId,
                            DepartmentId = attendee.DepartmentId,
                            RepresentativeNameEn = attendee.RepresentativeNameEn,
                            RepresentativeNameAr = attendee.RepresentativeNameAr,
                            RepresentativeId = attendee.RepresentativeId,
                            MeetingId = meetingVM.Meeting.MeetingId,
                            MeetingAttendeeTypeId = (int)MeetingAttendeeTypeEnum.GeAttendee,
                            IsPresent = false,

                            CreatedBy = meetingVM.Meeting.CreatedBy,
                            CreatedDate = meetingVM.Meeting.CreatedDate,
                            IsDeleted = meetingVM.Meeting.IsDeleted
                        };
                        attendeesList.Add(newAttendee);
                    }
                }
                _DbContext.MeetingAttendees.AddRange(attendeesList);

                //Remove Attendees 
                var deleteAttendees = meetingVM.DeletedGeAttendeeIds;
                await DeleteAttendees(deleteAttendees, meetingVM, _dbContext);

                await _DbContext.SaveChangesAsync();
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> DeleteAttendees(List<Guid> deleteAttendees, SaveMeetingVM meet, DatabaseContext _dbContext)
        {
            bool isSaved = true;
            try
            {
                if (deleteAttendees.Any())
                {
                    foreach (var item in deleteAttendees)
                    {
                        var deleteAttendee = await _dbContext.MeetingAttendees.FirstOrDefaultAsync(x => x.AttendeeUserId == item.ToString() && x.MeetingId == meet.Meeting.MeetingId);
                        if (deleteAttendee != null && meetingVM.Meeting.MeetingStatusId != (int)MeetingStatusEnum.SaveAsDraft)
                        {
                            deleteAttendee.IsDeleted = true;
                            deleteAttendee.DeletedDate = DateTime.Now;
                            deleteAttendee.DeletedBy = meetingVM.Meeting.CreatedBy;
                            _dbContext.MeetingAttendees.Update(deleteAttendee);
                            await _dbContext.SaveChangesAsync();

                        }
                    }
                    // also if that user have a pending request meeting task then mark it as a completed.
                    await UpdateDeletedAttendeeTaskAsCompleted(meet.Meeting, deleteAttendees, _dbContext);
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;

        }

        private async Task UpdateDeletedAttendeeTaskAsCompleted(Meeting meeting, List<Guid> deleteAttendees, DatabaseContext dbContext)
        {
            try
            {

                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                foreach (var item in deleteAttendees)
                {
                    var resultTask = await _DbContext.Tasks.Where(x => x.ReferenceId == meeting.MeetingId && x.AssignedTo == item.ToString()).FirstOrDefaultAsync();
                    if (resultTask != null)
                    {
                        resultTask.TaskStatusId = (int)TaskStatusEnum.Rejected;

                        if (!string.IsNullOrEmpty(resultTask.Url))
                        {
                            resultTask.Url = resultTask.Url.StartsWith("meeting-add") ? $"meeting-view/{meeting.MeetingId}/true" : resultTask.Url;
                        }
                        if (meeting.ModifiedBy != null)
                        {
                            resultTask.DeletedBy = meeting.ModifiedBy;
                        }
                        else
                        {
                            resultTask.DeletedBy = meeting.CreatedBy;
                        }
                        resultTask.DeletedDate = DateTime.Now;
                        resultTask.IsDeleted = true;

                        _DbContext.Tasks.Update(resultTask);
                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region MOM

        public async Task<bool> SaveMom(SaveMomVM meetingMom)
        {
            bool isSaved = true;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await SaveMOM(meetingMom.MeetingMom, false, _dbContext);
                        if (meetingMom.LegislationAttendee.Any())
                            isSaved = await MarkAttendanceFatwaAttendee(meetingMom, _dbContext);
                        //if (meetingVM.GeAttendee.Any())
                        isSaved = await MarkAttendanceGeAttendeeMOM(meetingMom, _dbContext);
                        isSaved = await SaveMomDecision(meetingMom, _dbContext);
                        var result = await GetOnlyGEAttendeeDetailInMOMDecisionTable(meetingMom, _dbContext);
                        if (result.Count() != 0)
                        {
                            meetingMom.MomAttendeeDecisionDetails = result;
                        }
                        else
                        {
                            meetingMom.MomAttendeeDecisionDetails = new List<MomAttendeeDecision>();
                        }
                        if (isSaved)
                        {
                            var entPara = CheckReferenceGuid((Guid)meetingMom.Meeting.ReferenceGuid);
                            meetingMom.NotificationParameter.FileNumber = entPara.FileNumber;
                            meetingMom.NotificationParameter.Entity = entPara.EntityName;
                            transaction.Commit();
                        }
                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                }
            }
            return (isSaved);
        }

        private async Task<bool> SaveMOM(MeetingMom meetingMom, bool isEdit, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                if (!isEdit)
                    await _dbContext.MeetingMoms.AddAsync(meetingMom);
                else
                    _dbContext.Entry(meetingMom).State = EntityState.Modified;
                // update meeting status
                //var meeting = _dbContext.Meetings.Where(x => x.MeetingId == meetingMom.MeetingId).FirstOrDefault();
                //if (meetingMom.MOMStatusId != (int)MeetingStatusEnum.SaveAsDraft)
                //{
                //    meeting.MeetingStatusId = (int)MeetingStatusEnum.Held;
                //    _dbContext.Meetings.Update(meeting);
                //}
                await _dbContext.SaveChangesAsync();
                // Get only fatwa attendee list to send them a task
                meetingMom.MeetingAttendees = await GetMeetingAttendeeByMeetingId(meetingMom.MeetingId, (int)MeetingAttendeeTypeEnum.LegislationAttendee);
                isSaved = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return (isSaved);
        }
        public async Task<bool> MarkAttendanceFatwaAttendee(SaveMomVM meetingVM, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                // get present fatwa attendees list from table and update only Present attendees record accordingly.
                //foreach (var attendee in meetingVM.LegislationAttendee)
                //{
                //    var attendeeExist = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId && x.AttendeeUserId == attendee.Id && x.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Accept).FirstOrDefaultAsync();
                //    if (attendeeExist != null)
                //    {
                //        attendeeExist.IsPresent = true;
                //        _dbContext.Update(attendeeExist);
                //    }
                //}
                var attendeeExist = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).ToListAsync();
                if (attendeeExist.Count() != 0)
                {
                    foreach (var attendee in meetingVM.LegislationAttendee)
                    {
                        foreach (var item in attendeeExist)
                        {
                            if (item.AttendeeUserId == attendee.Id && item.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Accept)
                            {
                                item.IsPresent = true;
                                _dbContext.Update(item);
                            }
                            else if (item.AttendeeUserId == null && item.RepresentativeId == null && item.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New)
                            {
                                item.IsPresent = true;
                                _dbContext.Update(item);
                            }
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        public async Task<bool> MarkAttendanceGeAttendeeMOM(SaveMomVM meetingVM, DatabaseContext _dbContext)
        {
            bool isSaved = true;
            try
            {
                // get present GE attendees list from UI and update only Present attendees record accordingly.
                //foreach (var attendee in meetingVM.GeAttendee)
                //{
                //    var attendeeExist = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == meetingVM.Meeting.CommunicationId && x.AttendeeUserId == attendee.AttendeeUserId).FirstOrDefaultAsync();
                //    if (attendeeExist != null)
                //    {
                //        attendeeExist.IsPresent = true;
                //        _dbContext.Update(attendeeExist);
                //    }
                //}
                var attendeeExist = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == meetingVM.Meeting.CommunicationId).ToListAsync();
                if (attendeeExist.Count() != 0)
                {
                    foreach (var attendee in meetingVM.GeAttendee)
                    {
                        foreach (var item in attendeeExist)
                        {
                            if (item.RepresentativeNameEn == attendee.RepresentativeNameEn)
                            {
                                item.IsPresent = true;
                                _dbContext.Update(item);
                            }
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        private async Task<List<MomAttendeeDecision>> GetOnlyGEAttendeeDetailInMOMDecisionTable(SaveMomVM meetingMom, DatabaseContext dbContext)
        {
            try
            {
                List<MomAttendeeDecision> MOMGEAttendeeDecisions = new List<MomAttendeeDecision>();
                var resultDetails = await dbContext.MomAttendeeDecisions.Where(x => x.MeetingMomId == meetingMom.MeetingMom.MeetingMomId && x.MeetingId == meetingMom.MeetingMom.MeetingId).ToListAsync();
                if (resultDetails.Count() != 0)
                {
                    foreach (var item in resultDetails)
                    {
                        var checkMeetingAttendee = await dbContext.MeetingAttendees.Where(x => x.MeetingAttendeeId == item.MeetingAttendeeId && x.RepresentativeId == null && x.AttendeeUserId == null).FirstOrDefaultAsync();
                        if (checkMeetingAttendee != null)
                        {
                            MOMGEAttendeeDecisions.Add(item);
                        }
                        var checkCommunicationAttendee = await dbContext.CommunicationAttendees.Where(x => x.CommunicationAttendeeId == item.MeetingAttendeeId && x.RepresentativeId == null && x.AttendeeUserId == null).FirstOrDefaultAsync();
                        if (checkCommunicationAttendee != null)
                        {
                            MOMGEAttendeeDecisions.Add(item);
                        }
                    }
                }
                return MOMGEAttendeeDecisions;
            }
            catch (Exception)
            {
                return new List<MomAttendeeDecision>();
                throw new NotImplementedException();
            }

        }

        #endregion

        #endregion

        #region Save Legislation Attandee
        public async Task<bool> SaveLegislationAttandee(SaveMeetingVM meetingVM)
        {
            bool isSaved = false;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (meetingVM.LegislationAttandeeSelected.Count() != 0)
                        {
                            isSaved = await UpdateLegislationAttendee(meetingVM, _dbContext);
                        }
                        if (meetingVM.GEAttandeeSelected.Count() != 0)
                        {
                            isSaved = await UpdateGEAttendee(meetingVM, _dbContext);
                        }
                        //isSaved = await SaveFileFromTempToUploadDocumentTable(meetingVM, _dbContext);

                        if (isSaved)
                        {
                            transaction.Commit();
                        }
                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                    return isSaved;
                }
            }
        }

        private async Task<bool> UpdateLegislationAttendee(SaveMeetingVM meetingVM, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in meetingVM.LegislationAttandeeSelected)
                {
                    var result = await dbContext.MeetingAttendees.Where(x => x.MeetingAttendeeId == Guid.Parse(item.Id) && x.MeetingId == meetingVM.Meeting.MeetingId).FirstOrDefaultAsync();
                    if (result != null)
                    {
                        result.IsPresent = true;
                        dbContext.Entry(result).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<bool> UpdateGEAttendee(SaveMeetingVM meetingVM, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in meetingVM.GEAttandeeSelected)
                {
                    var result = await dbContext.MeetingAttendees.Where(x => x.MeetingAttendeeId == item.MeetingAttendeeId && x.MeetingId == meetingVM.Meeting.MeetingId).FirstOrDefaultAsync();
                    if (result != null)
                    {
                        result.IsPresent = true;
                        dbContext.Entry(result).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Communication
        private async Task<bool> SaveCommunication(DatabaseContext dbContext, Meeting meeting)
        {
            try
            {
                //--Get the request Id by using file Id  
                if (meeting.SubModulId == (int)SubModuleEnum.CaseFile)
                {
                    var requestId = _dbContext.CaseFiles.Where(x => x.FileId == meeting.ReferenceGuid).Select(y => y.RequestId).FirstOrDefault();
                    var requestDetail = _dbContext.CaseRequests.Where(x => x.RequestId == requestId).FirstOrDefault(); // By Using Request id i get created by and govtEntityId
                    if (requestDetail != null)
                    {
                        meeting.ReceivedBy = requestDetail.CreatedBy;
                        meeting.GovtEntityId = requestDetail.GovtEntityId;
                    }
                }
                else if (meeting.SubModulId == (int)SubModuleEnum.RegisteredCase)
                {
                    var fileId = _dbContext.cmsRegisteredCases.Where(x => x.CaseId == meeting.ReferenceGuid).Select(y => y.FileId).FirstOrDefault();
                    var requestId = _dbContext.CaseFiles.Where(x => x.FileId == fileId).Select(y => y.RequestId).FirstOrDefault();
                    var requestDetail = _dbContext.CaseRequests.Where(x => x.RequestId == requestId).FirstOrDefault(); // By Using Request id i get created by and govtEntityId
                    if (requestDetail != null)
                    {
                        meeting.ReceivedBy = requestDetail.CreatedBy;
                        meeting.GovtEntityId = requestDetail.GovtEntityId;
                    }
                }
                else if (meeting.SubModulId == (int)SubModuleEnum.ConsultationFile)
                {
                    var requestId = _dbContext.ConsultationFiles.Where(x => x.FileId == meeting.ReferenceGuid).Select(y => y.RequestId).FirstOrDefault();
                    var requestDetail = _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == requestId).FirstOrDefault(); // By Using Request id i get created by and govtEntityId
                    if (requestDetail != null)
                    {
                        meeting.ReceivedBy = requestDetail.CreatedBy;
                        meeting.GovtEntityId = requestDetail.GovtEntityId;
                    }
                }
                ///--End-------///CaseRequests
                Communication communication = new Communication();
                communication.CommunicationId = new Guid();
                communication.CommunicationTypeId = (int)CommunicationTypeEnum.MeetingScheduled;
                communication.Title = "Schedule_Meeting";

                var resultOutBoxNumber = await _cMSCOMSInboxOutboxRequestPatternNumber.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.OutboxNumber);
                communication.OutboxNumber = resultOutBoxNumber.GenerateRequestNumber;
                communication.OutBoxNumberFormat = resultOutBoxNumber.FormatRequestNumber;
                communication.PatternSequenceResult = resultOutBoxNumber.PatternSequenceResult;

                //communication.OutboxNumber = DateTime.Now.Year + "OT" + (dbContext.Communications.Any() ? await dbContext.Communications.Select(x => x.OutboxShortNum).MaxAsync() + 1 : 1).ToString().PadLeft(6, '0');
                communication.OutboxShortNum = dbContext.Communications.Any() ? await dbContext.Communications.Select(x => x.OutboxShortNum).MaxAsync() + 1 : 1;
                communication.OutboxDate = DateTime.Now;
                communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
                communication.SourceId = (int)CommunicationSourceEnum.FATWA;
                communication.CreatedBy = meeting.CreatedBy;
                communication.CreatedDate = meeting.CreatedDate;
                communication.SectorTypeId = (int)meeting.SectorTypeId;
                communication.SentBy = meeting.SentBy;
                communication.ReceivedBy = meeting.ReceivedBy;
                communication.GovtEntityId = meeting.GovtEntityId;
                communication.ColorId = 4;
                communication.PreCommunicationId = Guid.Empty;
                await _communicationRepo.SaveCommunication(communication, dbContext);
                var doc = await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == meeting.MeetingId && x.AttachmentTypeId == (int)AttachmentTypeEnum.RequestForMeeting).FirstOrDefaultAsync();
                if (doc != null)
                {
                    doc.CommunicationGuid = communication.CommunicationId;
                    _dmsDbContext.UploadedDocuments.Update(doc);
                    await _dmsDbContext.SaveChangesAsync();

                }
                CommunicationTargetLink comTargetLink = new CommunicationTargetLink();
                comTargetLink.TargetLinkId = new Guid();
                comTargetLink.CommunicationId = communication.CommunicationId;
                comTargetLink.CreatedBy = meeting.CreatedBy;
                comTargetLink.CreatedDate = meeting.CreatedDate;
                await _communicationRepo.SaveCommunicationTargetLink(comTargetLink, dbContext);

                List<LinkTarget> linkTargets = new List<LinkTarget>();
                LinkTarget linkTarget;
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    IsPrimary = true,
                    ReferenceId = meeting.MeetingId,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.Meeting
                };
                linkTargets.Add(linkTarget);
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    ReferenceId = communication.CommunicationId,
                    IsPrimary = false,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
                };
                linkTargets.Add(linkTarget);
                await _communicationRepo.SaveLinkTarget(linkTargets, comTargetLink.TargetLinkId, dbContext);

                meetingCommunication.Communication = communication;
                meetingCommunication.CommunicationTargetLink = comTargetLink;
                meetingCommunication.LinkTarget = linkTargets;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public async Task<bool> UpdateMeetingStatus(MeetingDecisionVM meetingVM)
        {
            bool isSaved = true;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Meeting meeting = _dbContext.Meetings.FirstOrDefault(m => m.MeetingId == meetingVM.MeetingId);
                        if (meeting is not null)
                        {
                            meeting.MeetingStatusId = meetingVM.MeetingStatusId;
                            meeting.Comment = meetingVM.Comment;

                            _dbContext.Entry(meeting).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            //var a = _dbContext.Users.Where(x =>x.Email == meetingVM)
                            if (isSaved)
                                transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                }
            }
            return isSaved;
        }
        public async Task<bool> EditMom(SaveMomVM meetingMom)
        {
            bool isSaved = false;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await SaveMOM(meetingMom.MeetingMom, true, _dbContext);
                        if (meetingMom.LegislationAttendee.Any())
                            isSaved = await MarkAttendanceFatwaAttendee(meetingMom, _dbContext);
                        //if (meetingVM.GeAttendee.Any())
                        isSaved = await MarkAttendanceGeAttendeeMOM(meetingMom, _dbContext);
                        isSaved = await SaveMomDecision(meetingMom, _dbContext);
                        var result = await GetOnlyGEAttendeeDetailInMOMDecisionTable(meetingMom, _dbContext);
                        if (result.Count() != 0)
                        {
                            meetingMom.MomAttendeeDecisionDetails = result;
                        }
                        else
                        {
                            meetingMom.MomAttendeeDecisionDetails = new List<MomAttendeeDecision>();
                        }
                        if (isSaved)
                        {
                            if (meetingMom.Meeting.ReferenceGuid != null)
                            {
                                var entPara = CheckReferenceGuid((Guid)meetingMom.Meeting.ReferenceGuid);
                                meetingMom.NotificationParameter.FileNumber = entPara.FileNumber;
                                meetingMom.NotificationParameter.Entity = entPara.EntityName;
                            }
                            transaction.Commit();
                        }
                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                }
                return (isSaved);
            }
        }

        public async Task<AttendeeDecisionVM> GetMeetingAttendeeDecisionById(Guid meetingId, string userId, bool isMomAttendeeDecision)
        {
            try
            {
                string storedProc = string.Empty;
                if (isMomAttendeeDecision)
                {
                    storedProc = $"exec pMOMAttendeeDecision @MeetingMomId = N'{meetingId}', @UserId = N'{userId}'";
                }
                else
                {
                    storedProc = $"exec pMeetingAttendeeDecision @MeetingId = N'{meetingId}', @UserId = N'{userId}'";
                }
                var result = await _dbContext.AttendeeDecisionVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result != null)
                {
                    return result.FirstOrDefault();
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<bool> UpdateMeetingAttendeeDecision(AttendeeDecisionVM meetingVM, string UserId, bool isMomAttendeeDecision)
        {
            bool isSaved = true;
            try
            {
                if (isMomAttendeeDecision) // update MOM attendee decision
                {
                    var MeetingAttendeedetail = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.MeetingId && x.AttendeeUserId == UserId).Select(x => x.MeetingAttendeeId).FirstOrDefaultAsync();
                    if (MeetingAttendeedetail != null)
                    {
                        var MomAttendeeDecision = await _dbContext.MomAttendeeDecisions.Where(x => x.MeetingMomId == meetingVM.MeetingMomId && x.MeetingId == meetingVM.MeetingId && x.MeetingAttendeeId == MeetingAttendeedetail).FirstOrDefaultAsync();

                        if (MomAttendeeDecision is not null)
                        {
                            MomAttendeeDecision.AttendeeStatusId = (int)meetingVM.AttendeeStatusId;
                            if (meetingVM.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Reject)
                            {
                                MomAttendeeDecision.Comment = meetingVM.MOMAttendeeRejectReason;
                            }
                            _dbContext.Entry(MomAttendeeDecision).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            //await CheckAllMOMAttendeeDecision(meetingVM, _dbContext);
                        }
                        else
                        {
                            isSaved = false;
                        }
                    }
                    else
                    {
                        isSaved = false;
                    }
                }
                else // update meeting attendee decision
                {
                    MeetingAttendee meetingAttendee = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.MeetingId && x.AttendeeUserId == UserId).FirstOrDefaultAsync();

                    if (meetingAttendee is not null)
                    {
                        meetingAttendee.AttendeeStatusId = meetingVM.AttendeeStatusId;

                        _dbContext.Entry(meetingAttendee).State = EntityState.Modified;
                        meetingVM.NotificationParameter.Entity = "Meeting";
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        isSaved = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isSaved = false;
            }
            return isSaved;
        }

        private async Task CheckAllMOMAttendeeDecision(AttendeeDecisionVM meetingVM, DatabaseContext dbContext)
        {
            try
            {
                var resultAllAttendees = await dbContext.MomAttendeeDecisions.Where(x => x.MeetingMomId == meetingVM.MeetingMomId && x.MeetingId == meetingVM.MeetingId).Select(x => x.AttendeeStatusId).ToListAsync();
                if (resultAllAttendees.Count() != 0)
                {
                    // list contains Pending and Reject enum then return true ortherwise return false 
                    bool containsPendingAndReject = resultAllAttendees.Contains((int)MeetingAttendeeStatusEnum.Pending) && resultAllAttendees.Contains((int)MeetingAttendeeStatusEnum.Reject);
                    if (!containsPendingAndReject) // return true
                    {
                        // first update MOM status
                        var resultMomModel = await dbContext.MeetingMoms.Where(x => x.MeetingMomId == meetingVM.MeetingMomId && x.MeetingId == meetingVM.MeetingId).FirstOrDefaultAsync();
                        if (resultMomModel != null)
                        {
                            resultMomModel.MOMStatusId = (int)MeetingStatusEnum.Approved;
                            dbContext.Entry(resultMomModel).State = EntityState.Modified;
                        }
                        // then update Meeting status
                        var resultMeetingModel = await dbContext.Meetings.Where(x => x.MeetingId == meetingVM.MeetingId).FirstOrDefaultAsync();
                        if (resultMeetingModel != null)
                        {
                            resultMeetingModel.MeetingStatusId = (int)MeetingStatusEnum.Complete;
                            dbContext.Entry(resultMeetingModel).State = EntityState.Modified;
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task<bool> SaveMomDecision(SaveMomVM meetingMom, DatabaseContext dbContext)
        {
            bool isSaved;
            try
            {
                // first delete existing record from table
                if (dbContext.MomAttendeeDecisions.Any(c => c.MeetingMomId == meetingMom.MeetingMom.MeetingMomId && c.MeetingId == meetingMom.MeetingMom.MeetingId))
                {
                    dbContext.MomAttendeeDecisions.RemoveRange(dbContext.MomAttendeeDecisions.Where(c => c.MeetingMomId == meetingMom.MeetingMom.MeetingMomId && c.MeetingId == meetingMom.MeetingMom.MeetingId));
                }
                // first insert new fatwa attendees records into MOM decision table
                List<MomAttendeeDecision> momAttendeeDecisions = new List<MomAttendeeDecision>();
                var resultMeetingAttendees = await dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingMom.MeetingMom.MeetingId).ToListAsync();
                if (resultMeetingAttendees.Count() != 0) // meetingMom.MeetingMom.MeetingAttendees.Count() != 0
                {
                    foreach (var item in resultMeetingAttendees) // meetingMom.MeetingMom.MeetingAttendees
                    {
                        if (item.RepresentativeId == null)
                        {
                            MomAttendeeDecision attendeeDecision = new MomAttendeeDecision();
                            attendeeDecision.MomAttendeeDecisionId = Guid.NewGuid();
                            attendeeDecision.MeetingMomId = meetingMom.MeetingMom.MeetingMomId;
                            attendeeDecision.MeetingId = meetingMom.MeetingMom.MeetingId;
                            attendeeDecision.MeetingAttendeeId = item.MeetingAttendeeId;
                            attendeeDecision.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.Pending;
                            attendeeDecision.Comment = string.Empty;
                            momAttendeeDecisions.Add(attendeeDecision);
                        }
                    }
                }

                // Then insert new G2G concerned user attendee record into MOM decision table
                if (meetingMom.Meeting.CommunicationId != null && meetingMom.Meeting.IsReplyForMeetingRequest)
                {
                    var resultCommunicationAttendee = await dbContext.CommunicationAttendees.Where(x => x.CommunicationId == meetingMom.Meeting.CommunicationId).ToListAsync();

                    if (resultCommunicationAttendee.Count() != 0) // meetingMom.GeAttendee.Count() != 0
                    {
                        foreach (var attendee in resultCommunicationAttendee)
                        {
                            if (attendee.RepresentativeId == null)
                            {
                                MomAttendeeDecision attendeeDecision = new MomAttendeeDecision();
                                attendeeDecision.MomAttendeeDecisionId = Guid.NewGuid();
                                attendeeDecision.MeetingMomId = meetingMom.MeetingMom.MeetingMomId;
                                attendeeDecision.MeetingId = meetingMom.MeetingMom.MeetingId;
                                attendeeDecision.MeetingAttendeeId = attendee.CommunicationAttendeeId;
                                attendeeDecision.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.Pending;
                                attendeeDecision.Comment = string.Empty;
                                momAttendeeDecisions.Add(attendeeDecision);
                            }
                        }
                        //else
                        //{
                        //    foreach (var attendee in meetingMom.GeAttendee)
                        //    {
                        //        if (attendee.RepresentativeId == null)
                        //        {
                        //            MomAttendeeDecision attendeeDecision = new MomAttendeeDecision();
                        //            attendeeDecision.MomAttendeeDecisionId = Guid.NewGuid();
                        //            attendeeDecision.MeetingMomId = meetingMom.MeetingMom.MeetingMomId;
                        //            attendeeDecision.MeetingId = meetingMom.MeetingMom.MeetingId;
                        //            attendeeDecision.MeetingAttendeeId = attendee.MeetingAttendeeId;
                        //            attendeeDecision.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.Pending;
                        //            attendeeDecision.Comment = string.Empty;
                        //            momAttendeeDecisions.Add(attendeeDecision);
                        //        }

                        //    }
                        //}
                    }
                }
                if (momAttendeeDecisions.Count() != 0)
                {
                    await dbContext.MomAttendeeDecisions.AddRangeAsync(momAttendeeDecisions);

                    await dbContext.SaveChangesAsync();
                    momAttendeeDecisions = new List<MomAttendeeDecision>();
                    isSaved = true;
                }
                else
                {
                    isSaved = false;
                }
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw new Exception(ex.Message);
            }
            return isSaved;
        }

        #region Get MOM attendees decision details
        public async Task<List<MomAttendeeDecisionVM>> PopulateMOMAttendeesDecisionDetails(Guid meetingMomId, Guid meetingId)
        {
            try
            {
                var resultAttendeeDecision = await (from mad in _dbContext.MomAttendeeDecisions
                                                    join ma in _dbContext.MeetingAttendees on mad.MeetingAttendeeId equals ma.MeetingAttendeeId
                                                    where mad.MeetingMomId == meetingMomId && mad.MeetingId == meetingId && ma.IsDeleted == false
                                                    select mad).ToListAsync();


                //var resultAttendeeDecision = await _dbContext.MomAttendeeDecisions.Where(x => x.MeetingMomId == meetingMomId && x.MeetingId == meetingId).ToListAsync();
                if (resultAttendeeDecision.Count() != 0)
                {
                    int atttendeeLegislationSerialNo = 1;
                    foreach (var attendee in resultAttendeeDecision)
                    {
                        MomAttendeeDecisionVM newAttendeeDecision = new MomAttendeeDecisionVM()
                        {
                            MomAttendeeDecisionId = attendee.MomAttendeeDecisionId,
                            MeetingMomId = attendee.MeetingMomId,
                            MeetingId = attendee.MeetingId,
                            Comment = attendee.Comment,
                            SerialNo = atttendeeLegislationSerialNo++,
                            AttendeeStatusId = attendee.AttendeeStatusId,
                            MeetingAttendeeId = attendee.MeetingAttendeeId,

                        };
                        var AttendeeStatus = await _dbContext.MeetingAttendeeStatuses.FirstOrDefaultAsync(x => x.Id == attendee.AttendeeStatusId);
                        if (AttendeeStatus is not null)
                        {
                            newAttendeeDecision.AttendeeStatusEn = AttendeeStatus.NameEn;
                            newAttendeeDecision.AttendeeStatusAr = AttendeeStatus.NameAr;
                        }
                        var AttendeeName = await _dbContext.MeetingAttendees.FirstOrDefaultAsync(x => x.MeetingAttendeeId == attendee.MeetingAttendeeId);
                        if (AttendeeName != null)
                        {
                            newAttendeeDecision.RepresentativeNameEn = AttendeeName.RepresentativeNameEn;
                            newAttendeeDecision.RepresentativeNameAr = AttendeeName.RepresentativeNameAr;
                        }
                        else
                        {
                            var resultAttendeeName = await _dbContext.CommunicationAttendees.FirstOrDefaultAsync(x => x.CommunicationAttendeeId == attendee.MeetingAttendeeId);
                            if (resultAttendeeName != null)
                            {
                                newAttendeeDecision.RepresentativeNameEn = resultAttendeeName.RepresentativeNameEn;
                                newAttendeeDecision.RepresentativeNameAr = resultAttendeeName.RepresentativeNameAr;
                            }
                        }
                        momAttendeeDecisionVMs.Add(newAttendeeDecision);
                    }
                }
                return momAttendeeDecisionVMs;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Meeting detail By Meeting Id
        public async Task<SaveMomVM> GetMeetingDetailById(Guid meetingId)
        {
            try
            {
                Meeting meeting = await _dbContext.Meetings.FirstOrDefaultAsync(x => x.MeetingId == meetingId);
                if (meeting != null)
                {
                    meetingMomVM.Meeting = meeting;
                    meetingMomVM.Meeting.IsHeld = false;
                    var commMeetingStatus = await _dbContext.MeetingStatuses.FirstOrDefaultAsync(x => x.MeetingStatusId == meeting.MeetingStatusId);
                    if (commMeetingStatus != null)
                    {
                        meetingMomVM.Meeting.MeetingStatusEn = commMeetingStatus.NameEn;
                        meetingMomVM.Meeting.MeetingStatusAr = commMeetingStatus.NameAr;
                    }
                }
                MeetingMom meetingMom = await _dbContext.MeetingMoms.FirstOrDefaultAsync(x => x.MeetingId == meetingId);
                if (meetingMom != null)
                {
                    meetingMomVM.MeetingMom = meetingMom;
                }
                // get fatwa attendees details
                List<MeetingAttendeeVM> legisAattendees = await GetMeetingAttendeeByMeetingId(meetingId, (int)MeetingAttendeeTypeEnum.LegislationAttendee);
                if (legisAattendees.Count() != 0)
                {
                    int atttendeeLegislationSerialNo = 1;
                    foreach (var attendee in legisAattendees)
                    {
                        attendee.AttendeeUserId = attendee.AttendeeUserId ?? string.Empty;
                        FatwaAttendeeVM newAttendee = new FatwaAttendeeVM()
                        {
                            Id = attendee.AttendeeUserId,
                            FirstNameEnglish = attendee.RepresentativeNameEn,
                            FirstNameArabic = attendee.RepresentativeNameAr,
                            DepartmentId = attendee.DepartmentId,
                            SerialNo = atttendeeLegislationSerialNo++,
                            AttendeeStatusId = attendee.AttendeeStatusId,
                            AttendeeStatusEn = attendee.AttendeeStatusEn,
                            AttendeeStatusAr = attendee.AttendeeStatusAr,
                            IsPresent = attendee.IsPresent
                        };
                        var dept = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == attendee.DepartmentId);
                        if (dept is not null)
                        {
                            newAttendee.DepartmentEnglish = dept.Name_En;
                            newAttendee.DepartmentArabic = dept.Name_Ar;
                        }

                        meetingMomVM.LegislationAttendee.Add(newAttendee);
                    }
                }
                // get GE attendees details

                List<CommunicationAttendee> geattendees = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == meeting.CommunicationId).ToListAsync();
                if (geattendees.Count() != 0)
                {
                    int atttendeeGeSerialNo = 1;
                    foreach (var attendee in geattendees)
                    {
                        attendee.AttendeeUserId = attendee.AttendeeUserId ?? string.Empty;
                        MeetingAttendeeVM newAttendee = new MeetingAttendeeVM()
                        {
                            AttendeeUserId = attendee.AttendeeUserId,
                            RepresentativeNameEn = attendee.RepresentativeNameEn,
                            RepresentativeNameAr = attendee.RepresentativeNameAr,
                            DepartmentId = attendee.DepartmentId,
                            SerialNo = atttendeeGeSerialNo++,
                            IsPresent = attendee.IsPresent
                            //AttendeeStatusId = attendee.AttendeeStatusId,
                            //AttendeeStatusEn = attendee.AttendeeStatusEn,
                            //AttendeeStatusAr = attendee.AttendeeStatusAr,
                        };

                        var govt = _dbContext.GovernmentEntity.FirstOrDefault(x => x.EntityId == attendee.GovernmentEntityId);
                        if (govt is not null)
                        {
                            newAttendee.GovernmentEntityId = govt.EntityId;
                            newAttendee.GovernmentEntityNameEn = govt.Name_En;
                            newAttendee.GovernmentEntityNameAr = govt.Name_Ar;
                        }

                        var dept = _dbContext.Departments.FirstOrDefault(x => x.Id == attendee.DepartmentId);
                        if (dept is not null)
                        {
                            newAttendee.DepartmentId = dept.Id;
                            newAttendee.DepartmentNameEn = dept.Name_En;
                            newAttendee.DepartmentNameAr = dept.Name_Ar;
                        }
                        meetingMomVM.GeAttendee.Add(newAttendee);
                    }
                }
                else
                {
                    List<MeetingAttendeeVM> geAattendees = await GetMeetingAttendeeByMeetingId(meetingId, (int)MeetingAttendeeTypeEnum.GeAttendee);
                    if (geAattendees.Count() != 0)
                    {
                        meetingMomVM.GeAttendee = geAattendees;
                    }
                }
                return meetingMomVM;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Edit Meeting Status
        public async Task<MeetingStatusVM> EditMeetingStatus(MeetingStatusVM meeting)
        {
            try
            {

                var meetingStatus = _dbContext.Meetings.FirstOrDefault(m => m.MeetingId == meeting.MeetingId);
                if (meetingStatus is not null)
                {
                    meetingStatus.MeetingStatusId = meeting.MeetingStatusId;
                    _dbContext.Meetings.Update(meetingStatus);
                    await _dbContext.SaveChangesAsync();
                    return meeting;
                }

                return new MeetingStatusVM();
            }
            catch (Exception)
            {
                return new MeetingStatusVM();
                throw;
            }
        }
        #endregion

        #region Meeting Fatwa Attendee and GE Attendee Detail For MOM
        public async Task<List<MeetingAttendee>> GetAttendeeDetails(Guid meetingId)
        {
            try
            {
                List<MeetingAttendee> MeetingAttendee = _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingId && x.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Accept).ToList();
                return MeetingAttendee;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region  SubmitMom
        public async Task<bool> SubmitMom(SaveMomVM meetingMom)
        {
            bool isSaved = true;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await SubmitMOM(meetingMom.MeetingMom, _dbContext);
                        isSaved = await GetAttendanceFatwaAttendee(meetingMom, _dbContext);
                        isSaved = await GetAttendanceGeAttendeeMOM(meetingMom, _dbContext);
                        meetingMom.MomAttendeeDecisionDetails = new List<MomAttendeeDecision>();
                        isSaved = await CheckAllMOMAttendeeDecision(meetingMom, _dbContext);
                        if (isSaved)
                        {
                            if (meetingMom.Meeting.ReferenceGuid != null)
                            {
                                var entPara = CheckReferenceGuid((Guid)meetingMom.Meeting.ReferenceGuid);
                                meetingMom.NotificationParameter.FileNumber = entPara.FileNumber;
                                meetingMom.NotificationParameter.Entity = entPara.EntityName;
                            }

                            transaction.Commit();
                        }

                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                }
            }
            return (isSaved);
        }

        public async Task<bool> SubmitMOM(MeetingMom meetingMom, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                _dbContext.Entry(meetingMom).State = EntityState.Modified;
                isSaved = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return (isSaved);
        }

        public async Task<bool> GetAttendanceFatwaAttendee(SaveMomVM meetingVM, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                // get present fatwa attendees list from UI and update only Present attendees record accordingly.
                //foreach (var attendee in meetingVM.LegislationAttendee)
                //{
                //    var attendeeExist = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId && x.AttendeeUserId == attendee.Id && x.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Accept).FirstOrDefaultAsync();
                //    if (attendeeExist != null && attendeeExist.IsPresent != true)
                //    {
                //        attendeeExist.IsPresent = true;
                //        _dbContext.Update(attendeeExist);
                //    }
                //}
                var attendeeExist = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).ToListAsync();
                if (attendeeExist.Count() != 0)
                {
                    foreach (var attendee in meetingVM.LegislationAttendee)
                    {
                        foreach (var item in attendeeExist)
                        {
                            if (item.AttendeeUserId == attendee.Id && item.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Accept)
                            {
                                item.IsPresent = true;
                                _dbContext.Update(item);
                            }
                            else if (item.AttendeeUserId == null && item.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New)
                            {
                                item.IsPresent = true;
                                _dbContext.Update(item);
                            }
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                // get All Approved Meeting fatwa Attendee present or not  present to send them a task
                List<MeetingAttendeeVM> legisAattendees = await GetMeetingAttendeeByMeetingId(meetingVM.MeetingMom.MeetingId, (int)MeetingAttendeeTypeEnum.LegislationAttendee);
                if (legisAattendees.Count() != 0)
                {
                    int atttendeeLegislationSerialNo = 1;
                    foreach (var attendee in legisAattendees)
                    {
                        attendee.AttendeeUserId = attendee.AttendeeUserId ?? string.Empty;
                        FatwaAttendeeVM newAttendee = new FatwaAttendeeVM()
                        {
                            Id = attendee.AttendeeUserId,
                            FirstNameEnglish = attendee.RepresentativeNameEn,
                            DepartmentId = attendee.DepartmentId,
                            SerialNo = atttendeeLegislationSerialNo++,
                            AttendeeStatusId = attendee.AttendeeStatusId,
                            AttendeeStatusEn = attendee.AttendeeStatusEn,
                            AttendeeStatusAr = attendee.AttendeeStatusAr,
                            IsPresent = attendee.IsPresent
                        };
                        meetingMomVM.LegislationAttendee.Add(newAttendee);
                        meetingVM.LegislationAttendee = meetingMomVM.LegislationAttendee;
                    }
                }
                isSaved = true;
                return isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> GetAttendanceGeAttendeeMOM(SaveMomVM meetingVM, DatabaseContext _dbContext)
        {
            bool isSaved = true;
            try
            {
                //// get present GE attendees list from UI and update only Present attendees record accordingly.
                // get present GE attendees list from UI and update only Present attendees record accordingly.
                //foreach (var attendee in meetingVM.GeAttendee)
                //{
                //    var attendeeExist = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId && x.MeetingAttendeeId == attendee.MeetingAttendeeId).FirstOrDefaultAsync();
                //    if (attendeeExist != null && attendeeExist.IsPresent != true)
                //    {
                //        attendeeExist.IsPresent = true;
                //        _dbContext.Update(attendeeExist);
                //    }
                //}
                var attendeeExist = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == meetingVM.Meeting.CommunicationId).ToListAsync();
                if (attendeeExist.Count() != 0)
                {
                    foreach (var attendee in meetingVM.GeAttendee)
                    {
                        foreach (var item in attendeeExist)
                        {
                            if (item.RepresentativeNameEn == attendee.RepresentativeNameEn)
                            {
                                item.IsPresent = true;
                                _dbContext.Update(item);
                            }
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();

                // get All Ge Attendee 
                List<MeetingAttendeeVM> geAattendees = await GetMeetingAttendeeByMeetingId(meetingVM.MeetingMom.MeetingId, (int)MeetingAttendeeTypeEnum.GeAttendee);
                if (geAattendees.Count() != 0)
                {
                    meetingVM.GeAttendee = geAattendees;
                }
                return isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
        }

        private async Task<bool> CheckAllMOMAttendeeDecision(SaveMomVM meetingVM, DatabaseContext dbContext)
        {
            bool isSaved;
            try
            {
                var resultAllAttendees = await dbContext.MomAttendeeDecisions.Where(x => x.MeetingMomId == meetingVM.MeetingMom.MeetingMomId && x.MeetingId == meetingVM.MeetingMom.MeetingId).Select(x => x.AttendeeStatusId).ToListAsync();
                if (resultAllAttendees.Count() != 0)
                {
                    // list contains Pending and Reject enum then return true ortherwise return false 
                    bool containsPendingAndReject = resultAllAttendees.Contains((int)MeetingAttendeeStatusEnum.Pending) && resultAllAttendees.Contains((int)MeetingAttendeeStatusEnum.Reject);
                    if (!containsPendingAndReject) // return true
                    {
                        // first update MOM status
                        var resultMomModel = await dbContext.MeetingMoms.Where(x => x.MeetingMomId == meetingVM.MeetingMom.MeetingMomId && x.MeetingId == meetingVM.MeetingMom.MeetingId).FirstOrDefaultAsync();
                        if (resultMomModel != null)
                        {
                            resultMomModel.MOMStatusId = (int)MeetingStatusEnum.Approved;
                            dbContext.Entry(resultMomModel).State = EntityState.Modified;
                        }
                        // then update Meeting status
                        var resultMeetingModel = await dbContext.Meetings.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).FirstOrDefaultAsync();
                        if (resultMeetingModel != null)
                        {
                            resultMeetingModel.MeetingStatusId = (int)MeetingStatusEnum.Complete;
                            dbContext.Entry(resultMeetingModel).State = EntityState.Modified;
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
                return isSaved = true;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        #endregion 

        public async Task<List<GEDepartments>> GetDepartmentsByGeId(int GeId)
        {
            var department = await _dbContext.GEDepartments.Where(x => x.Id == GeId).ToListAsync();
            return department;
        }
        public async Task<MeetingDecisionVM> GetMeetingDetailsById(Guid meetingId)


        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string storedProc = $"exec pMeetingDecision @MeetingId = N'{meetingId}'";
                var result = await _DbContext.MeetingDecisionVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result != null)
                {

                    var res = result.FirstOrDefault();
                    if (res != null)
                    {
                        var notifPara = CheckReferenceGuid((Guid)res.ReferenceGuid);
                        if (notifPara != null)
                        {
                            res.NotificationParameter.Entity = notifPara.EntityName;
                            res.NotificationParameter.FileNumber = notifPara.FileNumber;
                            if (res.NotificationParameter.Entity == "CaseFile")
                            {

                                var query = from caseFile in _DbContext.CaseFiles
                                            where caseFile.FileId == res.ReferenceGuid
                                            join caseRequest in _DbContext.CaseRequests
                                            on caseFile.RequestId equals caseRequest.RequestId
                                            select new
                                            {
                                                caseRequest.GovtEntityId,
                                            };

                                var fileData = query.FirstOrDefault();
                                if (fileData != null)
                                {
                                    GovernmentEntity AssignedBy = await _iCMSCaseRequest.GetGovtEntityId((int)fileData.GovtEntityId);
                                    res.NotificationParameter.SenderName = AssignedBy.Name_En;

                                }

                            }
                            else if (res.NotificationParameter.Entity == "ConsultationFile")

                            {

                                var query = from ConsultationFile in _DbContext.ConsultationFiles
                                            where ConsultationFile.FileId == res.ReferenceGuid
                                            join consultationRequest in _DbContext.ConsultationRequests
                                            on ConsultationFile.RequestId equals consultationRequest.ConsultationRequestId
                                            select new
                                            {
                                                consultationRequest.GovtEntityId,
                                            };

                                var fileData = query.FirstOrDefault();
                                if (fileData != null)
                                {
                                    GovernmentEntity AssignedBy = await _iCMSCaseRequest.GetGovtEntityId((int)fileData.GovtEntityId);
                                    res.NotificationParameter.SenderName = AssignedBy.Name_En;

                                }

                            }
                        }
                    }
                    return res;

                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task<bool> CheckDraftExixt(Guid MeetingId)
        {
            bool result = false;
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var meeting = _DbContext.Meetings.Where(x => x.MeetingId == MeetingId).FirstOrDefault();
            if (meeting != null)
            {
                result = true;
                return result;
            }
            return result;
        }
        #region Take Request For Meeting Decision From Fatwa
        public async Task<MeetingCommunicationVM> TakeRequestForMeetingDecisionFromFatwa(SaveMeetingVM meetingVM)
        {
            bool isSaved = true;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await SaveMeeting(meetingVM, _dbContext);
                        if (isSaved && meetingVM.Meeting.IsApproved && meetingVM.Meeting.IsReplyForMeetingRequest)
                        {
                            var resultMeeting = await _dbContext.Meetings.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).FirstOrDefaultAsync();
                            if (resultMeeting != null)
                            {
                                resultMeeting.MeetingStatusId = (int)MeetingStatusEnum.Scheduled;
                                _dbContext.Meetings.Update(resultMeeting);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        if (meetingVM.LegislationAttendee.Any())
                            isSaved = await SaveFatwaAttendee(meetingVM, _dbContext);
                        if (meetingVM.GeAttendee.Any())
                            isSaved = await SaveGeAttendee(meetingVM, _dbContext);


                        // update communication meeting status
                        if (meetingVM.CommunicationId != null && meetingVM.CommunicationId != Guid.Empty)
                        {
                            var comMeeting = await _dbContext.CommunicationMeetings.Where(x => x.CommunicationId == meetingVM.CommunicationId).FirstOrDefaultAsync();
                            comMeeting.StatusId = meetingVM.Meeting.MeetingStatusId;
                            comMeeting.Location = meetingVM.Meeting.Location;
                            comMeeting.MeetingLink = meetingVM.Meeting.MeetingLink;
                            comMeeting.IsOnline = meetingVM.Meeting.IsOnline;
                            comMeeting.RequirePassword = meetingVM.Meeting.RequirePassword;
                            comMeeting.MeetingPassword = meetingVM.Meeting.MeetingPassword;
                            comMeeting.Note = meetingVM.Meeting.Note;
                            comMeeting.IsReplyForMeetingRequest = meetingVM.Meeting.IsReplyForMeetingRequest;
                            _dbContext.CommunicationMeetings.Update(comMeeting);
                            await _dbContext.SaveChangesAsync();
                        }
                        //if (meetingVM.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meetingVM.Meeting.IsApproved == true)
                        //{
                        //    isSaved = await UpdateCommunicationMeeting(_dbContext, meetingVM);
                        //}
                        // Update Task status
                        if (meetingVM.Meeting.IsApproved == true && (meetingVM.TaskId != null || meetingVM.TaskId != Guid.Empty))
                        {
                            var userTask = _dbContext.Tasks.FirstOrDefault(m => m.TaskId == meetingVM.TaskId);
                            if (userTask is not null)
                            {
                                userTask.TaskStatusId = (int)TaskStatusEnum.Done;
                                userTask.ModifiedBy = meetingVM.Meeting.ModifiedBy;
                                userTask.ModifiedDate = DateTime.Now;

                                _dbContext.Tasks.Update(userTask);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        if (isSaved)
                            transaction.Commit();

                        if (meetingVM.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meetingVM.Meeting.IsApproved == true)
                        {
                            Meeting meeting = await _dbContext.Meetings.FirstOrDefaultAsync(m => m.MeetingId == meetingVM.Meeting.MeetingId);
                            List<MeetingAttendee> meetingAttendees = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId).ToListAsync();
                            meetingCommunication.Meeting = meeting;
                            meetingCommunication.MeetingAttendees = meetingAttendees;
                            var resultCommunication = await _dbContext.Communications.FirstOrDefaultAsync(m => m.CommunicationId == meetingVM.CommunicationId);
                            meetingCommunication.Communication = resultCommunication;
                            var resultCommTargetLink = await _dbContext.CommunicationTargetLinks.FirstOrDefaultAsync(m => m.CommunicationId == resultCommunication.CommunicationId);
                            meetingCommunication.CommunicationTargetLink = resultCommTargetLink;
                            var resultCommlinkTargets = await _dbContext.LinkTargets.Where(m => m.TargetLinkId == resultCommTargetLink.TargetLinkId).ToListAsync();
                            meetingCommunication.LinkTarget = resultCommlinkTargets;
                            var comMeeting = await _dbContext.CommunicationMeetings.Where(x => x.CommunicationId == meetingVM.CommunicationId).FirstOrDefaultAsync();
                            meetingCommunication.CommunicationMeetings = comMeeting;
                            return meetingCommunication;
                        }
                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                        throw;
                    }
                    return null;
                }
            }
        }
        public async Task<bool> SaveFatwaAttendee(SaveMeetingVM meetingVM, DatabaseContext _dbContext)
        {
            bool isSaved;
            List<MeetingAttendee> attendeesList = new List<MeetingAttendee>();
            try
            {
                foreach (var attendee in meetingVM.LegislationAttendee)
                {
                    var attendeeExist = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == meetingVM.Meeting.MeetingId && x.AttendeeUserId == attendee.Id).FirstOrDefaultAsync();
                    if (attendeeExist is null)
                    {
                        int Status = 0;

                        if (attendee.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New)
                        {
                            Status = (int)MeetingAttendeeStatusEnum.Pending;
                        }
                        else
                        {
                            Status = (int)attendee.AttendeeStatusId;

                        }
                        MeetingAttendee newAttendee = new MeetingAttendee()
                        {
                            MeetingAttendeeId = Guid.NewGuid(),
                            AttendeeUserId = attendee.Id,
                            RepresentativeNameEn = attendee.FirstNameEnglish,
                            MeetingId = meetingVM.Meeting.MeetingId,
                            MeetingAttendeeTypeId = (int)MeetingAttendeeTypeEnum.LegislationAttendee,
                            AttendeeStatusId = Status,
                            IsPresent = false,
                            DepartmentId = attendee.DepartmentId,
                            CreatedBy = meetingVM.Meeting.ModifiedBy,
                            CreatedDate = (DateTime)meetingVM.Meeting.ModifiedDate,
                            IsDeleted = meetingVM.Meeting.IsDeleted,
                        };
                        attendeesList.Add(newAttendee);
                    }
                }
                await _dbContext.MeetingAttendees.AddRangeAsync(attendeesList);

                //Remove Attendees 
                var deleteAttendees = meetingVM.DeletedLegislationAttendeeIds;
                await DeleteFatwaAttendees(deleteAttendees, meetingVM, _dbContext);

                await _dbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> DeleteFatwaAttendees(List<Guid> deleteAttendees, SaveMeetingVM meet, DatabaseContext dbContext)
        {
            bool isSaved = true;
            try
            {
                if (deleteAttendees.Any())
                {
                    foreach (var item in deleteAttendees)
                    {
                        var deleteAttendee = await dbContext.MeetingAttendees.Where(x => x.MeetingId == meet.Meeting.MeetingId && x.AttendeeUserId == item.ToString()).FirstOrDefaultAsync();
                        if (deleteAttendee != null)
                        {
                            deleteAttendee.IsDeleted = true;
                            deleteAttendee.DeletedDate = DateTime.Now;
                            deleteAttendee.DeletedBy = meetingVM.Meeting.ModifiedBy;

                            dbContext.Entry(deleteAttendee).State = EntityState.Modified;
                        }
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;

        }
        #endregion

        #region Update Meeting MOM attendee decision
        //<History Author = 'Umer Zaman' Date='03-01-2024' Version="1.0" Branch="master"> Update Meeting MOM attendee decision</History>
        public async Task<bool> UpdateMeetingMOMAttendeeDecision(MomAttendeeDecision item)
        {
            bool isSaved = false;
            try
            {
                var MomAttendeeDecision = await _dbContext.MomAttendeeDecisions.Where(x => x.MomAttendeeDecisionId == item.MomAttendeeDecisionId && x.MeetingMomId == item.MeetingMomId && x.MeetingId == item.MeetingId).FirstOrDefaultAsync();
                if (MomAttendeeDecision != null)
                {
                    MomAttendeeDecision.AttendeeStatusId = item.AttendeeStatusId;
                    if (item.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Reject)
                    {
                        MomAttendeeDecision.Comment = item.Comment;
                    }
                    _dbContext.Entry(MomAttendeeDecision).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    isSaved = true;
                }
                return isSaved;
            }
            catch (Exception ex)
            {
                return isSaved;
            }
        }
        #endregion

        #region Get MOM CreatedBy Id By Using MOMId
        public async Task<User> GetMOMCreatedByIdByUsingMOMId(Guid meetingMomId)
        {
            try
            {
                var resultUser = await _dbContext.MeetingMoms.Where(x => x.MeetingMomId == meetingMomId).FirstOrDefaultAsync();
                if (resultUser != null)
                {
                    var resultUserId = await _dbContext.Users.Where(x => x.UserName == resultUser.CreatedBy).FirstOrDefaultAsync();
                    if (resultUserId != null)
                    {
                        return resultUserId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        public async Task<bool> GetAttendeeStatusesByMeetingId(Guid meetingId)
        {
            try
            {

                var resultUser = _dbContext.MeetingAttendees.Where(x => (x.MeetingId == meetingId) && (x.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Pending || x.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New) && (x.MeetingAttendeeTypeId == (int)MeetingAttendeeTypeEnum.LegislationAttendee) && (x.IsDeleted == false)).FirstOrDefault();
                if (resultUser != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<AttendeeDecisionVM> GetNotificationParameters(AttendeeDecisionVM decision)
        {
            var notifPara = CheckReferenceGuid((Guid)decision.ReferenceGuid);
            if (notifPara != null)
            {
                decision.NotificationParameter.Entity = notifPara.EntityName;
                decision.NotificationParameter.FileNumber = notifPara.FileNumber;
            }
            return decision;

        }
        public async Task<bool> CheckViceHosApproval(int sectorTypeId)
        {
            return _dbContext.OperatingSectorType.Where(x => x.Id == sectorTypeId).Select(y => y.IsOnlyViceHosApprovalRequired).FirstOrDefault();


        }


    }
}

using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using System.Linq;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using System.Text.RegularExpressions;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using static FATWA_DOMAIN.Enums.TaskEnums;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using FATWA_DOMAIN.Models.ViewModel;
using Itenso.TimePeriod;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;
using Humanizer;

namespace FATWA_INFRASTRUCTURE.Repository.Communications
{
    public class CommunicationRepository : ICommunication
    {
        #region Variables

        private string? storedProc;
        User UserDetails;
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private readonly IConfiguration _Config;
        private CommunicationResponseVM _CommunicationResponse;
        private List<CommunicationMeetingDetailVM> _communicationMeetingDetailVMs;
        private List<CommunicationSendMessageVM> _communicationSendMessageVMs;
        private List<CommunicationInboxOutboxVM> _inboxOutboxListVM;
        private List<CmsCaseRequestResponseVM> _CmsCaseRequestResponseVMs;
        private List<CommunicationListVM> _communicationListVM;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<CmsAnnouncementVM> _cmsAnnouncementsVMs;
        private List<CorrespondenceHistoryVM> _correspondenceHistoryVM;
        private readonly IAccount _IAccount;
        private readonly IRole _IRole;
        private readonly ITask _ITask;
        private IEnumerable<GovernmentEntity> _governmentEntities;
        private SendCommunicationVM communicationMeetingVM = new SendCommunicationVM()
        {
            Communication = new Communication(),
            CommunicationMeeting = new CommunicationMeeting(),
            CommunicationAttendee = new List<CommunicationAttendeeVM>(),
            DeletedGeAttendeeIds = new List<Guid>()

        };


        private CommunicationVM _CommunicationVM = new CommunicationVM()
        {
            Communication = new Communication(),
            CommunicationMeeting = new CommunicationMeeting(),
            MeetingAttendee = new List<MeetingAttendeeVM>(),
        };

        private CommunicationResponse communicationResponse;
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        #endregion

        public CommunicationRepository(DatabaseContext dbContext, DmsDbContext dmsDbContext, IServiceScopeFactory serviceScopeFactory, IAccount iAccount, IRole iRole, CMSCOMSInboxOutboxPatternNumberRepository CMSCOMSInboxOutboxPatternNumberRepository, IConfiguration Config, ITask ITask)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsDbContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _IAccount = iAccount;
            _IRole = iRole;
            _cMSCOMSInboxOutboxPatternNumberRepository = CMSCOMSInboxOutboxPatternNumberRepository;
            _Config = Config;
            _ITask = ITask;
        }

        #region Save

        string? outboxNo = null;
        DateTime? outboxDate = null;
        public async Task<bool> SendCommunication(SendCommunicationVM sendCommunication)
        {
            bool isSaved;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await SaveCommunicationAttachment(sendCommunication.Communication, sendCommunication.CommunicationAttachments, _dmsDbContext);
                        isSaved = await SaveCommunication(sendCommunication.Communication, _dbContext);
                        if (sendCommunication.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
                        {
                            isSaved = await SaveCommunicationMeeting(sendCommunication.CommunicationMeeting, _dbContext);
                            isSaved = await SaveCommunicationAttendee(sendCommunication.CommunicationAttendee, sendCommunication.Communication, _dbContext);
                        }
                        if (sendCommunication.CommunicationResponse != null)
                            isSaved = await SaveCommResponse(sendCommunication.CommunicationResponse, sendCommunication.Communication.CommunicationId, _dbContext);
                        isSaved = await SaveCommunicationTargetLink(sendCommunication.CommunicationTargetLink, _dbContext);
                        isSaved = await SaveLinkTarget(sendCommunication.LinkTarget, sendCommunication.CommunicationTargetLink.TargetLinkId, _dbContext);

                        if (isSaved == true)
                            transaction.Commit();
                    }
                    catch (Exception)
                    {
                        isSaved = false;
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return isSaved;

        }

        public async Task<bool> SaveCommunication(Communication communication, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                // Check the sourceId
                if (communication.SourceId == (int)CommunicationSourceEnum.G2G)
                {
                    if (String.IsNullOrEmpty((communication.ReceivedBy)))
                    {
                        User hosUser = await GetHOSBySectorId((int)communication.SectorTypeId, _dbContext);
                        communication.ReceivedBy = hosUser?.Email;
                        UserDetails = await _dbContext.Users.Where(x => x.UserName == communication.CreatedBy).FirstOrDefaultAsync();
                    }
                    communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Inbox;
                    {
                        var resultInboxNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0,(int)CmsComsNumPatternTypeEnum.InboxNumber);
                        communication.InboxNumber = resultInboxNumber.GenerateRequestNumber;
                        communication.InboxNumberFormat = resultInboxNumber.FormatRequestNumber;
                        communication.PatternSequenceResult = resultInboxNumber.PatternSequenceResult;
                    }
                    communication.InboxDate = DateTime.Now;
                    communication.ReferenceNumber = communication.OutboxNumber;
                    communication.ReferenceDate = communication.OutboxDate;
                    communication.OutboxNumber = null;
                    communication.OutboxDate = null;
                    communication.ColorId = 1;
                }
                else if (communication.SourceId == (int)CommunicationSourceEnum.FATWA)
                {
                    if (String.IsNullOrEmpty(communication.SentBy))
                    {
                        //communication.IsRead = true;
                        communication.SentBy = communication.CreatedBy;
                    }
                }

                await _dbContext.Communications.AddAsync(communication);
                await _dbContext.SaveChangesAsync();

                if (communication.PreCommunicationId != Guid.Empty && communication.PreCommunicationId != null)
                {
                    var oldCommunication = await _dbContext.Communications.Where(x => x.CommunicationId == communication.PreCommunicationId).FirstOrDefaultAsync();
                    if (communication.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMoreInformationReminder)
                        oldCommunication.IsReminderSent = true;
                    else
                        oldCommunication.IsReplied = true;
                    _dbContext.Communications.Update(oldCommunication);
                    await _dbContext.SaveChangesAsync();
                }

                isSaved = true;

            }
            catch
            {
                isSaved = false;
                throw;
            }
            return true;
        }

        public async Task<bool> SaveCommunicationMeeting(CommunicationMeeting communicationMeeting, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                _dbContext.CommunicationMeetings.Add(communicationMeeting);
                await _dbContext.SaveChangesAsync();

                isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> SaveCommunicationAttendee(List<CommunicationAttendeeVM> communicationAttendee, Communication communication, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                List<CommunicationAttendee> attendeesList = new List<CommunicationAttendee>();

                foreach (var attendee in communicationAttendee)
                {
                    CommunicationAttendee newAttendee = new CommunicationAttendee()
                    {
                        CommunicationAttendeeId = attendee.CommunicationAttendeeId,
                        GovernmentEntityId = attendee.GovernmentEntityId,
                        DepartmentId = attendee.DepartmentId,
                        RepresentativeNameEn = attendee.RepresentativeNameEn,
                        RepresentativeId = attendee.RepresentativeId,
                        RepresentativeNameAr = attendee.RepresentativeNameAr,
                        CommunicationId = communication.CommunicationId,
                        CreatedBy = communication.CreatedBy,
                        CreatedDate = communication.CreatedDate,
                        IsDeleted = communication.IsDeleted,
                        AttendeeUserId = attendee.AttendeeUserId,
                        //AttendeeStatusId = attendee.AttendeeStatusId,
                        IsPresent = attendee.IsPresent
                    };
                    attendeesList.Add(newAttendee);
                }

                _dbContext.CommunicationAttendees.AddRange(attendeesList);
                await _dbContext.SaveChangesAsync();

                isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> SaveCommunicationAttachment(Communication communication, List<UploadedDocument> CommunicationAttachments, DmsDbContext dmsDbContext)
        {
            bool isSaved;
            try
            {
                if (communication.CorrespondenceTypeId == (int)CommunicationCorrespondenceTypeEnum.Outbox && CommunicationAttachments == null)
                {
                    var attachements = dmsDbContext.TempAttachements.Where(x => x.Guid == communication.CommunicationId).ToList();
                    foreach (var file in attachements)
                    {
                        UploadedDocument documentObj = new UploadedDocument();
                        documentObj.Description = file.Description;
                        documentObj.CreatedDateTime = DateTime.Now;
                        documentObj.CreatedBy = communication.CreatedBy;
                        documentObj.DocumentDate = DateTime.Now;
                        documentObj.FileName = file.FileName;
                        documentObj.StoragePath = file.StoragePath;
                        documentObj.DocType = file.DocType;
                        documentObj.ReferenceGuid = communication.CommunicationId;
                        documentObj.IsActive = true;
                        documentObj.CreatedAt = file.StoragePath;
                        documentObj.AttachmentTypeId = file.AttachmentTypeId;
                        documentObj.IsDeleted = false;
                        documentObj.OtherAttachmentType = file.OtherAttachmentType;
                        documentObj.ReferenceNo = file.ReferenceNo;
                        if (documentObj.ReferenceNo is not null)
                            outboxNo = documentObj.ReferenceNo;
                        documentObj.ReferenceDate = file.ReferenceDate;
                        if (documentObj.ReferenceDate is not null)
                            outboxDate = documentObj.ReferenceDate;
                        await dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                        await dmsDbContext.SaveChangesAsync();
                        await Task.Delay(200);
                        CommunicationAttachments.Add(documentObj);

                        dmsDbContext.TempAttachements.Remove(file);
                        await dmsDbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    foreach (var file in CommunicationAttachments)
                    {
                        file.UploadedDocumentId = 0;
                        await dmsDbContext.UploadedDocuments.AddAsync(file);
                        await dmsDbContext.SaveChangesAsync();
                    }
                }
                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> SaveCommunicationTargetLink(CommunicationTargetLink communicationTargetLink, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                if (communicationTargetLink.TargetLinkId == null || communicationTargetLink.TargetLinkId == Guid.Empty)
                    communicationTargetLink.TargetLinkId = new Guid();
                _dbContext.CommunicationTargetLinks.Add(communicationTargetLink);
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

        public async Task<bool> SaveLinkTarget(List<LinkTarget> linkTarget, Guid targetLinkId, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                foreach (var target in linkTarget)
                {
                    target.TargetLinkId = targetLinkId;
                    _dbContext.LinkTargets.Add(target);
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

        public async Task<bool> SaveCommResponse(CommunicationResponse communicationResponse, Guid communicationId, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                communicationResponse.CommunicationId = communicationId;
                _dbContext.CommunicationResponses.Add(communicationResponse);
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

        public async Task<bool> SaveCommResponseGovtEntit(CommunicationResponse communicationResponse, DatabaseContext dbContext)
        {

            try
            {
                foreach (var entityId in communicationResponse.EntityIds)
                {
                    var govtEntity = new CommunicationResponseGovtEntity
                    {
                        CommunicationResponseId = communicationResponse.CommunicationResponseId,
                        EntityId = entityId
                    };

                    dbContext.CommunicationResponseGovtEntities.Add(govtEntity);

                }
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> getUserIdbyUserName(SendCommunicationVM sendCommunication)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                User hosUser = await GetHOSBySectorId((int)sendCommunication.Communication.SectorTypeId, databaseContext);
                UserDetails = await databaseContext.Users.Where(x => x.UserName == hosUser.Email).FirstOrDefaultAsync();
            }
            catch
            {
            }
            return UserDetails;
        }
        #endregion

        #region save case request status history

        public async Task SaveCaseRequestStatusHistory(string userName, CaseRequest caseRequest, int EventId, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseRequestHistory historyobj = new CmsCaseRequestHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.RequestId = caseRequest.RequestId;
                historyobj.StatusId = (int)caseRequest.StatusId;
                historyobj.CreatedBy = userName;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.EventId = EventId;
                historyobj.Remarks = caseRequest.Remarks;
                await dbContext.CmsCaseRequestHistories.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Communications

        int atttendeeGeSerialNo = 0;
        int fatwaAtttendeeSerialNo = 0;
        public async Task<CommunicationVM> GetCommunicationDetailCommunicationId(Guid communicationId)
        {
            try
            {
                var communication = await _dbContext.Communications.FirstOrDefaultAsync(x => x.CommunicationId == communicationId);
                if (communication != null)
                {
                    _CommunicationVM.Communication = communication;
                    //nabeel work
                    _CommunicationVM.Communication.IsRead = false;
                    _dbContext.Entry(_CommunicationVM.Communication).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                    var commMeeting = await _dbContext.CommunicationMeetings.FirstOrDefaultAsync(x => x.CommunicationId == communicationId);
                    if (commMeeting != null)
                    {
                        _CommunicationVM.CommunicationMeeting = commMeeting;
                        var commMeetingStatus = await _dbContext.MeetingStatuses.FirstOrDefaultAsync(x => x.MeetingStatusId == commMeeting.StatusId);
                        if (commMeetingStatus != null)
                        {
                            _CommunicationVM.CommunicationMeeting.MeetingStatusEn = commMeetingStatus.NameEn;
                            _CommunicationVM.CommunicationMeeting.MeetingStatusAr = commMeetingStatus.NameAr;
                        }
                    }
                    // Get GE attendees from COMM_COMMUNICATION_ATTENDEES table by using CommunicationId
                    var commMeetingAttendees = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == communicationId).ToListAsync();
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
                            newAttendee.SerialNo = ++atttendeeGeSerialNo;

                            _CommunicationVM.MeetingAttendee.Add(newAttendee);
                        }
                    }
                    // When Fatwa received meeting request from GE, and after added fatwa attendees in approval scenario, then get Fatwa attendees details from MEET_MEETING_ATTENDEE table by using MeetingId
                    var meetMeetingAttendees = await _dbContext.MeetingAttendees.Where(x => x.MeetingId == _dbContext.Meetings.Where(x => x.CommunicationId == communicationId).Select(x => x.MeetingId).FirstOrDefault()).ToListAsync();
                    if (meetMeetingAttendees.Count() != 0)
                    {
                        foreach (var attendee in meetMeetingAttendees)
                        {
                            FatwaAttendeeVM newAttendee = new FatwaAttendeeVM();

                            //var govt = _dbContext.GovernmentEntity.FirstOrDefault(x => x.EntityId == attendee.GovernmentEntityId);
                            //if (govt is not null)
                            //{
                            //    newAttendee.GovernmentEntityId = govt.EntityId;
                            //    newAttendee.GovernmentEntityNameEn = govt.Name_En;
                            //    newAttendee.GovernmentEntityNameAr = govt.Name_Ar;
                            //}

                            var dept = _dbContext.Departments.FirstOrDefault(x => x.Id == attendee.DepartmentId);
                            if (dept is not null)
                            {
                                newAttendee.DepartmentId = dept.Id;
                                newAttendee.DepartmentEnglish = dept.Name_En;
                                newAttendee.DepartmentArabic = dept.Name_Ar;
                            }
                            var recordattendeeStatus = _dbContext.MeetingAttendeeStatuses.FirstOrDefault(x => x.Id == attendee.AttendeeStatusId);
                            if (recordattendeeStatus is not null)
                            {
                                newAttendee.AttendeeStatusId = recordattendeeStatus.Id;
                                newAttendee.AttendeeStatusEn = recordattendeeStatus.NameEn;
                                newAttendee.AttendeeStatusAr = recordattendeeStatus.NameAr;
                            }
                            newAttendee.Id = attendee.AttendeeUserId;
                            newAttendee.FirstNameEnglish = attendee.RepresentativeNameEn;
                            newAttendee.SerialNo = ++fatwaAtttendeeSerialNo;

                            _CommunicationVM.FatwaAttendee.Add(newAttendee);
                        }
                    }
                }
                return _CommunicationVM;
            }
            catch
            {
                throw;
            }
        }
        public async Task<Meeting> GetMeetingDetailByUsingCommunicationId(Guid communicationId)
        {
            var meeting = await _dbContext.Meetings.Where(x => x.CommunicationId == communicationId).FirstOrDefaultAsync();
            if (meeting != null)
            {
                return meeting;

            }
            return new Meeting();
        }
        public async Task<string> StopExecutionRejectionReason(StopExecutionRejectionReason stopExecutionRejectionReason)
        {
            try
            {
                _dbContext.StopExecutionRejectionReason.Add(stopExecutionRejectionReason);
                await _dbContext.SaveChangesAsync();
                return _dbContext.Communications.Where(x => x.CommunicationId == Guid.Parse(stopExecutionRejectionReason.CommunicationId)).Select(x => x.ReceivedBy).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CommunicationResponseVM> GetCommunicationDetail(string communicationId)
        {
            try
            {
                if (_CommunicationResponse == null)
                {
                    storedProc = $"exec pCommunicationDetail @CommunicationId = '{communicationId}'";
                    var result = await _dbContext.CommunicationResponseVMs.FromSqlRaw(storedProc).ToListAsync();
                    if (result != null)
                        _CommunicationResponse = result.FirstOrDefault();
                }
                return _CommunicationResponse;
            }
            catch
            {
                throw;
            }
        }

        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History> 
        public async Task<List<CommunicationListVM>> GetCommunicationListByCaseRequestId(string caseRequestId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationListByCaseRequest @RequestId = '{caseRequestId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Get Communication Detail By Case Request ID, Case File ID & Case Id
        public async Task<CommunicationListVM> GetCommunicationDetailByCaseRequestId(string caseRequestId, Guid communicationId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationDetailByCaseRequestId @RequestId = '{caseRequestId}', @CommunicationId='{communicationId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CommunicationListVM> GetCommunicationDetailByCaseId(string caseId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationDetailByCaseId @CaseId = N'{caseId}',@CorrespondenceTypeId= N'{CorrespondenceTypeId}',@CommunicationId=N'{communicationId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CommunicationListVM> GetCommunicationDetailByCaseFileId(string fileId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationDetailByCaseFileId @CaseFileId = N'{fileId}',@CorrespondenceTypeId= N'{CorrespondenceTypeId}',@CommunicationId=N'{communicationId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get Communication Details By Consultation File ID & Consultation Request ID
        public async Task<CommunicationListVM> GetCommunicationDetailByConsultationFileId(string fileId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pComsGetCommuncationDetailByConsultationFileId @ConsultationFileId = N'{fileId}',@CorrespondenceTypeId= N'{CorrespondenceTypeId}',@CommunicationId=N'{communicationId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        public async Task<CommunicationListVM> GetCommunicationDetailByConsultationRequestId(string consultationRequestId, string communicationId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pComsGetCommuncationDetailByConsultationRequestId @RequestId = '{consultationRequestId}', @CommunicationId='{communicationId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public async Task<List<CommunicationListVM>> GetCommunicationListByConsultationRequestId(string consultationRequestId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationListByConsultationRequest @RequestId = '{consultationRequestId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History> 
        public async Task<List<CommunicationListVM>> GetCommunicationListByCaseId(string caseId, int CorrespondenceTypeId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationListByCase @CaseId = N'{caseId}',@CorrespondenceTypeId= N'{CorrespondenceTypeId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.ToList();
            }
            catch
            {

                throw;

            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-02-01' Version="1.0" Branch="master"> Get Inbox Outbox List</History> 
        public async Task<List<CommunicationInboxOutboxVM>> GetInboxOutboxList(int correspondenceType, string userName, int PageSize, int PageNumber, int channelId)
        {
            try
            {
                if (_inboxOutboxListVM == null)
                {
                    storedProc = $"exec pInboxOutboxList @correspondenceType='{correspondenceType}', @userName='{userName}', @PageSize='{PageSize}', @PageNumber='{PageNumber}'";
                    _inboxOutboxListVM = await _dbContext.CommunicationInboxOutboxVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _inboxOutboxListVM.ToList();
            }
            catch
            {

                throw;

            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-02-10' Version="1.0" Branch="master"> Get Inbox Outbox Need More Info Detail</History> 
        public async Task<CmsCaseRequestResponseVM> GetInboxOutboxRequestNeedMoreDetail(Guid CommunicationId)
        {
            try
            {

                if (_CmsCaseRequestResponseVMs == null)
                {
                    string StoredProc = $"exec pInboxOutboxRequestNeedMoreDetail @CommunicationId = N'{CommunicationId}', @CommunicationType = N'{(int)CommunicationTypeEnum.RequestMoreInfo}'";
                    _CmsCaseRequestResponseVMs = await _dbContext.CmsCaseRequestResponseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestResponseVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Muhammad Hassan'  Version="1.0" Branch="master"> </History> 
        public async Task<List<CommunicationListVM>> GetConslutationFileCommunication(Guid fileId, int CorrespondenceTypeId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationListByConsultationFile @ConsultationFileId = N'{fileId}', @CorrespondenceTypeId = N'{CorrespondenceTypeId}'";
                    //
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.ToList();
            }
            catch
            {

                throw;

            }
        }

        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History> 
        public async Task<List<CommunicationListVM>> GetCommunicationListByCaseFileId(string fileId, int CorrespondenceTypeId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    // Get communication against CaseFile
                    storedProc = $"exec pCommuncationListByCaseFile @CaseFileId = N'{fileId}',@CorrespondenceTypeId= N'{CorrespondenceTypeId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<CommunicationListVM> GetCommunicationDetailByCommunicationId(string fileId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                if (_communicationListVM == null)
                {
                    storedProc = $"exec pCommuncationDetailByCommunicationId @CaseFileId = N'{fileId}', @CorrespondenceTypeId= N'{CorrespondenceTypeId}', @CommunicationId=N'{communicationId}'";
                    _communicationListVM = await _dbContext.CommunicationListVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationListVM.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Request For More Information
        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        public async Task SaveCommunicationResponse(CommunicationResponseMoreInfoVM communicationRequestMore)
        {
            try
            {
                bool isSaved;
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            isSaved = await SaveCommunication(communicationRequestMore.Communication, _dbContext);
                            isSaved = await SaveCommResponse(communicationRequestMore.CommunicationResponse, communicationRequestMore.Communication.CommunicationId, _dbContext);
                            isSaved = await SaveCommunicationTargetLink(communicationRequestMore.CommunicationTargetLink, _dbContext);
                            isSaved = await SaveLinkTarget(communicationRequestMore.LinkTarget, communicationRequestMore.CommunicationTargetLink.TargetLinkId, _dbContext);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Communication Send Response Detail By Communication by Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-01-09' Version="1.0" Branch="master"> Get Send Response detail by Id </History> 
        public async Task<CommunicationSendResponseVM> CommunicationSendResponseDetailbyId(string CommunicationId)
        {
            try
            {
                storedProc = $"exec pCommSendResponseDetailbyId @CommunicationId = N'{CommunicationId}', " +
                            $"@CommunicationType = N'{(int)CommunicationTypeEnum.SendResponse}'";
                var result = await _dbContext.communicationSendResponseVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result is not null)
                    return result.FirstOrDefault();
                return null;
            }
            catch
            {
                return null;
                throw;
            }
        }
        public async Task<CommunicationSendResponseVM> CommunicationSendResponseDetailbyId(string CommunicationId, int CommunicationType)
        {
            try
            {
                storedProc = $"exec pCommSendResponseDetailbyId @CommunicationId = N'{CommunicationId}', " +
                            $"@CommunicationType = N'{CommunicationType}'";
                var result = await _dbContext.communicationSendResponseVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result is not null)
                    return result.FirstOrDefault();
                return null;
            }
            catch
            {
                return null;
                throw;
            }
        }
        public async Task<CommunicationDetailVM> CommunicationDetailbyComIdAndComType(string ReferenceId, string CommunicationId, int SubModuleId, int CommunicationTypeId)
        {
            try
            {
                if (ReferenceId == null)
                {
                    ReferenceId = Guid.Empty.ToString();
                }
                storedProc = $"exec pCommunicationDetailbyComId @ReferenceId = N'{ReferenceId}', @CommunicationId = N'{CommunicationId}', @SubModuleId = N'{SubModuleId}'," +
                            $"@CommunicationType = N'{CommunicationTypeId}'";
                var result = await _dbContext.communicationDetailVM.FromSqlRaw(storedProc).ToListAsync();
                if (result != null && result.Count > 0)
                {
                    var comm = await _dbContext.Communications.FindAsync(Guid.Parse(CommunicationId));
                    comm.IsRead = false;
                    _dbContext.Entry(comm).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    var stopExeReason = _dbContext.StopExecutionRejectionReason.Where(x => x.CommunicationId == CommunicationId).Select(x => x.Reason).FirstOrDefault();
                    if (stopExeReason != null)
                        result.FirstOrDefault().StopExeRejectionReason = stopExeReason;
                    await _dbContext.SaveChangesAsync();
                    return result.FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Communication Send Response Detail By Communication by Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-01-09' Version="1.0" Branch="master"> Get Send Response detail by Id </History> 


        public async Task<CommunicationSendMessageVM> CommunicationSendMessageDetailbyId(string CommunicationId)
        {
            try
            {
                if (_communicationSendMessageVMs == null)
                {
                    storedProc = $"exec pCommSendMessageDetailbyId @CommunicationId = N'{CommunicationId}',@CommunicationType = N'{(int)CommunicationTypeEnum.SendMessage}'";
                    _communicationSendMessageVMs = await _dbContext.communicationSendMessageVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationSendMessageVMs.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get Meeting Id Throug Communication by Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-01-09' Version="1.0" Branch="master"> Get Send Response detail by Id </History> 


        public async Task<CommunicationMeetingDetailVM> GetMeetingIdCommunitationbyId(string CommunicationId, int CommunicationTypeId)
        {
            try
            {
                if (_communicationMeetingDetailVMs == null)
                {
                    storedProc = $"exec pGetMeetingIdbyCommId @CommunicationId = N'{CommunicationId}',@CommunicationType = N'{CommunicationTypeId}'";
                    _communicationMeetingDetailVMs = await _dbContext.CommunicationMeetingDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationMeetingDetailVMs.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get Meeting list Id Throug Communication by Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-01-09' Version="1.0" Branch="master"> Get Send Response detail by Id </History> 


        public async Task<List<CommunicationMeetingDetailVM>> GetMeetinglistCommunitationbyId(string CommunicationId)
        {
            try
            {
                if (_communicationMeetingDetailVMs == null)
                {
                    storedProc = $"exec pGetMeetingIdbyCommId @CommunicationId = N'{CommunicationId}',@CommunicationType = N'{(int)CommunicationTypeEnum.RequestForMeeting}'";
                    _communicationMeetingDetailVMs = await _dbContext.CommunicationMeetingDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _communicationMeetingDetailVMs.ToList();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get Sector HOS
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        public async Task<User> GetHOSBySectorId(int sectorTypeId, DatabaseContext dbcontext)
        {
            try
            {
                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await dbcontext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Task Communication
        public async Task<TaskCommunication> GetTaskCommunication(SendCommunicationVM sendCommunicationVM)
        {
            TaskCommunication taskCommunication = new TaskCommunication();
            taskCommunication.Linktarget = sendCommunicationVM.LinkTarget.Where(x => x.IsPrimary == true).FirstOrDefault();
            var receivedby = await GetRecieverName(sendCommunicationVM);
            if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.SendMessage || sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
            {
                if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.SendMessage)
                {
                    taskCommunication.TaskName = "Send_Message";
                    taskCommunication.FirstUrl = "request";
                    taskCommunication.SecondUrl = "need-more-detail";
                    taskCommunication.ThirdUrl = taskCommunication.Linktarget.ReferenceId.ToString() + "/" + sendCommunicationVM.Communication.CommunicationId.ToString() + "/" + taskCommunication.Linktarget.LinkTargetTypeId + "/" + sendCommunicationVM.Communication.CommunicationTypeId;
                 
                        var fatwaUser = await _IRole.GetHOSByFileAndLinkTargetTypeId(taskCommunication.Linktarget);
                    
                    //var fatwauser = await _IRole.GetHOSBySectorId(sendCommunicationVM.Communication.SectorTypeId);
                    taskCommunication.AssignedTo = fatwaUser.Id;
                }
                else
                {
                    taskCommunication.TaskName = "Request_For_Meeting";
                    taskCommunication.FirstUrl = "request-meeting";
                    taskCommunication.SecondUrl = "detail";

                    receivedby = System.Net.WebUtility.UrlEncode(receivedby).Replace(".", "%999");
                    taskCommunication.ThirdUrl = sendCommunicationVM.Communication.CommunicationId + "/" + taskCommunication.Linktarget.ReferenceId.ToString() + "/" + taskCommunication.Linktarget.LinkTargetTypeId + "/" + receivedby;
                    //taskCommunication.ThirdUrl = sendCommunicationVM.Communication.CommunicationId + "/" + taskCommunication.Linktarget.ReferenceId.ToString() + "/" + (int)CommunicationTypeEnum.RequestForMeeting + "/" + true + "/" + taskCommunication.Linktarget.LinkTargetTypeId;
                    var fatwaUser = await _IRole.GetHOSByFileAndLinkTargetTypeId(taskCommunication.Linktarget);
                    taskCommunication.AssignedTo = fatwaUser.Id;
                }

            }
            else if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.SendResponse
                || sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.InterpretationofJudgment
                || sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.InvalidityofJudgment
                || sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.StopExecutionOfJudgment)
            {
                taskCommunication.FirstUrl = "request";
                taskCommunication.SecondUrl = "need-more-detail";
                taskCommunication.ThirdUrl = taskCommunication.Linktarget.ReferenceId.ToString() + "/" + sendCommunicationVM.Communication.CommunicationId.ToString() + "/" + taskCommunication.Linktarget.LinkTargetTypeId + "/" + sendCommunicationVM.Communication.CommunicationTypeId;
                taskCommunication.AssignedTo = await _IAccount.UserIdByUserEmail(sendCommunicationVM.Communication.ReceivedBy);
                if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.SendResponse)
                {
                    taskCommunication.TaskName = "Send_Response";

                }
                else if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.InterpretationofJudgment)
                {
                    taskCommunication.TaskName = "Interpretation_Judgment";
                }
                else if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.InvalidityofJudgment)
                {
                    taskCommunication.TaskName = "Invalidity_Judgment";
                }
                else
                {
                    taskCommunication.TaskName = "Request_For_Stop_Execution_Judgment";
                }
            }
            else if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.AcceptSaveAndCloseFile || sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.RejectSaveAndCloseFile)
            {
                taskCommunication.FirstUrl = "request";
                taskCommunication.SecondUrl = "need-more-detail";
                taskCommunication.ThirdUrl = taskCommunication.Linktarget.ReferenceId.ToString() + "/" + sendCommunicationVM.Communication.CommunicationId.ToString() + "/" + taskCommunication.Linktarget.LinkTargetTypeId + "/" + sendCommunicationVM.Communication.CommunicationTypeId;
                taskCommunication.AssignedTo = await _IAccount.UserIdByUserEmail(sendCommunicationVM.Communication.ReceivedBy);
                taskCommunication.TaskName = sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.AcceptSaveAndCloseFile ? "Accept_save_and_close_File" : "Reject_save_and_close_File";
            }
            return taskCommunication;
        }
        #endregion
        public async Task<string> GetRecieverName(SendCommunicationVM sendCommunicationVM)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var receivedby = "";

            var linktarget = sendCommunicationVM.LinkTarget.Where(x => x.IsPrimary == true).FirstOrDefault();
            if (linktarget.LinkTargetTypeId == (int)LinkTargetTypeEnum.CaseRequest)
            {
                var caseRequest = await _dbContext.CaseRequests.Where(x => x.RequestId == linktarget.ReferenceId).FirstOrDefaultAsync();
                sendCommunicationVM.NotificationParameter.Entity = new CaseRequest().GetType().Name;
                sendCommunicationVM.NotificationParameter.ReferenceNumber = caseRequest.RequestNumber;
                receivedby = caseRequest.CreatedBy;
            }
            if (linktarget.LinkTargetTypeId == (int)LinkTargetTypeEnum.File)
            {
                var file = await _dbContext.CaseFiles.Where(x => x.FileId == linktarget.ReferenceId).FirstOrDefaultAsync();
                receivedby = await _dbContext.CaseRequests.Where(x => x.RequestId == file.RequestId).Select(x => x.CreatedBy).FirstOrDefaultAsync();
                sendCommunicationVM.NotificationParameter.Entity = new CaseFile().GetType().Name;
                sendCommunicationVM.NotificationParameter.ReferenceNumber = file.FileNumber;
            }
            if (linktarget.LinkTargetTypeId == (int)LinkTargetTypeEnum.RegisteredCase)
            {
                var registeredCase = await _dbContext.CmsRegisteredCases.Where(x => x.CaseId == linktarget.ReferenceId).FirstOrDefaultAsync();
                var requestid = await _dbContext.CaseFiles.Where(x => x.FileId == registeredCase.FileId).Select(x => x.RequestId).FirstOrDefaultAsync();
                receivedby = await _dbContext.CaseRequests.Where(x => x.RequestId == requestid).Select(x => x.CreatedBy).FirstOrDefaultAsync();
                sendCommunicationVM.NotificationParameter.Entity = "Case";
                sendCommunicationVM.NotificationParameter.ReferenceNumber = registeredCase.CaseNumber;
            }
            if (linktarget.LinkTargetTypeId == (int)LinkTargetTypeEnum.ConsultationRequest)
            {
                var consultationRequest = await _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == linktarget.ReferenceId).FirstOrDefaultAsync();
                sendCommunicationVM.NotificationParameter.Entity = new ConsultationRequest().GetType().Name;
                sendCommunicationVM.NotificationParameter.ReferenceNumber = consultationRequest.RequestNumber;
                receivedby = consultationRequest.CreatedBy;
            }
            if (linktarget.LinkTargetTypeId == (int)LinkTargetTypeEnum.ConsultationFile)
            {
                var consultationFile = await _dbContext.ConsultationFiles.Where(x => x.FileId == linktarget.ReferenceId).FirstOrDefaultAsync();
                receivedby = await _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == consultationFile.RequestId).Select(x => x.CreatedBy).FirstOrDefaultAsync();
                sendCommunicationVM.NotificationParameter.Entity = new ConsultationFile().GetType().Name;
                sendCommunicationVM.NotificationParameter.ReferenceNumber = consultationFile.FileNumber;
            }
            return receivedby;

        }
        #region GetCommunicationMeetingDetailCommunicationId
        public async Task<SendCommunicationVM> GetCommunicationMeetingDetailCommunicationId(Guid communicationId)
        {
            try
            {
                var communication = await _dbContext.Communications.FirstOrDefaultAsync(x => x.CommunicationId == communicationId);
                if (communication != null)
                {
                    communicationMeetingVM.Communication = communication;

                    var commMeeting = await _dbContext.CommunicationMeetings.FirstOrDefaultAsync(x => x.CommunicationId == communicationId);
                    if (commMeeting != null)
                    {
                        communicationMeetingVM.CommunicationMeeting = commMeeting;
                        var commMeetingStatus = await _dbContext.MeetingStatuses.FirstOrDefaultAsync(x => x.MeetingStatusId == commMeeting.StatusId);
                        if (commMeetingStatus != null)
                        {
                            communicationMeetingVM.CommunicationMeeting.MeetingStatusEn = commMeetingStatus.NameEn;
                            communicationMeetingVM.CommunicationMeeting.MeetingStatusAr = commMeetingStatus.NameAr;
                        }
                    }
                    List<CommunicationAttendeeVM> geAattendees = await GetCommunicationMeetingAttendeeByCommunicationId(communicationId);
                    if (geAattendees != null)
                    {
                        communicationMeetingVM.CommunicationAttendee = geAattendees;
                    }
                    var commMeetingAttendees = await _dbContext.CommunicationAttendees.Where(x => x.CommunicationId == communicationId).ToListAsync();
                    if (commMeetingAttendees != null)
                    {
                        foreach (var attendee in commMeetingAttendees)
                        {
                            CommunicationAttendeeVM newAttendee = new CommunicationAttendeeVM();

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

                            //newAttendee.RepresentativeNameEn = attendee.RepresentativeNameEn;
                            //newAttendee.SerialNo = ++atttendeeGeSerialNo;

                            //communicationMeetingVM.CommunicationAttendee.Add(newAttendee);
                        }
                    }
                }
                return communicationMeetingVM;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetCommunicationMeetingAttendeeByCommunicationId
        public async Task<List<CommunicationAttendeeVM>> GetCommunicationMeetingAttendeeByCommunicationId(Guid CommunicationId)
        {
            try
            {
                string storedProc = $"exec pCommunicationMeetingAttendeeByCommunicationId @CommunicationId = N'{CommunicationId}'";
                return await _dbContext.CommunicationAttendeeVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        //<History Author = 'Ijaz Ahmad' Date='2024-03-11' Version="1.0" Branch="master">Announcements List ByCaseId </History> 
        public async Task<List<CmsAnnouncementVM>> GetGetAnnouncementsListByCaseId(string caseId)
        {
            try
            {
                if (_cmsAnnouncementsVMs == null)
                {
                    storedProc = $"exec pCmsAnnouncementsListByCaseId @CaseId = N'{caseId}'";
                    _cmsAnnouncementsVMs = await _dbContext.CmsAnnouncementsVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _cmsAnnouncementsVMs.ToList();
            }
            catch
            {

                throw;

            }
        }
        #region Save Communication For vice hos
        public async Task<CaseRequestCommunicationVM> SaveCommunicationForViceHos(CaseRequestCommunicationVM copyCommunication, User viceHos, DatabaseContext _dbContext, string InboxNo, string InboxFormat)
        {
            CaseRequestCommunicationVM resultInboxNumber = new CaseRequestCommunicationVM();
            try
            {
                // Check the sourceId
                if (copyCommunication.Communication.SourceId == (int)CommunicationSourceEnum.G2G)
                {
                    copyCommunication.Communication.ReceivedBy = viceHos?.Email;
                    UserDetails = await _dbContext.Users.Where(x => x.UserName == copyCommunication.Communication.CreatedBy).FirstOrDefaultAsync();

                    copyCommunication.Communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Inbox;
                    {
                        resultInboxNumber = await UpdateCommunicationInboxInfoReturnCommunicationDetail(copyCommunication, _dbContext, InboxNo, InboxFormat);
                        copyCommunication.Communication.InboxNumber = resultInboxNumber.Communication.InboxNumber;
                        copyCommunication.Communication.InboxNumberFormat = resultInboxNumber.Communication.InboxNumberFormat;
                        copyCommunication.Communication.PatternSequenceResult = resultInboxNumber.Communication.PatternSequenceResult;
                        copyCommunication.Communication.ReferenceNumber = resultInboxNumber.Communication.InboxNumber;
                        copyCommunication.Communication.ReferenceDate = resultInboxNumber.Communication.InboxDate;
                        copyCommunication.Communication.OutboxNumber = null;
                        copyCommunication.Communication.OutboxDate = null;
                        copyCommunication.Communication.ColorId = (int)CommunicationColorEnum.Yellow;
                        copyCommunication.Communication.InboxDate = DateTime.Now;
                    }

                }
                await _dbContext.Communications.AddAsync(copyCommunication.Communication);
                await _dbContext.SaveChangesAsync();

                if (copyCommunication.Communication.PreCommunicationId != null && copyCommunication.Communication.PreCommunicationId != Guid.Empty)
                {
                    var oldCommunication = await _dbContext.Communications.Where(x => x.CommunicationId == copyCommunication.Communication.PreCommunicationId).FirstOrDefaultAsync();
                    if (copyCommunication.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMoreInformationReminder)
                        oldCommunication.IsReminderSent = true;
                    else
                        oldCommunication.IsReplied = true;
                    _dbContext.Communications.Update(oldCommunication);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
            return resultInboxNumber;
        }
        public async Task<CaseRequestCommunicationVM> UpdateCommunicationInboxInfoReturnCommunicationDetail(CaseRequestCommunicationVM sendCommunication, DatabaseContext dbContext, string? inboxNo = null, string? inboxFormat = null)
        {
            NumberPatternResult resultInboxBoxNumber = new NumberPatternResult();
            if (string.IsNullOrEmpty(inboxFormat) && string.IsNullOrEmpty(inboxNo))
            {
                resultInboxBoxNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0,(int)CmsComsNumPatternTypeEnum.InboxNumber, dbContext);
            }
           
            sendCommunication.Communication.InboxNumber = resultInboxBoxNumber.GenerateRequestNumber;
            sendCommunication.Communication.InboxNumberFormat = resultInboxBoxNumber.FormatRequestNumber;
            sendCommunication.Communication.PatternSequenceResult = resultInboxBoxNumber.PatternSequenceResult;
            //sendCommunication.Communication.OutboxShortNum = dbContext.Communications.Any() ? await dbContext.Communications.Select(x => x.OutboxShortNum).MaxAsync() + 1 : 1;
            dbContext.Communications.Update(sendCommunication.Communication);
            return sendCommunication;

        }
        #endregion



        public async Task ForwardCorrespondenceToLawyer(CommunicationHistory communicationHistory)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //_dbContext.Remove(_dbContext.CommunicationRecipients.Single(a => (a.RecipientId == communicationHistory.SentBy) && (a.CommunicationId == communicationHistory.ReferenceId)));
                        foreach (var user in communicationHistory.RecieversId)
                        {
                            var checkExist = await _dbContext.CommunicationRecipients.Where(x => x.RecipientId == Guid.Parse(user) && x.CommunicationId == communicationHistory.ReferenceId).FirstOrDefaultAsync();
                            if (checkExist == null)
                            {
                                communicationHistory.Id = Guid.NewGuid();
                                communicationHistory.SentTo = Guid.Parse(user);
                                communicationHistory.ActionId = (int)CommunicationHistoryEnum.ForwardToLawyer;
                                await _dbContext.CommunicationHistories.AddAsync(communicationHistory);
                                await _dbContext.SaveChangesAsync();
                                CommunicationRecipient recipient = new();
                                recipient.CommunicationId = communicationHistory.ReferenceId;
                                recipient.RecipientId = Guid.Parse(user);
                                recipient.CreatedDate = communicationHistory.CreatedDate;
                                recipient.CreatedBy = communicationHistory.CreatedBy;
                                await _dbContext.CommunicationRecipients.AddAsync(recipient);
                                await _dbContext.SaveChangesAsync();

                            }
                        }
                        var communication = await _dbContext.Communications.Where(x => x.CommunicationId == communicationHistory.ReferenceId).FirstOrDefaultAsync();
                        communicationHistory.NotificationParameter = new();

                        communicationHistory.NotificationParameter.CorrespodenceNumber = communication.InboxNumber;
                        if (communication != null && communication.Archive ==false)
                        {
                            communication.Archive = true;
                            _dbContext.Communications.Update(communication);

                        }
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

        }
        public async Task ForwardCorrespondenceToSector(CommunicationHistory communicationHistory)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{communicationHistory.SectorId}'";
                        var users = await _dbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                        var user = users.FirstOrDefault();
                        communicationHistory.SentTo = Guid.Parse(user.Id);
                        communicationHistory.ActionId = (int)CommunicationHistoryEnum.ForwardToSector;
                        await _dbContext.CommunicationHistories.AddAsync(communicationHistory);
                        await _dbContext.SaveChangesAsync();
                        CommunicationRecipient recipient = new();
                        //var a = await _dbContext.CommunicationRecipients.Where(x => x.RecipientId == communicationHistory.SentBy).FirstOrDefaultAsync();
                        _dbContext.Remove(_dbContext.CommunicationRecipients.Single(a => (a.RecipientId == communicationHistory.SentBy) && (a.CommunicationId == communicationHistory.ReferenceId)));
                        await _dbContext.SaveChangesAsync();

                        recipient.CommunicationId = communicationHistory.ReferenceId;
                        recipient.RecipientId = communicationHistory.SentTo;
                        recipient.CreatedDate = communicationHistory.CreatedDate;
                        recipient.CreatedBy = communicationHistory.CreatedBy;
                        await _dbContext.CommunicationRecipients.AddAsync(recipient);
                        await _dbContext.SaveChangesAsync();
                        var comm = await _dbContext.Communications.Where(x => x.CommunicationId == recipient.CommunicationId).FirstOrDefaultAsync();
                        communicationHistory.NotificationParameter = new();
                        communicationHistory.NotificationParameter.CorrespodenceNumber = comm.InboxNumber;
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }
            }

        }
        public async Task AssignBackToHos(CommunicationHistory communicationHistory)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{communicationHistory.SectorId}'";
                        var users = await _dbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                        var user = users.FirstOrDefault();
                        communicationHistory.SentTo = Guid.Parse(user.Id);
                        communicationHistory.ActionId = (int)CommunicationHistoryEnum.ReturnToHOS;
                        await _dbContext.CommunicationHistories.AddAsync(communicationHistory);
                        await _dbContext.SaveChangesAsync();
                        _dbContext.Remove(_dbContext.CommunicationRecipients.Single(a => (a.RecipientId == communicationHistory.SentBy) && (a.CommunicationId == communicationHistory.ReferenceId)));
                        await _dbContext.SaveChangesAsync();
                        var comm = await _dbContext.Communications.Where(x => x.CommunicationId == communicationHistory.ReferenceId).FirstOrDefaultAsync();
                        communicationHistory.NotificationParameter = new();
                        communicationHistory.NotificationParameter.CorrespodenceNumber = comm.InboxNumber;
                        // CommunicationRecipient recipient = new();

                        //var checkExist = await _dbContext.CommunicationRecipients.Where(x => x.RecipientId == Guid.Parse(communicationHistory.SentTo) && x.CommunicationId == communicationHistory.ReferenceId).FirstOrDefaultAsync();
                        //if (checkExist == null)
                        //{
                        //    recipient.CommunicationId = communicationHistory.ReferenceId;
                        //    recipient.RecipientId = communicationHistory.SentTo;
                        //    recipient.CreatedDate = communicationHistory.CreatedDate;
                        //    recipient.CreatedBy = communicationHistory.CreatedBy;
                        //    await _dbContext.CommunicationRecipients.AddAsync(recipient);
                        //    await _dbContext.SaveChangesAsync();
                        //}

                        transaction.Commit();

                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }


        }
        public async Task SendBackToSender(CommunicationHistory communicationHistory)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                       
                        communicationHistory.SentTo = Guid.Empty;
                        communicationHistory.ActionId = (int)CommunicationHistoryEnum.ReturnToSender;
                        await _dbContext.CommunicationHistories.AddAsync(communicationHistory);
                        await _dbContext.SaveChangesAsync();
                        //var a = await _dbContext.CommunicationRecipients.Where(x => x.RecipientId == communicationHistory.SentBy).FirstOrDefaultAsync();
                        _dbContext.Remove(_dbContext.CommunicationRecipients.Single(a => (a.RecipientId == communicationHistory.SentBy) && (a.CommunicationId == communicationHistory.ReferenceId)));
                        await _dbContext.SaveChangesAsync();
                        var communication = await _dbContext.Communications.Where(x => x.CommunicationId == communicationHistory.ReferenceId).FirstOrDefaultAsync();
                        communication.ReturnCorrespondence = true;
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }


        }
        public async Task<List<CorrespondenceHistoryVM>> GetCorrespondenceHistoryByCommunicationId(Guid CommunicationId)
        {
            try
            {
                
                    storedProc = $"exec pGetCorrespondenceHistoryByCommunicationId @CommunicationId = N'{CommunicationId}'";
                    return await _dbContext.CorrespondenceHistoryVMs.FromSqlRaw(storedProc).ToListAsync();
                
                
            }
            catch (Exception ex) 
            {

                throw ex;

            }
        }
        public async Task<CommunicationRecipient> CheckUserExistByUserAndCommunicationId(Guid userId, Guid communicationId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            return  await _DbContext.CommunicationRecipients.Where(x => x.RecipientId == userId && x.CommunicationId == communicationId).FirstOrDefaultAsync();
        }
        #region Mobile Application

        //<History Author = 'Hassan Abbas' Date='2025-07-02' Version="1.0" Branch="master"> Get Inbox Outbox List For Mobile App</History> 
        public async Task<List<CommunicationInboxOutboxVM>> GetInboxOutboxListForMobileApp(int correspondenceType, string userName, int top, int channelId)
        {
            try
            {
                if (_inboxOutboxListVM == null)
                {
                    if (top > 0)
                        storedProc = $"exec pInboxOutboxListForMobileApp @correspondenceType='{correspondenceType}', @userName='{userName}', @top='{top}'";
                    else
                        storedProc = $"exec pInboxOutboxListForMobileApp @correspondenceType='{correspondenceType}', @userName='{userName}'";

                    _inboxOutboxListVM = await _dbContext.CommunicationInboxOutboxVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _inboxOutboxListVM.ToList();
            }
            catch
            {

                throw;

            }
        }
        public async Task<dynamic> GetInboxOutboxDetailForMobileApp(int communicationTypeId, string communicationId ,int linkTargetTypeId , string referenceId, string CultureType)
        {
            try
            {
                if (communicationTypeId== (int)CommunicationTypeEnum.CaseRequest)
                {
                    storedProc = $"exec pMobileAppCaseRequestDetailsById @CaseRequestId='{referenceId}',@CultureType='{CultureType}'";
                    var data = (await _dbContext.CMSCOMSRequestDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                    if (data != null)
                    {
                        var transformedData = await _ITask.TransformData(data, referenceId);
                        return transformedData;
                    }
                }
                else if (communicationTypeId >= (int)CommunicationTypeEnum.ContractRequest
                 && communicationTypeId <= (int)CommunicationTypeEnum.InternationalArbitrationRequest)
                {
                    storedProc = $"exec pMobileAppConsultationRequestDetailsById @consultationRequestId='{referenceId}',@CultureType='{CultureType}'";
                    var data = (await _dbContext.CMSCOMSRequestDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                    if (data != null)
                    {
                        var transformedData = await _ITask.TransformData(data, referenceId);
                        return transformedData;
                    }
                }
                else if (communicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
                {
                    storedProc = $"exec pMobileAppCommMeetingDetailsById @communicationId='{referenceId}',@CultureType='{CultureType}'";
                    var data = (await _dbContext.MobileAppMeetingDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                    if (data != null)
                    {
                        var transformedData = await _ITask.TransformData(data, referenceId);
                        return transformedData;
                    }
                }
                else if (communicationTypeId == (int)CommunicationTypeEnum.MeetingScheduled)
                {
                    storedProc = $"exec pMobileAppMeetingsDetailsById @MeetingId='{referenceId}',@CultureType='{CultureType}'";
                    var data = ( await _dbContext.MobileAppMeetingDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                    if (data != null)
                    {
                        var transformedData = await _ITask.TransformData(data, referenceId);
                        return transformedData;
                    }
                }
                else if (communicationTypeId == (int)CommunicationTypeEnum.WithdrawRequested)
                {
                    storedProc = $"exec pMobileAppWithdrawRequestDetailsById @WithdrawRequestId='{referenceId}',@CultureType='{CultureType}'";
                    var data = (await _dbContext.WithdrawCMSCOMSRequestDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                    if (data != null)
                    {
                        var transformedData = await _ITask.TransformData(data, referenceId);
                        return transformedData;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(communicationId) && communicationTypeId > 0 && linkTargetTypeId > 0)
                    {
                        storedProc = $"exec pMobileAppCommunicationDetailsbyId @ReferenceId='{referenceId}',@CommunicationType='{communicationTypeId}',@CommunicationId='{communicationId}',@SubModuleId='{linkTargetTypeId}',@CultureType='{CultureType}'";
                        var data = (await _dbContext.CMSCOMSCommunicationDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await _ITask.TransformData(data, communicationId);
                            return transformedData;
                        }
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

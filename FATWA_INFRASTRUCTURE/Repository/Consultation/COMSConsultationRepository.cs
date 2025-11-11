
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces.Consultation;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.CaseManagement;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using FATWA_INFRASTRUCTURE.Repository.G2G;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using G2G_DOMAIN.Models.ViewModel.CommonVMs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static System.Formats.Asn1.AsnWriter;

namespace FATWA_INFRASTRUCTURE.Repository.Consultation
{
    //<!-- <History Author = 'Muhammad Zaeem' Date='2022-1-2' Version="1.0" Branch="master">Create repository for consultation request</History> -->

    public class COMSConsultationRepository : ICOMSConsultation
    {
        #region varaiable declaration
        private List<ConsultationRequestVM> _CmsConsultationRequestVMs;
        private List<ComsWithDrawConsultationRequestVM> _ComswithDrawConsultationRequestVMs;
        private List<ConsultationPartyListVM> _ComConsultationPartyListVMs;
        private ViewConsultationVM _ViewConsultationVM;
        private ComsConsultationRequestResponseVM _ConsultationRequestResponseVM;
        private ConsultationRequest _DetailConsultation;
        private List<ConsultationArticleByConsultationIdListVM> _ConsultationArticleByConsultationIdListVM;
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private List<ComsConsultationRequestHistoryVM> _comsconsultationRequestHistoryVMs;
        private List<ConsultationPartyVM> _ConsultationPartyVMs;
        private readonly CommunicationRepository _communicationRepo;
        private readonly CMSCaseRequestRepository _caseRequestRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        private readonly COMSConsultationFileRepository _comsConsultationFileRepository;
        private readonly G2GRepository _g2gRepository;

        #endregion

        #region Constructor
        public COMSConsultationRepository(DatabaseContext dbContext, DmsDbContext dmsDbContext, CommunicationRepository communicationRepo, IServiceScopeFactory serviceScopeFactory, CMSCOMSInboxOutboxPatternNumberRepository CMSCOMSInboxOutboxPatternNumberRepository, CMSCaseRequestRepository caseRequestRepository, COMSConsultationFileRepository comsConsultationFileRepository)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsDbContext;
            _communicationRepo = communicationRepo;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _caseRequestRepository = caseRequestRepository;
            _cMSCOMSInboxOutboxPatternNumberRepository = CMSCOMSInboxOutboxPatternNumberRepository;
            _comsConsultationFileRepository = comsConsultationFileRepository;
            _g2gRepository = scope.ServiceProvider.GetRequiredService<G2GRepository>();
        }
        #endregion

        #region Get  consultation List
        public async Task<List<ConsultationRequestVM>> GetConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchVM)
        {

            try
            {
                string StoredProc;
                if (_CmsConsultationRequestVMs == null)
                {
                    string requestFrom = advanceSearchVM.RequestFrom != null ? Convert.ToDateTime(advanceSearchVM.RequestFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string requestTo = advanceSearchVM.RequestTo != null ? Convert.ToDateTime(advanceSearchVM.RequestTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    if (advanceSearchVM.userDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector)
                    {
                        StoredProc = $"exec pComsConsultationConfidentialRequestList @requestNumber =N'{advanceSearchVM.RequestNumber}' , @statusId='{advanceSearchVM.StatusId}' , @subject=N'{advanceSearchVM.Subject}',@requestTypeId='{advanceSearchVM.RequestTypeId}' , @requestFrom='{requestFrom}' , @requestTo='{requestTo}', @showUndefinedRequests='{advanceSearchVM.ShowUndefinedRequest}', @sectorTypeId='{advanceSearchVM.SectorTypeId}'" +
                            $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    }
                    else
                    {
                        StoredProc = $"exec pComsConsultationRequestList @requestNumber =N'{advanceSearchVM.RequestNumber}' , @statusId='{advanceSearchVM.StatusId}' , @subject=N'{advanceSearchVM.Subject}', @sectorTypeId='{advanceSearchVM.SectorTypeId}' , @requestFrom='{requestFrom}' , @requestTo='{requestTo}', @showUndefinedRequests='{advanceSearchVM.ShowUndefinedRequest}'" +
                            $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    }
                    _CmsConsultationRequestVMs = await _dbContext.ConsultationRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsConsultationRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation Request By Id
        public async Task<ViewConsultationVM> GetConsultationRequest(Guid consultationId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoredProc = $"exec pConsultationDetailById @consultationId ='{consultationId}'";
                var res = await _DbContext.ViewConsultationVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (res != null)
                {
                    _ViewConsultationVM = res.FirstOrDefault();
                }
                return _ViewConsultationVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation Request By Id
        public async Task<ComsConsultationRequestResponseVM> GetConsultationRequestResponseById(Guid consultationId)
        {
            try
            {
                string StoredProc = $"exec pConsultationRequestNeedMoreDetail @consultationId ='{consultationId}', @CommunicationType = N'{(int)CommunicationTypeEnum.RequestMoreInfo}'";

                var res = await _dbContext.ComsConsultationRequestResponseVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (res != null)
                {
                    _ConsultationRequestResponseVM = res.FirstOrDefault();

                }
                return _ConsultationRequestResponseVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion 

        #region Get Consultation Request By Id
        public async Task<ComsConsultationRequestResponseVM> GetConsultationFileResponseById(Guid consultationId)
        {
            try
            {
                string StoredProc = $"exec pConsultationFileRequestNeedMoreDetail @FileId ='{consultationId}', @CommunicationType = N'{(int)CommunicationTypeEnum.RequestMoreInfo}'";

                var res = await _dbContext.ComsConsultationRequestResponseVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (res != null)
                {
                    _ConsultationRequestResponseVM = res.FirstOrDefault();

                }
                return _ConsultationRequestResponseVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation Request By Id(Main Model)
        public async Task<ConsultationRequest> GetConsultationRequestById(Guid consultationId)
        {
            try
            {
                ConsultationRequest entity = await _dbContext.ConsultationRequests.FindAsync(consultationId);
                if (entity == null)
                {
                    throw new ArgumentNullException();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation PARTY By Id
        public async Task<List<ConsultationPartyListVM>> GetConsultationPartyByConsultationId(Guid consultationId)
        {
            try
            {
                string StoredProc = $"exec pConsultationPartyList @consultationId ='{consultationId}'";
                _ComConsultationPartyListVMs = await _dbContext.ConsultationPartyVMs.FromSqlRaw(StoredProc).ToListAsync();

                return _ComConsultationPartyListVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation ARTICLE By Id
        public async Task<List<ConsultationArticleByConsultationIdListVM>> GetConsultationArticleByConsultationId(Guid consultationId)
        {
            try
            {
                string StoredProc = $"exec pConsultationArticleByConsultationId @consultationId ='{consultationId}'";
                _ConsultationArticleByConsultationIdListVM = await _dbContext.ConsultationArticleByConsultationIdListVMs.FromSqlRaw(StoredProc).ToListAsync();

                return _ConsultationArticleByConsultationIdListVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get consultation request status history by Id
        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  consultation Requests detail by Id  </History>

        public async Task<List<ComsConsultationRequestHistoryVM>> GetCOMSConsultationRequestStatusHistory(string ConsultationRequestId)
        {
            try
            {
                if (_comsconsultationRequestHistoryVMs == null)
                {
                    string StoredProc = $"exec pGetConsultationRequestStatusHistory @ConsultationRequestId = N'{ConsultationRequestId}'";
                    _comsconsultationRequestHistoryVMs = await _dbContext.ComsConsultationRequestHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _comsconsultationRequestHistoryVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region save case request status history

        public async Task<ComsConsultationRequestHistory> SaveConsultationRequestStatusHistory(string userName, ConsultationRequest consultationRequest, int EventId, DatabaseContext dbContext)
        {
            try
            {
                ComsConsultationRequestHistory historyobj = new ComsConsultationRequestHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.ConsultationRequestId = consultationRequest.ConsultationRequestId;
                historyobj.StatusId = (int)consultationRequest.RequestStatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = userName;
                historyobj.EventId = EventId;
                historyobj.Remarks = consultationRequest.Remarks;
                await dbContext.ComsConsultationRequestHistories.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
                return historyobj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update consultation Request Transfer Status
        public async Task<bool> UpdateConsultationRequestTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId)
        {
            bool isSaved = false;

            try
            {

                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                ConsultationRequest consultationRequest = await _DbContext.ConsultationRequests.FirstOrDefaultAsync(x => x.ConsultationRequestId == approvalTracking.ReferenceId);
                if (consultationRequest is not null)
                {
                    consultationRequest.TransferStatusId = taskStatusId;
                    consultationRequest.ModifiedBy = approvalTracking.CreatedBy;
                    consultationRequest.ModifiedDate = DateTime.Now;

                    _DbContext.ConsultationRequests.Update(consultationRequest);
                    await _DbContext.SaveChangesAsync();
                    await _caseRequestRepository.SaveTransferHistory(approvalTracking, taskStatusId, approvalTracking.TransferCaseType, _DbContext);
                    isSaved = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isSaved;
        }

        #endregion

        #region update Consultation request  
        public async Task UpdateConsultationRequest(ConsultationRequest consultationRequest, DatabaseContext dbContext)
        {
            try
            {
                dbContext.ConsultationRequests.Update(consultationRequest);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region update consultation request status 

        public async Task UpdateConsultationRequestStatus(string Username, Guid RequestId, int StatusId, int EventId, DatabaseContext dbContext)
        {
            try
            {
                var req = await dbContext.ConsultationRequests.FindAsync(RequestId);
                req.RequestStatusId = StatusId;
                dbContext.ConsultationRequests.Update(req);
                await dbContext.SaveChangesAsync();
                await SaveConsultationRequestStatusHistory(Username, req, EventId, dbContext);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region CopyConsultationPartiesFromSourceToDestination
        public async Task<List<CopyAttachmentVM>> CopyConsultationPartiesFromSourceToDestination(Guid sourceId, Guid destinationId, string username, DatabaseContext dbContext)
        {
            try
            {
                List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
                var requestParties = await dbContext.ConsultationParties.Where(p => p.ConsultationRequestId == sourceId).ToListAsync();
                foreach (var party in requestParties)
                {
                    ConsultationParty fileParty = party;
                    var partyId = party.PartyId;
                    fileParty.PartyId = Guid.NewGuid();
                    fileParty.ConsultationRequestId = destinationId;
                    fileParty.CreatedBy = username;
                    fileParty.CreatedDate = DateTime.Now;
                    await dbContext.ConsultationParties.AddAsync(fileParty);
                    await dbContext.SaveChangesAsync();

                    copyAttachments.Add(
                    new CopyAttachmentVM()
                    {
                        SourceId = partyId,
                        DestinationId = fileParty.PartyId,
                        CreatedBy = username
                    });

                }
                return copyAttachments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region CopyConsultationAttachmentsFromSourceToDestination
        public async Task CopyConsultationAttachmentsFromSourceToDestination(Guid sourceId, Guid destinationId, string username, DmsDbContext dmsDbContext)
        {
            try
            {
                var requestDocs = await dmsDbContext.UploadedDocuments.Where(p => p.ReferenceGuid == sourceId).ToListAsync();
                foreach (var reqDoc in requestDocs)
                {
                    UploadedDocument fileDoc = reqDoc;
                    fileDoc.UploadedDocumentId = 0;
                    fileDoc.ReferenceGuid = destinationId;
                    fileDoc.CreatedBy = username;
                    fileDoc.CreatedDateTime = DateTime.Now;
                    await dmsDbContext.UploadedDocuments.AddAsync(fileDoc);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Create Consultation  file 
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Create Case File </History>
        //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Generate File Number from Pattern </History>
        public async Task<ConsultationFile> CreateConsultationfile(Guid RequestId, string UserId, int? FatwaPriorityId, DatabaseContext dbContext, int StatusId)
        {
            try
            {
                ConsultationRequest consultreq = await dbContext.ConsultationRequests.FindAsync(RequestId);
                GovernmentEntity entity = await dbContext.GovernmentEntity.FindAsync(consultreq.GovtEntityId);
                ConsultationFile consultationfile = new ConsultationFile();
                var resultConsultationFileNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber);
                consultationfile.FileNumber = resultConsultationFileNumber.GenerateRequestNumber;
                consultationfile.ComsFileNumberFormat = resultConsultationFileNumber.FormatRequestNumber;
                consultationfile.PatternSequenceResult = resultConsultationFileNumber.PatternSequenceResult;
                consultationfile.FileName = consultationfile.FileNumber + "_" + entity?.Name_En + "_" + DateOnly.FromDateTime(DateTime.Now).ToString();
                consultationfile.RequestId = consultreq.ConsultationRequestId;
                consultationfile.CreatedBy = UserId;
                consultationfile.CreatedDate = DateTime.Now;
                consultationfile.StatusId = StatusId;
                consultationfile.SectorTypeId = consultreq.SectorTypeId;
                consultationfile.FatwaPriorityId = FatwaPriorityId;

                await dbContext.ConsultationFiles.AddAsync(consultationfile);
                await dbContext.SaveChangesAsync();
                await SaveConsultationFileSectorAssignment(consultationfile.FileId, consultationfile.CreatedBy, consultationfile.SectorTypeId, dbContext);
                return consultationfile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Consultation File Sector Assignment
        //<History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master"> Save Case File Sector Assignment</History>
        public async Task SaveConsultationFileSectorAssignment(Guid FileId, string CreatedBy, int? SectorTypeId, DatabaseContext dbContext)
        {
            try
            {
                ComsConsultationFileSectorAssignment assignmentobj = new ComsConsultationFileSectorAssignment();
                assignmentobj.FileId = FileId;
                assignmentobj.SectorTypeId = (int)SectorTypeId;
                assignmentobj.CreatedBy = CreatedBy;
                assignmentobj.CreatedDate = DateTime.Now;
                await dbContext.ComsConsultationFileSectorAssignments.AddAsync(assignmentobj);
                await dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region File Repository


        #region Update Consultation File Transfer Status

        public async Task<bool> UpdateConsultationFileTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId)
        {
            bool isSaved = false;

            try
            {

                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                ConsultationFile consultationFile = await _DbContext.ConsultationFiles.FirstOrDefaultAsync(x => x.FileId == approvalTracking.ReferenceId);
                if (consultationFile is not null)
                {
                    consultationFile.TransferStatusId = taskStatusId;
                    consultationFile.ModifiedBy = approvalTracking.CreatedBy;
                    consultationFile.ModifiedDate = DateTime.Now;
                    _DbContext.ConsultationFiles.Update(consultationFile);
                    await _DbContext.SaveChangesAsync();
                    await _caseRequestRepository.SaveTransferHistory(approvalTracking, taskStatusId, approvalTracking.TransferCaseType, _DbContext);
                    isSaved = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isSaved;
        }

        #endregion

        #region save Consultation File status history
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Save Case File Status History</History>
        public async Task SaveConsultationFileStatusHistory(ConsultationFile consultationFile, int EventId, DatabaseContext dbContext)
        {
            try
            {
                ConsultationFileHistory historyobj = new ConsultationFileHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.FileId = consultationFile.FileId;
                historyobj.StatusId = consultationFile.StatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = consultationFile.CreatedBy;
                historyobj.EventId = EventId;
                await dbContext.ConsultationFileHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Save Case File Sector AssignmentFile
        public async Task SaveConsultationFileSectorAssignmentFile(Guid fileId, int sectorTypeId, string username, DatabaseContext dbContext)
        {
            try
            {
                ComsConsultationFileSectorAssignment assignment = await dbContext.ComsConsultationFileSectorAssignments.Where(f => f.FileId == fileId).FirstOrDefaultAsync();
                if (assignment != null)
                {
                    assignment.FileId = fileId;
                    assignment.SectorTypeId = sectorTypeId;
                    assignment.CreatedBy = username;
                    assignment.CreatedDate = DateTime.Now;
                    dbContext.ComsConsultationFileSectorAssignments.Update(assignment);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Remove Consultation File Sector Assignment
        //<History Author = 'Muhammad Zaeem' Date='2022-1-17' Version="1.0" Branch="master"> Save Consultation File Sector Assignment</History>
        public async Task RemoveConsultationFileSectorAssignment(Guid fileId, int sectorTypeId, string username, DatabaseContext dbContext)
        {
            try
            {
                ComsConsultationFileSectorAssignment assignmentobj = await dbContext.ComsConsultationFileSectorAssignments.Where(f => f.FileId == fileId && f.SectorTypeId == sectorTypeId).FirstOrDefaultAsync();
                if (assignmentobj != null)
                {
                    assignmentobj.DeletedBy = username;
                    assignmentobj.DeletedDate = DateTime.Now;
                    assignmentobj.IsDeleted = true;
                    dbContext.ComsConsultationFileSectorAssignments.Update(assignmentobj);
                    await dbContext.SaveChangesAsync();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region Get consultation party detail by Id

        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  Case party detail by Id  </History>

        public async Task<List<ConsultationPartyVM>> GetCOMSCosnultationPartyDetailById(string Id)
        {
            try
            {
                if (_ConsultationPartyVMs == null)
                {
                    string StoredProc = $"exec pConsultationPartyViewDetail @Id = N'{Id}'";
                    _ConsultationPartyVMs = await _dbContext.ConsultationPartyDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _ConsultationPartyVMs.ToList();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create Request 
        public async Task CreateConsultationRequest(CaseRequestCommunicationVM consultationRequestCommunication)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var resultConsultation = await _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == consultationRequestCommunication.ConsultationRequests.ConsultationRequestId).FirstOrDefaultAsync();
                        if (resultConsultation == null)
                        {
                            var consultationRequest = consultationRequestCommunication.ConsultationRequests;
                            var user = await GetHOSBySectorId(consultationRequest.SectorTypeId);
                            consultationRequest.ReceivedBy = user.Email;
                            await _dbContext.ConsultationRequests.AddAsync(consultationRequest);
                            await _dbContext.SaveChangesAsync();
                            await SaveConsultationRequestStatusHistory(consultationRequestCommunication.ConsultationRequests.CreatedBy, consultationRequestCommunication.ConsultationRequests, (int)CaseRequestEventEnum.Created, _dbContext);
                            if (consultationRequestCommunication.ConsultationRequests.ConsultationPartys.Count() != 0)
                            {
                                await SaveConsultationParties(consultationRequestCommunication.ConsultationRequests.ConsultationRequestId, consultationRequestCommunication.ConsultationRequests.ConsultationPartys, _dbContext);
                            }
                            if (consultationRequestCommunication.ConsultationRequests.ConsultationArticles.Count() != 0)
                            {
                                await SaveConsultationArticles(consultationRequestCommunication.ConsultationRequests.ConsultationRequestId, consultationRequestCommunication.ConsultationRequests.ConsultationArticles, _dbContext);
                            }
                            await _communicationRepo.SaveCommunication(consultationRequestCommunication.Communication, _dbContext);
                            await _communicationRepo.SaveCommunicationTargetLink(consultationRequestCommunication.CommunicationTargetLink, _dbContext);
                            await _communicationRepo.SaveLinkTarget(consultationRequestCommunication.LinkTarget, consultationRequestCommunication.CommunicationTargetLink.TargetLinkId, _dbContext);
                            await UpdateConsultationRequestCommuniationId(consultationRequestCommunication.Communication, consultationRequestCommunication.ConsultationRequests, _dmsDbContext);

                            transaction.Commit();
                            // for Notification
                            consultationRequest.NotificationParameter.RequestNumber = consultationRequestCommunication.ConsultationRequests.RequestNumber;
                            consultationRequest.NotificationParameter.Entity = "ConsultationRequest";
                            consultationRequest.NotificationParameter.RequestType = _dbContext.RequestTypes.Where(x => x.Id == consultationRequestCommunication.ConsultationRequests.RequestTypeId).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task UpdateConsultationRequestCommuniationId(Communication communication, ConsultationRequest Request, DmsDbContext dmsDbContext)
        {
            try
            {
                var uploadedDocument = await dmsDbContext.UploadedDocuments.Where(x => x.ReferenceNo == communication.ReferenceNumber && x.ReferenceGuid == Request.ConsultationRequestId).FirstOrDefaultAsync();
                if (uploadedDocument != null)
                {
                    uploadedDocument.CommunicationGuid = communication.CommunicationId;
                    dmsDbContext.UploadedDocuments.Update(uploadedDocument);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        private async Task SaveConsultationRequestAttachments(ConsultationRequest consultationRequest, DmsDbContext dmsDbContext)
        {
            try
            {
                //remove existing attachments
                var attachements = dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == consultationRequest.ConsultationRequestId).ToList();
                if (attachements.Count() != 0)
                {
                    foreach (var attachement in attachements)
                    {
                        string filePath = attachement.StoragePath;
                        string basePath = Path.Combine(Directory.GetCurrentDirectory() + attachement.StoragePath);
                        basePath = basePath.Replace("G2G_API", "G2G_WEB");

                        if (File.Exists(basePath))
                        {
                            File.Delete(basePath);
                        }
                        dmsDbContext.UploadedDocuments.Remove(attachement);
                    }
                    await dmsDbContext.SaveChangesAsync();
                }
                else
                {
                    //add new attachments
                    await dmsDbContext.UploadedDocuments.AddRangeAsync(consultationRequest.UploadedDocuments?.Select(x => { x.UploadedDocumentId = 0; return x; }).ToList());
                    await dmsDbContext.SaveChangesAsync();
                }
            }

            catch (Exception e)
            {
                throw new NotImplementedException();
            }
        }

        private async Task SaveConsultationArticles(Guid consultationRequestId, IList<ConsultationArticle> consultationArticles, DatabaseContext dbContext)
        {
            try
            {
                //remove existing articles

                dbContext.ConsultationArticles.RemoveRange(dbContext.ConsultationArticles.Where(p => p.ConsultationRequestId == consultationRequestId));

                //add new articles
                foreach (var articles in consultationArticles)
                {
                    await dbContext.ConsultationArticles.AddAsync(articles);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task SaveConsultationParties(Guid consultationRequestId, IList<ConsultationParty> consultationPartys, DatabaseContext dbContext)
        {
            try
            {
                //remove existing parties

                dbContext.ConsultationParties.RemoveRange(dbContext.ConsultationParties.Where(p => p.ConsultationRequestId == consultationRequestId));

                //add new parties
                foreach (var party in consultationPartys)
                {
                    await dbContext.ConsultationParties.AddAsync(party);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        //private async Task SaveConsultationRequestStatusHistory(ConsultationRequest consultationRequest, int EventId, DatabaseContext dbContext)
        //{
        //    try
        //    {
        //        ConsultationRequestHistory historyobj = new ConsultationRequestHistory();
        //        historyobj.HistoryId = Guid.NewGuid();
        //        historyobj.ConsultationRequestId = consultationRequest.ConsultationRequestId;
        //        historyobj.RequestStatusId = (int)consultationRequest.RequestStatusId;
        //        if (EventId == (int)CaseRequestEventEnum.Created)
        //        {
        //            historyobj.CreatedDate = consultationRequest.CreatedDate;
        //            historyobj.CreatedBy = consultationRequest.CreatedBy;
        //        }
        //        else if (EventId == (int)CaseRequestEventEnum.Edited || EventId == (int)CaseRequestEventEnum.Withdrawn)
        //        {
        //            historyobj.CreatedDate = (DateTime)consultationRequest.ModifiedDate;
        //            historyobj.CreatedBy = consultationRequest.ModifiedBy;
        //        }
        //        historyobj.EventId = EventId;
        //        await dbContext.ConsultationRequestHistories.AddAsync(historyobj);
        //        await dbContext.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        #endregion

        #region Get WithDraw Consultation Request by RquestId

        //<History Author = 'Muhammad Zaeem' Date='2023-02-15' Version="1.0" Branch="master"> Get with Draw consultation Request By RequestId</History>
        public async Task<List<ComsWithDrawConsultationRequestVM>> GetWithDrawConsultationRequestByRequestId(Guid requestId)
        {
            try
            {
                if (_ComswithDrawConsultationRequestVMs == null)
                {
                    string StoredProc = $"exec pComsRequestWithDrawListByRequestId @requestId ='{requestId}' ";

                    _ComswithDrawConsultationRequestVMs = await _dbContext.ComsWithDrawConsultationRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _ComswithDrawConsultationRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Request 
        //<History Author = 'Umer Zaman' Date='2023-01-18' Version="1.0" Branch="master">Update Consultation Request</History>

        public async Task UpdateConsultationRequest(CaseRequestCommunicationVM consultationRequestCommunication)
        {
            if (_dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == consultationRequestCommunication.ConsultationRequests.ConsultationRequestId).Any())
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.ConsultationRequests.Update(consultationRequestCommunication.ConsultationRequests);
                            await _dbContext.SaveChangesAsync();
                            await SaveConsultationRequestStatusHistory(consultationRequestCommunication.ConsultationRequests.ModifiedBy, consultationRequestCommunication.ConsultationRequests, (int)CaseRequestEventEnum.Edited, _dbContext);
                            if (consultationRequestCommunication.ConsultationRequests.ConsultationPartys.Count() != 0)
                            {
                                await SaveConsultationParties(consultationRequestCommunication.ConsultationRequests.ConsultationRequestId, consultationRequestCommunication.ConsultationRequests.ConsultationPartys, _dbContext);
                            }
                            if (consultationRequestCommunication.ConsultationRequests.ConsultationArticles.Count() != 0)
                            {
                                await SaveConsultationArticles(consultationRequestCommunication.ConsultationRequests.ConsultationRequestId, consultationRequestCommunication.ConsultationRequests.ConsultationArticles, _dbContext);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            else
            {
                await CreateConsultationRequest(consultationRequestCommunication);
            }
        }
        #endregion


        #region withdraw consultation Request 
        public async Task CreateConsultationWithDrawRequest(WithdrawRequestCommunicationVM cmsWithdrawRequestCommunication)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.ComsWithdrawRequests.AddAsync(cmsWithdrawRequestCommunication.ComsWithdrawRequest);
                        await _dbContext.SaveChangesAsync();

                        var resultConsultationRequest = await ConsultationRequestRecordUpdate(cmsWithdrawRequestCommunication.ComsWithdrawRequest, _dbContext);
                        if (resultConsultationRequest != null)
                        {
                            await SaveConsultationRequestStatusHistory(resultConsultationRequest.ModifiedBy, resultConsultationRequest, (int)CaseRequestEventEnum.Withdraw, _dbContext);
                        }
                        //await SaveAttachmentsByWithdrawConsultationRequest(comsWithdrawRequest.UploadedDocument, _dbContext);
                        // change file status if exist
                        await ChangeConsultationFileStatus(cmsWithdrawRequestCommunication.ComsWithdrawRequest.ConsultationRequestId, _dbContext);
                        await _communicationRepo.SaveCommunication(cmsWithdrawRequestCommunication.Communication, _dbContext);
                        await _communicationRepo.SaveCommunicationTargetLink(cmsWithdrawRequestCommunication.CommunicationTargetLink, _dbContext);
                        await _communicationRepo.SaveLinkTarget(cmsWithdrawRequestCommunication.LinkTarget, cmsWithdrawRequestCommunication.CommunicationTargetLink.TargetLinkId, _dbContext);
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        private async Task ChangeConsultationFileStatus(Guid consultationRequestId, DatabaseContext dbContext)
        {
            try
            {
                ConsultationFile consultationFile = await dbContext.ConsultationFiles.Where(x => x.RequestId == consultationRequestId).FirstOrDefaultAsync();
                if (consultationFile != null)
                {
                    consultationFile.StatusId = (int)CaseFileStatusEnum.WithdrawRequested;
                    dbContext.Entry(consultationFile).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }


        private async Task<ConsultationRequest> ConsultationRequestRecordUpdate(ComsWithdrawRequest comsWithdrawRequest, DatabaseContext dbContext)
        {
            try
            {
                ConsultationRequest consultationRequest = await dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == comsWithdrawRequest.ConsultationRequestId).FirstOrDefaultAsync();
                if (consultationRequest != null)
                {
                    consultationRequest.ModifiedBy = comsWithdrawRequest.RequestedBy;
                    consultationRequest.ModifiedDate = comsWithdrawRequest.RequestedDate;
                    consultationRequest.RequestStatusId = (int)CaseRequestStatusEnum.WithdrawRequested;
                    dbContext.Entry(consultationRequest).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return consultationRequest;
                }
                return new ConsultationRequest();
            }
            catch (Exception ex)
            {
                return new ConsultationRequest();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Send A Copy
        public async Task SendACopyConsultationRequest(ConsultationRequest oldConsultationRequest)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var newConsultationRequest = await CopyConsultationRequest(oldConsultationRequest, _dbContext);
                            await CopyConsultationAttachmentsFromSourceToDestination(oldConsultationRequest.ConsultationRequestId, newConsultationRequest.ConsultationRequestId, oldConsultationRequest.CreatedBy, _dmsDbContext);
                            await SaveConsultationRequestStatusHistory(oldConsultationRequest.CreatedBy, oldConsultationRequest, (int)CaseRequestEventEnum.SentCopy, _dbContext);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ConsultationRequest> CopyConsultationRequest(ConsultationRequest oldConsultationRequest, DatabaseContext dbContext)
        {
            try
            {
                var newRequest = await dbContext.ConsultationRequests.FindAsync(oldConsultationRequest.ConsultationRequestId);
                newRequest.SectorTypeId = oldConsultationRequest.SectorTypeId;
                //newRequest.RequestNumber = dbContext.ConsultationRequests.Any() ? await dbContext.ConsultationRequests.Select(x => x.RequestNumber).MaxAsync() + 1 : 1;
                var reqnum = await GetNewConsultationRequestNumber(dbContext);
                newRequest.RequestNumber = reqnum;
                newRequest.ConsultationRequestId = Guid.NewGuid();
                newRequest.RequestStatusId = (int)CaseRequestStatusEnum.Submitted;
                newRequest.CreatedBy = oldConsultationRequest.CreatedBy;
                newRequest.CreatedDate = DateTime.Now;
                await dbContext.ConsultationRequests.AddAsync(newRequest);
                await dbContext.SaveChangesAsync();
                await SaveConsultationRequestStatusHistory(newRequest.CreatedBy, newRequest, (int)CaseRequestEventEnum.ReceivedCopy, dbContext);
                return newRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> GetNewConsultationRequestNumber(DatabaseContext dbContext)
        {
            try
            {
                if (dbContext.ConsultationRequests.Any())
                {
                    string currentLatestRequestNumber = await dbContext.ConsultationRequests.Select(x => x.RequestNumber).MaxAsync();
                    string UpdatedRequestNumber = await IncreseRequestNumber(currentLatestRequestNumber, 1);
                    return UpdatedRequestNumber;
                }
                else
                {
                    return "CO1";
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<string> IncreseRequestNumber(string name, int minNumericalCharacters)
        {
            var prefix = System.Text.RegularExpressions.Regex.Match(name, @"\d+$");
            if (prefix.Success)
            {
                var capture = prefix.Captures[0];
                int number = int.Parse(capture.Value) + 1;
                name = name.Remove(capture.Index, capture.Length) + number.ToString("D" + minNumericalCharacters);
            }
            return name;
        }
        #endregion

        #region Update Withdraw Consultation Request status

        public async Task<List<UpdateEntityHistoryVM>> UpdateWithdrawConsultationRequestStatus(WithdrawRequestDetailVM request, bool isRejected)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        List<UpdateEntityHistoryVM> listOfHistoryObjs = new List<UpdateEntityHistoryVM>();
                        dynamic returnHistoryObject = null;
                        ComsWithdrawRequest comsWithdrawRequest = _dbContext.ComsWithdrawRequests.Where(x => x.ConsultationRequestId == request.ReferenceGuid && x.Id == request.WithdrawRequestId).FirstOrDefault();
                        if (comsWithdrawRequest != null)
                        {
                            if (isRejected == false)
                                comsWithdrawRequest.RequestStatusId = (int)WithdrawRequestStatusEnum.WithdrawnByGE;
                            else
                                comsWithdrawRequest.RequestStatusId = (int)WithdrawRequestStatusEnum.Rejected;

                            comsWithdrawRequest.ApprovedBy = request.ModifiedBy;
                            comsWithdrawRequest.ApprovedDate = request.ModifiedDate;
                            comsWithdrawRequest.RejectionReason = request.RejectionReason;
                            _dbContext.Entry(comsWithdrawRequest).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            if (isRejected == false) //For WithDraw Acceptance
                            {
                                ConsultationRequest consultationRequest = _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == request.ReferenceGuid).FirstOrDefault();
                                if (consultationRequest != null)
                                {
                                    consultationRequest.RequestStatusId = (int)CaseRequestStatusEnum.WithdrawnByGE;
                                    consultationRequest.ModifiedBy = request.ModifiedBy;
                                    consultationRequest.ModifiedDate = request.ModifiedDate;

                                    _dbContext.Entry(consultationRequest).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                    returnHistoryObject = await SaveConsultationRequestStatusHistory(request.CreatedBy, consultationRequest, (int)CaseRequestEventEnum.Withdrawn, _dbContext);
                                    listOfHistoryObjs.Add(PrepareEntityHistory((int)SubModuleEnum.ConsultationRequest, returnHistoryObject, ""));
                                }
                            }
                            else//For withdraw Rejection 
                            {
                                ConsultationRequest consultationRequest = _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == request.ReferenceGuid).FirstOrDefault();
                                if (consultationRequest != null)
                                {
                                    var requestSecondlasthistory = _dbContext.ComsConsultationRequestHistories.Where(x => x.ConsultationRequestId == request.ReferenceGuid).OrderByDescending(x => x.CreatedDate).Skip(1).FirstOrDefault();
                                    var requestStatusId = _dbContext.ComsConsultationRequestHistories.Where(x => x.HistoryId == requestSecondlasthistory.HistoryId).Select(x => x.StatusId).FirstOrDefault();
                                    consultationRequest.RequestStatusId = (int)requestStatusId;
                                    consultationRequest.ModifiedBy = request.ModifiedBy;
                                    consultationRequest.ModifiedDate = request.ModifiedDate;
                                    _dbContext.Entry(consultationRequest).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                    returnHistoryObject = await SaveConsultationRequestStatusHistory(request.CreatedBy, consultationRequest, (int)CaseRequestEventEnum.WithdrawRejected, _dbContext);
                                    listOfHistoryObjs.Add(PrepareEntityHistory((int)SubModuleEnum.ConsultationRequest, returnHistoryObject, request.RejectionReason));
                                }
                            }
                        }
                        transaction.Commit();
                        return listOfHistoryObjs;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public UpdateEntityHistoryVM PrepareEntityHistory(int SubModuleId, dynamic result, string reason)
        {
            try
            {
                UpdateEntityHistoryVM updateEntityHistory = new UpdateEntityHistoryVM();
                updateEntityHistory.ReferenceId = result.ConsultationRequestId;
                updateEntityHistory.HistoryId = result.HistoryId;
                updateEntityHistory.EventId = result.EventId;
                updateEntityHistory.StatusId = result.StatusId;
                updateEntityHistory.Remarks = reason;
                updateEntityHistory.CreatedBy = result.CreatedBy;
                updateEntityHistory.CreatedDate = result.CreatedDate;
                updateEntityHistory.SubModuleId = SubModuleId;
                return updateEntityHistory;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Update ConsultationRequest 

        //<History Author = 'Nadia Gull' Date='2023-01-13' Version="1.0" Branch="master"> Update CaseRequest / CaseFile Status</History>
        public async Task UpdateComsEntityStatus(UpdateEntityStatusVM updateEntity, DatabaseContext dbContext)
        {
            try
            {
                var consultationRequest = await dbContext.ConsultationRequests.FindAsync(updateEntity.ReferenceId);
                consultationRequest.RequestStatusId = updateEntity.StatusId;
                consultationRequest.ModifiedBy = updateEntity.ModifiedBy;
                consultationRequest.ModifiedDate = updateEntity.ModifiedDate;
                dbContext.ConsultationRequests.Update(consultationRequest);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Update withdraw request table reason

        private async Task<ComsWithdrawRequest> UpdateWithdrawRequestReasonTable(ComsWithDrawConsultationRequestVM request, DatabaseContext dbContext)
        {
            try
            {
                ComsWithdrawRequest? Obj = new ComsWithdrawRequest();
                Obj.Id = Guid.NewGuid();
                Obj.ConsultationRequestId = request.ConsultationRequestId;
                Obj.Reason = request.Reason;
                Obj.RequestStatusId = (int)WithdrawRequestStatusEnum.Rejected;
                if (!string.IsNullOrWhiteSpace(request.ModifiedBy.ToString()))
                {
                    Obj.RejectedBy = request.ModifiedBy;
                }
                else
                {
                    Obj.RejectedBy = request.CreatedBy;
                }
                Obj.RejectedDate = DateTime.Now;
                Obj.IsDeleted = false;
                await dbContext.ComsWithdrawRequests.AddAsync(Obj);
                await dbContext.SaveChangesAsync();
                return Obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Article Status
        public async Task<List<ConsultationArticleStatus>> GetArticleStatusList()
        {
            try
            {
                return await _dbContext.ConsultationArticleStatuses.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> GetArticleNewNumber(Guid consultationRequestId)
        {
            try
            {
                if (_dbContext.ConsultationArticles.Count() > 0)
                {
                    return await _dbContext.ConsultationArticles.Where(x => x.ConsultationRequestId == consultationRequestId).Select(x => x.ArticleNumber).MaxAsync() + 1;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get consultation request details by using consultationrequestid
        public async Task<ConsultationRequest> GetConsultationRequestByReferenceId(Guid consultationRequestId)
        {
            try
            {
                var consultationResult = await _dbContext.ConsultationRequests.FindAsync(consultationRequestId);
                if (consultationResult != null)
                {
                    var resultParty = await GetConsultationPartyDetail(consultationResult.ConsultationRequestId, _dbContext);
                    if (resultParty.Count() != 0)
                    {
                        consultationResult.ConsultationPartys = resultParty;
                    }
                    var resultArticle = await GetConsultationArticleDetail(consultationResult.ConsultationRequestId, _dbContext);
                    if (resultParty.Count() != 0)
                    {
                        consultationResult.ConsultationArticles = resultArticle;
                    }
                    return consultationResult;
                }
                return new ConsultationRequest();
            }
            catch (Exception ex)
            {
                return new ConsultationRequest();
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ConsultationArticle>> GetConsultationArticleDetail(Guid consultationRequestId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.ConsultationArticles.Where(x => x.ConsultationRequestId == consultationRequestId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<ConsultationArticle>();
            }
            catch (Exception ex)
            {
                return new List<ConsultationArticle>();
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ConsultationParty>> GetConsultationPartyDetail(Guid consultationRequestId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.ConsultationParties.Where(x => x.ConsultationRequestId == consultationRequestId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<ConsultationParty>();
            }
            catch (Exception ex)
            {
                return new List<ConsultationParty>();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get section list
        public async Task<List<ConsultationSection>> GetSectionList()
        {
            try
            {
                return await _dbContext.ConsultationSections.OrderBy(u => u.SectionId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region section parent list get
        public async Task<List<ConsultationSection>> GetSectionParentList()
        {
            try
            {
                var task = await _dbContext.ConsultationSections.OrderByDescending(x => x.SectionId).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get consultation file detail by reference ID
        public async Task<ConsultationFile> GetConsultationFileDetailsByReferenceId(Guid fileId)
        {
            try
            {
                var consultationfileIdResult = await _dbContext.ConsultationFiles.FindAsync(fileId);
                if (consultationfileIdResult != null)
                {
                    return consultationfileIdResult;
                }
                return new ConsultationFile();
            }
            catch (Exception ex)
            {
                return new ConsultationFile();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Sector HOS
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        public async Task<User> GetHOSBySectorId(int? sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Consultation  Request list by Sector
        //<History Author = 'ijaz Ahmad' Date='2023-08-02' Version="1.0" Branch="master">Get all Consultation Requests by Sector</History>
        public async Task<List<ConsultationRequestDmsVM>> GetAllConsultationBySectorTypeId(int sectorTypeId)
        {
            try
            {
                List<ConsultationRequestDmsVM> _consultationRequests;
                string StoredProc = $"exec pComsConsultationRequestListForDms @sectorTypeId ='{sectorTypeId}' ";
                _consultationRequests = await _dbContext.ConsultationRequestDmsVMs.FromSqlRaw(StoredProc).ToListAsync();
                return _consultationRequests;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Contract Template
        //<History Author = 'Umer Zaman' Date='2023-01-02' Version="1.0" Branch="master">Populate contract template details</History>

        public async Task<List<ConsultationTemplate>> GetConsultationTemplate()
        {
            try
            {
                var resultTemplate = await _dbContext.ConsultationTemplates.OrderBy(x => x.TemplateId).ToListAsync();
                if (resultTemplate.Count() != 0)
                {
                    return resultTemplate;
                }
                return new List<ConsultationTemplate>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ConsultationTemplateSection>> GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(int templateId)
        {
            try
            {
                List<ConsultationTemplateSection>? ObjSection = new List<ConsultationTemplateSection>();
                var resultTemplateSectionDetails = await _dbContext.ConsultationTemplateTemplateSections.Where(x => x.TemplateId == templateId).ToListAsync();
                if (resultTemplateSectionDetails.Count() != 0)
                {
                    foreach (var item in resultTemplateSectionDetails)
                    {
                        var resultSectionModel = await _dbContext.ConsultationTemplateSections.Where(x => x.TemplateSectionId == item.TemplateSectionId).FirstOrDefaultAsync();
                        if (resultSectionModel != null)
                        {
                            ObjSection.Add(resultSectionModel);
                        }
                    }
                    return ObjSection;
                }
                return new List<ConsultationTemplateSection>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Create Consultation Request From Fatwa
        public async Task<CaseRequestCommunicationVM> CreateConsultationRequestFromFatwa(ConsultationRequest consultationRequest)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CaseRequestCommunicationVM consultationRequestCommunication = new CaseRequestCommunicationVM();
                        if (consultationRequest.RequestStatusId != (int)CaseRequestStatusEnum.Draft)
                        {
                            consultationRequest.ApprovedBy =  consultationRequest.ReviewedBy = consultationRequest.AssignedBy;
                        }
                        if (consultationRequest.IsEdit == false)
                        {
                            var result = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern((int)consultationRequest.GovtEntityId, (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber);
                            consultationRequest.RequestNumber = result.GenerateRequestNumber;
                            consultationRequest.RequestNumberFormat = result.FormatRequestNumber;
                            consultationRequest.PatternSequenceResult = result.PatternSequenceResult;
                            await _dbContext.ConsultationRequests.AddAsync(consultationRequest);

                        }
                        else
                        {
                            _dbContext.ConsultationRequests.Update(consultationRequest);

                        }
                        await _dbContext.SaveChangesAsync();
                        await SaveConsultationRequestStatusHistory(consultationRequest.CreatedBy, consultationRequest, (int)CaseRequestEventEnum.Created, _dbContext);
                        // For G2G to Fatwa Communication
                        if (consultationRequest.RequestStatusId != (int)CaseRequestStatusEnum.Draft)
                        {
                            consultationRequestCommunication = await SaveCommunication(_dbContext, consultationRequest);
                            consultationRequestCommunication.ConsultationRequests = consultationRequest;
                            consultationRequestCommunication = await CreateConsultationFileFromFatwa(consultationRequestCommunication, _dbContext);
                            if(consultationRequest.GovtEntityId != null)
                            {
                                var g2gUser = await _g2gRepository.GetNextGEUserForRequestAssignment((int)consultationRequest.GovtEntityId, false);
                                if (!string.IsNullOrEmpty(g2gUser.Item1))
                                {
                                    consultationRequest.CreatedBy = g2gUser.Item1;
                                    consultationRequest.DepartmentId = g2gUser.Item2;
                                    _dbContext.ConsultationRequests.Update(consultationRequest);
                                }
                            }
                        }
                        transaction.Commit();
                        return consultationRequestCommunication;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Save Communication
        private async Task<CaseRequestCommunicationVM> SaveCommunication(DatabaseContext dbContext, ConsultationRequest consultationRequest)
        {
            try
            {
                Communication communication = new Communication();
                communication.CommunicationId = new Guid();
                if (consultationRequest.RequestTypeId == (int)RequestTypeEnum.Contracts)
                {
                    communication.CommunicationTypeId = (int)CommunicationTypeEnum.ContractRequest;
                    communication.Title = "Create_Contract_Request";
                }
                if (consultationRequest.RequestTypeId == (int)RequestTypeEnum.Legislations)
                {
                    communication.CommunicationTypeId = (int)CommunicationTypeEnum.LegislationRequest;
                    communication.Title = "Create_legislation_Request";
                }
                if (consultationRequest.RequestTypeId == (int)RequestTypeEnum.LegalAdvice)
                {
                    communication.CommunicationTypeId = (int)CommunicationTypeEnum.LegalAdviceRequest;
                    communication.Title = "Create_Legal_Advice_Request";
                }
                if (consultationRequest.RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
                {
                    communication.CommunicationTypeId = (int)CommunicationTypeEnum.AdministrativeComplaintRequest;
                    communication.Title = "Create_Administrative_Complaints_Request";
                }
                if (consultationRequest.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration)
                {
                    communication.CommunicationTypeId = (int)CommunicationTypeEnum.InternationalArbitrationRequest;
                    communication.Title = "Create_International_Arbitration_Request";
                }
                if (consultationRequest.RequestTypeId == (int)RequestTypeEnum.Contracts)
                {
                    communication.OutboxNumber = consultationRequest.MandatoryTempFiles.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.OfficialLetter).Select(x => x.ReferenceNo).First();
                    communication.OutboxDate = consultationRequest.MandatoryTempFiles.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.OfficialLetter).Select(x => x.ReferenceDate).First();
                }
                else
                {
                    communication.OutboxNumber = consultationRequest.MandatoryTempFiles.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.OfficialLetter).Select(x => x.ReferenceNo).First();
                    communication.OutboxDate = consultationRequest.MandatoryTempFiles.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.OfficialLetter).Select(x => x.ReferenceDate).First();
                }
                communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
                communication.CreatedBy = consultationRequest.CreatedBy;
                communication.CreatedDate = consultationRequest.CreatedDate;
                communication.SectorTypeId = (int)consultationRequest.SectorTypeId;
                communication.GovtEntityId = consultationRequest.GovtEntityId;
                communication.SourceId = (int)CommunicationSourceEnum.FATWA;
                communication.SentBy = consultationRequest.CreatedBy;
                communication.OutboxShortNum = 0;
                await _communicationRepo.SaveCommunication(communication, dbContext);

                CommunicationTargetLink comTargetLink = new CommunicationTargetLink();
                comTargetLink.TargetLinkId = new Guid();
                comTargetLink.CommunicationId = communication.CommunicationId;
                comTargetLink.CreatedBy = consultationRequest.CreatedBy;
                comTargetLink.CreatedDate = consultationRequest.CreatedDate;
                await _communicationRepo.SaveCommunicationTargetLink(comTargetLink, dbContext);

                List<LinkTarget> linkTargets = new List<LinkTarget>();
                LinkTarget linkTarget;
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    IsPrimary = true,
                    ReferenceId = consultationRequest.ConsultationRequestId,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.ConsultationRequest
                };
                linkTargets.Add(linkTarget);
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    IsPrimary = true,
                    ReferenceId = consultationRequest.ConsultationRequestId,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.ConsultationRequest
                };
                linkTargets.Add(linkTarget);
                await _communicationRepo.SaveLinkTarget(linkTargets, comTargetLink.TargetLinkId, dbContext);

                CaseRequestCommunicationVM sendCommunication = new CaseRequestCommunicationVM();
                sendCommunication.Communication = communication;
                sendCommunication.CommunicationTargetLink = comTargetLink;
                sendCommunication.LinkTarget = linkTargets;
                return sendCommunication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Create Consulation File From Fatwa
        protected async Task<CaseRequestCommunicationVM> CreateConsultationFileFromFatwa(CaseRequestCommunicationVM caseRequestCommunicationVM, DatabaseContext dbContext)
        {
            try
            {
                caseRequestCommunicationVM.ConsultationFile = await CreateConsultationfile((Guid)caseRequestCommunicationVM.ConsultationRequests.ConsultationRequestId, caseRequestCommunicationVM.ConsultationRequests.CreatedBy, caseRequestCommunicationVM.ConsultationRequests.PriorityId, dbContext, (int)CaseFileStatusEnum.InProgress);
                await UpdateConsultationRequestStatus(caseRequestCommunicationVM.ConsultationRequests.CreatedBy, caseRequestCommunicationVM.ConsultationRequests.ConsultationRequestId,(int)CaseRequestStatusEnum.ConvertedToFile, (int)CaseRequestEventEnum.Created, dbContext);
                await SaveConsultationFileStatusHistory(caseRequestCommunicationVM.ConsultationFile, (int)CaseFileEventEnum.Created, dbContext);
                caseRequestCommunicationVM.ConsultationFile = caseRequestCommunicationVM.ConsultationFile;
                return caseRequestCommunicationVM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}

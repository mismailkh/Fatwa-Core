using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_INFRASTRUCTURE.Repository.CaseManagement
{
    //<!-- <History Author = 'Nabeel Ur Rehman' Date='2022-08-08' Version="1.0" Branch="master">Create class and add functionality</History> -->
    public class CMSCaseRequestRepository : ICMSCaseRequest
    {
        #region Variables declaration

        private List<CmsCaseRequestVM> _CmsCaseRequestVMs;
        private List<CmsWithDrawCaseRequestVM> _CmswithDrawCaseRequestVMs;
        private List<CaseRequestDetailVM> _CaseRequestDetailVMs;
        private List<CmsCaseRequestResponseVM> _CmsCaseRequestResponseVMs;
        private List<DetailSubCaseVM> _DetailSubCaseVMs;
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private List<CasePartyLinkVM> _CasePartyVMs;
        private List<CmsCaseRequestHistoryVM> _cmsCaseRequestHistoryVMs;

        private readonly CommunicationRepository _communicationRepo;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        private readonly RoleRepository _roleRepo;


        #endregion

        #region Constructor
        public CMSCaseRequestRepository(DatabaseContext dbContext, CommunicationRepository communicationRepo, IServiceScopeFactory serviceScopeFactory, CMSCOMSInboxOutboxPatternNumberRepository CMSCOMSInboxOutboxPatternNumberRepository, DmsDbContext dmsDbContext, RoleRepository roleRepo)
        {
            _dbContext = dbContext;
            _communicationRepo = communicationRepo;
            _serviceScopeFactory = serviceScopeFactory;
            _cMSCOMSInboxOutboxPatternNumberRepository = CMSCOMSInboxOutboxPatternNumberRepository;
            _dmsDbContext = dmsDbContext;
            _roleRepo = roleRepo;
        }
        #endregion                                                                             

        #region Get Case Request list
        //<History Author = 'Danish Tameez' Date='2022-10-22' Version="1.0" Branch="master">Get all Case Request details </History>
        public async Task<List<CmsCaseRequestVM>> GetCMSCaseRequests(AdvanceSearchCmsCaseRequestVM advanceSearchVM)
        {
            try

            {
                if (_CmsCaseRequestVMs == null)
                {
                    string fromDate = advanceSearchVM.RequestFrom != null ? Convert.ToDateTime(advanceSearchVM.RequestFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = advanceSearchVM.RequestTo != null ? Convert.ToDateTime(advanceSearchVM.RequestTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoredProc = $"exec pCmsCaseRequestList @requestNumber ='{advanceSearchVM.RequestNumber}' , @statusId='{advanceSearchVM.StatusId}' , @subject='{advanceSearchVM.Subject}' , @sectorTypeId='{advanceSearchVM.SectorTypeId}' , @requestFrom='{fromDate}' , @requestTo='{toDate}' , @showUndefinedRequests='{advanceSearchVM.ShowUndefinedRequest}', @govEntityId='{advanceSearchVM.GovEntityId}',@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _CmsCaseRequestVMs = await _dbContext.CmsCaseRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion                                        

        #region Get Case Request list by Sector
        //<History Author = 'Hassan Abbas' Date='2023-08-02' Version="1.0" Branch="master">Get all Case Requests by Sector</History>
        public async Task<List<CmsCaseRequestDmsVM>> GetAllCaseRequestsBySectorTypeId(int sectorTypeId)
        {
            try
            {
                List<CmsCaseRequestDmsVM> _caseRequests;
                string StoredProc = $"exec pCmsCaseRequestListForDms @sectorTypeId ='{sectorTypeId}' ";
                _caseRequests = await _dbContext.CmsCaseRequestDmsVMs.FromSqlRaw(StoredProc).ToListAsync();
                return _caseRequests;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Case Request Details View Detail by ID
        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  Case Requests detail by Id  </History>
        public async Task<CaseRequestDetailVM> GetCMSCaseRequestsDetailById(string RequestId, int? channelId)
        {
            try
            {
                if (_CaseRequestDetailVMs == null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    string StoredProc = $"exec pCaseRequestViewDetail @RequestId = N'{RequestId}'";
                    _CaseRequestDetailVMs = await _DbContext.CaseRequestDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CaseRequestDetailVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get case request by Id
        ///Hassan
        public async Task<CaseRequest> GetCaseRequestById(Guid RequestId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                CaseRequest entity = await _DbContext.CaseRequests.FindAsync(RequestId);
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

        #region Create Request 
        public async Task CreateCMSCaseRequest(CaseRequestCommunicationVM caseRequestCommunication)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.CaseRequests.Add(caseRequestCommunication.CaseRequest);
                        await _dbContext.SaveChangesAsync();
                        await SaveCaseRequestStatusHistory(caseRequestCommunication.CaseRequest.CreatedBy, caseRequestCommunication.CaseRequest, (int)CaseRequestEventEnum.Created, _dbContext);
                        await _dbContext.SaveChangesAsync();
                        await SaveCaseParties(_dbContext, caseRequestCommunication.CaseRequest);
                        await _communicationRepo.SaveCommunication(caseRequestCommunication.Communication, _dbContext);
                        await _communicationRepo.SaveCommunicationTargetLink(caseRequestCommunication.CommunicationTargetLink, _dbContext);
                        await _communicationRepo.SaveLinkTarget(caseRequestCommunication.LinkTarget, caseRequestCommunication.CommunicationTargetLink.TargetLinkId, _dbContext);
                        await UpdateCaseRequestCommuniationId(caseRequestCommunication.Communication, caseRequestCommunication.CaseRequest, _dmsDbContext);
                        transaction.Commit();
                        caseRequestCommunication.CaseRequest.NotificationParameter.RequestNumber = caseRequestCommunication.CaseRequest.RequestNumber;
                        caseRequestCommunication.CaseRequest.NotificationParameter.Entity = new CaseRequest().GetType().Name;}
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        public async Task SaveCaseParties(DatabaseContext dbContext, CaseRequest caseRequest)
        {
            try
            {
                //remove existing parties
                dbContext.CasePartyLink.RemoveRange(dbContext.CasePartyLink.Where(p => p.ReferenceGuid == caseRequest.RequestId));

                if (caseRequest.CaseParties != null)
                {
                    if (!caseRequest.CaseParties.Where(p => p.CategoryId == (int)CasePartyCategoryEnum.Plaintiff && p.TypeId == (int)CasePartyTypeEnum.GovernmentEntity && p.EntityId == caseRequest.GovtEntityId).Any())
                    {
                        var geRepresentative = await dbContext.GovernmentEntityRepresentative.Where(x => x.GovtEntityId == caseRequest.GovtEntityId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                        caseRequest.CaseParties.Add(new CasePartyLink { Id = Guid.NewGuid(), ReferenceGuid = caseRequest.RequestId, CategoryId = (int)CasePartyCategoryEnum.Plaintiff, TypeId = (int)CasePartyTypeEnum.GovernmentEntity, EntityId = caseRequest.GovtEntityId, RepresentativeId = geRepresentative?.Id, CreatedBy = caseRequest.CreatedBy, CreatedDate = caseRequest.CreatedDate });
                    }
                }
                foreach (var CaseParty in caseRequest.CaseParties)
                {
                    //add new parties
                    await dbContext.CasePartyLink.AddAsync(CaseParty);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CommunicationForViceHos(CaseRequestCommunicationVM caseRequestCommunication)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (dbContext)
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dynamic request;
                        string entName ="";

                        if(caseRequestCommunication.CaseRequest !=null)
                        {
                            request = caseRequestCommunication.CaseRequest;
                            entName = new CaseRequest().GetType().Name;
                        }
                        else
                        {
                            request = caseRequestCommunication.ConsultationRequests;
                            entName = new ConsultationRequest().GetType().Name;
                        }
                        List<User> Users = await _roleRepo.GetViceHOSBySectorId((int)request.SectorTypeId);
                        string InboxNo = "";
                        string InboxFormat = "";
                        foreach (var user in Users)
                        {
                            CaseRequestCommunicationVM copyCommunication = new CaseRequestCommunicationVM();
                            copyCommunication = caseRequestCommunication;
                            copyCommunication.Communication.CommunicationId = Guid.NewGuid();
                            var resultInboxNumber = await _communicationRepo.SaveCommunicationForViceHos(copyCommunication, user, dbContext, InboxNo, InboxFormat);
                            InboxNo = resultInboxNumber.Communication.InboxNumber;
                            InboxFormat = resultInboxNumber.Communication.InboxNumberFormat;
                            copyCommunication.CommunicationTargetLink.CommunicationId = copyCommunication.Communication.CommunicationId;
                            copyCommunication.CommunicationTargetLink.TargetLinkId = Guid.NewGuid();
                            await _communicationRepo.SaveCommunicationTargetLink(copyCommunication.CommunicationTargetLink, dbContext);
                            copyCommunication.LinkTarget.Where(x => x.IsPrimary == false).ToList().ForEach(y => { y.ReferenceId = copyCommunication.Communication.CommunicationId; y.LinkTargetId = Guid.NewGuid(); y.TargetLinkId = copyCommunication.CommunicationTargetLink.TargetLinkId; });
                            copyCommunication.LinkTarget.Where(x => x.IsPrimary == true).ToList().ForEach(y => { y.LinkTargetId = Guid.NewGuid(); y.TargetLinkId = copyCommunication.CommunicationTargetLink.TargetLinkId; });
                            await _communicationRepo.SaveLinkTarget(copyCommunication.LinkTarget, copyCommunication.CommunicationTargetLink.TargetLinkId, dbContext);
                            request.NotificationParameter.RequestNumber = request.RequestNumber;
                            request.NotificationParameter.Entity = entName;
                        }
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

        #endregion
        //<History Author = 'Nadia Gull' Date='2022-12-14' Version="1.0" Branch="master">Update Case Request</History>
        #region Update Request 
        public async Task UpdatCMSeCaseRequest(CaseRequestCommunicationVM caseRequestCommunication)
        {
            if (_dbContext.CaseRequests.Where(x => x.RequestId == caseRequestCommunication.CaseRequest.RequestId).Any())
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.CaseRequests.Update(caseRequestCommunication.CaseRequest);
                            await _dbContext.SaveChangesAsync();
                            await SaveCaseRequestStatusHistory(caseRequestCommunication.CaseRequest.CreatedBy, caseRequestCommunication.CaseRequest, (int)CaseRequestEventEnum.Edited, _dbContext);
                            await _dbContext.SaveChangesAsync();
                            await SaveCaseParties(_dbContext, caseRequestCommunication.CaseRequest);
                            //await SaveAttachmentsByCaseRequest(_dbContext, caseRequestCommunication.CaseRequest);
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
                await CreateCMSCaseRequest(caseRequestCommunication);
            }
        }
        #endregion

        #region Get Linked Requests By Primary Request
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get Linked Case Requests by Primary Request</History>
        public async Task<List<CmsCaseRequestVM>> GetLinkedRequestsByPrimaryRequestId(Guid RequestId)
        {
            try
            {
                if (_CmsCaseRequestVMs == null)
                {
                    string StoredProc = $"exec pLinkedRequestsByPrimaryRequestId @requestId = N'{RequestId}'";
                    _CmsCaseRequestVMs = await _dbContext.CmsCaseRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Case Request and File Request Detail Need More Detail 
        public async Task<CmsCaseRequestResponseVM> GetCaseRequestResponsebyRequestId(Guid RequestId, Guid CommunicationId)
        {
            try
            {
                if (_CmsCaseRequestResponseVMs == null)
                {
                    string StoredProc = $"exec pCaseRequestNeedMoreDetail @RequestId = N'{RequestId}', @CommunicationType = N'{(int)CommunicationTypeEnum.RequestMoreInfo}',@CommunicationId = N'{CommunicationId}'";
                    _CmsCaseRequestResponseVMs = await _dbContext.CmsCaseRequestResponseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestResponseVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<CmsCaseRequestResponseVM> GetFileRequestNeedMoreDetail(Guid FileId, Guid CommunicationId)
        {
            try
            {

                if (_CmsCaseRequestResponseVMs == null)
                {
                    string StoredProc = $"exec pFileRequestNeedMoreDetail @FileId = N'{FileId}', @CommunicationType = N'{(int)CommunicationTypeEnum.RequestMoreInfo}',@CommunicationId = N'{CommunicationId}'";
                    _CmsCaseRequestResponseVMs = await _dbContext.CmsCaseRequestResponseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestResponseVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Sub Case File Detail 
        public async Task<DetailSubCaseVM> GetSubCaseByCaseId(Guid CaseId)
        {
            try
            {
                if (_DetailSubCaseVMs == null)
                {
                    string StoredProc = $"exec pCaseSubDetail @CaseId = N'{CaseId}'";
                    _DetailSubCaseVMs = await _dbContext.DetailSubCaseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _DetailSubCaseVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get case party detail by Id

        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  Case party detail by Id  </History>

        public async Task<List<CasePartyLinkVM>> GetCMSCasePartyDetailById(string Id)
        {
            try
            {
                if (_CasePartyVMs == null)
                {
                    string StoredProc = $"exec pCasePartyViewDetail @Id = N'{Id}'";
                    _CasePartyVMs = await _dbContext.CasePartyVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CasePartyVMs.ToList();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Ijaz Ahmad' Date='2024-03-08' Version="1.0" Branch="master">Get  Case party detail by Id  </History>
        public async Task<CasePartyLinkVM> GetCasePartyDetailById(string Id)
        {
            try
            {
                if (_CasePartyVMs == null)
                {
                    string StoredProc = $"exec pCasePartyDetail @Id = N'{Id}'";
                    _CasePartyVMs = await _dbContext.CasePartyVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CasePartyVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get case request status history by Id
        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  Case Requests detail by Id  </History>

        public async Task<List<CmsCaseRequestHistoryVM>> GetCMSCaseRequestStatusHistory(string RequestId)
        {
            try
            {
                if (_cmsCaseRequestHistoryVMs == null)
                {
                    string StoredProc = $"exec pGetCaseRequestStatusHistory @RequestId = N'{RequestId}'";
                    _cmsCaseRequestHistoryVMs = await _dbContext.CmsCaseRequestHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _cmsCaseRequestHistoryVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Case File status 

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Update Case File Status</History>
        private async Task UpdateCaseFileStatus(Guid fileId, int StatusId, DatabaseContext dbContext)
        {
            try
            {
                var file = await dbContext.CaseFiles.FindAsync(fileId);
                file.StatusId = StatusId;
                dbContext.CaseFiles.Update(file);
                await dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Send A Copy
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Create a new copy of the Case Request and Send it to another Sector </History>
        //public async Task SendACopyCaseRequest(CaseRequest oldCaseRequest)
        //{
        //    try
        //    {
        //        using (_dbContext)
        //        {
        //            using (var transaction = _dbContext.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    var newCaseRequest = await CopyCaseRequest(oldCaseRequest, _dbContext);
        //                    await CopyCasePartiesFromSourceToDestination(oldCaseRequest.RequestId, newCaseRequest.RequestId, oldCaseRequest.CreatedBy, _dbContext);
        //                    await CopyCaseAttachmentsFromSourceToDestination(oldCaseRequest.RequestId, newCaseRequest.RequestId, oldCaseRequest.CreatedBy, _dbContext);
        //                    await SaveCaseRequestStatusHistory(oldCaseRequest.CreatedBy, oldCaseRequest, (int)CaseRequestEventEnum.SentCopy, _dbContext);
        //                    transaction.Commit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Copy Case Request</History>
        public async Task<CaseRequest> CopyCaseRequest(CaseRequest oldCaseRequest, CmsApprovalTracking approvalTracking, DatabaseContext dbContext)
        {
            try
            {
                Guid oldRquestId = oldCaseRequest.RequestId;
                var newRequest = await dbContext.CaseRequests.Where(x => x.RequestId == oldRquestId).FirstOrDefaultAsync();
                newRequest.RequestId = Guid.NewGuid();
                //newRequest.RequestNumber = "COPY_CMS_DEC19";
                newRequest.ParentRequestId = oldRquestId;
                newRequest.SectorTypeId = oldCaseRequest.SectorTypeId;
                newRequest.StatusId = (int)CaseRequestStatusEnum.Submitted;
                newRequest.CreatedBy = approvalTracking.UserName;
                newRequest.CreatedDate = DateTime.Now;
                await dbContext.CaseRequests.AddAsync(newRequest);
                await dbContext.SaveChangesAsync();
                await SaveCaseRequestStatusHistory(newRequest.CreatedBy, newRequest, (int)CaseRequestEventEnum.ReceivedCopy, dbContext);
                return newRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        #region save case request status history

        public async Task<CmsCaseRequestHistory> SaveCaseRequestStatusHistory(string userName, CaseRequest caseRequest, int EventId, DatabaseContext dbContext)
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
                return historyobj;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region save case File status history

        public async Task<CmsCaseFileStatusHistory> SaveCaseFileStatusHistory(string userName, CaseFile caseFile, int EventId, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileStatusHistory historyobj = new CmsCaseFileStatusHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.FileId = caseFile.FileId;
                historyobj.StatusId = (int)caseFile.StatusId;
                historyobj.CreatedBy = userName;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.EventId = EventId;
                historyobj.Remarks = caseFile.Remarks;
                await dbContext.CmsCaseFileStatusHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
                return historyobj;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region save case request Transfer history

        public async Task SaveTransferHistory(CmsApprovalTracking approvalTracking, int taskStatusId, int subModuleId, DatabaseContext _dbContext)
        {
            try
            {
                CmsTransferHistory historyobj = new CmsTransferHistory();
                historyobj.TransferHistoryId = Guid.NewGuid();
                historyobj.ReferenceId = approvalTracking.ReferenceId;
                historyobj.CreatedBy = approvalTracking.CreatedBy;
                historyobj.SectorFrom = approvalTracking.SectorFrom;
                historyobj.SectorTo = approvalTracking.SectorTo;
                historyobj.Reason = approvalTracking.Remarks;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.SubModuleId = subModuleId;
                historyobj.ApprovalTrackingId = approvalTracking.Id;
                if (taskStatusId == (int)ApprovalStatusEnum.Approved)
                {
                    historyobj.StatusId = approvalTracking.SectorTo;
                    historyobj.CreatedBy = approvalTracking.ModifiedBy;
                }
                else if (taskStatusId == (int)ApprovalStatusEnum.Pending)
                {
                    historyobj.StatusId = (int)ApprovalTrackingStatusEnum.Pending;

                }
                if (taskStatusId == (int)CaseFileStatusEnum.AssignedToRegionalSector)
                {
                    historyobj.StatusId = approvalTracking.SectorTo;
                    historyobj.CreatedBy = approvalTracking.CreatedBy;
                }
                await _dbContext.CmsTransferHistories.AddAsync(historyobj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateTransferHistory(CmsTransferHistoryVM transferHistory)
        {
            var cmsTransferHistory = _dbContext.CmsTransferHistories.Where(x => x.TransferHistoryId == transferHistory.TransferHistoryId).FirstOrDefault();
            cmsTransferHistory.StatusId = transferHistory.StatusId;
            cmsTransferHistory.SectorTo = transferHistory.SectorTo;
            cmsTransferHistory.SectorFrom = transferHistory.SectorFrom;

            _dbContext.CmsTransferHistories.Update(cmsTransferHistory);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region save registered case  status history
        private async Task SaveRegisteredCaseStatusHistory(CmsRegisteredCase registeredCase, int EventId, DatabaseContext dbContext)
        {
            try
            {
                CmsRegisteredCaseStatusHistory historyobj = new CmsRegisteredCaseStatusHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.CaseId = registeredCase.CaseId;
                historyobj.FileId = registeredCase.FileId;
                historyobj.StatusId = (int)registeredCase.StatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = registeredCase.CreatedBy;
                historyobj.EventId = EventId;
                historyobj.Remarks = registeredCase.Remarks;
                await dbContext.CmsRegisteredCaseStatusHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Update Case Request Transfer Status

        public async Task<bool> UpdateCaseRequestTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId, int transferCaseType)
        {
            bool isSaved = false;

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                CaseRequest caseRequest = await _DbContext.CaseRequests.FirstOrDefaultAsync(x => x.RequestId == approvalTracking.ReferenceId);
                if (caseRequest is not null)
                {
                    caseRequest.TransferStatusId = taskStatusId;
                    caseRequest.ModifiedBy = approvalTracking.CreatedBy;
                    caseRequest.ModifiedDate = DateTime.Now;

                    _DbContext.CaseRequests.Update(caseRequest);
                    await _DbContext.SaveChangesAsync();
                    await SaveTransferHistory(approvalTracking, taskStatusId, transferCaseType, _DbContext);

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

        #region update case request status 

        public async Task UpdateCaseRequestStatus(string userName, Guid RequestId, int StatusId, int EventId, DatabaseContext dbContext)
        {
            try
            {
                var req = await dbContext.CaseRequests.FindAsync(RequestId);
                req.StatusId = StatusId;
                dbContext.CaseRequests.Update(req);
                await dbContext.SaveChangesAsync();
                await SaveCaseRequestStatusHistory(userName, req, EventId, dbContext);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region update case request  
        public async Task UpdateCaseRequest(CaseRequest caseRequest, DatabaseContext dbContext)
        {
            try
            {
                dbContext.CaseRequests.Update(caseRequest);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task GetFileNumberPatternbyGroup(CaseAssignment caseAssignment, DatabaseContext dbContext)
        {
            try
            {

                var UserId = await dbContext.CaseFileAssignment.FindAsync(caseAssignment.TaskUserId);

                if (UserId != null)
                {
                    var GroupId = await dbContext.Users.FindAsync(UserId);
                    if (GroupId != null)
                    {
                        //List<CmsComsNumPatternGroups> comsNumPatternGroups = await dbContext.CmsComsNumPatternGroups..FindAsync(GroupId);
                    }
                }
                //  dbContext.CaseRequests.Update(caseRequest);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Update Case Request Communication
        public async Task UpdateCaseRequestCommuniationId(Communication communication, CaseRequest caseRequest, DmsDbContext dmsDbContext)
        {
            try
            {
                var uploadedDocument = await dmsDbContext.UploadedDocuments.Where(x => x.ReferenceNo == communication.ReferenceNumber && x.ReferenceGuid == caseRequest.RequestId).FirstOrDefaultAsync();
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
        #endregion

        #region Create Case file 
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Create Case File </History>
        //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Generate File Number from Pattern </History>
        public async Task<CaseFile> CreateCasefile(Guid RequestId, string UserId, DatabaseContext dbContext, int StatusId, NumberPatternResult resultCaseFileNumber = null )
        {
            try
            {
                CaseFile casefile = new CaseFile();
                //string result = await AddPatternNumberPattrenGroupsofCasefile(UserId);
                //casefile.FileNumber = result;
                CaseRequest casereq = await dbContext.CaseRequests.FindAsync(RequestId);
                GovernmentEntity entity = await dbContext.GovernmentEntity.FindAsync(casereq.GovtEntityId);
                if(resultCaseFileNumber == null)
                    resultCaseFileNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0,(int)CmsComsNumPatternTypeEnum.CaseFileNumber);
                casefile.FileNumber = resultCaseFileNumber.GenerateRequestNumber;
                casefile.CaseFileNumberFormat = resultCaseFileNumber.FormatRequestNumber;
                casefile.PatternSequenceResult = resultCaseFileNumber.PatternSequenceResult;


                casefile.FileName = casefile.FileNumber + "_" + entity?.Name_En + "_" + DateOnly.FromDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                casefile.RequestId = casereq.RequestId;
                casefile.CreatedBy = UserId;
                casefile.CreatedDate = DateTime.Now;
                casefile.StatusId = StatusId;
                casefile.SectorTypeId = casereq.SectorTypeId;
                await dbContext.CaseFiles.AddAsync(casefile);
                await dbContext.SaveChangesAsync();
                await SaveCaseFileSectorAssignment(casefile, dbContext);
                return casefile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Save Case file parties from cAse Request </History>
        //<History Author = 'Hassan Abbas' Date='2022-12-20' Version="1.0" Branch="master"> Modified this Function to handle all cases of Copying from Case Requests/Files/Cases </History>
        public async Task<List<CopyAttachmentVM>> CopyCasePartiesFromSourceToDestination(Guid sourceId, Guid destinationId, string username, DatabaseContext dbContext)
        {
            List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
            try
            {
                var requestParties = await dbContext.CasePartyLink.Where(p => p.ReferenceGuid == sourceId).ToListAsync();
                foreach (var party in requestParties)
                {
                    var partyId = party.Id;
                    CasePartyLink fileParty = party;
                    fileParty.Id = Guid.NewGuid();
                    fileParty.ReferenceGuid = destinationId;
                    fileParty.CreatedBy = username;
                    fileParty.CreatedDate = DateTime.Now;
                    await dbContext.CasePartyLink.AddAsync(fileParty);
                    await dbContext.SaveChangesAsync();

                    copyAttachments.Add(
                        new CopyAttachmentVM()
                        {
                            SourceId = partyId,
                            DestinationId = fileParty.Id,
                            CreatedBy = username
                        });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return copyAttachments;
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Save Case file Documents from cAse Request </History>
        //<History Author = 'Hassan Abbas' Date='2022-12-20' Version="1.0" Branch="master"> Modified this Function to handle all cases of Copying from Case Requests/Files/Cases </History>
        public async Task CopyCaseAttachmentsFromSourceToDestination(Guid sourceId, Guid destinationId, string username, DatabaseContext dbContext, DmsDbContext dmsDbContext)
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
        public async Task<List<CasePartyLink>> CopyCasePartiesFromSourceToDestinationWorkflow(Guid sourceId, Guid destinationId, string username, DatabaseContext dbContext, DmsDbContext dmsDbContext)
        {
            List<CasePartyLink> copyCaseParties = new List<CasePartyLink>();
            try
            {
                var requestParties = await dbContext.CasePartyLink.Where(p => p.ReferenceGuid == sourceId).ToListAsync();
                foreach (var party in requestParties)
                {
                    var partyId = party.Id;
                    CasePartyLink fileParty = party;
                    fileParty.Id = Guid.NewGuid();
                    fileParty.ReferenceGuid = destinationId;
                    fileParty.CreatedBy = username;
                    fileParty.CreatedDate = DateTime.Now;
                    await dbContext.CasePartyLink.AddAsync(fileParty);
                    await dbContext.SaveChangesAsync();

                    //party attachment if any  
                    var requestDocs = await dmsDbContext.UploadedDocuments.Where(p => p.ReferenceGuid == party.Id).ToListAsync();
                    foreach (var reqDoc in requestDocs)
                    {
                        UploadedDocument fileDoc = reqDoc;
                        fileDoc.UploadedDocumentId = 0;
                        fileDoc.ReferenceGuid = fileParty.Id;
                        await dmsDbContext.UploadedDocuments.AddAsync(fileDoc);
                        await dmsDbContext.SaveChangesAsync();
                    }
                    copyCaseParties.Add(fileParty);

                }
                return copyCaseParties;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CopyCaseAttachmentsFromSourceToDestinationWorkflow(Guid sourceId, Guid destinationId, string username, DmsDbContext dmsDbContext)
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

        #region Save Case File Sector Assignment
        //<History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master"> Save Case File Sector Assignment</History>
        public async Task SaveCaseFileSectorAssignment(CaseFile file, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileSectorAssignment assignmentobj = new CmsCaseFileSectorAssignment();
                assignmentobj.FileId = file.FileId;
                assignmentobj.SectorTypeId = (int)file.SectorTypeId;
                assignmentobj.CreatedBy = file.CreatedBy;
                assignmentobj.CreatedDate = DateTime.Now;
                await dbContext.CmsCaseFileSectorAssignment.AddAsync(assignmentobj);
                await dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Link Case Requests with Primary Request

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link Selected Case Requests with Primary Request </History>
        public async Task LinkCaseRequests(LinkCaseRequestsVM linkCaseRequest)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            CaseRequest primaryRequest = await UpdatePrimaryCaseRequest(linkCaseRequest.PrimaryRequestId, _dbContext);
                            //Link selected Secondary Requests
                            foreach (var request in linkCaseRequest.LinkedRequests)
                            {
                                CaseRequest caseRequest = await UpdateLinkedCaseRequest(request.RequestId, _dbContext);
                                await SaveCaseRequestStatusHistory(linkCaseRequest.CreatedBy, caseRequest, (int)CaseRequestEventEnum.Linked, _dbContext);
                                await LinkCaseRequestWithPrimaryRequest(linkCaseRequest, request.RequestId, _dbContext);
                                if (primaryRequest.StatusId == (int)CaseRequestStatusEnum.ConvertedToFile)
                                {
                                    //Copy parties and docs to File
                                    CaseFile caseFile = await _dbContext.CaseFiles.Where(f => f.RequestId == primaryRequest.RequestId).FirstOrDefaultAsync();
                                    await CopyCasePartiesFromSourceToDestination(request.RequestId, caseFile.FileId, linkCaseRequest.CreatedBy, _dbContext);
                                    await CopyCaseAttachmentsFromSourceToDestination(request.RequestId, caseFile.FileId, linkCaseRequest.CreatedBy, _dbContext, _dmsDbContext);
                                }
                                //Update selected Linked Request if its a parent and also update parent of its childs
                                await ProcessChildsOfLinkedRequest(linkCaseRequest, primaryRequest, request.RequestId, _dbContext);
                            }
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Process Child Requests Of Linked Requests </History>
        private async Task ProcessChildsOfLinkedRequest(LinkCaseRequestsVM linkCaseRequest, CaseRequest newPrimaryRequest, Guid oldPrimaryRequestId, DatabaseContext dbContext)
        {
            try
            {
                List<CmsCaseRequestVM> linkedRequestsOfSecondaryRequest = await GetLinkedRequestsByPrimaryRequestId(oldPrimaryRequestId);
                foreach (var childRequest in linkedRequestsOfSecondaryRequest)
                {
                    await UpdatePrimaryRequestOfLinkedRequest(linkCaseRequest, newPrimaryRequest, childRequest, dbContext);
                    if (newPrimaryRequest.StatusId == (int)CaseRequestStatusEnum.ConvertedToFile)
                    {
                        //Copy parties and docs to File
                        CaseFile caseFile = await _dbContext.CaseFiles.Where(f => f.RequestId == newPrimaryRequest.RequestId).FirstOrDefaultAsync();
                        await CopyCasePartiesFromSourceToDestination(childRequest.RequestId, caseFile.FileId, linkCaseRequest.CreatedBy, _dbContext);
                        await CopyCaseAttachmentsFromSourceToDestination(childRequest.RequestId, caseFile.FileId, linkCaseRequest.CreatedBy, _dbContext, _dmsDbContext);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Update Link between Primary and Linked Requests </History>
        private async Task UpdatePrimaryRequestOfLinkedRequest(LinkCaseRequestsVM linkCaseRequest, CaseRequest newPrimaryRequest, CmsCaseRequestVM childRequest, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseRequestLinkedRequest caseRequestLinkedRequest = await dbContext.CaseRequestLinkedRequests.Where(r => r.LinkedRequestId == childRequest.RequestId).FirstOrDefaultAsync();
                caseRequestLinkedRequest.PrimaryRequestId = newPrimaryRequest.RequestId;
                caseRequestLinkedRequest.CreatedBy = linkCaseRequest.CreatedBy;
                caseRequestLinkedRequest.CreatedDate = DateTime.Now;
                dbContext.CaseRequestLinkedRequests.Update(caseRequestLinkedRequest);
                await dbContext.SaveChangesAsync();
                //Update child request and save status history
                CaseRequest caseRequest = await dbContext.CaseRequests.FindAsync(childRequest.RequestId);
                await SaveCaseRequestStatusHistory(linkCaseRequest.CreatedBy, caseRequest, (int)CaseRequestEventEnum.Linked, dbContext);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Create Link between Primary and Linked Requests </History>
        public async Task LinkCaseRequestWithPrimaryRequest(LinkCaseRequestsVM linkCaseRequest, Guid linkedRequestId, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseRequestLinkedRequest caseRequestLinkedRequest = new CmsCaseRequestLinkedRequest
                {
                    PrimaryRequestId = linkCaseRequest.PrimaryRequestId,
                    LinkedRequestId = linkedRequestId,
                    CreatedBy = linkCaseRequest.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                await dbContext.CaseRequestLinkedRequests.AddAsync(caseRequestLinkedRequest);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Update Linked Case Request</History>
        public async Task<CaseRequest> UpdateLinkedCaseRequest(Guid linkedRequestId, DatabaseContext dbContext)
        {
            try
            {
                CaseRequest caseRequest = await dbContext.CaseRequests.FindAsync(linkedRequestId);
                caseRequest.IsLinked = true;
                caseRequest.IsPrimary = false;
                dbContext.CaseRequests.Update(caseRequest);
                await dbContext.SaveChangesAsync();
                return caseRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Update Primary Case Request</History>
        private async Task<CaseRequest> UpdatePrimaryCaseRequest(Guid primaryRequestId, DatabaseContext dbContext)
        {
            try
            {
                CaseRequest caseRequest = await dbContext.CaseRequests.FindAsync(primaryRequestId);
                caseRequest.IsPrimary = true;
                dbContext.CaseRequests.Update(caseRequest);
                await dbContext.SaveChangesAsync();
                return caseRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Get Case Request list
        //<History Author = 'Nabeel ur Rehman' Date='2022-10-22' Version="1.0" Branch="master">Get all Draft Case Request details </History>
        public async Task<List<CmsCaseRequestVM>> GetDraftCasesCmsCaseRequests(AdvanceSearchCmsCaseRequestVM advanceSearchVM)
        {
            try
            {
                if (_CmsCaseRequestVMs == null)
                {
                    string fromDate = advanceSearchVM.RequestFrom != null ? Convert.ToDateTime(advanceSearchVM.RequestFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = advanceSearchVM.RequestTo != null ? Convert.ToDateTime(advanceSearchVM.RequestTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoredProc = $"exec pCmsCaseRequestList @requestNumber ='{advanceSearchVM.RequestNumber}' , @statusId='{advanceSearchVM.StatusId}' , @subject='{advanceSearchVM.Subject}' , @sectorTypeId='{advanceSearchVM.SectorTypeId}' , @requestFrom='{fromDate}' , @requestTo='{toDate}'";
                    _CmsCaseRequestVMs = await _dbContext.CmsCaseRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get WithDraw Case Request by RquestId

        //<History Author = 'ijaz Ahmad' Date='2022-01-09' Version="1.0" Branch="master">Get WithDraw Case Request by RquestId </History>
        public async Task<List<CmsWithDrawCaseRequestVM>> GetWithDrawCaseRequestByRequestId(Guid requestId)
        {
            try
            {
                if (_CmswithDrawCaseRequestVMs == null)
                {
                    string StoredProc = $"exec pCmsRequestWithDrawListByRequestId @requestId ='{requestId}' ";

                    _CmswithDrawCaseRequestVMs = await _dbContext.CmsWithDrawCaseRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmswithDrawCaseRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get case request status history by Id
        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  Case Requests detail by Id  </History>

        public async Task<CmsCaseRequestHistoryVM> GetCaseRequestHistoryDetailByHistoryId(Guid historyId)
        {
            try
            {
                string storedProc = $"exec pGetCaseRequestStatusHistory @HistoryId = N'{historyId}'";
                var result = await _dbContext.CmsCaseRequestHistoryVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result is not null)
                {
                    return result.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion

        #region Create Withdraw Case Request 
        public async Task CreateWithDrawCaseRequest(WithdrawRequestCommunicationVM cmsWithdrawRequestCommunication)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CaseRequest caseRequest = _dbContext.CaseRequests.Where(x => x.RequestId == cmsWithdrawRequestCommunication.WithdrawRequest.CaseRequestId).FirstOrDefault();
                        if (caseRequest != null)
                        {
                            caseRequest.ModifiedBy = cmsWithdrawRequestCommunication.WithdrawRequest.CreatedBy;
                            caseRequest.ModifiedDate = cmsWithdrawRequestCommunication.WithdrawRequest.CreatedDate;
                            caseRequest.StatusId = (int)CaseRequestStatusEnum.WithdrawRequested;
                            _dbContext.Entry(caseRequest).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            await SaveCaseRequestStatusHistory(cmsWithdrawRequestCommunication.WithdrawRequest.CreatedBy, caseRequest, (int)CaseRequestEventEnum.Withdraw, _dbContext);
                        }

                        await _dbContext.WithdrawRequests.AddAsync(cmsWithdrawRequestCommunication.WithdrawRequest);
                        await _dbContext.SaveChangesAsync();


                        // change file status if exist
                        CaseFile caseFile = _dbContext.CaseFiles.Where(x => x.RequestId == cmsWithdrawRequestCommunication.WithdrawRequest.CaseRequestId).FirstOrDefault();
                        if (caseFile != null)
                        {
                            caseFile.StatusId = (int)CaseFileStatusEnum.WithdrawRequested;
                            _dbContext.Entry(caseFile).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            await SaveCaseFileStatusHistory(cmsWithdrawRequestCommunication.WithdrawRequest.CreatedBy, caseFile, (int)CaseFileEventEnum.Withdraw, _dbContext);
                        }
                        await _communicationRepo.SaveCommunication(cmsWithdrawRequestCommunication.Communication, _dbContext);
                        await _communicationRepo.SaveCommunicationTargetLink(cmsWithdrawRequestCommunication.CommunicationTargetLink, _dbContext);
                        await _communicationRepo.SaveLinkTarget(cmsWithdrawRequestCommunication.LinkTarget, cmsWithdrawRequestCommunication.CommunicationTargetLink.TargetLinkId, _dbContext);
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
        #endregion

        #region save attachments of Withdraw case request
        private async Task SaveAttachmentsByWithdrawCaseRequest(DmsDbContext dmsDbContext, UploadedDocument document)
        {
            try
            {
                document.UploadedDocumentId = 0;
                await dmsDbContext.UploadedDocuments.AddAsync(document);
                await dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Withdraw Case Request 

        public async Task<List<UpdateEntityHistoryVM>> UpdateWithdrawCaseRequestStatus(WithdrawRequestDetailVM request, bool isRejected)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        List<UpdateEntityHistoryVM> listOfHistoryObjs = new List<UpdateEntityHistoryVM>();
                        dynamic returnHistoryObject = null;
                        CmsWithdrawRequest cmsWithdrawRequest = _dbContext.WithdrawRequests.Where(x => x.CaseRequestId == request.ReferenceGuid).FirstOrDefault();

                        if (cmsWithdrawRequest != null)
                        {
                            if (isRejected == false)
                                cmsWithdrawRequest.StatusId = (int)WithdrawRequestStatusEnum.WithdrawnByGE;
                            else
                                cmsWithdrawRequest.StatusId = (int)WithdrawRequestStatusEnum.Rejected;

                            cmsWithdrawRequest.ModifiedBy = request.ModifiedBy;
                            cmsWithdrawRequest.ModifiedDate = request.ModifiedDate;
                            cmsWithdrawRequest.RejectionReason = request.RejectionReason;
                            _dbContext.Entry(cmsWithdrawRequest).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            if (isRejected == false)//For WithDraw Acceptance
                            {
                                CaseRequest caseRequest = _dbContext.CaseRequests.Where(x => x.RequestId == request.ReferenceGuid).FirstOrDefault();
                                if (caseRequest != null)
                                {
                                    caseRequest.StatusId = (int)CaseRequestStatusEnum.WithdrawnByGE;
                                    caseRequest.ModifiedBy = request.ModifiedBy;
                                    caseRequest.ModifiedDate = request.ModifiedDate;

                                    _dbContext.Entry(caseRequest).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                    returnHistoryObject = await SaveCaseRequestStatusHistory(request.CreatedBy, caseRequest, (int)CaseRequestEventEnum.Withdrawn, _dbContext);
                                    listOfHistoryObjs.Add(PrepareEntityHistory((int)SubModuleEnum.CaseRequest, returnHistoryObject, ""));
                                    CaseFile caseFile = _dbContext.CaseFiles.Where(x => x.RequestId == request.ReferenceGuid).FirstOrDefault();
                                    if (caseFile != null)
                                    {
                                        caseFile.StatusId = (int)CaseFileStatusEnum.WithdrawnByGE;
                                        caseFile.ModifiedBy = request.ModifiedBy;
                                        caseFile.ModifiedDate = request.ModifiedDate;
                                        _dbContext.Entry(caseFile).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();
                                        returnHistoryObject = await SaveCaseFileStatusHistory(request.CreatedBy, caseFile, (int)CaseFileEventEnum.Withdrawn, _dbContext);
                                        listOfHistoryObjs.Add(PrepareEntityHistory((int)SubModuleEnum.CaseFile, returnHistoryObject, ""));
                                    }
                                }
                            }//For withdraw Rejection 
                            else
                            {
                                CaseRequest caseRequest = _dbContext.CaseRequests.Where(x => x.RequestId == request.ReferenceGuid).FirstOrDefault();
                                if (caseRequest != null)
                                {
                                    var requestSecondlasthistory = _dbContext.CmsCaseRequestHistories.Where(x => x.RequestId == request.ReferenceGuid).OrderByDescending(x => x.CreatedDate).Skip(1).FirstOrDefault();
                                    var requestStatusId = _dbContext.CmsCaseRequestHistories.Where(x => x.HistoryId == requestSecondlasthistory.HistoryId).Select(x => x.StatusId).FirstOrDefault();
                                    caseRequest.StatusId = (int)requestStatusId;
                                    caseRequest.ModifiedBy = request.ModifiedBy;
                                    caseRequest.ModifiedDate = request.ModifiedDate;
                                    _dbContext.Entry(caseRequest).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                    returnHistoryObject = await SaveCaseRequestStatusHistory(request.CreatedBy, caseRequest, (int)CaseRequestEventEnum.WithdrawRejected, _dbContext);
                                    listOfHistoryObjs.Add(PrepareEntityHistory((int)SubModuleEnum.CaseRequest, returnHistoryObject, request.RejectionReason));
                                    // For CaseFile 
                                    CaseFile caseFile = _dbContext.CaseFiles.Where(x => x.RequestId == request.ReferenceGuid).FirstOrDefault();
                                    if (caseFile != null)
                                    {
                                        var fileSecondlasthistory = _dbContext.CmsCaseFileStatusHistory.Where(x => x.FileId == caseFile.FileId).OrderByDescending(x => x.CreatedDate).Skip(1).FirstOrDefault();
                                        var fileStatusId = _dbContext.CmsCaseFileStatusHistory.Where(x => x.HistoryId == fileSecondlasthistory.HistoryId).Select(x => x.StatusId).FirstOrDefault();
                                        caseFile.StatusId = fileStatusId;
                                        caseFile.ModifiedBy = request.ModifiedBy;
                                        caseFile.ModifiedDate = request.ModifiedDate;
                                        _dbContext.Entry(caseFile).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();
                                        returnHistoryObject = await SaveCaseFileStatusHistory(request.CreatedBy, caseFile, (int)CaseFileEventEnum.WithdrawRejected, _dbContext);
                                        listOfHistoryObjs.Add(PrepareEntityHistory((int)SubModuleEnum.CaseFile, returnHistoryObject, request.RejectionReason));
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                        return listOfHistoryObjs;
                    }
                    catch (Exception ex)
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
                updateEntityHistory.ReferenceId = SubModuleId == (int)SubModuleEnum.CaseRequest ? result.RequestId : result.FileId;
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

        #region View detail of Withdraw Request
        public async Task<WithdrawRequestDetailVM> GetRequestWithdrawDetailById(Guid WithdrawRequestId, int CommunicationTypeId)
        {
            try
            {
                string StoredProc = $"exec pCommuncationListByWithdrawRequest @WithdrawRequestId ='{WithdrawRequestId}' , @TypeId='{CommunicationTypeId}'";
                var result = await _dbContext.cmsWithdrawCaseRequestDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result is not null)
                {
                    var comm = result.FirstOrDefault();
                    var id = comm.CommunicationId;

                    var commAdd = await _dbContext.Communications.FindAsync(id);
                    commAdd.IsRead = false;
                    _dbContext.Entry(commAdd).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return result.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Request 
        public async Task UpdateCaseRequestViewedStatus(Guid RequestId)
        {
            try
            {
                var caseRequest = await _dbContext.CaseRequests.Where(x => x.RequestId == RequestId).FirstOrDefaultAsync();
                if (caseRequest != null)
                {
                    caseRequest.IsViewed = true;
                    _dbContext.CaseRequests.Update(caseRequest);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Get Govt Entity By Id 

        public async Task<GovernmentEntity> GetGovtEntityId(int Id)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var getGovtEntity = await _DbContext.GovernmentEntity.Where(x => x.EntityId == Id).ToListAsync();
                return getGovtEntity.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }
}

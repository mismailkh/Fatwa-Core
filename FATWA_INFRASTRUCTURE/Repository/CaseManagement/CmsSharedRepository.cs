using System.IO;
using System.Linq;
using System.Transactions;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace FATWA_INFRASTRUCTURE.Repository.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master">Shared Repo For Case Management</History> -->
    public class CmsSharedRepository : ICmsShared
    {
        #region Variables declaration

        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private readonly CmsCaseFileRepository _caseFileRepository;
        private readonly CMSCaseRequestRepository _caseRequestRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<CmsCaseRequestVM> _CmsCaseRequestVMs;
        private CmsAssignCaseFileBackToHos _sendBackToHos;
        private List<CmsAssignCaseFileBackToHos> _sendBackToHosList;
        private CaseAssignment _caseAssignment;
        private WorkflowRepository _workflowRepository;

        private List<CmsTransferHistoryVM> _cmsTransferHistoryVMs;

        private List<RequestListVM> _CmsRequestVMs;
        private List<CasePartyLinkExecutionVM> _CaseParty;
        private List<CmsCaseFileTransferRequestVM> _CmsCaseFileTransferRequestVMs;
        private CmsCaseFileTransferRequestDetailVM _CmsCaseFileTransferRequestDetailVMs;
        #endregion

        #region Constructor
        public CmsSharedRepository(DatabaseContext dbContext, DmsDbContext dmsDbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsDbContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _caseRequestRepository = scope.ServiceProvider.GetRequiredService<CMSCaseRequestRepository>();
            _caseFileRepository = scope.ServiceProvider.GetRequiredService<CmsCaseFileRepository>();
        }
        #endregion

        #region Add Approval Tracking

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Save Approval Tracking Process </History>
        public async Task SaveApprovalTrackingProcess(CmsApprovalTracking approvalTracking)
        {
            try
            {
                int transferType = approvalTracking.TransferCaseType;
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await _DbContext.CmsApprovalTracking.AddAsync(approvalTracking);
                await _DbContext.SaveChangesAsync();
                _workflowRepository = scope.ServiceProvider.GetRequiredService<WorkflowRepository>();
                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile && approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.Transfer)
                {
                    _DbContext.CmsCaseFileSectorAssignment.Remove(await _DbContext.CmsCaseFileSectorAssignment.Where(f => f.FileId == approvalTracking.ReferenceId && f.SectorTypeId == approvalTracking.SectorFrom).FirstOrDefaultAsync());
                    _DbContext.CaseFileAssignment.RemoveRange(await _DbContext.CaseFileAssignment.Where(f => f.ReferenceId == approvalTracking.ReferenceId).ToListAsync());
                    await _DbContext.SaveChangesAsync();
                }
                if (approvalTracking.ProcessTypeId != (int)ApprovalProcessTypeEnum.ExecutionRequest)
                    await _workflowRepository.LinkEntityWithActiveWorkflow(approvalTracking, _DbContext, approvalTracking.SubModuleId, transferType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveApprovalTrackingProcessForCivilPartialUrgentSector(CmsApprovalTracking approvalTracking, int TransferCaseType)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                using (var serviceScope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    await dbContext.CmsApprovalTracking.AddAsync(approvalTracking);
                    await dbContext.SaveChangesAsync();
                    await _caseFileRepository.UpdateCaseFileTransferStatus(approvalTracking, TransferCaseType);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                scope.Dispose();
                throw ex;
            }
        }

        #endregion

        #region Add Approval Tracking For Assign

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Save Approval Tracking Process </History>
        public async Task SaveApprovalTrackingProcessForAssign(CmsApprovalTracking approvalTracking)
        {
            try
            {
                int transferType = approvalTracking.TransferCaseType;
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await _DbContext.CmsApprovalTracking.AddAsync(approvalTracking);
                await _DbContext.SaveChangesAsync();
                // For Notification
                approvalTracking.NotificationParameter.Entity = new CaseFile().GetType().Name;
                approvalTracking.NotificationParameter.ReferenceNumber = _DbContext.CaseFiles.Where(x => x.FileId == approvalTracking.ReferenceId).FirstOrDefault().FileNumber;
                approvalTracking.NotificationParameter.SectorFrom = _DbContext.OperatingSectorType.Where(x => x.Id == approvalTracking.SectorFrom).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                approvalTracking.NotificationParameter.SectorTo = _DbContext.OperatingSectorType.Where(x => x.Id == approvalTracking.SectorTo).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Transfer Sector
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Approve Transfer of Sector </History>
        public async Task<CmsCaseFileStatusHistory> ApproveTransferSector(dynamic Item, int TransferCaseType)
        {
            CmsCaseFileStatusHistory historyobj = null;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CaseFile caseFile = System.Text.Json.JsonSerializer.Deserialize<CaseFile>(Item);
                        caseFile.StatusId = (int)CaseFileStatusEnum.AssignedToRegionalSector;
                        _dbContext.CaseFiles.Update(caseFile);
                        await _dbContext.SaveChangesAsync();
                        await _caseFileRepository.SaveCaseFileSectorAssignment(caseFile.FileId, (int)caseFile.SectorTypeId, caseFile.ModifiedBy, _dbContext);
                        historyobj = await _caseFileRepository.SaveCaseFileStatusHistory(caseFile.ModifiedBy, caseFile, (int)CaseFileEventEnum.AssignToSector, _dbContext);
                        await UpdateApprovalTrackingStatus(caseFile.FileId, caseFile.ModifiedBy, (int)caseFile.SectorTypeId, caseFile.Remarks, (int)ApprovalProcessTypeEnum.FileAssignment, (int)ApprovalStatusEnum.Approved, TransferCaseType, _dbContext);
                        await _caseFileRepository.UpdateReadOnlyCaseFileSectorAssignment(caseFile.FileId, (int)caseFile.SectorFrom, _dbContext, caseFile.ModifiedBy);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return historyobj;
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Reject Transfer of Sector </History>
        public async Task RejectTransferSector(dynamic Item, int TransferCaseType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {

                        CaseFile caseFile = System.Text.Json.JsonSerializer.Deserialize<CaseFile>(Item);
                        await _caseFileRepository.SaveCaseFileStatusHistory(caseFile.CreatedBy, caseFile, (int)CaseFileEventEnum.AssignToSector, _dbContext);

                        await UpdateApprovalTrackingStatus(caseFile.FileId, caseFile.CreatedBy, (int)caseFile.SectorTypeId, caseFile.Remarks, (int)ApprovalProcessTypeEnum.FileAssignment, (int)ApprovalStatusEnum.Rejected, TransferCaseType, _dbContext);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Get case request Transfer history by Id

        public async Task<List<CmsTransferHistoryVM>> GetCMSTransferHistory(string RequestId)
        {
            try
            {
                if (_cmsTransferHistoryVMs == null)
                {
                    string StoredProc = $"exec pGetTransferHistory @ReferenceId = N'{RequestId}'";
                    _cmsTransferHistoryVMs = await _dbContext.CmsTransferHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _cmsTransferHistoryVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Send A Copy
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Approve Send A Copy</History>
        //public async Task<dynamic> ApproveSendACopy(dynamic Item, int TransferCaseType)
        //{
        //    using (_dbContext)
        //    {
        //        using (var transaction = _dbContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
        //                {
        //                    CaseRequest paramCaseRequest = System.Text.Json.JsonSerializer.Deserialize<CaseRequest>(Item);
        //                    CaseRequest newCaseRequest = await _caseRequestRepository.CopyCaseRequest(paramCaseRequest, _dbContext);
        //                    //Copy Case Parties From Source To Destination
        //                    var copyAttachments = await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(paramCaseRequest.RequestId, newCaseRequest.RequestId, paramCaseRequest.CreatedBy, _dbContext);
        //                    if (copyAttachments.Any())
        //                    {
        //                        newCaseRequest.CopyAttachmentVMs.AddRange(copyAttachments);
        //                    }

        //                    newCaseRequest.CopyAttachmentVMs.Add(new CopyAttachmentVM()
        //                    {
        //                        SourceId = (Guid)paramCaseRequest.RequestId,
        //                        DestinationId = (Guid)newCaseRequest.RequestId,
        //                        CreatedBy = paramCaseRequest.CreatedBy
        //                    });

        //                    await _caseRequestRepository.SaveCaseRequestStatusHistory(paramCaseRequest.CreatedBy, paramCaseRequest, (int)CaseRequestEventEnum.SentCopy, _dbContext);
        //                    await UpdateApprovalTrackingStatus(paramCaseRequest.RequestId, paramCaseRequest.CreatedBy, (int)paramCaseRequest.SectorTypeId, paramCaseRequest.Remarks, (int)ApprovalProcessTypeEnum.SendaCopy, (int)ApprovalStatusEnum.Approved, _dbContext);

        //                    transaction.Commit();
        //                    return newCaseRequest;
        //                }
        //                else if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
        //                {
        //                    CaseFile paramCaseFile = System.Text.Json.JsonSerializer.Deserialize<CaseFile>(Item);
        //                    CaseFile oldCaseFile = await _dbContext.CaseFiles.FindAsync(paramCaseFile.FileId);
        //                    CaseFile newCaseFile = await _caseFileRepository.CopyCaseFile(oldCaseFile.FileId, oldCaseFile.CreatedBy, _dbContext);
        //                    await _caseFileRepository.SaveCaseFileStatusHistory(oldCaseFile.CreatedBy, oldCaseFile, (int)CaseFileEventEnum.SentCopy, _dbContext);

        //                    //Copy Case Parties From Source To Destination
        //                    //await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(oldCaseFile.FileId, newCaseFile.FileId, paramCaseFile.CreatedBy, _dbContext);
        //                    var copyAttachments = await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(oldCaseFile.FileId, newCaseFile.FileId, paramCaseFile.CreatedBy, _dbContext);
        //                    if (copyAttachments.Any())
        //                    {
        //                        paramCaseFile.CopyAttachmentVMs.AddRange(copyAttachments);
        //                    }

        //                    //await _caseRequestRepository.CopyCaseAttachmentsFromSourceToDestination(oldCaseFile.FileId, newCaseFile.FileId, paramCaseFile.CreatedBy, _dbContext);
        //                    //Copy Case Attachments From Source To Destination
        //                    paramCaseFile.CopyAttachmentVMs.Add(new CopyAttachmentVM()
        //                    {
        //                        SourceId = (Guid)oldCaseFile.FileId,
        //                        DestinationId = (Guid)newCaseFile.FileId,
        //                        CreatedBy = paramCaseFile.CreatedBy
        //                    });
        //                    await _caseFileRepository.UpdateCaseFileSectorAssignment(newCaseFile.FileId, (int)paramCaseFile.SectorTypeId, paramCaseFile.CreatedBy, _dbContext);
        //                    await UpdateApprovalTrackingStatus(paramCaseFile.FileId, paramCaseFile.CreatedBy, (int)paramCaseFile.SectorTypeId, paramCaseFile.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, _dbContext);
        //                    await UpdateCaseFileTransferStatus(paramCaseFile.RequestId, paramCaseFile.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);

        //                    transaction.Commit();

        //                    return paramCaseFile;
        //                }
        //                return null;
        //            }
        //            catch
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Reject Send A Copy </History>
        public async Task RejectSendACopy(dynamic Item, int TransferCaseType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                        {
                            var caseRequest = System.Text.Json.JsonSerializer.Deserialize<CaseRequest>(Item);
                            await UpdateApprovalTrackingStatus(caseRequest.RequestId, caseRequest.CreatedBy, caseRequest.SectorTypeId, caseRequest.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Rejected, _dbContext);
                            //await UpdateCaseRequestTransferStatus(caseRequest.RequestId, caseRequest.CreatedBy, (int)ApprovalStatusEnum.Rejected, _dbContext);
                            await _caseRequestRepository.SaveCaseRequestStatusHistory(caseRequest.CreatedBy, caseRequest, (int)CaseRequestEventEnum.SentCopy, _dbContext);
                        }
                        else if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                        {
                            var caseFile = System.Text.Json.JsonSerializer.Deserialize<CaseFile>(Item);
                            await UpdateApprovalTrackingStatus(caseFile.FileId, caseFile.CreatedBy, caseFile.SectorTypeId, caseFile.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Rejected, _dbContext);
                            await UpdateCaseFileTransferStatus(caseFile.RequestId, caseFile.CreatedBy, (int)ApprovalStatusEnum.Rejected, _dbContext);
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Update Approval Tracking status

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Update Status of Approval Tracking </History>
        public async Task UpdateApprovalTrackingStatus(Guid? referenceId, string username, int sectorTypeId, string remarks, int processTypeId, int statusId, DatabaseContext _dbContext)
        {
            try
            {
                CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == referenceId && x.ProcessTypeId == processTypeId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                if (approvalTracking is not null)
                {
                    approvalTracking.StatusId = statusId;
                    approvalTracking.ModifiedBy = username;
                    approvalTracking.ModifiedDate = DateTime.Now;
                    _dbContext.CmsApprovalTracking.Update(approvalTracking);
                    await _dbContext.SaveChangesAsync();
                    approvalTracking.Remarks = remarks;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateWorkflowApprovalTrackingStatus(Guid? referenceId, string username, int sectorTypeId, string remarks, int processTypeId, int statusId, DatabaseContext _dbContext, int SectorFrom)
        {
            try
            {
                CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == referenceId && x.ProcessTypeId == processTypeId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                if (approvalTracking is not null)
                {
                    approvalTracking.SectorFrom = SectorFrom;
                    approvalTracking.SectorTo = sectorTypeId;
                    approvalTracking.StatusId = statusId;
                    approvalTracking.ModifiedBy = username;
                    approvalTracking.ModifiedDate = DateTime.Now;
                    _dbContext.CmsApprovalTracking.Update(approvalTracking);
                    await _dbContext.SaveChangesAsync();
                    approvalTracking.Remarks = remarks;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateApprovalTrackingStatus(Guid? referenceId, string username, int sectorTypeId, string remarks, int processTypeId, int statusId, int TransferCaseType, DatabaseContext _dbContext)
        {
            try
            {
                CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == referenceId && x.ProcessTypeId == processTypeId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                if (approvalTracking is not null)
                {
                    approvalTracking.StatusId = statusId;
                    approvalTracking.ModifiedBy = username;
                    approvalTracking.ModifiedDate = DateTime.Now;
                    _dbContext.CmsApprovalTracking.Update(approvalTracking);
                    await _dbContext.SaveChangesAsync();
                    approvalTracking.Remarks = remarks;
                    await _caseRequestRepository.SaveTransferHistory(approvalTracking, statusId, TransferCaseType, _dbContext);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateApprovalTrackingWorkflow(Guid? referenceId, string username, int sectorTypeId, string remarks, int processTypeId, int statusId, int TransferCaseType, DatabaseContext _dbContext, int? sectorFrom)
        {
            try
            {
                CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == referenceId && x.ProcessTypeId == processTypeId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                if (approvalTracking is not null)
                {
                    approvalTracking.StatusId = statusId;
                    approvalTracking.ModifiedBy = username;
                    approvalTracking.ModifiedDate = DateTime.Now;
                    approvalTracking.SectorTo = sectorTypeId;
                    approvalTracking.SectorFrom = (int)sectorFrom;
                    _dbContext.CmsApprovalTracking.Update(approvalTracking);
                    await _dbContext.SaveChangesAsync();
                    approvalTracking.Remarks = remarks;
                    await _caseRequestRepository.SaveTransferHistory(approvalTracking, statusId, TransferCaseType, _dbContext);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Approval Tracking Detail

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Get Approval Tracking Process</History>
        public async Task<CmsApprovalTracking> GetApprovalTrackingProcess(Guid referenceId, int sectorTypeId, int processTypeId)
        {
            try
            {
                var test = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == referenceId && x.SectorTo == sectorTypeId && x.ProcessTypeId == processTypeId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                return test;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Update Case Request Transfer status

        public async Task<bool> UpdateCaseRequestTransferStatus(Guid? referenceId, string? createdBy, int taskStatusId, DatabaseContext dbContext)
        {
            bool isSaved = false;

            try
            {
                CaseRequest caseRequest = await dbContext.CaseRequests.FirstOrDefaultAsync(x => x.RequestId == referenceId);
                if (caseRequest is not null)
                {
                    caseRequest.TransferStatusId = taskStatusId;
                    caseRequest.ModifiedBy = createdBy;
                    caseRequest.ModifiedDate = DateTime.Now;

                    dbContext.CaseRequests.Update(caseRequest);
                    await dbContext.SaveChangesAsync();
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

        #region Update Case File Transfer status

        public async Task<bool> UpdateCaseFileTransferStatus(Guid? referenceId, string? createdBy, int taskStatusId, DatabaseContext dbContext)
        {
            bool isSaved = false;

            try
            {
                CaseFile caseFile = await dbContext.CaseFiles.FirstOrDefaultAsync(x => x.FileId == referenceId);
                if (caseFile is not null)
                {
                    caseFile.TransferStatusId = taskStatusId;
                    caseFile.ModifiedBy = createdBy;
                    caseFile.ModifiedDate = DateTime.Now;
                    dbContext.CaseFiles.Update(caseFile);
                    await dbContext.SaveChangesAsync();
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

        #region Assign To Lawyer

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Assign to Lawyer, Change Request and File status, save history and Create Case File with parties and docs from Case Request </History>
        public async Task<Guid?> AssignCaseToLawyer(CaseAssignment caseFileAssignment)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    Guid? referenceId = null;
                    try
                    {
                        if ((int)AssignCaseToLawyerTypeEnum.CaseRequest == caseFileAssignment.AssignCaseToLawyerType)
                        {
                            var file = await _caseRequestRepository.CreateCasefile((Guid)caseFileAssignment.RequestId, caseFileAssignment.CreatedBy, _dbContext, (int)CaseFileStatusEnum.AssignToLawyer);
                            CaseRequest caseRequest = await _dbContext.CaseRequests.FindAsync((Guid)caseFileAssignment.RequestId);
                            caseRequest.ApprovedBy = caseFileAssignment.CreatedBy;
                            caseRequest.ReviewedBy = caseFileAssignment.CreatedBy;
                            // await _caseRequestRepository.GetFileNumberPatternbyGroup(caseFileAssignment, _dbContext);
                            await _caseRequestRepository.UpdateCaseRequest(caseRequest, _dbContext);
                            await _caseRequestRepository.UpdateCaseRequestStatus(caseFileAssignment.CreatedBy, (Guid)caseFileAssignment.RequestId, (int)CaseRequestStatusEnum.ConvertedToFile, (int)CaseRequestEventEnum.AssignToLawyer, _dbContext);
                            // var file = await _caseRequestRepository.CreateCasefile((Guid)caseFileAssignment.RequestId, caseFileAssignment.CreatedBy, _dbContext);
                            await _caseFileRepository.SaveCaseFileStatusHistory(file.CreatedBy, file, (int)CaseFileEventEnum.Created, _dbContext);
                            caseFileAssignment.FileId = file.FileId;

                            caseFileAssignment.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                            {
                                SourceId = (Guid)caseFileAssignment.RequestId,
                                DestinationId = (Guid)caseFileAssignment.FileId,
                                CreatedBy = caseFileAssignment.CreatedBy
                            });

                            var copyAttachments = await _caseRequestRepository.CopyCasePartiesFromSourceToDestination((Guid)caseFileAssignment.RequestId, file.FileId, caseFileAssignment.CreatedBy, _dbContext);
                            if (copyAttachments.Any())
                            {
                                caseFileAssignment.CopyAttachmentVMs.AddRange(copyAttachments);
                            }
                            var linkedRequests = await GetLinkedRequestsByPrimaryRequestId((Guid)caseFileAssignment.RequestId, _dbContext);
                            if (linkedRequests.Any())
                            {
                                foreach (var rqst in linkedRequests)
                                {
                                    caseFileAssignment.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                                    {
                                        SourceId = rqst.RequestId,
                                        DestinationId = (Guid)caseFileAssignment.FileId,
                                        CreatedBy = caseFileAssignment.CreatedBy
                                    });
                                }
                            }

                            await SaveCaseAssignmentToLawyer(file.FileId, caseFileAssignment, _dbContext);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            referenceId = file.FileId;
                            // For Notification 
                            caseFileAssignment.NotificationParameter.ReferenceNumber = file.FileNumber;
                        }
                        else if ((int)AssignCaseToLawyerTypeEnum.RegisteredCase == caseFileAssignment.AssignCaseToLawyerType)
                        {
                            await SaveCaseAssignmentToLawyer(caseFileAssignment.ReferenceId, caseFileAssignment, _dbContext);
                            var registeredCase = _dbContext.CmsRegisteredCases.Where(c => c.CaseId == caseFileAssignment.ReferenceId).FirstOrDefault();
                            await SaveCaseAssignmentToLawyer(registeredCase.FileId, caseFileAssignment, _dbContext);
                            await UpdateLawyerIdInMojMigratedData(caseFileAssignment.ReferenceId, caseFileAssignment, _dbContext);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            referenceId = caseFileAssignment.ReferenceId;
                            // For Notification 
                            caseFileAssignment.NotificationParameter.ReferenceNumber = registeredCase.CaseNumber;
                        }
                        else if ((int)AssignCaseToLawyerTypeEnum.CaseFile == caseFileAssignment.AssignCaseToLawyerType)
                        {
                            var file = await _dbContext.CaseFiles.FindAsync(caseFileAssignment.ReferenceId);
                            await _caseFileRepository.UpdateCaseFileStatus(file, (int)CaseFileStatusEnum.AssignToLawyer, _dbContext);
                            await _caseFileRepository.SaveCaseFileStatusHistory(caseFileAssignment.CreatedBy, file, (int)CaseFileEventEnum.AssignToLawyer, _dbContext);
                            await SaveCaseAssignmentToLawyer(caseFileAssignment.ReferenceId, caseFileAssignment, _dbContext);
                            var sectorId = _dbContext.UserEmploymentInformation.Where(u => u.UserId == (caseFileAssignment.SelectedUsers.Any() ? caseFileAssignment.SelectedUsers.FirstOrDefault().Id : caseFileAssignment.LawyerId)).FirstOrDefault().SectorTypeId;
                            List<CmsRegisteredCase> regCases = await _dbContext.CmsRegisteredCases.Where(c => c.FileId == caseFileAssignment.ReferenceId && c.SectorTypeId == sectorId).ToListAsync();
                            foreach (var regCase in regCases)
                            {
                                await SaveCaseAssignmentToLawyer(regCase.CaseId, caseFileAssignment, _dbContext);
                            }
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            referenceId = caseFileAssignment.ReferenceId;
                            // For Notification 
                            caseFileAssignment.NotificationParameter.ReferenceNumber = file.FileNumber;
                        }
                        return referenceId;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }

        }

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master">Copy Parties and Attachments from Linked Requests of Primary Case Request </History>
        private async Task CopyPartiesAndAttachmentsFromLinkedRequests(CaseAssignment caseFileAssignment, Guid fileId, DatabaseContext dbContext)
        {
            try
            {
                var linkedRequests = await GetLinkedRequestsByPrimaryRequestId((Guid)caseFileAssignment.RequestId, dbContext);
                foreach (var rqst in linkedRequests)
                {
                    await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(rqst.RequestId, fileId, caseFileAssignment.CreatedBy, _dbContext);
                    await _caseRequestRepository.CopyCaseAttachmentsFromSourceToDestination(rqst.RequestId, fileId, caseFileAssignment.CreatedBy, _dbContext, _dmsDbContext);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Save Case Assignees i.e. Lawyers and Supervisors </History>
        public async Task SaveTempCaseAssignmentToLawyer(Guid referenceId, CaseAssignment caseRequestLawyerAssignment, DatabaseContext dbcontext)
        {
            try
            {
                dbcontext.TempCaseAssignments.RemoveRange(dbcontext.TempCaseAssignments.Where(p => p.ReferenceId == referenceId));
                await dbcontext.SaveChangesAsync();

                if ((bool)caseRequestLawyerAssignment.SelectedUsers?.Any())
                {
                    foreach (var lawyer in caseRequestLawyerAssignment.SelectedUsers)
                    {
                        TempCaseAssignment lawyerObj = new TempCaseAssignment();
                        lawyerObj.ReferenceId = referenceId;
                        lawyerObj.SupervisorId = caseRequestLawyerAssignment.SupervisorId;
                        lawyerObj.Remarks = caseRequestLawyerAssignment.Remarks;
                        lawyerObj.LawyerId = lawyer.Id;
                        if (lawyer.Id == caseRequestLawyerAssignment.PrimaryLawyerId)
                        {
                            lawyerObj.IsPrimary = true;
                        }
                        lawyerObj.CreatedBy = caseRequestLawyerAssignment.CreatedBy;
                        lawyerObj.CreatedDate = caseRequestLawyerAssignment.CreatedDate;

                        await dbcontext.TempCaseAssignments.AddAsync(lawyerObj);
                        await dbcontext.SaveChangesAsync();

                    }
                }
                else
                {
                    TempCaseAssignment lawyerObj = new TempCaseAssignment();
                    lawyerObj.ReferenceId = referenceId;
                    lawyerObj.SupervisorId = caseRequestLawyerAssignment.SupervisorId;
                    lawyerObj.Remarks = caseRequestLawyerAssignment.Remarks;
                    lawyerObj.LawyerId = caseRequestLawyerAssignment.LawyerId;
                    lawyerObj.IsPrimary = true;
                    lawyerObj.CreatedBy = caseRequestLawyerAssignment.CreatedBy;
                    lawyerObj.CreatedDate = caseRequestLawyerAssignment.CreatedDate;
                    await dbcontext.TempCaseAssignments.AddAsync(lawyerObj);
                    await dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-15' Version="1.0" Branch="master">Update Lawyer Id for Migrated data from MOJ </History>
        public async Task UpdateLawyerIdInMojMigratedData(Guid referenceId, CaseAssignment caseRequestLawyerAssignment, DatabaseContext dbcontext)
        {
            try
            {
                string lawyerId = string.Empty;
                if ((bool)caseRequestLawyerAssignment.SelectedUsers?.Any())
                {
                    lawyerId = caseRequestLawyerAssignment.PrimaryLawyerId;
                }
                else
                {
                    lawyerId = caseRequestLawyerAssignment.LawyerId;
                }
                var hearings = await dbcontext.Hearings.Where(h => h.CaseId == referenceId && h.LawyerId == null && h.CreatedBy.ToLower() == "moj rpa").ToListAsync();
                foreach (var hearing in hearings)
                {
                    hearing.LawyerId = lawyerId;
                    dbcontext.Hearings.Update(hearing);
                    await dbcontext.SaveChangesAsync();
                    var hearingOutcomes = await dbcontext.OutcomeHearings.Where(h => h.HearingId == hearing.Id && h.LawyerId == null && h.CreatedBy.ToLower() == "moj rpa").ToListAsync();
                    foreach (var outcome in hearingOutcomes)
                    {
                        outcome.LawyerId = lawyerId;
                        dbcontext.OutcomeHearings.Update(outcome);
                        await dbcontext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Save Case Assignees i.e. Lawyers and Supervisors </History>
        public async Task SaveCaseAssignmentToLawyer(Guid referenceId, CaseAssignment caseRequestLawyerAssignment, DatabaseContext dbcontext)
        {
            try
            {
                CaseAssignment primaryLawyer = await dbcontext.CaseFileAssignment.Where(p => p.ReferenceId == referenceId && p.IsPrimary).FirstOrDefaultAsync();
                dbcontext.CaseFileAssignment.RemoveRange(dbcontext.CaseFileAssignment.Where(p => p.ReferenceId == referenceId));
                await dbcontext.SaveChangesAsync();

                if (primaryLawyer != null)
                {
                    UserTask task = await dbcontext.Tasks.Where(t => t.ReferenceId == referenceId && t.TypeId == (int)TaskTypeEnum.Assignment && t.ModuleId == (int)WorkflowModuleEnum.CaseManagement && t.AssignedTo == primaryLawyer.LawyerId).FirstOrDefaultAsync();
                    if (task != null)
                    {
                        task.TaskStatusId = (int)TaskStatusEnum.Done;
                        dbcontext.Tasks.Update(task);
                        await dbcontext.SaveChangesAsync();
                    }
                }

                if ((bool)caseRequestLawyerAssignment.SelectedUsers?.Any())
                {
                    foreach (var lawyer in caseRequestLawyerAssignment.SelectedUsers)
                    {
                        CaseAssignment lawyerObj = new CaseAssignment();
                        lawyerObj.ReferenceId = referenceId;
                        lawyerObj.SupervisorId = caseRequestLawyerAssignment.SupervisorId;
                        lawyerObj.Remarks = caseRequestLawyerAssignment.Remarks;
                        lawyerObj.LawyerId = lawyer.Id;
                        if (lawyer.Id == caseRequestLawyerAssignment.PrimaryLawyerId)
                        {
                            lawyerObj.IsPrimary = true;
                        }
                        lawyerObj.CreatedBy = caseRequestLawyerAssignment.CreatedBy;
                        lawyerObj.CreatedDate = caseRequestLawyerAssignment.CreatedDate;

                        await dbcontext.CaseFileAssignment.AddAsync(lawyerObj);
                        await dbcontext.SaveChangesAsync();
                        await SaveCaseAssignmentHistory(referenceId, caseRequestLawyerAssignment.CreatedBy, lawyer.Id, caseRequestLawyerAssignment.Remarks, dbcontext);
                    }
                }
                else
                {
                    CaseAssignment lawyerObj = new CaseAssignment();
                    lawyerObj.ReferenceId = referenceId;
                    lawyerObj.SupervisorId = caseRequestLawyerAssignment.SupervisorId;
                    lawyerObj.Remarks = caseRequestLawyerAssignment.Remarks;
                    lawyerObj.LawyerId = caseRequestLawyerAssignment.LawyerId;
                    lawyerObj.IsPrimary = true;
                    lawyerObj.CreatedBy = caseRequestLawyerAssignment.CreatedBy;
                    lawyerObj.CreatedDate = caseRequestLawyerAssignment.CreatedDate;
                    await dbcontext.CaseFileAssignment.AddAsync(lawyerObj);
                    await dbcontext.SaveChangesAsync();
                    await SaveCaseAssignmentHistory(referenceId, caseRequestLawyerAssignment.CreatedBy, caseRequestLawyerAssignment.LawyerId, caseRequestLawyerAssignment.Remarks, dbcontext);
                }
                await SaveCaseAssignmentHistory(referenceId, caseRequestLawyerAssignment.CreatedBy, caseRequestLawyerAssignment.SupervisorId, caseRequestLawyerAssignment.Remarks, dbcontext);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Save Case Assigneement History </History>
        public async Task SaveCaseAssignmentHistory(Guid referenceId, string userId, string assigneeId, string remarks, DatabaseContext dbcontext)
        {
            try
            {
                CaseAssignmentHistory historyObj = new CaseAssignmentHistory();
                historyObj.ReferenceId = referenceId;
                historyObj.AssigneeId = assigneeId;
                historyObj.Remarks = remarks;
                historyObj.CreatedBy = userId;
                historyObj.CreatedDate = DateTime.Now;
                await dbcontext.CaseFileAssignmentHistory.AddAsync(historyObj);
                await dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Linked Requests By Primary Request

        public async Task<List<CmsCaseRequestVM>> GetLinkedRequestsByPrimaryRequestId(Guid RequestId, DatabaseContext dbContext)
        {
            try
            {
                if (_CmsCaseRequestVMs == null)
                {
                    string StoredProc = $"exec pLinkedRequestsByPrimaryRequestId @requestId = N'{RequestId}'";
                    _CmsCaseRequestVMs = await dbContext.CmsCaseRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Create Ge Representative

        public async Task<GovernmentEntityRepresentative> CreateGeRepresentative(GovernmentEntityRepresentative geRepresentative)
        {
            try
            {
                await _dbContext.GovernmentEntityRepresentative.AddAsync(geRepresentative);
                await _dbContext.SaveChangesAsync();
                return geRepresentative;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Assign Decision Request To Lawyer

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Assign to Lawyer, Change Request and File status, save history and Create Case File with parties and docs from Case Request </History>
        public async Task<bool> AssignDecisionRequestToLawyer(CmsCaseDecisionAssignee casedecisionAssignment)
        {
            bool isSaved = true;
            try
            {

                CmsCaseDecisionAssignee decisionObj = new CmsCaseDecisionAssignee();
                decisionObj.Id = casedecisionAssignment.Id;
                decisionObj.DecisionId = casedecisionAssignment.DecisionId;
                decisionObj.UserId = casedecisionAssignment.UserId;
                decisionObj.CreatedBy = casedecisionAssignment.CreatedBy;
                decisionObj.CreatedDate = casedecisionAssignment.CreatedDate;
                await _dbContext.CmsCaseDecisionAssignees.AddAsync(decisionObj);
                await UpdateDecisionStatus(casedecisionAssignment, (int)CaseDecisionStatusEnum.AssignedToLawyer);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                isSaved = false;
            }
            return isSaved;
        }

        public async Task UpdateDecisionStatus(CmsCaseDecisionAssignee item, int StatusId)
        {
            try
            {
                CmsCaseDecision cmsCaseDecision = _dbContext.CmsCaseDecisions.FirstOrDefault(m => m.Id == item.DecisionId);
                if (cmsCaseDecision is not null)
                {
                    cmsCaseDecision.StatusId = StatusId;

                    cmsCaseDecision.ModifiedBy = item.ModifiedBy;
                    cmsCaseDecision.ModifiedDate = DateTime.Now;

                    _dbContext.CmsCaseDecisions.Update(cmsCaseDecision);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Execution Request Approve/Reject

        public async Task<string> SendExecutionRequestToMOJExecution(MojExecutionRequest executionRequest)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            string messengerId = "";
                            string StoreProc = $"exec pGetMojBySectorId @sectorTypeId = '{(int)OperatingSectorTypeEnum.Execution}'";
                            var users = await _dbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                            messengerId = users?.FirstOrDefault()?.Id;

                            MojExecutionRequestAssignee mojExecutionRequestAssignee = new MojExecutionRequestAssignee
                            {
                                RequestId = executionRequest.Id,
                                UserId = messengerId,
                                CreatedBy = executionRequest.ModifiedBy,
                                CreatedDate = DateTime.Now
                            };

                            await _dbContext.MojExecutionRequestAssignees.AddAsync(mojExecutionRequestAssignee);
                            await _dbContext.SaveChangesAsync();

                            CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == executionRequest.Id && x.SectorTo == executionRequest.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.ExecutionRequest).FirstOrDefaultAsync();
                            if (approvalTracking is not null)
                            {
                                _dbContext.CmsApprovalTracking.Remove(approvalTracking);
                                await _dbContext.SaveChangesAsync();
                            }
                            //For Notification
                            executionRequest.NotificationParameter.CaseNumber = _dbContext.CmsRegisteredCases.Where(x => x.CaseId == executionRequest.CaseId).FirstOrDefault().CaseNumber;
                            executionRequest.NotificationParameter.SectorFrom = _dbContext.OperatingSectorType.Where(x => x.Id == executionRequest.SectorTypeId).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            transaction.Commit();
                            return messengerId;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ApproveExecutionRequest(MojExecutionRequest executionRequest)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == executionRequest.Id && x.SectorTo == executionRequest.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.ExecutionRequest).FirstOrDefaultAsync();
                            if (approvalTracking is not null)
                            {
                                _dbContext.CmsApprovalTracking.Remove(approvalTracking);
                                await _dbContext.SaveChangesAsync();
                            }
                            //For Notification
                            executionRequest.NotificationParameter.CaseNumber = _dbContext.CmsRegisteredCases.Where(x => x.CaseId == executionRequest.CaseId).FirstOrDefault().CaseNumber;
                            executionRequest.NotificationParameter.SectorFrom = _dbContext.OperatingSectorType.Where(x => x.Id == executionRequest.SectorTypeId).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task RejectExecutionRequest(MojExecutionRequest executionRequest)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == executionRequest.Id && x.SectorTo == executionRequest.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.ExecutionRequest).FirstOrDefaultAsync();
                            if (approvalTracking is not null)
                            {
                                _dbContext.CmsApprovalTracking.Remove(approvalTracking);
                                await _dbContext.SaveChangesAsync();
                            }
                            //For Notification
                            executionRequest.NotificationParameter.CaseNumber = _dbContext.CmsRegisteredCases.Where(x => x.CaseId == executionRequest.CaseId).FirstOrDefault().CaseNumber;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Govt Entity By ReferenceId
        //<History Author = 'Nadia Gull' Date='2023-01-27' Version="1.0" Branch="master"> Get RequestNumber/FileNumbers</History>
        public async Task<int> GetGovtEnityByReferencId(Guid ReferenceId, int SubModulId)
        {

            try
            {
                int? govtEntityId = 0;
                if (SubModulId == (int)SubModuleEnum.CaseRequest)
                {
                    var caseRequest = await _dbContext.CaseRequests.FindAsync(ReferenceId);
                    if (caseRequest != null)
                    {
                        govtEntityId = caseRequest.GovtEntityId;
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.CaseFile)
                {
                    var caseFile = await _dbContext.CaseFiles.FindAsync(ReferenceId);
                    if (caseFile != null)
                    {
                        CaseRequest caseRequest = await _dbContext.CaseRequests.Where(d => d.RequestId == caseFile.RequestId).FirstOrDefaultAsync();
                        if (caseRequest != null)
                        {
                            govtEntityId = caseRequest.GovtEntityId;
                        }

                    }

                }
                else if (SubModulId == (int)SubModuleEnum.RegisteredCase)
                {

                    var registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(ReferenceId);
                    if (registeredCase != null)
                    {
                        var caseFile = await _dbContext.CaseFiles.Where(d => d.FileId == registeredCase.FileId).FirstOrDefaultAsync(); ;

                        CaseRequest caseRequest = await _dbContext.CaseRequests.Where(d => d.RequestId == caseFile.RequestId).FirstOrDefaultAsync();
                        if (caseRequest != null)
                        {
                            govtEntityId = caseRequest.GovtEntityId;
                        }
                        if (registeredCase != null)
                        {
                            govtEntityId = registeredCase.GovtEntityId;

                        }
                    }

                }
                else if (SubModulId == (int)SubModuleEnum.ConsultationRequest)
                {
                    var consultationRequest = await _dbContext.ConsultationRequests.FindAsync(ReferenceId);
                    if (consultationRequest != null)
                    {
                        govtEntityId = consultationRequest.GovtEntityId;
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.ConsultationFile)
                {
                    var consultationFile = await _dbContext.ConsultationFiles.FindAsync(ReferenceId);
                    ConsultationRequest consultationRequest = await _dbContext.ConsultationRequests.Where(d => d.ConsultationRequestId == consultationFile.RequestId).FirstOrDefaultAsync();
                    if (consultationRequest != null)
                    {
                        govtEntityId = consultationRequest.GovtEntityId;
                    }
                }

                return govtEntityId != null ? (int)govtEntityId : 0;
            }


            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get send back to hos by reference Id
        public async Task<CmsAssignCaseFileBackToHos> GetSendBackToHosByReferenceId(Guid ReferenceId, string LawyerId)
        {
            try
            {
                if (_sendBackToHos == null)
                {
                    var user = await _dbContext.Users.FindAsync(LawyerId);
                    _sendBackToHos = await _dbContext.CmsAssignCaseFileBackToHos.Where(x => x.ReferenceId == ReferenceId && x.CreatedBy == user.Email).FirstOrDefaultAsync();

                }
                if (_sendBackToHos != null)
                {
                    return _sendBackToHos;
                }
                else
                {
                    return new CmsAssignCaseFileBackToHos();

                }


            }
            catch (Exception)
            {
                throw;

            }
        }
        #endregion

        #region Get case assigment by lawyer id and file id
        public async Task<CaseAssignment> GetCaseAssigmentByLawyerIdAndFileId(Guid FileId, string UserId)

        {
            try
            {
                if (_caseAssignment == null)
                {
                    _caseAssignment = await _dbContext.CaseFileAssignment.Where(x => x.ReferenceId == FileId && x.LawyerId == UserId).FirstOrDefaultAsync();

                }
                if (_caseAssignment != null)
                {
                    return _caseAssignment;
                }
                else
                {
                    return new CaseAssignment();

                }


            }
            catch (Exception)
            {
                throw;

            }
        }
        #endregion

        #region Approve case file 
        public async Task<CmsCaseFileStatusHistory> ApproveCaseFile(CmsCaseFileDetailVM item)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CaseFile file = await _dbContext.CaseFiles.FindAsync(item.FileId);
                        if (file != null)
                        {
                            file.StatusId = (int)CaseFileStatusEnum.InProgress;
                        }
                        _dbContext.CaseFiles.Update(file);
                        await _dbContext.SaveChangesAsync();
                        var historyobj = await _caseFileRepository.SaveCaseFileStatusHistory(file.CreatedBy, file, (int)CaseFileEventEnum.AcceptedByLawyer, _dbContext);
                        transaction.Commit();
                        return historyobj;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return null;
                        throw;
                    }
                }
            }

        }

        #endregion

        #region Get Case Consultation List
        public async Task<List<RequestListVM>> GetCaseConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchVM)
        {

            try
            {
                string StoredProc;
                if (_CmsRequestVMs == null)
                {
                    string requestFrom = advanceSearchVM.RequestFrom != null ? Convert.ToDateTime(advanceSearchVM.RequestFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string requestTo = advanceSearchVM.RequestTo != null ? Convert.ToDateTime(advanceSearchVM.RequestTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    StoredProc = $"exec pConfidentialRequestList @requestNumber =N'{advanceSearchVM.RequestNumber}' , @statusId='{advanceSearchVM.StatusId}' , @subject=N'{advanceSearchVM.Subject}'" +
                        $",@requestTypeId='{advanceSearchVM.RequestTypeId}' , @requestFrom='{requestFrom}' , @requestTo='{requestTo}', @showUndefinedRequests='{advanceSearchVM.ShowUndefinedRequest}'" +
                        $", @sectorTypeId='{advanceSearchVM.SectorTypeId}' ,@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";

                    _CmsRequestVMs = await _dbContext.RequestListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Link Entity With Active Workflow
        private async Task LinkEntityWithActiveWorkflow(CmsApprovalTracking approvalTracking, DatabaseContext dbContext, int transferType)
        {
            try
            {
                int moduleTriggerId = 0;
                if (approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.SendaCopy)
                {
                    moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest : (int)WorkflowModuleTriggerEnum.SendCopyCaseFile;

                }
                else
                {
                    if (approvalTracking.IsConfidential)
                    {
                        moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseRequest : (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseFile;
                    }
                    else
                    {
                        moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.TransferCaseRequest : (int)WorkflowModuleTriggerEnum.TransferCaseFile;
                    }
                }
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{(int)WorkflowModuleEnum.CaseManagement}', @moduleTriggerId = '{moduleTriggerId}'";
                var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                if (activeWorkflow?.Count() > 0)
                {
                    WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                    WorkflowInstance workflowInstance = new WorkflowInstance { ReferenceId = approvalTracking.Id, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
                    SLA sla = await dbContext.SLA.Where(s => s.WorkflowActivityId == workflowInstance.WorkflowActivityId).FirstOrDefaultAsync();
                    if (sla != null)
                    {
                        workflowInstance.ApplySla = true;
                        workflowInstance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                        workflowInstance.SlaEndDate = workflowInstance.SlaStartDate.AddDays(sla.Duration - 1);
                    }
                    else
                    {
                        workflowInstance.ApplySla = false;
                        workflowInstance.SlaStartDate = DateTime.Now.Date;
                        workflowInstance.SlaEndDate = DateTime.Now.Date;
                    }
                    await dbContext.WorkflowInstance.AddAsync(workflowInstance);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Save Copy History
        public async Task SaveCopyHistory(CmsApprovalTracking approvalTracking, int taskStatusId, int subModuleId, DatabaseContext _dbContext)
        {
            try
            {
                CmsCopyHistory historyobj = new CmsCopyHistory();
                historyobj.CopyHistoryId = Guid.NewGuid();
                historyobj.ReferenceId = approvalTracking.ReferenceId;
                historyobj.ApprovalTrackingId = approvalTracking.Id;
                historyobj.CreatedBy = approvalTracking.UserName;
                historyobj.SectorFrom = approvalTracking.SectorFrom;
                historyobj.SectorTo = approvalTracking.SectorTo;
                historyobj.Reason = approvalTracking.Remarks;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.SubModuleId = subModuleId;
                if (taskStatusId == (int)ApprovalStatusEnum.Approved)
                {
                    historyobj.StatusId = approvalTracking.SectorTo;
                    historyobj.CreatedBy = approvalTracking.UserName;
                }
                else if (taskStatusId == (int)ApprovalStatusEnum.Pending)
                {
                    historyobj.StatusId = (int)ApprovalStatusEnum.Pending;

                }
                await _dbContext.CmsCopyHistories.AddAsync(historyobj);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get CaseParty
        public async Task<List<CasePartyLinkExecutionVM>> GetCasePartiesByCaseIdForExecution(Guid Id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    if (_CaseParty == null)
                    {
                        string StoredProc = $"exec pGetCmsCasePartiesByRefId @ReferenceGuid='{Id}'";
                        _CaseParty = await _dbContext.CasePartyLinkExecutionVMs.FromSqlRaw(StoredProc).ToListAsync();
                    }
                    return _CaseParty;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region 
        public async Task<List<string>> GetCaseAssignmentListByReferenceId(Guid Id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var result = await _DbContext.CaseFileAssignment.Where(x => x.ReferenceId == Id && x.IsPrimary != true).Select(x => x.LawyerId).ToListAsync();
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Get Sector Users List
        protected List<SectorUsersVM> SectorUsersList;
        public async Task<List<SectorUsersVM>> GetSectorUsersList(string RoleId, int? SectorTypeId, int? pageNumber, int? pageSize,string UserId)
        {
            try
            {

                string cmsUserRoleId = null;
                string comsUserRoleId = null;
                if ((RoleId == SystemRoles.HOS || RoleId == SystemRoles.Lawyer || RoleId == SystemRoles.Supervisor || (RoleId == SystemRoles.ViceHOS)) &&
                !((SectorTypeId >= (int)OperatingSectorTypeEnum.LegalAdvice) && (SectorTypeId <= (int)OperatingSectorTypeEnum.InternationalArbitration)))
                {
                    cmsUserRoleId = RoleId;
                }
                else if (RoleId == SystemRoles.ComsHOS || RoleId == SystemRoles.ComsLawyer || RoleId == SystemRoles.ComsSupervisor || RoleId == SystemRoles.ViceHOS)
                {
                    comsUserRoleId = RoleId;
                }

                string StoredProc = $"exec pGetSectorUsersList @SectorTypeId='{SectorTypeId}', @CMSUserRoleId='{cmsUserRoleId}', @COMSUserRoleId='{comsUserRoleId}', @PageNumber='{pageNumber}', @PageSize='{pageSize}', @UserId='{UserId}'";
                SectorUsersList = await _dbContext.SectorUsersVM.FromSqlRaw(StoredProc).ToListAsync();
                return SectorUsersList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Get CMS Users By Role and Sector Id
        protected List<SectorUsersVM> SectorUsers;
        public async Task<List<SectorUsersVM>> GetUsersByRoleAndSector(string RoleId, int? SectorTypeId)
        {
            try
            {

                string cmsUserRoleId = null;
                string comsUserRoleId = null;
                if ((RoleId == SystemRoles.HOS || RoleId == SystemRoles.Lawyer || RoleId == SystemRoles.Supervisor || (RoleId == SystemRoles.ViceHOS)) &&
                !((SectorTypeId >= (int)OperatingSectorTypeEnum.LegalAdvice) && (SectorTypeId <= (int)OperatingSectorTypeEnum.InternationalArbitration)))
                {
                    cmsUserRoleId = RoleId;
                }
                else if (RoleId == SystemRoles.ComsHOS || RoleId == SystemRoles.ComsLawyer || RoleId == SystemRoles.ComsSupervisor || RoleId == SystemRoles.ViceHOS)
                {
                    comsUserRoleId = RoleId;
                }

                string StoredProc = $"exec pGetSectorUsersByRoleId @SectorTypeId='{SectorTypeId}', @CMSUserRoleId='{cmsUserRoleId}', @COMSUserRoleId='{comsUserRoleId}'";
                SectorUsers = await _dbContext.SectorUsersVM.FromSqlRaw(StoredProc).ToListAsync();
                return SectorUsers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
        #region Save Request for Transfer
        public async Task AddCaseFileTransferRequest(CmsCaseFileTranferRequest cmsCaseFileTranferRequest)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (_dbContext.CmsCaseFileTranferRequest.Any())
                        {
                            var requestNo = await _dbContext.CmsCaseFileTranferRequest.Select(x => Convert.ToInt32(x.RequestNo)).MaxAsync();
                            cmsCaseFileTranferRequest.RequestNo = (Convert.ToInt32(requestNo) + 1).ToString();
                        }
                        else
                        {
                            cmsCaseFileTranferRequest.RequestNo = "1";
                        }
                        cmsCaseFileTranferRequest.IsDeleted = false;
                        await _dbContext.CmsCaseFileTranferRequest.AddAsync(cmsCaseFileTranferRequest);
                        await _dbContext.SaveChangesAsync();
                        // For Notification
                        cmsCaseFileTranferRequest.NotificationParameter.Entity = new CaseFile().GetType().Name;
                        cmsCaseFileTranferRequest.NotificationParameter.SectorFrom = _dbContext.OperatingSectorType.Where(x => x.Id == cmsCaseFileTranferRequest.SectorFrom).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                        cmsCaseFileTranferRequest.NotificationParameter.SectorTo = _dbContext.OperatingSectorType.Where(x => x.Id == cmsCaseFileTranferRequest.SectorTo).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
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
        public async Task RejectCaseFileTransferRequest(CmsCaseFileTransferRequestDetailVM cmsCaseFileTransferRequestDetail)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        RejectReason rejectReason = new RejectReason();
                        using var scope = _serviceScopeFactory.CreateScope();
                        var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                        rejectReason.RejectionId = Guid.NewGuid();
                        rejectReason.ReferenceId = cmsCaseFileTransferRequestDetail.Id;
                        rejectReason.Reason = cmsCaseFileTransferRequestDetail.RejectionReason;
                        rejectReason.CreatedBy = cmsCaseFileTransferRequestDetail.UserName;
                        rejectReason.CreatedDate = DateTime.Now;
                        await _DbContext.RejectReasons.AddAsync(rejectReason);
                        await UpdateCaseFileTransferRequestForStatus(cmsCaseFileTransferRequestDetail);
                        await _DbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task UpdateCaseFileTransferRequestForStatus(CmsCaseFileTransferRequestDetailVM cmsCaseFileTransferRequestDetail)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                CmsCaseFileTranferRequest cmsCaseFileTranferRequest = await _dbContext.CmsCaseFileTranferRequest.Where(d => d.Id == cmsCaseFileTransferRequestDetail.Id).FirstOrDefaultAsync();
                if (cmsCaseFileTranferRequest != null)
                {
                    cmsCaseFileTranferRequest.StatusId = (int)cmsCaseFileTransferRequestDetail.StatusId;
                    cmsCaseFileTranferRequest.ModifiedBy = cmsCaseFileTransferRequestDetail.UserName;
                    cmsCaseFileTranferRequest.ModifiedDate = DateTime.Now;
                }
                _DbContext.CmsCaseFileTranferRequest.Update(cmsCaseFileTranferRequest);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get Transfer Request List
        public async Task<List<CmsCaseFileTransferRequestVM>> GetCaseFileTransferRequestList(int sectorTypeId)
        {
            try
            {
                if (_CmsCaseFileTransferRequestVMs == null)
                {
                    string StoreProc = $"exec pCmsCaseFileTranferRequestsList @sectorTypeId = '{sectorTypeId}'";
                    _CmsCaseFileTransferRequestVMs = await _dbContext.CmsCaseFileTransferRequestVM.FromSqlRaw(StoreProc).ToListAsync();
                }
                return _CmsCaseFileTransferRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Request Transfer Detail
        public async Task<CmsCaseFileTransferRequestDetailVM> GetCaseFileTransferRequestDetailById(Guid ReferenceId)
        {
            try
            {
                if (_CmsCaseFileTransferRequestDetailVMs == null)
                {
                    string StoredProc = $"exec pCmsCaseFileTranferRequestDetailById @ReferenceId = N'{ReferenceId}'";
                    var result = await _dbContext.CmsCaseFileTransferRequestDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                    if (result.Any())
                    {
                        _CmsCaseFileTransferRequestDetailVMs = result.FirstOrDefault();
                        _CmsCaseFileTransferRequestDetailVMs.rejectReason = await _dbContext.RejectReasons.FirstOrDefaultAsync(x => x.ReferenceId == ReferenceId);
                    }
                }
                return _CmsCaseFileTransferRequestDetailVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}

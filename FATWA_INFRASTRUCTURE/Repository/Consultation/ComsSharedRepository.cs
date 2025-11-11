using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Consultation;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.Consultation;
using FATWA_INFRASTRUCTURE.Repository.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_INFRASTRUCTURE.Repository.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master">Shared Repo For Case Management</History> -->
    public class ComsSharedRepository : IComsShared
    {
        #region Variables declaration

        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private readonly COMSConsultationRepository _consultationRequestRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<CmsCaseRequestVM> _CmsCaseRequestVMs;
        private CmsAssignCaseFileBackToHos _sendBackToHos;
        private readonly COMSConsultationFileRepository _consultationFileRepository;
        private readonly TaskRepository _taskRepository;
        private ConsultationAssignment _consultationAssignment;
        private readonly CMSCaseRequestRepository _caseRequestRepository;
        private readonly CmsCaseFileRepository _caseFileRepository;
        private WorkflowRepository _workflowRepository;
        #endregion

        #region Constructor
        public ComsSharedRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory, DmsDbContext dmsDbContext)
        {
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _consultationRequestRepository = scope.ServiceProvider.GetRequiredService<COMSConsultationRepository>();
            _consultationFileRepository = scope.ServiceProvider.GetRequiredService<COMSConsultationFileRepository>();
            _taskRepository = scope.ServiceProvider.GetRequiredService<TaskRepository>();
            _caseRequestRepository = scope.ServiceProvider.GetRequiredService<CMSCaseRequestRepository>();
            _caseFileRepository = scope.ServiceProvider.GetRequiredService<CmsCaseFileRepository>();
            _dmsDbContext = dmsDbContext;
        }
        #endregion

        #region Add Approval Tracking

        //<History Author = 'Muhammad Zaeem' Date='2023-1-9' Version="1.0" Branch="master"Save Approval Tracking Process </History>
        public async Task SaveApprovalTrackingProcess(CmsApprovalTracking approvalTracking)
        {
            try
            {
                int transferType = approvalTracking.TransferCaseType;
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await _DbContext.CmsApprovalTracking.AddAsync(approvalTracking);
                await _DbContext.SaveChangesAsync();
                approvalTracking.NotificationParameter.Entity = "Transfer";
                _workflowRepository = scope.ServiceProvider.GetRequiredService<WorkflowRepository>();
                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                {
          
                    _DbContext.ComsConsultationFileSectorAssignments.Remove(await _DbContext.ComsConsultationFileSectorAssignments.Where(f => f.FileId == approvalTracking.ReferenceId && f.SectorTypeId == approvalTracking.SectorFrom).FirstOrDefaultAsync() ?? new ComsConsultationFileSectorAssignment());
                    _DbContext.ConsultationAssignments.RemoveRange(await _DbContext.ConsultationAssignments.Where(f => f.ReferenceId == approvalTracking.ReferenceId).ToListAsync() ?? new List<ConsultationAssignment>());
                    await _DbContext.SaveChangesAsync();

                    // For Notification 
                    var file = await _dbContext.ConsultationFiles.FindAsync(approvalTracking.ReferenceId);
                    approvalTracking.NotificationParameter.FileNumber = file.FileNumber;
                    var requestNumber1 = _dbContext.ConsultationRequests.Where(c => c.ConsultationRequestId == file.RequestId).FirstOrDefault().RequestNumber;
                    approvalTracking.NotificationParameter.RequestNumber = requestNumber1;


                }
                await _workflowRepository.LinkEntityWithActiveWorkflow(approvalTracking, _DbContext, approvalTracking.SubModuleId, transferType);
                var requestNumber2 = await _dbContext.ConsultationRequests.FindAsync(approvalTracking.ReferenceId);
                if (requestNumber2 != null)
                approvalTracking.NotificationParameter.RequestNumber = requestNumber2.RequestNumber;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Transfer Sector
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Approve Transfer of Sector </History>
        public async Task ApproveTransferComsSector(dynamic Item, int TransferConsultationType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                        {
                            ConsultationRequest consultationRequest = System.Text.Json.JsonSerializer.Deserialize<ConsultationRequest>(Item);
                            ConsultationRequest res = await _dbContext.ConsultationRequests.FindAsync(consultationRequest.ConsultationRequestId);
                            res.SectorTypeId = consultationRequest.SectorTypeId;

                            _dbContext.ConsultationRequests.Update(res);
                            await _dbContext.SaveChangesAsync();
                            await _consultationRequestRepository.SaveConsultationRequestStatusHistory(consultationRequest.CreatedBy, consultationRequest, (int)CaseRequestEventEnum.Transfer, _dbContext);
                            //await UpdateApprovalTrackingStatus(consultationRequest.ConsultationRequestId, consultationRequest.CreatedBy, (int)consultationRequest.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, _dbContext);
                            await UpdateConsultationRequestTransferStatus(consultationRequest.ConsultationRequestId, consultationRequest.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);

                        }
                        else if (TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                        {
                            ConsultationFile consultationFile = System.Text.Json.JsonSerializer.Deserialize<ConsultationFile>(Item);
                            CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == consultationFile.FileId && x.SectorTo == consultationFile.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.FileAssignment).FirstOrDefaultAsync();

                            if (approvalTracking != null)
                            {
                                await _consultationRequestRepository.SaveConsultationFileSectorAssignmentFile(consultationFile.FileId, (int)consultationFile.SectorTypeId, consultationFile.CreatedBy, _dbContext);
                                await _consultationRequestRepository.SaveConsultationFileStatusHistory(consultationFile, (int)CaseFileEventEnum.Transfer, _dbContext);
                                //await UpdateApprovalTrackingStatus(consultationFile.FileId, consultationFile.CreatedBy, (int)consultationFile.SectorTypeId, (int)ApprovalProcessTypeEnum.FileAssignment, (int)ApprovalStatusEnum.Approved, _dbContext);

                            }
                            else
                            {
                                approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == consultationFile.FileId && x.SectorTo == consultationFile.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.Transfer).FirstOrDefaultAsync();
                                if (approvalTracking != null)
                                {
                                    await _consultationRequestRepository.RemoveConsultationFileSectorAssignment(consultationFile.FileId, (int)approvalTracking.SectorFrom, consultationFile.CreatedBy, _dbContext);
                                    await _consultationRequestRepository.SaveConsultationFileSectorAssignmentFile(consultationFile.FileId, (int)consultationFile.SectorTypeId, consultationFile.CreatedBy, _dbContext);
                                    await _consultationRequestRepository.SaveConsultationFileStatusHistory(consultationFile, (int)CaseFileEventEnum.Transfer, _dbContext);
                                    //await UpdateApprovalTrackingStatus(consultationFile.FileId, consultationFile.CreatedBy, (int)consultationFile.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, _dbContext);
                                }
                            }
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

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Reject Transfer of Sector </History>
        public async Task RejectTransferComsSector(dynamic Item, int TransferConsultationType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                        {
                            ConsultationRequest consultationRequest = System.Text.Json.JsonSerializer.Deserialize<ConsultationRequest>(Item);
                            //await UpdateApprovalTrackingStatus(consultationRequest.ConsultationRequestId, consultationRequest.CreatedBy, (int)consultationRequest.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Rejected, _dbContext);
                            await UpdateConsultationRequestTransferStatus(consultationRequest.ConsultationRequestId, consultationRequest.CreatedBy, (int)ApprovalStatusEnum.Rejected, _dbContext);
                        }
                        else if (TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                        {
                            ConsultationFile consultationFile = System.Text.Json.JsonSerializer.Deserialize<ConsultationFile>(Item);
                            //await UpdateApprovalTrackingStatus(consultationFile.FileId, consultationFile.CreatedBy, (int)consultationFile.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Rejected, _dbContext);
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

        public async Task UpdateApprovalTrackingStatus(Guid? referenceId, string username, int sectorTypeId, int processTypeId, int statusId, DatabaseContext _dbContext, string remarks, int TransferCaseType)
        {
            try
            {
                CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == referenceId && x.SectorTo == sectorTypeId && x.ProcessTypeId == processTypeId).FirstOrDefaultAsync();
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

        #endregion

        #region Update Consultation Request Transfer status

        public async Task<bool> UpdateConsultationRequestTransferStatus(Guid? referenceId, string? createdBy, int taskStatusId, DatabaseContext dbContext)
        {
            bool isSaved = false;

            try
            {
                ConsultationRequest consultationRequest = await dbContext.ConsultationRequests.FirstOrDefaultAsync(x => x.ConsultationRequestId == referenceId);
                if (consultationRequest is not null)
                {
                    consultationRequest.TransferStatusId = taskStatusId;
                    consultationRequest.ModifiedBy = createdBy;
                    consultationRequest.ModifiedDate = DateTime.Now;
                    if (consultationRequest.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector)
                    {
                        consultationRequest.IsConfidential = true;
                    }
                    else
                    {
                        consultationRequest.IsConfidential = false;
                    }
                    dbContext.ConsultationRequests.Update(consultationRequest);
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
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Assign to Lawyer, Change Request and File status, save history and Create Case File with parties and docs from Case Request</History>
        public async Task<Guid?> AssignConsultationToLawyer(ConsultationAssignment consultationfileAssignment)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        Guid? referenceId = null;
                        try
                        {
                            if ((int)AssignCaseToLawyerTypeEnum.ConsultationRequest == consultationfileAssignment.AssignConsultationLawyerType)
                            {
                                ConsultationRequest consultationRequest = await _dbContext.ConsultationRequests.FindAsync((Guid)consultationfileAssignment.ConsultationRequestId);
                                consultationRequest.ApprovedBy = consultationfileAssignment.CreatedBy;
                                consultationRequest.ReviewedBy = consultationfileAssignment.CreatedBy;
                                await _consultationRequestRepository.UpdateConsultationRequest(consultationRequest, _dbContext);
                                await _consultationRequestRepository.UpdateConsultationRequestStatus(consultationfileAssignment.CreatedBy, (Guid)consultationfileAssignment.ConsultationRequestId, (int)CaseRequestStatusEnum.ConvertedToFile, (int)CaseRequestEventEnum.AssignToLawyer, _dbContext);
                                var file = await _consultationRequestRepository.CreateConsultationfile((Guid)consultationfileAssignment.ConsultationRequestId, consultationfileAssignment.CreatedBy, consultationfileAssignment.FatwaPriorityId, _dbContext , (int)CaseFileStatusEnum.AssignToLawyer);
                                await _consultationRequestRepository.SaveConsultationFileStatusHistory(file, (int)CaseFileEventEnum.Created, _dbContext);

                                consultationfileAssignment.FileId = file.FileId;
                                //await _consultationRequestRepository.CopyConsultationAttachmentsFromSourceToDestination((Guid)consultationfileAssignment.ConsultationRequestId, file.FileId, consultationfileAssignment.CreatedBy, _dbContext);
                                // Copy Consultation Attachments From Source To Destination
                                consultationfileAssignment.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                                {
                                    SourceId = (Guid)consultationfileAssignment.ConsultationRequestId,
                                    DestinationId = (Guid)file.FileId,
                                    CreatedBy = consultationfileAssignment.CreatedBy
                                });
                                // copy of CopyConsultation Parties From Source To Destination
                                var copyAttachments = await _consultationRequestRepository.CopyConsultationPartiesFromSourceToDestination((Guid)consultationfileAssignment.ConsultationRequestId, file.FileId, consultationfileAssignment.CreatedBy, _dbContext);
                                if (copyAttachments.Any())
                                {
                                    consultationfileAssignment.CopyAttachmentVMs.AddRange(copyAttachments);
                                }
                                await SaveConsultationAssignmentToLawyer(file.FileId, consultationfileAssignment, _dbContext);
                                await _dbContext.SaveChangesAsync();
                                transaction.Commit();
                                referenceId = file.FileId;
                                // For Notification 
                                consultationfileAssignment.NotificationParameter.FileNumber = file.FileNumber;
                                consultationfileAssignment.NotificationParameter.ReferenceNumber = consultationRequest.RequestNumber;
                            }
                            else if ((int)AssignCaseToLawyerTypeEnum.ConsultationFile == consultationfileAssignment.AssignConsultationLawyerType)
                            {
                                await _consultationFileRepository.UpdateConsultationFileStatus(consultationfileAssignment.ReferenceId, (int)CaseFileStatusEnum.AssignToLawyer, (int)CaseFileEventEnum.AssignToLawyer, consultationfileAssignment.FatwaPriorityId, _dbContext);
                                await SaveConsultationAssignmentToLawyer(consultationfileAssignment.ReferenceId, consultationfileAssignment, _dbContext);
                                await _dbContext.SaveChangesAsync();
                                transaction.Commit();

                                referenceId = consultationfileAssignment.ReferenceId;
                                // For Notification 
                                var file = await _dbContext.ConsultationFiles.FindAsync(consultationfileAssignment.ReferenceId);
                                consultationfileAssignment.NotificationParameter.FileNumber = file.FileNumber;
                                var requestNumber = _dbContext.ConsultationRequests.Where(c => c.ConsultationRequestId == file.RequestId).FirstOrDefault().RequestNumber;
                                consultationfileAssignment.NotificationParameter.RequestNumber = requestNumber;
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master">Copy Parties and Attachments from Linked Requests of Primary Case Request </History>


        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Save Case Assignees i.e. Lawyers and Supervisors </History>
        public async Task SaveConsultationAssignmentToLawyer(Guid referenceId, ConsultationAssignment consultationRequestLawyerAssignment, DatabaseContext dbcontext)
        {
            try
            {
                dbcontext.ConsultationAssignments.RemoveRange(dbcontext.ConsultationAssignments.Where(p => p.ReferenceId == referenceId));
                await dbcontext.SaveChangesAsync();

                if ((bool)consultationRequestLawyerAssignment.SelectedUsers?.Any())
                {
                    foreach (var lawyer in consultationRequestLawyerAssignment.SelectedUsers)
                    {
                        ConsultationAssignment lawyerObj = new ConsultationAssignment();
                        lawyerObj.ReferenceId = referenceId;
                        lawyerObj.SupervisorId = consultationRequestLawyerAssignment.SupervisorId;
                        lawyerObj.Remarks = consultationRequestLawyerAssignment.Remarks;
                        lawyerObj.LawyerId = lawyer.Id;
                        if (lawyer.Id == consultationRequestLawyerAssignment.PrimaryLawyerId)
                        {
                            lawyerObj.IsPrimary = true;
                        }
                        lawyerObj.CreatedBy = consultationRequestLawyerAssignment.CreatedBy;
                        lawyerObj.CreatedDate = consultationRequestLawyerAssignment.CreatedDate;

                        await dbcontext.ConsultationAssignments.AddAsync(lawyerObj);
                        await dbcontext.SaveChangesAsync();
                        await SaveConsultationAssignmentHistory(referenceId, consultationRequestLawyerAssignment.CreatedBy, consultationRequestLawyerAssignment.LawyerId, consultationRequestLawyerAssignment.Remarks, dbcontext);


                    }
                }
                else
                {
                    ConsultationAssignment lawyerObj = new ConsultationAssignment();
                    lawyerObj.ReferenceId = referenceId;
                    lawyerObj.SupervisorId = consultationRequestLawyerAssignment.SupervisorId;
                    lawyerObj.Remarks = consultationRequestLawyerAssignment.Remarks;
                    lawyerObj.LawyerId = consultationRequestLawyerAssignment.LawyerId;
                    lawyerObj.IsPrimary = true;
                    lawyerObj.CreatedBy = consultationRequestLawyerAssignment.CreatedBy;
                    lawyerObj.CreatedDate = consultationRequestLawyerAssignment.CreatedDate;
                    await dbcontext.ConsultationAssignments.AddAsync(lawyerObj);
                    await dbcontext.SaveChangesAsync();
                    await SaveConsultationAssignmentHistory(referenceId, consultationRequestLawyerAssignment.CreatedBy, consultationRequestLawyerAssignment.LawyerId, consultationRequestLawyerAssignment.Remarks, dbcontext);

                }
                await SaveConsultationAssignmentHistory(referenceId, consultationRequestLawyerAssignment.CreatedBy, consultationRequestLawyerAssignment.SupervisorId, consultationRequestLawyerAssignment.Remarks, dbcontext);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Approval Tracking Detail

        public async Task<CmsApprovalTracking> GetApprovalTrackingProcess(Guid referenceId, int sectorTypeId, int processTypeId)
        {
            try
            {
                var approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == referenceId && x.SectorTo == sectorTypeId && x.ProcessTypeId == processTypeId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                if (approvalTracking != null)
                {
                    return approvalTracking;
                }
                else
                {
                    return new CmsApprovalTracking();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Send Copy

        public async Task ApproveSendACopy(dynamic Item, int TransferCaseType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                        {
                            ConsultationRequest paramCaseRequest = System.Text.Json.JsonSerializer.Deserialize<ConsultationRequest>(Item);
                            ConsultationRequest newCaseRequest = await _consultationRequestRepository.CopyConsultationRequest(paramCaseRequest, _dbContext);
                            await _consultationRequestRepository.CopyConsultationPartiesFromSourceToDestination(paramCaseRequest.ConsultationRequestId, newCaseRequest.ConsultationRequestId, paramCaseRequest.CreatedBy, _dbContext);
                            await _consultationRequestRepository.CopyConsultationAttachmentsFromSourceToDestination(paramCaseRequest.ConsultationRequestId, newCaseRequest.ConsultationRequestId, paramCaseRequest.CreatedBy, _dmsDbContext);
                            await _consultationRequestRepository.SaveConsultationRequestStatusHistory(paramCaseRequest.CreatedBy, paramCaseRequest, (int)CaseRequestEventEnum.SentCopy, _dbContext);
                            //await UpdateApprovalTrackingStatus(paramCaseRequest.ConsultationRequestId, paramCaseRequest.CreatedBy, (int)paramCaseRequest.SectorTypeId, (int)ApprovalProcessTypeEnum.SendaCopy, (int)ApprovalStatusEnum.Approved, _dbContext);
                            await UpdateConsultationRequestTransferStatus(paramCaseRequest.ConsultationRequestId, paramCaseRequest.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);
                        }
                        else if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                        {
                            ConsultationFile paramConsultationFile = System.Text.Json.JsonSerializer.Deserialize<ConsultationFile>(Item);
                            ConsultationFile oldConsultationFile = await _dbContext.ConsultationFiles.FindAsync(paramConsultationFile.FileId);
                            ConsultationFile newConsultationFile = await _consultationFileRepository.CopyConsultationFile(oldConsultationFile.FileId, oldConsultationFile.CreatedBy, _dbContext);
                            await _consultationRequestRepository.CopyConsultationAttachmentsFromSourceToDestination(oldConsultationFile.FileId, newConsultationFile.FileId, paramConsultationFile.CreatedBy, _dmsDbContext);
                            await _consultationRequestRepository.SaveConsultationFileSectorAssignmentFile(newConsultationFile.FileId, (int)paramConsultationFile.SectorTypeId, paramConsultationFile.CreatedBy, _dbContext);
                            //await UpdateApprovalTrackingStatus(paramConsultationFile.FileId, paramConsultationFile.CreatedBy, (int)paramConsultationFile.SectorTypeId, (int)ApprovalProcessTypeEnum.SendaCopy, (int)ApprovalStatusEnum.Approved, _dbContext);
                            await UpdateConsultationFileTransferStatus(paramConsultationFile.RequestId, paramConsultationFile.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);
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



        public async Task RejectSendACopy(dynamic Item, int TransferCaseType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                        {
                            var consultationRequest = System.Text.Json.JsonSerializer.Deserialize<ConsultationRequest>(Item);
                            //await UpdateApprovalTrackingStatus(consultationRequest.ConsultationRequestId, consultationRequest.CreatedBy, consultationRequest.SectorTypeId, (int)ApprovalProcessTypeEnum.SendaCopy, (int)ApprovalStatusEnum.Rejected, _dbContext);
                            await UpdateConsultationRequestTransferStatus(consultationRequest.ConsultationRequestId, consultationRequest.CreatedBy, (int)ApprovalStatusEnum.Rejected, _dbContext);
                        }
                        else if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                        {
                            var consultationFile = System.Text.Json.JsonSerializer.Deserialize<ConsultationFile>(Item);
                            //await UpdateApprovalTrackingStatus(consultationFile.FileId, consultationFile.CreatedBy, consultationFile.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Rejected, _dbContext);
                            await UpdateConsultationFileTransferStatus(consultationFile.RequestId, consultationFile.CreatedBy, (int)ApprovalStatusEnum.Rejected, _dbContext);
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

        #region  Update Consultation File Transfer Status
        public async Task<bool> UpdateConsultationFileTransferStatus(Guid? referenceId, string? createdBy, int taskStatusId, DatabaseContext dbContext)
        {

            bool isSaved = false;
            try
            {
                ConsultationFile consultationFile = await dbContext.ConsultationFiles.FirstOrDefaultAsync(x => x.FileId == referenceId);
                if (consultationFile is not null)
                {
                    consultationFile.TransferStatusId = taskStatusId;
                    consultationFile.ModifiedBy = createdBy;
                    consultationFile.ModifiedDate = DateTime.Now;
                    dbContext.ConsultationFiles.Update(consultationFile);
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

        #region Approve Consultation file 
        public async Task<ConsultationFileHistory> ApproveConsultationFile(ConsultationFileDetailVM item)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        ConsultationFile file = await _dbContext.ConsultationFiles.FindAsync(item.FileId);
                        if (file != null)
                        {
                            file.StatusId = (int)CaseFileStatusEnum.InProgress;
                        }
                        _dbContext.ConsultationFiles.Update(file);
                        await _dbContext.SaveChangesAsync();
                        var historyobj = await _consultationFileRepository.SaveConsultationFileStatusHistoryReturn(file, (int)CaseFileEventEnum.AssignToLawyer, _dbContext);
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
        public async Task<ConsultationAssignment> GetConsultationAssigmentByLawyerIdAndFileId(Guid FileId, string UserId)

        {
            try
            {
                if (_consultationAssignment == null)
                {
                    _consultationAssignment = await _dbContext.ConsultationAssignments.Where(x => x.ReferenceId == FileId && x.LawyerId == UserId).FirstOrDefaultAsync();

                }
                if (_consultationAssignment != null)
                {
                    return _consultationAssignment;
                }
                else
                {
                    return new ConsultationAssignment();

                }

            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task AssignConsultationBackToHos(CmsAssignCaseFileBackToHos assignBackToHos)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.CmsAssignCaseFileBackToHos.AddAsync(assignBackToHos);
                        await _dbContext.SaveChangesAsync();
                        await UpdateConsultationFileAssignedBackToHos(assignBackToHos.ReferenceId, true, assignBackToHos.CreatedBy);
                        var file = await _dbContext.ConsultationFiles.FindAsync(assignBackToHos.ReferenceId);
                        if (file is not null)
                        {

                            await UpdateConsultationFileStatus(file, (int)CaseFileStatusEnum.RejectedByLawyer, _dbContext);
                            await SaveConsultationFileStatusHistory(assignBackToHos.CreatedBy, file, (int)CaseFileEventEnum.AssignedBackToHos, _dbContext);
                            var taskstatus = await _taskRepository.DeleteTask(assignBackToHos.TaskId);
                            var ConsultationfileAssigment = await _dbContext.ConsultationAssignments.Where(x => x.ReferenceId == file.FileId && x.LawyerId == assignBackToHos.TaskUserId).FirstOrDefaultAsync();
                            if (ConsultationfileAssigment != null)
                            {
                                _dbContext.Entry(ConsultationfileAssigment).State = EntityState.Deleted;
                            }
                            await _dbContext.SaveChangesAsync();
                            await SaveConsultationFileStatusHistory(assignBackToHos.CreatedBy, file, (int)CaseFileEventEnum.AssignedBackToHos, _dbContext);
                        }
                        transaction.Commit();

                        // For Notification 
                        assignBackToHos.NotificationParameter.Entity = new ConsultationFile().GetType().Name;
                        assignBackToHos.NotificationParameter.FileNumber = file.FileNumber;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        #region Update Case File IsAssignedBack to Hos Status

        public async Task<bool> UpdateConsultationFileAssignedBackToHos(Guid referenceId, bool IsAssignedBack, string createdBy)
        {
            bool isSaved = false;
            try
            {

                ConsultationFile? consultationFile = await _dbContext.ConsultationFiles.Where(x => x.FileId == referenceId).FirstOrDefaultAsync();
                if (consultationFile is not null)
                {
                    consultationFile.IsAssignedBack = IsAssignedBack;
                    consultationFile.ModifiedBy = createdBy;
                    consultationFile.ModifiedDate = DateTime.Now;
                    _dbContext.ConsultationFiles.Update(consultationFile);
                    await _dbContext.SaveChangesAsync();
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

        public async Task UpdateConsultationFileStatus(ConsultationFile file, int StatusId, DatabaseContext dbContext)
        {
            try
            {
                file.StatusId = StatusId;
                dbContext.ConsultationFiles.Update(file);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task SaveConsultationFileStatusHistory(string userName, ConsultationFile consultationFile, int EventId, DatabaseContext dbContext)
        {
            try
            {
                ConsultationFileHistory historyobj = new ConsultationFileHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.FileId = consultationFile.FileId;
                historyobj.StatusId = consultationFile.StatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = userName;
                historyobj.EventId = EventId;
                historyobj.Remarks = null;
                await dbContext.ConsultationFileHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
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
        public async Task SaveConsultationAssignmentHistory(Guid referenceId, string userId, string assigneeId, string remarks, DatabaseContext dbcontext)
        {
            try
            {
                ConsultationAssignmentHistory historyObj = new ConsultationAssignmentHistory();
                historyObj.ReferenceId = referenceId;
                historyObj.AssigneeId = assigneeId;
                historyObj.Remarks = remarks;
                historyObj.CreatedBy = userId;
                historyObj.CreatedDate = DateTime.Now;
                await dbcontext.ConsultationAssignmentHistorys.AddAsync(historyObj);
                await dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task LinkEntityWithActiveWorkflow(CmsApprovalTracking approvalTracking, DatabaseContext dbContext, int transferType)
        {
            try
            {
                int moduleTriggerId = 0;
                if (approvalTracking.IsConfidential)
                {
                    moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequest : (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationFile;
                }
                else
                {
                    moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)WorkflowModuleTriggerEnum.TransferConsultationRequest : (int)WorkflowModuleTriggerEnum.TransferConsultationFile;
                }
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{(int)WorkflowModuleEnum.COMSConsultationManagement}', @moduleTriggerId = '{moduleTriggerId}'";
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

    }
}

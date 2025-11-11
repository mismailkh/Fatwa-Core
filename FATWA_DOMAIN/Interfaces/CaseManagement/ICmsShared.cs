using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master">Shared Interface For Case Management</History> -->
    public interface ICmsShared
    {
        Task<string> SendExecutionRequestToMOJExecution(MojExecutionRequest executionRequest);
        Task ApproveExecutionRequest(MojExecutionRequest executionRequest);
        Task RejectExecutionRequest(MojExecutionRequest executionRequest);
        Task<CmsCaseFileStatusHistory> ApproveTransferSector(dynamic Item, int TransferCaseType);
        Task RejectTransferSector(dynamic Item, int TransferCaseType);
        //Task<dynamic> ApproveSendACopy(dynamic Item, int TrnsferCaseType);
        Task RejectSendACopy(dynamic Item, int TransferCaseType);
        Task SaveApprovalTrackingProcess(CmsApprovalTracking approvalTracking);
        Task SaveApprovalTrackingProcessForCivilPartialUrgentSector(CmsApprovalTracking approvalTracking, int TransferCaseType);
        Task SaveApprovalTrackingProcessForAssign(CmsApprovalTracking approvalTracking);
        Task<CmsApprovalTracking> GetApprovalTrackingProcess(Guid referenceId, int sectorTypeId, int processTypeId);
        Task<Guid?> AssignCaseToLawyer(CaseAssignment caseAssignment);
        //Task AssignCaseFileBackToHos(CmsAssignCaseFileBackToHos caseAssignment);
        Task<GovernmentEntityRepresentative> CreateGeRepresentative(GovernmentEntityRepresentative geRepresentative);
        Task<bool> AssignDecisionRequestToLawyer(CmsCaseDecisionAssignee casedecisionAssignment);
        Task<int> GetGovtEnityByReferencId(Guid ReferenceId, int SubModulId);
        Task<List<RequestListVM>> GetCaseConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchVM);

        Task<CmsAssignCaseFileBackToHos> GetSendBackToHosByReferenceId(Guid ReferenceId, string LawyerId);
        Task<CaseAssignment> GetCaseAssigmentByLawyerIdAndFileId(Guid FileId, string UserId);
        Task<CmsCaseFileStatusHistory> ApproveCaseFile(CmsCaseFileDetailVM item);
        Task<List<CmsTransferHistoryVM>> GetCMSTransferHistory(string RequestId);
        Task<List<CasePartyLinkExecutionVM>> GetCasePartiesByCaseIdForExecution(Guid Id);
        Task<List<string>> GetCaseAssignmentListByReferenceId(Guid Id);
        Task<List<SectorUsersVM>> GetUsersByRoleAndSector(string RoleId, int? SectorTypeId);
        Task<List<SectorUsersVM>> GetSectorUsersList(string RoleId, int? SectorTypeId, int? pageNumber, int? pageSizes, string UserId);
        Task AddCaseFileTransferRequest(CmsCaseFileTranferRequest cmsCaseFileTranferRequest);
        Task RejectCaseFileTransferRequest(CmsCaseFileTransferRequestDetailVM cmsCaseFileTransferRequestDetail);
        Task UpdateCaseFileTransferRequestForStatus(CmsCaseFileTransferRequestDetailVM cmsCaseFileTransferRequestDetail);
        Task<List<CmsCaseFileTransferRequestVM>> GetCaseFileTransferRequestList(int sectorTypeId);
        Task<CmsCaseFileTransferRequestDetailVM> GetCaseFileTransferRequestDetailById(Guid ReferenceId);
    }
}

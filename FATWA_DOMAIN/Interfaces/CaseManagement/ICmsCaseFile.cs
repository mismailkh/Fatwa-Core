using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.CaseManagement
{
    public interface ICmsCaseFile
    {
        Task<List<CmsCaseFileVM>> GetAllCmsCaseFile(AdvanceSearchCmsCaseFileVM advanceSearch);
        Task<List<CmsCaseFileDmsVM>> GetAllCaseFilesBySector(int sectorTypeId, string userId);
        Task<List<RegisteredCaseFileVM>> GetRegisteredCaseFile(AdvanceSearchCmsCaseFileVM advanceSearch);
        Task<List<CmsRegisteredCaseFileDetailVM>> GetAllRegisteredCasesByFileId(Guid fileId, bool? isFinal);
        Task<List<CmsRegisteredCaseVM>> GetExecutionCasesByFileId(Guid fileId);
        Task<CmsCaseFileDetailVM> GetCaseFileDetailByIdVM(Guid fileId, string userName);
        Task<CaseFile> GetCaseFileById(Guid fileId);
        Task<CaseRequest> GetCaseRequestByFileId(Guid fileId);
        Task<CaseFile> CaseFileDetailWithPartiesAndAttachments(Guid fileId);
        Task<List<CmsCaseAssigneesHistoryVM>> GetCaseAssigmentHistory(Guid referenceId);
        Task<List<CaseAssignment>> GetCaseAssigment(Guid referenceId);
        Task<List<CmsCaseAssigneeVM>> GetCaseAssigeeList(Guid referenceId);
        Task<List<CmsCaseFileStatusHistoryVM>> GetCMSCaseFileStatusHistory(Guid fileId);
        Task <bool> CreateCaseParty(CasePartyLinkVM party);
        Task CreateMojExecutionRequest(MojExecutionRequest request);
        Task DeleteCaseParty(CasePartyLinkVM party);
        Task<CmsCaseFileStatusHistory> CreateMojRegistrationRequest(List<MojRegistrationRequest> registrationRequestList);
        Task<List<MojRegistrationRequestVM>> GetMojRegistrationRequests(int? sectorTypeId, bool? IsRegistered, int? pageNumber, int? pageSize);
        Task<List<MojDocumentPortfolioRequestVM>> GetMojDocumentPortfolioRequests(int? sectorTypeId, int pageNumber, int pageSize);
        Task<MojRegistrationRequest> GetMojRegistrationRequestById(Guid id);
        //Task LinkCaseFiles(LinkCaseFilesVM linkCaseFile);
        // updated LinkCaseFiles
        Task<LinkCaseFilesVM> LinkCaseFiles(LinkCaseFilesVM linkCaseFile);
        Task<bool> UpdateCaseFileTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId, int TransferCaseType);
        Task<bool> UpdateCaseFileTransferStatus(CmsApprovalTracking approvalTracking, int TransferCaseType);
       
        Task<CaseAssignment> GetPrimaryCaseAssignmentByCaseId(Guid caseId);
        Task<List<CmsCaseFileVM>> GetLinkedFilesByPrimaryFileId(Guid RequestId);
        Task<CmsCaseFileStatusHistoryVM> GetCaseFileHistoryDetailByHistoryId(Guid historyId); 
        Task<List<MojExecutionRequestVM>> GetMojExecutionRequests(string username, int? pageNumber, int? pageSize);
        Task AssignCaseFileBackToHos(CmsAssignCaseFileBackToHos caseAssignment);
        //Task<bool> UpdatCaseFileaAssignedBackToHos(Guid referenceId, bool IsAssignedBack, string createdBy);
        Task<bool> UpdateCaseFileIsAssigned(Guid fileId,bool IsAssignedBack);
        Task UpdateCaseFileStatusandAddHistory(Guid FileId, string UserName);
        Task<List<CasePartyLink>> GetCMSCasePartyDetailByGuid(Guid Id);
        Task ProcessCaseFile(Guid MojRegistrationRequestId, string CreatedBy);
        Task<List<UserBasicDetailVM>> GetLawyersByCaseAndCanNumber(string caseNumber, string canNumber);
        Task<List<UserBasicDetailVM>> GetAllLawyersBySectorId(int sectorId);
        Task<List<CmsCaseFileSectorAssignment>> GetCaseFileSectorAssigmentByFileId(Guid fileId, int sectorTypeId);
        Task<CaseRequestCommunicationVM> CreateCaseFile(CaseRequest caseRequest);
        Task<List<CmsDraftedRequestListVM>> GetDraftedCaseRequestList(AdvanceSearchCmsCaseRequestVM advanceSearchVM);
    }
}

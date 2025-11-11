using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Microsoft.EntityFrameworkCore;

namespace FATWA_DOMAIN.Interfaces.CaseManagement
{
    //<!-- <History Author = 'Nabeel Ur Rehman' Date='2022-05-26' Version="1.0" Branch="master">Create interface</History> -->
    public interface ICMSCaseRequest
    {
        Task<List<CmsCaseRequestVM>> GetCMSCaseRequests(AdvanceSearchCmsCaseRequestVM advanceSearchVM);
        Task<List<CmsCaseRequestDmsVM>> GetAllCaseRequestsBySectorTypeId(int sectorTypeId);
        Task<CaseRequestDetailVM> GetCMSCaseRequestsDetailById(string RequestId, int? channelId);
        Task CreateCMSCaseRequest(CaseRequestCommunicationVM caseRequestCommunication);
        Task UpdatCMSeCaseRequest(CaseRequestCommunicationVM caseRequestCommunication);
        Task<List<CmsCaseRequestVM>> GetLinkedRequestsByPrimaryRequestId(Guid RequestId);
        Task<List<CasePartyLinkVM>> GetCMSCasePartyDetailById(string Id);
        Task<CasePartyLinkVM> GetCasePartyDetailById(string Id);
        Task<List<CmsCaseRequestHistoryVM>> GetCMSCaseRequestStatusHistory(string RequestId);
        Task<CmsCaseRequestResponseVM> GetCaseRequestResponsebyRequestId(Guid RequestId, Guid CommunicationId);
        Task<CmsCaseRequestResponseVM> GetFileRequestNeedMoreDetail(Guid FileId, Guid CommunicationId);
        Task<DetailSubCaseVM> GetSubCaseByCaseId(Guid CaseId);
        Task<CaseRequest> GetCaseRequestById(Guid RequestId);
        //Task SendACopyCaseRequest(CaseRequest caseRequest);
        Task LinkCaseRequests(LinkCaseRequestsVM linkCaseRequest);
        Task<bool> UpdateCaseRequestTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId,int TransferCaseType);
        Task<List<CmsWithDrawCaseRequestVM>> GetWithDrawCaseRequestByRequestId(Guid RequestId);
        Task CreateWithDrawCaseRequest(WithdrawRequestCommunicationVM cmsWithdrawRequestCommunication);
        Task<List<UpdateEntityHistoryVM>> UpdateWithdrawCaseRequestStatus(WithdrawRequestDetailVM caseRequest, bool isRejected);
        Task<CmsCaseRequestHistoryVM> GetCaseRequestHistoryDetailByHistoryId(Guid historyId);
		Task<WithdrawRequestDetailVM> GetRequestWithdrawDetailById(Guid RequestId, int CommunicationTypeId);
        Task UpdateCaseRequestViewedStatus(Guid RequestId);
        Task<GovernmentEntity> GetGovtEntityId(int Id);
        Task UpdateTransferHistory(CmsTransferHistoryVM transferHistory);
        Task CommunicationForViceHos(CaseRequestCommunicationVM caseRequestCommunication);

    }
}

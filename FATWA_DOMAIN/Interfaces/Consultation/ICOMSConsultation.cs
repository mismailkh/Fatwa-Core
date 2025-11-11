using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;

namespace FATWA_DOMAIN.Interfaces.Consultation
{
    public interface ICOMSConsultation
    {
        Task<List<ConsultationRequestVM>> GetConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchVM);
        Task<ViewConsultationVM> GetConsultationRequest(Guid consultationId);
        Task<ComsConsultationRequestResponseVM> GetConsultationRequestResponseById(Guid consultationId);
        Task<ComsConsultationRequestResponseVM> GetConsultationFileResponseById(Guid consultationId);
        Task<ConsultationRequest> GetConsultationRequestById(Guid consultationId);
        Task<List<ConsultationPartyListVM>> GetConsultationPartyByConsultationId(Guid consultationId);
        Task<List<ComsConsultationRequestHistoryVM>> GetCOMSConsultationRequestStatusHistory(string ConsultationRequestId);

        Task<List<ConsultationArticleByConsultationIdListVM>> GetConsultationArticleByConsultationId(Guid consultationId);
        Task<bool> UpdateConsultationRequestTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId);
        Task<bool> UpdateConsultationFileTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId);
        Task<List<ConsultationPartyVM>> GetCOMSCosnultationPartyDetailById(string Id);
        Task CreateConsultationRequest(CaseRequestCommunicationVM consultationRequestCommunication);
        Task UpdateConsultationRequest(CaseRequestCommunicationVM consultationRequestCommunication);
        Task CreateConsultationWithDrawRequest(WithdrawRequestCommunicationVM cmsWithdrawRequestCommunication);
        Task<List<ComsWithDrawConsultationRequestVM>> GetWithDrawConsultationRequestByRequestId(Guid RequestId);
        Task<List<UpdateEntityHistoryVM>> UpdateWithdrawConsultationRequestStatus(WithdrawRequestDetailVM caseRequest, bool isRejected);
        Task<List<ConsultationArticleStatus>> GetArticleStatusList();
        Task<int> GetArticleNewNumber(Guid consultationRequestId);
        Task<ConsultationRequest> GetConsultationRequestByReferenceId(Guid consultationRequestId);

        Task<List<ConsultationSection>> GetSectionList();

        Task<List<ConsultationSection>> GetSectionParentList();
        Task<ConsultationFile> GetConsultationFileDetailsByReferenceId(Guid fileId);
        //Task<ComsWithDrawConsultationRequestVM> GetConsultaionRequestWithdrawDetailById(Guid ConsultationRequestId, int CommunicationTypeId);
        Task<List<ConsultationRequestDmsVM>> GetAllConsultationBySectorTypeId(int sectorTypeId);
        Task<List<ConsultationTemplate>> GetConsultationTemplate();
        Task<List<ConsultationTemplateSection>> GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(int templateId);
        Task<CaseRequestCommunicationVM> CreateConsultationRequestFromFatwa(ConsultationRequest consultationRequest);


    }
}
 
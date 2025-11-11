using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;

namespace FATWA_DOMAIN.Interfaces.Communication
{
    public interface ICommunication
	{
        #region Save

        Task<bool> SendCommunication(SendCommunicationVM communication);
        Task<User> getUserIdbyUserName(SendCommunicationVM communication);

        #endregion

        #region Get

        Task<CommunicationVM> GetCommunicationDetailCommunicationId(Guid communicationId);
        Task<Meeting> GetMeetingDetailByUsingCommunicationId(Guid communicationId);
        Task<string> StopExecutionRejectionReason(StopExecutionRejectionReason stopExecutionRejectionReason);
        Task<CommunicationResponseVM> GetCommunicationDetail(string communicationId);
        Task<List<CommunicationListVM>> GetCommunicationListByCaseRequestId(string caseRequestId);
        Task<CommunicationListVM> GetCommunicationDetailByCaseRequestId(string caseRequestId, Guid communicationId);
        Task<List<CommunicationListVM>> GetCommunicationListByConsultationRequestId(string consultationRequestId);
        Task<List<CommunicationListVM>> GetCommunicationListByCaseFileId(string fileId, int CorrespondenceTypeId);
        Task<CommunicationListVM> GetCommunicationDetailByCaseFileId(string fileId, int CorrespondenceTypeId, string communicationId);
        Task<CommunicationListVM> GetCommunicationDetailByCommunicationId(string fileId, int CorrespondenceTypeId, string communicationId);
        Task<CommunicationListVM> GetCommunicationDetailByConsultationFileId(string fileId, int CorrespondenceTypeId, string communicationId);
        Task<CommunicationListVM> GetCommunicationDetailByConsultationRequestId(string consultationRequestId, string communicationId);
        Task<List<CommunicationListVM>> GetCommunicationListByCaseId(string caseId, int communicationTypeId);
        Task<CommunicationListVM> GetCommunicationDetailByCaseId(string caseId, int CorrespondenceTypeId, string communicationId);
        Task<List<CommunicationInboxOutboxVM>> GetInboxOutboxList(int correspondenceType, string userName, int PageSize, int PageNumber, int channelId);
        Task<CmsCaseRequestResponseVM> GetInboxOutboxRequestNeedMoreDetail(Guid CommunicationId);
        Task<List<CommunicationListVM>> GetConslutationFileCommunication(Guid fileId, int CorrespondenceTypeId);
        Task<CommunicationSendResponseVM> CommunicationSendResponseDetailbyId(string CommunicationId);
        Task<CommunicationSendResponseVM> CommunicationSendResponseDetailbyId(string CommunicationId, int CommunicationType);
        Task<CommunicationSendMessageVM> CommunicationSendMessageDetailbyId(string CommunicationId);
        Task<CommunicationMeetingDetailVM> GetMeetingIdCommunitationbyId(string CommunicationId, int CommunicationTypeId);
        Task<List<CommunicationMeetingDetailVM>> GetMeetinglistCommunitationbyId(string CommunicationId);
        Task<CommunicationDetailVM> CommunicationDetailbyComIdAndComType(string ReferenceId, string CommunicationId, int SubModuleId, int CommunicationTypeId);
        Task<SendCommunicationVM> GetCommunicationMeetingDetailCommunicationId(Guid communicationId);
        Task<List<CmsAnnouncementVM>> GetGetAnnouncementsListByCaseId(string caseId);
        Task<List<CorrespondenceHistoryVM>> GetCorrespondenceHistoryByCommunicationId(Guid CommunicationId);
        #endregion

        #region post

        Task SaveCommunicationResponse(CommunicationResponseMoreInfoVM communicationRequestMore);
        Task ForwardCorrespondenceToLawyer(CommunicationHistory communicationHistory);
        Task ForwardCorrespondenceToSector(CommunicationHistory communicationHistory);
        Task AssignBackToHos(CommunicationHistory communicationHistory);
        Task SendBackToSender(CommunicationHistory communicationHistory);
        Task<CommunicationRecipient> CheckUserExistByUserAndCommunicationId(Guid userId, Guid communicationId);
        Task<dynamic> GetInboxOutboxDetailForMobileApp(int communicationTypeId, string communicationId, int linkTargetTypeId, string referenceId, string CultureType);
        Task<List<CommunicationInboxOutboxVM>> GetInboxOutboxListForMobileApp(int correspondenceType, string userName, int top, int channelId);
        #endregion

        #region Task Communication
        Task<TaskCommunication> GetTaskCommunication(SendCommunicationVM sendCommunicationVM);
		#endregion

	}
}

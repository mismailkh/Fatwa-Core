using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.CaseManagement
{
    //<!-- <History Author = 'Ijaz Ahmad' Date='2022-11-29' Version="1.0" Branch="master">Create interface</History> -->
    public interface ICmsRegisteredCase
    {
        Task<CmsRegisteredCase> CreateRegisteredCase(CmsRegisteredCase registeredCase);
        Task CreateMergeRequest(MergeRequest mergeRequest);
        Task ApproveMergeRequest(Guid mergeRequestId, string loggedInUser);
        Task RejectMergeRequest(Guid mergeRequestId);
        Task<List<MergeRequestVM>> GetMergeRequestsForApproval();
        Task<List<CmsRegisteredCaseDmsVM>> GetAllRegisteredCasesByCourtTypeId(int courtTypeId, string userId); 
        Task<CmsRegisteredCaseDetailVM> GetRegisteredCaseDetailByIdVM(Guid caseId, string userId); 
        Task<bool> AddHearing(Hearing hearing);
        Task AddOutcomeHearing(OutcomeHearing outcomeHearing);
        Task<List<HearingVM>> GetHearingsByCase(Guid caseId);
        Task<HearingDetailVM> GetHearingDetail(Guid hearingId);
        Task<Hearing> GetHearingDetailByHearingId(Guid hearingId);
        Task<OutcomeHearingDetailVM> GetOutcomeDetail(Guid outcomeId);
        Task<List<OutcomeHearingVM>> GetOutcomesHearingByCase(Guid caseId);
        Task<CmsJudgmentExecutionDetailVM> GetExecutionDetail(Guid executionId);
        Task<List<JudgementVM>> GetJudgementsByCase(Guid caseId);
        Task<List<JudgementVM>> GetJudgementsByOutcome(Guid outcomeId);
        Task<List<TransferHistoryVM>> GetTransferHistoryByOutcome(Guid outcomeId, Guid CaseId);
		Task<Guid> GetRequestfordocumentbyCaseId(Guid CaseId);
        Task<List<CmsRegisteredCaseStatusHistoryVM>> GetRegisteredCaseStatusHistory(Guid caseId);
        Task<MergeRequestVM> GetMergeRequestDetailById(Guid id);
        Task<List<CmsRegisteredCaseVM>> GetMergedCasesByMergeRequestId(Guid mergeRequestId);
        Task<CmsRegisteredCase> GetRegisteredCaseById(Guid caseId);
        Task<List<CmsRegisteredCaseVM>> GetAllRegisteredCases();
        Task CreateRequestForDocument(MojRequestForDocument item);
        Task CreateSchedulingCourtVists(SchedulingCourtVisits item);
        Task<List<CmsRegisteredCaseVM>> GetSubCasesByCaseId(Guid caseId);
        Task<List<CmsJugdmentDecisionVM>> GetJudgmentDecision(Guid caseId);
        Task<List<CmsRequestDocumentsVM>> GetRequestedDocuments();
        Task<List<CmsRegisteredCaseVM>> GetMergedCasesbyCaseId(Guid caseId);
        Task AddJudgement(Judgement judgement);
        Task AddPostponeHearingRequest(PostponeHearing postponeHearing);
        Task LinkCANs(LinkCANsVM linkCAN);
        Task SaveAndCloseCaseFiles(CmsSaveCloseCaseFile cmsSaveCloseCaseFile);
        Task<List<SchedulingCourtVisitVM>> GetSchedulCourtVisitByHearingId(Guid HearingId);
        Task<List<CmsJudgmentExecutionVM>> GetJudgmentExecutions(Guid caseId);
        Task AddJudgmentExecution(CmsJudgmentExecution cmsJudgmentExecution);
        Task AddExecutionRequest(MojExecutionRequest executionRequest);
        Task EditJudgmentExecution(CmsJudgmentExecution cmsJudgmentExecution);
        Task<CmsJudgmentExecution> GetExecutionById(Guid ExecutionId);
        Task<CmsJudgmentExecution> GetJudgementExecutionByExecutionRequestId(Guid ExecutionRequestId);
        Task<MojExecutionRequest> GetExecutionRequestById(Guid ExecutionId);
        //Task CreateSubCase(CmsRegisteredCase party);
        //Update Createsubcase
        Task<CmsRegisteredCase> CreateSubCase(CmsRegisteredCase party);
        Task<MojRequestForDocument> GetRequestForDocumentById(Guid Id);
        Task UpdateDocumentPortfolioRequest(Guid Id);
        Task<CmsCaseDecision> AddJudgementDecision(CmsCaseDecision cmsCaseDecision);
        Task<CmsJugdmentDecisionVM> GetJudgmentDecisionDetailbyId(Guid decisionId);
        Task<bool> RejectDecision(CmsJugdmentDecisionVM cmsJugdmentDecisionVM);
        Task<bool> ApproveDecision(CmsJugdmentDecisionVM cmsJugdmentDecisionVM); 
        Task<bool> SendDecisionToMoj(CmsJugdmentDecisionVM cmsJugdmentDecisionVM);
        Task<List<CmsJugdmentDecisionVM>> GetJudgmentDecisionList(Guid userId, int pageNumber, int pageSize);
        Task<CmsCaseRequestResponseVM> GetRegisteredCaseNeedMoreDetail(Guid CaseId, Guid CommunicationId);
        Task<User> GetMojBySectorId(int sectorTypeId);
        Task UpdateRegisteredCaseChamberNumber(CMSRegisteredCaseTransferHistoryVM cMSRegisteredCaseTransferHistoryVM);
        Task<CmsJudgementDetailVM> GetJudgementDetailById(Guid judgementId);
        Task<Judgement> GetJudgementDetailByJudgementId(Guid judgementId);
        Task<List<CmsExecutionFileStatus>> GetExecutionFileStatus();
        Task<List<CaseOutcomePartyLinkHistoryVM>> GetCMSCaseOutcomePartyHistoryDetailById(string Id);
        Task<List<OutcomeAndHearingVM>> GetOutcomesAndHearingByCase(Guid caseId);
        Task<List<OutcomeAndHearingVM>> GetHearingsOfLawyer(string LawyerId, bool isPrevious);
        Task<OutcomeHearing> GetOutcomeByOutcomeId(Guid outcomeId);
        Task<List<CmsRegisteredCase>> GetRegisteredCasesByChamberNumberId(int chamberNumberId);
		Task<string>GetFileAndCommunicationTypeInfo(int CommunicationTypeId, Guid ReferenceId);
        Task SaveImportantCase(CaseUserImportant ImportantCase);
        Task DeleteImportantCase(CaseUserImportant ImportantCase);
        Task<List<MobileAppHearingListVM>> GetHearingListForMobileApp(string userId);
        Task<MobileAppHearingDetailVM> GetHearingDetailsForMobileApp(string hearingId);
        Task<List<CmsRegisteredCaseTransferRequestVM>> GetRegisterdCaseTransferRequestList(Guid outcomeId);
        Task RejectRegisteredCaseTransferRequest(CmsRegisteredCaseTransferRequestVM cmsRegisteredCaseTransferRequestVM);
        Task ApproveRegisteredCaseTransferRequest(CmsRegisteredCaseTransferRequestVM cmsRegisteredCaseTransferRequestVM);
        Task<CmsRegisteredCaseTransferRequestVM> GetResgisteredCaseTansferRequestDetailById(Guid ReferenceId);
        Task<bool> SoftDeleteCaseTransferRequest(Guid Id, string userName);
        Task<CaseDetailMOJVM> GetCaseDetailForMOJ(Guid caseId);
    }
}



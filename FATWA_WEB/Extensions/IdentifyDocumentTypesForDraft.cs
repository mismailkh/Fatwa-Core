using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Data;
using FATWA_WEB.Services;
using FATWA_WEB.Services.CaseManagement;
using Microsoft.AspNetCore.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Extensions
{
    //<History Author = 'Hassan Abbas' Date='2023-05-15' Version="1.0" Branch="master"> Document type Identifier for Selecting Draft Popup to show specific document types in DD based on Entity, Entity Journey, Sector etc</History>
    public class IdentifyDocumentTypesForDraft
    {
        public List<AttachmentType> AttachmentTypes { get; set; }
        private CmsCaseFileService cmsCaseFileService;
        private CmsRegisteredCaseService cmsRegCaseService;
        public IdentifyDocumentTypesForDraft(CmsCaseFileService _cmsCaseFileService, CmsRegisteredCaseService _cmsRegCaseService)
        {
            cmsCaseFileService = _cmsCaseFileService;
            cmsRegCaseService = _cmsRegCaseService;
        }
        public async Task<List<AttachmentType>> GetDocumentTypes(dynamic entity, int draftEntityType, int sectorId)
        {
            try
            {
                List<JudgementVM> judgements = new List<JudgementVM>();
                AttachmentTypes = new List<AttachmentType>();

                //Attachment Types for Case File
                if (draftEntityType == (int)DraftEntityTypeEnum.CaseFile)
                {// barfi
                    CmsCaseFileDetailVM caseFile = (CmsCaseFileDetailVM)entity;
                    var SubTypeId = caseFile.CaseRequest.FirstOrDefault().SubTypeId;
                    if (SubTypeId == 0 || SubTypeId == (int)RequestSubTypeEnum.Lawsuit
                        || SubTypeId == (int)RequestSubTypeEnum.ComplaintAgainstDecision || caseFile.ShowClaimStatement)
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.ClaimStatement });
                    else
                    {
                        if (SubTypeId == (int)RequestSubTypeEnum.PerformOrderRequest)
                            AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.PerformOrderNotes });
                        else if (SubTypeId == (int)RequestSubTypeEnum.OrderOnPetitionRequest)
                            AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.OrderOnPetitionNotes });
                        else
                            AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.ClaimStatement });
                    }
                }

                //Attachment Types available anytime during Case Journey
                if (draftEntityType > (int)DraftEntityTypeEnum.RequestNeedMoreInfo && draftEntityType <= (int)DraftEntityTypeEnum.CaseNeedMoreInfo && draftEntityType != (int)DraftEntityTypeEnum.ConsultationFile)
                {
                    //For Execution Sector Only
                    if (sectorId == (int)OperatingSectorTypeEnum.Execution)
                    {
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.GeneralUpdateNotification });
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.ExecutionAdditionalInformationNotification });
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.ExecutionFileOpened });
                    }
                    else
                    {
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.PresentationNotes });
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.GeneralUpdateNotification });
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.AdditionalInformationReminderNotification });
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.CaseClosingDocument });
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.CaseRegisteredNotification });
                    }
                }

                //Attachment Types for NeedMoreInfo
                if (draftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo || draftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || draftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo)
                {
                    AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.AdditionalInformationNotification });
                    AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.CmsLegalNotification });
                }

                if (draftEntityType == (int)DraftEntityTypeEnum.Case)
                {
                    CmsRegisteredCaseDetailVM regCase = (CmsRegisteredCaseDetailVM)entity;
                    judgements = await GetJudgementsByCase(regCase.CaseId);
                }

                //Attachment Types for Case
                if (draftEntityType == (int)DraftEntityTypeEnum.Case)
                {
                    AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.OpenPleadingRequest });
                    AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.HearingDocument });
                    AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.DefenseDocument });
                    AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.PostponeHearingDocument });
                    if (judgements.Any())
                    {
                        if (judgements.Where(j => j.IsFinal).Any())
                        {
                            if (sectorId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases || sectorId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases || sectorId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases || sectorId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
                            {
                                AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.OpinionDocument });
                            }
                            AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.FinalJudgementNotification });
                        }
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.InitialJudgementNotification });
                        AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.SavingFileNotification });
                        if (sectorId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases)
                            AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.RequestForStopExecutionOfJudgment });
                        if (sectorId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                            AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.StopExecutionOfJudgment });
                    }
                }

                if (draftEntityType == (int)DraftEntityTypeEnum.StopExecutionOfJudgment)
                {
                    AttachmentTypes.Add(new AttachmentType { AttachmentTypeId = (int)AttachmentTypeEnum.RequestForStopExecutionOfJudgment });
                }

                return AttachmentTypes;
            }
            catch (Exception ex)
            {
                return AttachmentTypes;
            }
        }

        public async Task<List<JudgementVM>> GetJudgementsByCase(Guid caseId)
        {
            try
            {
                List<JudgementVM> caseJudgements = new List<JudgementVM>();
                ApiCallResponse response;
                response = await cmsRegCaseService.GetJudgementsByCase(caseId);
                if (response.IsSuccessStatusCode)
                {
                    caseJudgements = (List<JudgementVM>)response.ResultData;
                }
                return caseJudgements;
            }
            catch (Exception ex)
            {
                return new List<JudgementVM>();
            }
        }
    }
}

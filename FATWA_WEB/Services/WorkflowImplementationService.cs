using DocumentFormat.OpenXml.Wordprocessing;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_WEB.Data;
using FATWA_WEB.Services.Tasks;
using Newtonsoft.Json;
using Radzen;
using SelectPdf;
using System.Net;
using System.Net.Mail;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_DOMAIN.Enums.WorkflowParameterEnums;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using static SkiaSharp.HarfBuzz.SKShaper;
//using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FATWA_WEB.Services
{
    //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Workflow Implementation Service for executing Activity Method through Reflection</History>
    public static class WorkflowImplementationService
    {
        private static TranslationState translationState;
        private static WorkflowService workflowService;
        private static FileUploadService fileUploadService;
        private static TaskService taskService;
        private static CmsCaseTemplateService cmsCaseTemplateService;
        private static IConfiguration config;
        private static Task<bool> resultInstance { get; set; }
        private static List<CaseTemplate> HeaderFooterTemplates { get; set; }
        private static byte[] FileData { get; set; }
        public static void WorkflowImplementationServiceConfigure(TranslationState _translationState, WorkflowService _workflowService, FileUploadService _fileUploadService, CmsCaseTemplateService _cmsCaseTemplateService, IConfiguration _configuration, TaskService _taskService)
        {
            translationState = _translationState;
            workflowService = _workflowService;
            fileUploadService = _fileUploadService;
            cmsCaseTemplateService = _cmsCaseTemplateService;
            config = _configuration;
            taskService = _taskService;
        }

        #region LDS Document
        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Document activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Lds_ReviewDocument(LegalLegislation document, WorkflowActivityParametersVM param2)
        {
            if (WorkflowParams.LdsReview_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                document.RoleId = param2.Value;
            document.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.InReview;
            document.WorkflowActivityId = (int)param2.WorkflowActivityId;
            var result = workflowService.UpdateDocumentInstance(document);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Publish Document activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Lds_PublishUnpublishDocument(LegalLegislation document, WorkflowActivityParametersVM param2)
        {
            if (WorkflowParams.LdsPublishUnpublish_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                document.RoleId = param2.Value;

            document.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.Approved;

            document.WorkflowActivityId = (int)param2.WorkflowActivityId;
            var result = workflowService.UpdateDocumentInstance(document);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        #endregion

        #region LPS Principle
        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Principle activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Lps_ReviewPrinciple(LLSLegalPrincipleSystem principle, WorkflowActivityParametersVM param2)
        {
            if (WorkflowParams.LpsReview_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                principle.RoleId = param2.Value;

            principle.FlowStatus = (int)PrincipleFlowStatusEnum.InReview;
            principle.WorkflowActivityId = (int)param2.WorkflowActivityId;
            var result = workflowService.UpdatePrincipleInstance(principle);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Publish Principle activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Lps_PublishUnpublishPrinciple(LLSLegalPrincipleSystem principle, WorkflowActivityParametersVM param2)
        {

            if (WorkflowParams.LpsPublishUnpublish_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                principle.RoleId = param2.Value;

            principle.FlowStatus = (int)PrincipleFlowStatusEnum.Approve;

            principle.WorkflowActivityId = (int)param2.WorkflowActivityId;
            var result = workflowService.UpdatePrincipleInstance(principle);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Principle_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        #endregion

        #region General Tasks

        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Send Email activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> SendEmail(WorkflowActivityParametersVM param1)
        {
            try
            {
                var email = "";
                if (WorkflowParams.SendEmail_Email == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                {
                    email = param1.Value;
                }
                if (WorkflowParams.Sla_Email == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                {
                    email = param1.Value;
                }

                //MailMessage message = new MailMessage();
                //SmtpClient smtp = new SmtpClient();
                //message.From = new MailAddress("");
                //message.To.Add(new MailAddress(email));
                //message.Subject = "Testing Workflow";
                //message.IsBodyHtml = false; //to make message body as html  
                //message.Body = "This email is sent for testing purposes.";
                //smtp.Port = 587;
                //smtp.Host = "smtp.gmail.com";
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential("", "");
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Send(message);

                return new WorkflowActivityResponse { Success = true, Message = email != "" ? "Email has been sent successfully to " + email : "Email has been sent successfully" };
            }
            catch (Exception ex)
            {
                return new WorkflowActivityResponse { Success = false, Message = "Something went wrong sending the email." };
            }
        }

        #endregion

        #region Cms review Case Draft


        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Case Draft activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Cms_ReviewDraftDocument(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.CmsReviewDraftDocument_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            if (WorkflowParams.CmsReviewDraftDocument_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;

            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Case Draft activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Cms_ReviewDraftDocumentHOS(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.CmsReviewDraftDocumentHos_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            if (WorkflowParams.CmsReviewDraftDocumentHos_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;

            //draft.DraftedTemplateVersion.StatusId = (int)CaseDraftDocumentStatusEnum.ApproveBySupervisor;
            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_ReviewDraftDocumentViceHOS(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            try
            {
                draft.DraftedTemplateVersion.ReviewerUserId = "";
                draft.DraftedTemplateVersion.ReviewerRoleId = "";
                if (WorkflowParams.CmsReviewDraftDocumentViceHos_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                    draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
                if (WorkflowParams.CmsReviewDraftDocumentViceHos_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                    draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;
                draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
                var result = workflowService.UpdateCaseDraftInstance(draft);
                if (result != null)
                {
                    return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
                }
                else
                {
                    return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_ReviewDraftDocumentGS(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.CmsReviewDraftDocumentGS_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            if (WorkflowParams.CmsReviewDraftDocumentGS_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;

            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_ReviewDraftDocumentPOO(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.CmsReviewDraftDocumentPOO_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            if (WorkflowParams.CmsReviewDraftDocumentPOO_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;

            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_ReviewDraftDocumentLawyer(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.CmsReviewDraftDocumentLawyer_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            if (WorkflowParams.CmsReviewDraftDocumentLawyer_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;

            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }

        #endregion

        #region COnsultation review Case Draft


        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Case Draft activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Coms_ReviewDraftDocument(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.ComsReviewDraftDocument_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            if (WorkflowParams.ComsReviewDraftDocument_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;

            draft.DraftedTemplateVersion.StatusId = (int)CaseDraftDocumentStatusEnum.InReview;
            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }



        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Case Draft activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Coms_ReviewDraftDocumentHOS(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.ComsReviewDraftDocumentHos_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            if (WorkflowParams.ComsReviewDraftDocumentHos_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param2.PKey))
                draft.DraftedTemplateVersion.ReviewerRoleId = param2.Value;

            draft.DraftedTemplateVersion.StatusId = (int)CaseDraftDocumentStatusEnum.ApproveBySupervisor;
            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);

            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }

        #endregion

        #region DMS Document Review
        public static async Task<WorkflowActivityResponse> Lps_ReviewDMSDocument(DmsAddedDocument document, WorkflowActivityParametersVM param1)
        {
            document.DocumentVersion.ReviewerUserId = "";
            document.DocumentVersion.ReviewerRoleId = "";
            if (WorkflowParams.DMSDocumentReview_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                document.DocumentVersion.ReviewerRoleId = param1.Value;
            document.WorkflowActivityId = (int)param1.WorkflowActivityId;
            document.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            var result = workflowService.UpdateDMSDocumentInstance(document);
            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Lps_PublishDMSDocument(DmsAddedDocument document, WorkflowActivityParametersVM param1)
        {
            document.DocumentVersion.ReviewerUserId = "";
            document.DocumentVersion.ReviewerRoleId = "";
            if (WorkflowParams.DMSDocumentPublish_UserRole == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                document.DocumentVersion.ReviewerRoleId = param1.Value;
            document.WorkflowActivityId = (int)param1.WorkflowActivityId;
            document.DocumentVersion.StatusId = (int)DocumentStatusEnum.Published;
            document.DocumentVersion.VersionNo = document.DocumentVersion.VersionNo + 1.00M;
            document.DocumentVersion.CreatedDate = DateTime.Now;
            var result = workflowService.UpdateDMSDocumentInstance(document);
            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> InitiatorDocument_Modification(DmsAddedDocument document, WorkflowActivityParametersVM param1)
        {
            document.DocumentVersion.ReviewerUserId = "";
            document.DocumentVersion.ReviewerRoleId = "";
            if (WorkflowParams.InitiatorDocumentModification_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                document.DocumentVersion.ReviewerUserId = param1.Value;
            document.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateDMSDocumentInstance(document);
            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        #endregion

        #region Cms  Transfer

        //<History Author = 'Muhammad Zaeem' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Case Draft activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Cms_TransferToSector(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            try
            {
                cmsApprovalTracking.ReviewerUserId = "";
                if (WorkflowParams.CmsTransferUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                    cmsApprovalTracking.ReviewerUserId = param1.Value;
                cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
                cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
                cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
                if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                {
                    resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
                }
                else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                {
                    resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);
                }
                if (resultInstance != null)
                {
                    return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
                }
                else
                {
                    return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_TransferToInitiator(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsTransferToInitiatorUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_SendToGS(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendToGSUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_SendToPOO(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendToPOOUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_SendToPOS(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendToPOSUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_TransferToPOButSendToFPForDecision(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1, WorkflowActivityParametersVM param2)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendToFPUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_TransferToInitiatorAndEndflow(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsTransferToInitiatorAndEndflowUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            cmsApprovalTracking.StatusId = (int)ApprovalTrackingStatusEnum.Transfered;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_TransferToRecieverAndEndflow(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsTransferToRecieverAndEndflowUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalTrackingStatusEnum.Transfered;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_TransferToRespectiveSectorAndEndflow(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsTransferToRespectiveSectorAndEndflowUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalTrackingStatusEnum.Transfered;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_ApproveAndWork(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsApproveTransferAndWorkUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Approved;
            cmsApprovalTracking.Remarks = "";
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }

        public static async Task<WorkflowActivityResponse> Cms_TransferToPOAndEndFlow(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsTransferToPOAndEndFlowUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalTrackingStatusEnum.Transfered;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingInstance(cmsApprovalTracking);
            }
            else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                resultInstance = workflowService.UpdateApprovalTrackingConsultationInstance(cmsApprovalTracking);

            }
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Transfered") : translationState.Translate("File_Transfered") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        #endregion

        #region Lawyer Modification & Publish Draft
        public static async Task<WorkflowActivityResponse> Lawyer_ModifyDraft(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.LawModifyDraftDocument_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            var result = workflowService.UpdateCaseDraftInstance(draft);
            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> PublishDraftand_EndFlow(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.PublishDraftandEndFlow_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            draft.DraftedTemplateVersion.OldStatusId = draft.DraftedTemplateVersion.StatusId;
            draft.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Published;
            draft.DraftedTemplateVersion.CreatedBy = draft.userName;
            draft.DraftedTemplateVersion.CreatedDate = DateTime.Now;
            draft.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            draft.DraftedTemplateVersion.OldVersionId = draft.DraftedTemplateVersion.VersionId;
            draft.DraftedTemplateVersion.VersionNumber = Decimal.Add(draft.DraftedTemplateVersion.VersionNumber, 1m);
            draft.DraftedTemplateVersion.VersionId = Guid.NewGuid();
            draft.IsEndofFlow = true;
            PopulatePdfFromHtml(draft.DraftedTemplateVersion.Content, draft);
            //Save Draft Template To Document
            var docResponse = fileUploadService.SaveDraftTemplateToDocument(draft);
            if (docResponse == null)
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                || draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
            {
                List<SendCommunicationVM> sendCommunication = JsonConvert.DeserializeObject<List<SendCommunicationVM>>(draft.Payload);
                var docResponse1 = fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = sendCommunication.Where(x => x.Communication != null).Select(x => x.Communication.CommunicationId).ToList(),
                    CreatedBy = sendCommunication.FirstOrDefault().Communication.CreatedBy,
                    FilePath = config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null,
                    isCommunication = true,
                    LiteratureId = 0,
                    Token = draft.Token
                });
                if (sendCommunication.Count > 0)
                {
                    foreach (var item in sendCommunication)
                    {
                        if (item.SelectedDocuments.Count > 0)
                        {
                            CopySelectedAttachmentsToDestination(item, draft.Token);
                        }
                    }
                }
            }
            var result = workflowService.UpdateCaseDraftInstance(draft);
            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> SignDraftPublishDraftand_EndFlow(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.SignDraftPublishDraftandEndFlow_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            draft.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Published;
            draft.DraftedTemplateVersion.CreatedBy = draft.userName;
            draft.DraftedTemplateVersion.CreatedDate = DateTime.Now;
            draft.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            draft.DraftedTemplateVersion.VersionNumber = Decimal.Add(draft.DraftedTemplateVersion.VersionNumber, 1m);
            draft.DraftedTemplateVersion.VersionId = Guid.NewGuid();
            draft.IsEndofFlow = true;
            if (draft.IsDraftToDocumentConversion)
            {
                PopulatePdfFromHtml(draft.DraftedTemplateVersion.Content, draft);
                //Save Draft Template To Document
                var docResponse = fileUploadService.SaveDraftTemplateToDocument(draft);
                if (docResponse == null)
                {
                    return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
                }
            }
            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                || draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
            {
                List<SendCommunicationVM> sendCommunication = JsonConvert.DeserializeObject<List<SendCommunicationVM>>(draft.Payload);
                var docResponse1 = fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = sendCommunication.Where(x => x.Communication != null).Select(x => x.Communication.CommunicationId).ToList(),
                    CreatedBy = sendCommunication.FirstOrDefault().Communication.CreatedBy,
                    FilePath = config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null,
                    isCommunication = true,
                    LiteratureId = 0,
                    Token = draft.Token
                });
                if (sendCommunication.Count > 0)
                {
                    foreach (var item in sendCommunication)
                    {
                        if (item.SelectedDocuments.Count > 0)
                        {
                            CopySelectedAttachmentsToDestination(item, draft.Token);
                        }
                    }
                }
            }
            var result = workflowService.UpdateCaseDraftInstance(draft);
            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> SignDraftPublishDraftSendtoG2Gand_EndFlow(CmsDraftedTemplate draft, WorkflowActivityParametersVM param1)
        {
            draft.DraftedTemplateVersion.ReviewerUserId = "";
            draft.DraftedTemplateVersion.ReviewerRoleId = "";
            if (WorkflowParams.SignDraftPublishDraftSendtoG2GandEndFlow_User == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                draft.DraftedTemplateVersion.ReviewerUserId = param1.Value;
            draft.WorkflowActivityId = (int)param1.WorkflowActivityId;
            draft.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Published;
            draft.DraftedTemplateVersion.CreatedBy = draft.userName;
            draft.DraftedTemplateVersion.CreatedDate = DateTime.Now;
            draft.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            draft.DraftedTemplateVersion.VersionNumber = Decimal.Add(draft.DraftedTemplateVersion.VersionNumber, 1m);
            draft.DraftedTemplateVersion.VersionId = Guid.NewGuid();
            if (config["Environment"] == "DS" || config["Environment"] == "PROD")
            {
                draft.IsG2GSend = draft.IsDraftSigned ? true : false;
            }
            else
            {
                draft.IsG2GSend = true;
            }
            draft.IsEndofFlow = true;
            draft.CommunicationId = Guid.NewGuid();
            if(draft.IsDraftToDocumentConversion)
            {
                PopulatePdfFromHtml(draft.DraftedTemplateVersion.Content, draft);
                //Save Draft Template To Document
                var docResponse = fileUploadService.SaveDraftTemplateToDocument(draft);
                if (docResponse == null)
                {
                    return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
                }
            }
            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                || draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
            {
                if (draft.Payload != null)
                {
                    List<SendCommunicationVM> sendCommunication = JsonConvert.DeserializeObject<List<SendCommunicationVM>>(draft.Payload);
                    if (sendCommunication.Any(x => x.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.FinalDocument))
                    {
                        var res = taskService.CompleteAssignTask((Guid)draft.ReferenceId, draft.Token);
                    }
                    var docResponses = fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                    {
                        RequestIds = sendCommunication.Where(x => x.Communication != null).Select(x => x.Communication.CommunicationId).ToList(),
                        CreatedBy = sendCommunication.FirstOrDefault().Communication.CreatedBy,
                        FilePath = config.GetValue<string>("dms_file_path"),
                        DeletedAttachementIds = null,
                        isCommunication = true,
                        LiteratureId = 0,
                        Token = draft.Token
                    });
                    if (sendCommunication.Count > 0)
                    {
                        foreach (var item in sendCommunication)
                        {
                            if (item.SelectedDocuments.Count > 0)
                            {
                                CopySelectedAttachmentsToDestination(item, draft.Token);
                            }
                        }
                    }
                }
            }
            Task.Delay(2000);
            var result = workflowService.UpdateCaseDraftInstance(draft);
            if (result != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = translationState.Translate("Document_Submitted") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task PopulatePdfFromHtml(string TemplateContent, CmsDraftedTemplate draft)
        {
            try
            {
                await PopulateHeaderFooter();
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter options
                if (draft.AttachmentTypeId != (int)AttachmentTypeEnum.ContractReview)
                {
                    converter.Options.DisplayHeader = true;
                    converter.Options.DisplayFooter = true;
                    converter.Header.DisplayOnFirstPage = true;
                    converter.Header.DisplayOnOddPages = true;
                    converter.Header.DisplayOnEvenPages = true;
                    converter.Header.Height = 100;
                    converter.Footer.Height = 50;
                    converter.Footer.DisplayOnFirstPage = true;
                    converter.Footer.DisplayOnOddPages = true;
                    converter.Footer.DisplayOnEvenPages = true;
                    string headerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Content).FirstOrDefault();
                    string footerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Content).FirstOrDefault();
                    PdfHtmlSection headerHtml = new PdfHtmlSection(headerHtmlContent, "");
                    PdfHtmlSection footerHtml = new PdfHtmlSection(footerHtmlContent, "");
                    headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Header.Add(headerHtml);
                    converter.Footer.Add(footerHtml);
                }
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;
                converter.Options.MarginRight = 30;
                converter.Options.MarginLeft = 30;
				converter.Options.EmbedFonts = true;
				TemplateContent = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", TemplateContent);
				SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(TemplateContent);
                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                FileData = stream.ToArray();
                draft.FileData = FileData;
            }
            catch (Exception ex)
            {
                throw new Exception(translationState.Translate("Something_Went_Wrong"));
            }
        }
        public static async Task CopySelectedAttachmentsToDestination(SendCommunicationVM sendCommunication, string token)
        {
            try
            {
                CopySelectedAttachmentsVM copyAttachments = new CopySelectedAttachmentsVM
                {
                    SelectedDocuments = sendCommunication.SelectedDocuments.ToList(),
                    DestinationId = sendCommunication.Communication.CommunicationId,
                    CreatedBy = sendCommunication.Communication.CreatedBy,
                    Token = token
                };
                var docResponse = await fileUploadService.CopySelectedAttachmentsToDestination(copyAttachments);
            }
            catch (Exception)
            {
                throw new Exception(translationState.Translate("Something_Went_Wrong"));
            }
        }
        public static async Task PopulateHeaderFooter()
        {
            var response = await cmsCaseTemplateService.GetHeaderFooter();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }
        #endregion

        #region Cms Send Copy

        //<History Author = 'Muhammad Zaeem' Date='2022-06-20' Version="1.0" Branch="master"> Method for executing Review Case Draft activity through Reflection</History>
        public static async Task<WorkflowActivityResponse> Cms_SendCopyToSector(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            try
            {
                cmsApprovalTracking.ReviewerUserId = "";
                if (WorkflowParams.CmsSendCopyUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                    cmsApprovalTracking.ReviewerUserId = param1.Value;
                cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
                cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
                cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
                resultInstance = workflowService.UpdateCopyTrackingInstance(cmsApprovalTracking);
                if (resultInstance != null)
                {
                    return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Copy_Sent") : translationState.Translate("File_Copy_Sent") };
                }
                else
                {
                    return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_SendCopyToInitiator(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendCopyToInitiatorUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            resultInstance = workflowService.UpdateCopyTrackingInstance(cmsApprovalTracking);
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Copy_Sent") : translationState.Translate("File_Copy_Sent") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_SendCopyToGS(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendCopyToGSUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Pending;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            resultInstance = workflowService.UpdateCopyTrackingInstance(cmsApprovalTracking);
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Copy_Sent") : translationState.Translate("File_Copy_Sent") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_SendCopyToInitiatorAndEndflow(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendCopyToInitiatorAndEndflowUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Approved;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            resultInstance = workflowService.UpdateCopyTrackingInstance(cmsApprovalTracking);
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Copy_Sent") : translationState.Translate("File_Copy_Sent") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_SendCopyToRecieverAndEndflow(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsSendCopyToRecieverAndEndflowUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Approved;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            cmsApprovalTracking.SectorFrom = (int)cmsApprovalTracking.SectorTypeId;
            resultInstance = workflowService.UpdateCopyApprovedTrackingInstance(cmsApprovalTracking);
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Copy_Sent") : translationState.Translate("File_Copy_Sent") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }
        public static async Task<WorkflowActivityResponse> Cms_ApproveCopyAndWork(CmsApprovalTracking cmsApprovalTracking, WorkflowActivityParametersVM param1)
        {
            cmsApprovalTracking.ReviewerUserId = "";
            if (WorkflowParams.CmsApproveCopyAndWorkUser == (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param1.PKey))
                cmsApprovalTracking.ReviewerUserId = param1.Value;
            cmsApprovalTracking.WorkflowActivityId = (int)param1.WorkflowActivityId;
            cmsApprovalTracking.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
            cmsApprovalTracking.StatusId = (int)ApprovalStatusEnum.Approved;
            cmsApprovalTracking.Remarks = "";
            resultInstance = workflowService.UpdateCopyApprovedTrackingInstance(cmsApprovalTracking);
            if (resultInstance != null)
            {
                return new WorkflowActivityResponse { Success = true, Message = (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest) ? translationState.Translate("Request_Copy_Sent") : translationState.Translate("File_Copy_Sent") };
            }
            else
            {
                return new WorkflowActivityResponse { Success = false, Message = translationState.Translate("Something_Went_Wrong") };
            }
        }

        #endregion
    }
}

using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.Shared
{
    public partial class ComsRequestForMoreInformation : ComponentBase
    {
        #region parameter

        [Parameter]
        public string ReferenceId { get; set; }

        [Parameter]
        public dynamic IsConsultationRequest { get; set; }
        [Parameter]
        public dynamic IsReminder { get; set; }

        [Parameter]
        public dynamic PreCommunicationId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        [Parameter]
        public dynamic? IsFinalDraft { get; set; }

        //public bool? IsFinalDraft { get { return Convert.ToBoolean(IsDraft); } set { IsDraft = value; } }
        public bool IsRequest { get { return Convert.ToBoolean(IsConsultationRequest); } set { IsConsultationRequest = value; } }
        public bool IsReminderFlag { get { return Convert.ToBoolean(IsReminder); } set { IsReminder = value; } }

        #endregion

        #region Variables

        protected int Priority_Id;
        protected int GE_Id;
        protected string typeValidationMsg = "";
        protected string reasonValidationMsg = "";
        protected string dateValidationMsg = "";
      
        protected List<Priority> Priorities { get; set; } = new List<Priority>();
        protected List<ResponseType> ResponseType { get; set; } = new List<ResponseType>();
        public DateTime ResponseDate = DateTime.Now;
        public DateTime NowTime = DateTime.Now;
        protected ViewConsultationVM consultationRequest { get; set; } = new ViewConsultationVM();
        protected ConsultationFileDetailVM consultationFileDetail { get; set; } = new ConsultationFileDetailVM();
        protected List<GovernmentEntity> GovernmentEntities { get; set; } = new List<GovernmentEntity>();
        protected List<Frequency> frequency { get; set; } = new List<Frequency>();
        protected ComsRequestMoreInfoVM comsrequestMoreInfoVM { get; set; } = new ComsRequestMoreInfoVM();
        protected Validations validations { get; set; } = new Validations();
        public string consultationRequestUrl { get; set; }
        public string consultationFileUrl { get; set; }
        int DocType { get; set; } = 0;

        protected List<TempAttachementVM> uploadedAttachment { get; set; } = new List<TempAttachementVM>();
        protected RadzenDataGrid<TempAttachementVM>? upload = new RadzenDataGrid<TempAttachementVM>();
        protected bool isUploaded { get; set; } = true;

        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();

        protected SendCommunicationVM sendCommunication = new SendCommunicationVM()
        {
            Communication = new Communication(),
            CommunicationResponse = new CommunicationResponse(),
            CommunicationTargetLink = new CommunicationTargetLink(),
            LinkTarget = new List<LinkTarget>(),
        };

        LinkTarget linkTarget { get; set; }
        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected ObservableCollection<TempAttachementVM> attachments { get; set; }
        public bool allowRowSelectOnRowClick = true;
        #endregion

        #region LOAD

        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        protected override async Task OnInitializedAsync()
        {
            sendCommunication.Communication.CommunicationId = Guid.NewGuid();
            sendCommunication.CommunicationResponse.PriorityId = (int)PriorityEnum.Low;
            if (!IsRequest)
            {
                var sectorId = loginState.UserDetail.SectorTypeId;
                consultationFileUrl = "/consultationfile-view/" + ReferenceId + "/" + sectorId;
                await GetConsultationFileDetailById(Guid.Parse(ReferenceId),false);
            }
            else
            {
                await PopulateConsultationRequestGrid(Guid.Parse(ReferenceId));
                consultationRequestUrl = "/consultationrequest-detail/" + ReferenceId + "/" + consultationRequest.SectorTypeId;
                await PopulateResponseTypes();
                await PopulateAttachmentTypes();
            }
            await PopulateAttachementsGrid();
        }
        //<History Author = 'Muhammad Zaeem'  Version="1.0" Branch="master"> </History>
        protected void PopulateLinkTargets()
        {
            linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = sendCommunication.Communication.CommunicationId,
                IsPrimary = false,
                LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
            };
            sendCommunication.LinkTarget.Add(linkTarget); linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = sendCommunication.CommunicationResponse.CommunicationResponseId,
                IsPrimary = false,
                LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
            };
            sendCommunication.LinkTarget.Add(linkTarget); linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = Guid.Parse(ReferenceId),
                IsPrimary = true,
                LinkTargetTypeId = !IsRequest ? (int)LinkTargetTypeEnum.ConsultationFile : (int)LinkTargetTypeEnum.ConsultationRequest
            };
            sendCommunication.LinkTarget.Add(linkTarget);
        }
        #endregion

        #region Validation
        protected class Validations
        {
            public string reason { get; set; } = string.Empty;
            public string types { get; set; } = string.Empty;
            public string dueDate { get; set; } = string.Empty;

        }
        protected bool ValidateBasicDetailsOnChange()
        {
            bool basicDetailsValid = true;
            if (sendCommunication.CommunicationResponse.ResponseTypeId == null)
            {
                typeValidationMsg = @translationState.Translate("Required_Field");
                validations.types = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.types = "k-valid";
            }

            if (basicDetailsValid != true)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });


            }
            return basicDetailsValid;
        }
        #endregion

        #region Button Click
        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        public async Task ButtonSaveClick()
        {

            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
            {
                DocType = (int)AttachmentTypeEnum.ComsLegalAdvice;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
            {
                DocType = (int)AttachmentTypeEnum.ComsLegisltation;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
            {
                DocType = (int)AttachmentTypeEnum.comsInternationArbitration;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
            {
                DocType = (int)AttachmentTypeEnum.ContractReview;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
            {
                DocType = (int)AttachmentTypeEnum.ComsAdministrativeComplaints;
            }
            typeValidationMsg = "";
            reasonValidationMsg = "";

            bool res = ValidateBasicDetailsOnChange();
            if (res != false)
            {
                bool? dialogResponse = await dialogService.Confirm(
                translationState.Translate("Sure_Save_Changes"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = @translationState.Translate("OK"),
                    CancelButtonText = @translationState.Translate("Cancel")
                });
                if (dialogResponse == true)
                {
                    await FillCommunicationModel();

                    if (DocType == (int)AttachmentTypeEnum.ContractReview)
                    {
                        int AttachmentTypeId = sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.FinalDocument ? DocType : (int)AttachmentTypeEnum.ComsLegalNotification;
                        int ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement;
                        int TemplateId = 0;
                        await GetConsultationFileDetailById(Guid.Parse(ReferenceId),true);
                        if (consultationFileDetail != null)
                        {
                            TemplateId = (int)consultationFileDetail.TemplateId;
                        }
                        else
                        {
                            TemplateId = (int)CaseTemplateEnum.NoTemplate;
                        }
                        TaskId = TaskId != null ? TaskId : null;

                        dataCommunicationService.draftEntityData.Payload = Newtonsoft.Json.JsonConvert.SerializeObject(sendCommunication);
                        dataCommunicationService.draftEntityData.DraftEntityType = IsRequest ? (int)DraftEntityTypeEnum.RequestNeedMoreInfo : (int)DraftEntityTypeEnum.FileNeedMoreInfo;

                        if (TaskId == null)
                            navigationManager.NavigateTo("/create-filedraft/" + ReferenceId + "/" + AttachmentTypeId + "/" + TemplateId + "/" + ModuleId);
                        else
                            navigationManager.NavigateTo("/create-filedraft/" + ReferenceId + "/" + AttachmentTypeId + "/" + TemplateId + "/" + TaskId + "/" + ModuleId);
                    }
                    else
                    {
                        var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
                        new Dictionary<string, object>()
                        {
                            { "ReferenceId", ReferenceId },
                            { "Document_Type",  sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.FinalDocument ? DocType : (int)AttachmentTypeEnum.ComsLegalNotification},
                            { "ModuleId", WorkflowModuleEnum.COMSConsultationManagement },
                            { "DraftEntityType",  IsRequest ? (int)DraftEntityTypeEnum.RequestNeedMoreInfo : (int)DraftEntityTypeEnum.FileNeedMoreInfo},
                            { "Payload", Newtonsoft.Json.JsonConvert.SerializeObject(sendCommunication) },
                            { "ResponseTypeId", (int)sendCommunication.CommunicationResponse.ResponseTypeId },
                            { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(sendCommunication,IsRequest ? (int)DraftEntityTypeEnum.RequestNeedMoreInfo : (int)DraftEntityTypeEnum.FileNeedMoreInfo, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
                            { "TaskId" , TaskId != null ?  TaskId : null }
                        },
                        new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                    }
                }
            }
        }
        #endregion

        #region Get contract template id.
        private async Task GetConsultationFileDetailById(Guid guid, bool fromFunction)
        {
            try
            {
                var result = await consultationFileService.GetConsultationFileDetailById(guid);
               

                if (result.IsSuccessStatusCode)
                {
                    consultationFileDetail = (ConsultationFileDetailVM)result.ResultData;
                    if (!fromFunction)
                    {
                        await PopulatePriorities();
                        await PopulateResponseTypes();
                        await PopulateFrequency();
                        await PopulateAttachmentTypes();
                        await PopulateConsultationRequestGrid(consultationFileDetail.ConsultationRequestId);
                        await PopulateGovernmentEntities();

                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events

        protected async Task PopulatePriorities()
        {
            var response = await lookupService.GetCasePriorities();
            if (response.IsSuccessStatusCode)
            {
                Priorities = (List<Priority>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateGovernmentEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovernmentEntities = (List<GovernmentEntity>)response.ResultData;
                GovernmentEntities = GovernmentEntities?
                     .Where(x => x.EntityId != consultationRequest?.GovtEntityId)
                     .ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateResponseTypes()
        {
            var response = await lookupService.GetResponseTypes();
            if (response.IsSuccessStatusCode)
            {
                ResponseType = (List<ResponseType>)response.ResultData;
                if (!IsRequest)
                {
                    ResponseType = ResponseType.Where(x =>
                                x.Id == (int)ResponseTypeEnum.RequestForMoreInformation
                             || x.Id == (int)ResponseTypeEnum.RequestForMoreInformationReminder
                             || x.Id == (int)ResponseTypeEnum.FinalDocument
                             || x.Id == (int)ResponseTypeEnum.SaveAndCloseFile).ToList();
                             
                }
                if (IsReminderFlag)
                {
                    ResponseType = ResponseType.Where(x => x.Id == (int)ResponseTypeEnum.RequestForMoreInformationReminder).ToList();
                }
                else if (IsFinalDraft != null)
                {
                    ResponseType = ResponseType.Where(x => x.Id == (int)ResponseTypeEnum.FinalDocument).ToList();
                    sendCommunication.CommunicationResponse.ResponseTypeId = (int)ResponseTypeEnum.FinalDocument;   
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected IEnumerable<ResponseType> getExcludedConsultationResponseType = new List<ResponseType>();
        private void ChangeApprovalTypeForAddingResponse(IEnumerable<ResponseType> getResponseType)
        {
            try
            {
                List<ResponseType> approvalTypes = new List<ResponseType>();
                foreach (var item in getResponseType)
                {
                    if (item.Id == (int)SaveAndCloseCaseFileEnum.NeedMoreInformation)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Need_More_Information");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Need_More_Information");
                            approvalTypes.Add(item);
                        }
                    }
                    if (item.Id == (int)SaveAndCloseCaseFileEnum.SaveAndCloseConsultationFile)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Save_And_Close_ConsultationFile");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Save_And_Close_ConsultationFile");
                            approvalTypes.Add(item);
                        }
                    }
                   

                }
                getExcludedConsultationResponseType = approvalTypes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected async Task PopulateFrequency()
        {
            var response = await lookupService.GetFrequency();
            if (response.IsSuccessStatusCode)
            {
                frequency = (List<Frequency>)response.ResultData;


            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateConsultationRequestGrid(Guid consultationId)
        {
            var consultationRequestResponse = await consultationRequestService.GetConsultationDetailById(consultationId);
            if (consultationRequestResponse.IsSuccessStatusCode)
            {
                consultationRequest = (ViewConsultationVM)consultationRequestResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(consultationRequestResponse);
            }
        }
        protected async Task PopulateAttachementsGrid()
        {
            try
            {
                if (ReferenceId != null)
                {
                    attachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(ReferenceId));
                    if (attachments != null && attachments.Any())
                    {
                        Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
                        attachments = new ObservableCollection<TempAttachementVM>(attachments?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Redirect Buttons
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task ButtonBackClick(MouseEventArgs args)
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Fill Communication Model
        protected async Task FillCommunicationModel()
        {
            //Communication
            await CalculateCommunicationType();
            sendCommunication.Communication.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
            sendCommunication.Communication.SourceId = (int)CommunicationSourceEnum.FATWA;
            sendCommunication.Communication.SentBy = loginState.Username;
            sendCommunication.Communication.ReceivedBy = consultationRequest.CreatedBy;
            sendCommunication.Communication.GovtEntityId = consultationRequest.GovtEntityId;
            sendCommunication.Communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
            sendCommunication.Communication.CreatedBy = loginState.Username;
            sendCommunication.Communication.CreatedDate = DateTime.Now;
            sendCommunication.Communication.IsDeleted = false;
            sendCommunication.Communication.OutboxShortNum = 0;
            sendCommunication.Communication.PreCommunicationId = PreCommunicationId != null ? Guid.Parse(PreCommunicationId) : Guid.Empty;
            //if (loginState.UserRoles.Where(r => r.RoleId == SystemRoles.ComsLawyer).Any())
            //{
            //    sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Yellow;
            //}
            //else if (loginState.UserRoles.Where(r => r.RoleId == SystemRoles.ComsHOS).Any())
            //{
            //    sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Green;
            //}
            if (!IsRequest)
            {
                sendCommunication.CommunicationResponse.RequestDate = consultationFileDetail.FileDate;
                sendCommunication.CommunicationResponse.ResponseDate = NowTime;
            }
            else
            {
                sendCommunication.CommunicationResponse.RequestDate = (DateTime)consultationRequest.RequestDate;
                sendCommunication.CommunicationResponse.ResponseDate = ResponseDate;
            }


            //Communication Response 
            sendCommunication.CommunicationResponse.CommunicationResponseId = Guid.NewGuid();
            sendCommunication.CommunicationResponse.CommunicationId = sendCommunication.Communication.CommunicationId;
            sendCommunication.CommunicationResponse.CreatedBy = loginState.Username;
            sendCommunication.CommunicationResponse.CreatedDate = DateTime.Now;
            sendCommunication.CommunicationResponse.IsDeleted = false;
            sendCommunication.CommunicationResponse.PartyEntityId = consultationRequest.GovtEntityId;


            //CommunicationTargetLink
            sendCommunication.CommunicationTargetLink.CreatedBy = loginState.Username;
            sendCommunication.CommunicationTargetLink.CreatedDate = DateTime.Now;
            sendCommunication.CommunicationTargetLink.IsDeleted = false;
            sendCommunication.CommunicationTargetLink.CommunicationId = sendCommunication.Communication.CommunicationId;
            PopulateLinkTargets();
        }

        protected async Task CalculateCommunicationType()
        {
            if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformation)
            {
                sendCommunication.Communication.Title = "Request_For_More_Information";
                sendCommunication.Communication.Description = "Request_For_More_Information";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.RequestMoreInfo;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformationReminder)
            {
                sendCommunication.Communication.Title = "Request_For_More_Information_Reminder";
                sendCommunication.Communication.Description = "Request_For_More_Information_Reminder";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.RequestForMoreInformationReminder;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.SaveAndCloseFile)
            {
                sendCommunication.Communication.Title = "Save_And_Close_File";
                sendCommunication.Communication.Description = "Save_And_Close_File";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.SaveAndCloseFile;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.CaseRegistered)
            {
                sendCommunication.Communication.Title = "Case_Registered";
                sendCommunication.Communication.Description = "Case_Registered";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.CaseRegistered;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.GeneralUpdate)
            {
                sendCommunication.Communication.Title = "General_Update";
                sendCommunication.Communication.Description = "General_Update";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.GeneralUpdate;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.FinalDocument)
            {
                sendCommunication.Communication.Title = "Final_Document";
                sendCommunication.Communication.Description = "Final_Document";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.FinalDocument;
            }
        }
        #endregion

        #region Add Document 
        protected async Task AddDocument()
        {
            var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                new Dictionary<string, object>()
                {
                        { "ReferenceGuid", Guid.Empty },
                        { "CommunicationId", sendCommunication.Communication.CommunicationId },
                        { "IsViewOnly", false },
                        { "IsUploadPopup", true },
                        { "FileTypes", new List<string>() { ".pdf" } },
                        { "MaxFileSize", systemSettingState.File_Maximum_Size },
                        { "Multiple", false },
                        { "UploadFrom", "ConsultationManagement" },
                        { "ModuleId", (int)WorkflowModuleEnum.COMSConsultationManagement },
                        { "IsCaseRequest", false },
                }
                ,
                new DialogOptions() { Width = "30% !important" });
            uploadedAttachment = uploadedAttachment ?? new List<TempAttachementVM>();
            if (result != null)
                uploadedAttachment.Add((TempAttachementVM)result);
            if (uploadedAttachment.Count != 0)
            {
                isUploaded = true;
            }
            else
            {
                isUploaded = false;
            }
            upload.Reset();
            StateHasChanged();
        }
        #endregion

        #region Reomove Document
        protected async Task RemoveDocument(TempAttachementVM file)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_File"), translationState.Translate("Remove_File"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var result = false;
                if (file == null)
                    return;

                result = await fileUploadService.RemoveTempAttachement(file?.FileName, file?.UploadedBy, " ConsultationManagement", "G2G_WEB", file?.AttachmentTypeId);
                if (result)
                {
                    uploadedAttachment = uploadedAttachment ?? new List<TempAttachementVM>();
                    var fileToRemove = uploadedAttachment.Find(f => f.FileName == file.FileName);
                    if (fileToRemove != null)
                    {
                        uploadedAttachment.Remove(fileToRemove);
                        upload.Reset();
                    }

                    if (uploadedAttachment.Count == 0)
                    {
                        isUploaded = false;
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }

        #endregion

        #region Attatchment Types
        protected async Task PopulateAttachmentTypes()
        {
            ApiCallResponse response;
            response = await lookupService.GetAttachmentTypes(0);

            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Review Contract Template Button Click
        public async Task ReviewContractTemplateButtonClick()
        {

        }
        #endregion
    }
}

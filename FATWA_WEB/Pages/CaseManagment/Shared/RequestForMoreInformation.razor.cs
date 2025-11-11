using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class RequestForMoreInformation : ComponentBase
    {
        #region parameter

        [Parameter]
        public string ReferenceId { get; set; }

        [Parameter]
        public dynamic IsCaseRequest { get; set; }
        [Parameter]
        public dynamic IsReminder { get; set; }

        [Parameter]
        public dynamic PreCommunicationId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        #endregion

        #region Variables

        protected int GE_Id;
        protected string typeValidationMsg = "";
        protected string reasonValidationMsg = "";
        protected string dateValidationMsg = "";
        public bool IsRequest { get { return Convert.ToBoolean(IsCaseRequest); } set { IsCaseRequest = value; } }
        public bool IsReminderFlag { get { return Convert.ToBoolean(IsReminder); } set { IsReminder = value; } }
        protected List<Priority> Priorities { get; set; } = new List<Priority>();
        protected List<ResponseType> ResponseType { get; set; }
        public DateTime ResponseDate = DateTime.Now;
        public DateTime NowTime = DateTime.Now;

        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected CaseRequestDetailVM caseRequest { get; set; } = new CaseRequestDetailVM();
        protected CmsCaseFileDetailVM caseFileDetail { get; set; } = new CmsCaseFileDetailVM();
        protected List<GovernmentEntity> GovernmentEntities { get; set; } = new List<GovernmentEntity>();
        protected List<GovernmentEntity> PartiesGovernmentEntities { get; set; } = new List<GovernmentEntity>();
        protected List<Frequency> frequency { get; set; } = new List<Frequency>();
        protected RequestMoreInfoVM requestMoreInfoVM { get; set; } = new RequestMoreInfoVM();
        protected Validations validations { get; set; } = new Validations();
        public string caseRequestUrl { get; set; }
        public string caseFileUrl { get; set; }
        protected List<TempAttachementVM> uploadedAttachment { get; set; } = new List<TempAttachementVM>();
        protected RadzenDataGrid<TempAttachementVM>? upload = new RadzenDataGrid<TempAttachementVM>();
        protected bool isUploaded { get; set; } = true;


        protected SendCommunicationVM sendCommunication = new SendCommunicationVM()
        {
            Communication = new Communication(),
            CommunicationResponse = new CommunicationResponse(),
            CommunicationTargetLink = new CommunicationTargetLink(),
            LinkTarget = new List<LinkTarget>(),
        };

        LinkTarget linkTarget { get; set; }
        protected List<CasePartyLinkVM> CasePartyLinks = new List<CasePartyLinkVM>();
        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected ObservableCollection<TempAttachementVM> attachments { get; set; }
        public bool allowRowSelectOnRowClick = true;
        #endregion

        #region LOAD

        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        protected override async Task OnInitializedAsync()
        {
            sendCommunication.Communication.CommunicationId = Guid.NewGuid();

            if (!IsRequest)
            {
                caseFileUrl = "/casefile-view/" + ReferenceId + "";
                var result = await cmsCaseFileService.GetCaseFileDetailByIdVM(Guid.Parse(ReferenceId));
                if (result.IsSuccessStatusCode)
                {
                    caseFileDetail = (CmsCaseFileDetailVM)result.ResultData;
                    await PopulateCaseRequestDetail();
                    await PopulatePriorities();
                    await PopulateResponseTypes();
                    await PopulateGovernmentEntities();
                    await PopulateFrequency();
                    await PopulateAttachmentTypes();
                }
            }
            else
            {
                caseRequestUrl = "/caserequest-view/" + ReferenceId + "";
                await PopulateCaseRequestDetail();
                await PopulateResponseTypes();
                await PopulateAttachmentTypes();
                await PopulateGovernmentEntities();
            }
            await PopulateCasePartyGrid();
            await PopulateAttachementsGrid();
        }
        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        protected void PopulateLinkTargets()
        {
            linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = sendCommunication.Communication.CommunicationId,
                IsPrimary = false,
                LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication,
            };
            sendCommunication.LinkTarget.Add(linkTarget);

            linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = sendCommunication.CommunicationResponse.CommunicationResponseId,
                IsPrimary = false,
                LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication,
            };
            sendCommunication.LinkTarget.Add(linkTarget);

            linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = Guid.Parse(ReferenceId),
                IsPrimary = true,
                LinkTargetTypeId = !IsRequest ? (int)LinkTargetTypeEnum.File : (int)LinkTargetTypeEnum.CaseRequest,
            };
            sendCommunication.LinkTarget.Add(linkTarget);
        }
        protected async Task PopulateCasePartyGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(ReferenceId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Where(x => x.TypeId == (int)CasePartyTypeEnum.GovernmentEntity).ToList();
                if (CasePartyLinks.Count > 0)
                {
                    var entityIds = CasePartyLinks?.Select(link => link.EntityId).ToList();
                    PartiesGovernmentEntities = GovernmentEntities?
                        .Where(entity => entityIds?.Contains(entity.EntityId) == true)
                        .ToList();
                    GovernmentEntities = GovernmentEntities?
                        .Where(entity => entityIds?.Contains(entity.EntityId) == false)
                        .ToList();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
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
            //if (string.IsNullOrWhiteSpace(sendCommunication.CommunicationResponse.Reason))
            //{

            //    reasonValidationMsg = @translationState.Translate("Required_Field");
            //    validations.reason = "k-invalid";
            //    basicDetailsValid = false;
            //}
            //else
            //{
            //    validations.reason = "k-valid";
            //}

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
            typeValidationMsg = "";
            reasonValidationMsg = "";


            bool res = ValidateBasicDetailsOnChange();
            if (res != false)
            {
                await FillCommunicationModel();

                if (caseRequest != null && sendCommunication.CommunicationResponse.PartyEntityIds.Any(id => id == caseRequest.GovtEntityId))

                {

                }
                else
                {

                    sendCommunication.CommunicationResponse.PartyEntityIds = sendCommunication.CommunicationResponse.PartyEntityIds
           .Concat(new List<int> { caseRequest.GovtEntityId.Value });

                }
                    var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
                new Dictionary<string, object>()
                {
                    { "ReferenceId", ReferenceId },
                    { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                    { "DraftEntityType",  IsRequest ? (int)DraftEntityTypeEnum.RequestNeedMoreInfo : (int)DraftEntityTypeEnum.FileNeedMoreInfo},
                    { "Payload", Newtonsoft.Json.JsonConvert.SerializeObject(sendCommunication) },
                    { "Document_Type", (int)AttachmentTypeEnum.CmsLegalNotification },
                    { "ResponseTypeId", (int)sendCommunication.CommunicationResponse.ResponseTypeId },
                    { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(sendCommunication,IsRequest ? (int)DraftEntityTypeEnum.RequestNeedMoreInfo : (int)DraftEntityTypeEnum.FileNeedMoreInfo, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
                    { "TaskId" , TaskId != null ?  TaskId : null }
                }
                ,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            }
        }

        protected async Task ButtonBackClick(MouseEventArgs args)
        {
            if (!IsRequest)
            {
                navigationManager.NavigateTo("/casefile-view/" + ReferenceId);
            }
            else
            {
                navigationManager.NavigateTo("/caserequest-view/" + ReferenceId);
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
                var CopyOfResponseType = new List<ResponseType>(ResponseType);
                if (!IsRequest)
                {
                    ResponseType = ResponseType.Where(x =>
                                x.Id == (int)ResponseTypeEnum.RequestForMoreInformation
                             || x.Id == (int)ResponseTypeEnum.SaveAndCloseFile).ToList();
                    if (caseFileDetail.IsCaseRegistered)
                    {
                        ResponseType.Add(CopyOfResponseType.Where(x => x.Id == (int)ResponseTypeEnum.CaseRegistered).FirstOrDefault());
                    }
                }
                else
                {
                    ResponseType = ResponseType.Where(x => x.Id == (int)ResponseTypeEnum.GeneralUpdate).ToList();
                }
                if (IsReminderFlag)
                {
                    ResponseType = CopyOfResponseType.Where(x => x.Id == (int)ResponseTypeEnum.RequestForMoreInformationReminder).ToList();
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
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

        protected async Task PopulateCaseRequestDetail()
        {
            var caseRequestResponse = await caseRequestService.GetCaseRequestDetailById(IsRequest ? Guid.Parse(ReferenceId) : caseFileDetail.RequestId);
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseRequest = JsonConvert.DeserializeObject<CaseRequestDetailVM>(caseRequestResponse.ResultData.ToString());
                //caseRequest = (CaseRequestDetailVM)caseRequestResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }

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
                        attachments = new ObservableCollection<TempAttachementVM>(attachments?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).OrderByDescending(item => item.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement).ThenBy(item => item.AttachmentTypeId).ToList());
                        sendCommunication.SelectedDocuments = attachments.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement).ToList();
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
            sendCommunication.Communication.ReceivedBy = sendCommunication.CommunicationResponse.PartyEntityIds.Count() > 0 && sendCommunication.CommunicationResponse.PartyEntityIds != null ? "" : caseRequest.CreatedBy;
            sendCommunication.Communication.GovtEntityId = caseRequest.GovtEntityId;
            sendCommunication.Communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
            sendCommunication.Communication.CreatedBy = loginState.Username;
            sendCommunication.Communication.CreatedDate = DateTime.Now;
            sendCommunication.Communication.IsDeleted = false;
            sendCommunication.Communication.OutboxShortNum = 0;
            sendCommunication.Communication.PreCommunicationId = PreCommunicationId != null ? Guid.Parse(PreCommunicationId) : Guid.Empty;
            //if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.Lawyer))
            //{
            //    sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Yellow;
            //}
            //else if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS))
            //{
            //    sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Green;
            //}
            if (!IsRequest)
            {
                sendCommunication.CommunicationResponse.RequestDate = caseFileDetail.CreatedDate;
                sendCommunication.CommunicationResponse.ResponseDate = NowTime;
            }
            else
            {
                sendCommunication.CommunicationResponse.RequestDate = (DateTime)caseRequest.RequestDate;
                sendCommunication.CommunicationResponse.ResponseDate = ResponseDate;
            }


            //Communication Response 
            sendCommunication.CommunicationResponse.CommunicationResponseId = Guid.NewGuid();
            sendCommunication.CommunicationResponse.CommunicationId = sendCommunication.Communication.CommunicationId;
            sendCommunication.CommunicationResponse.CreatedBy = loginState.Username;
            sendCommunication.CommunicationResponse.CreatedDate = DateTime.Now;
            sendCommunication.CommunicationResponse.IsDeleted = false;


            //CommunicationTargetLink
            sendCommunication.CommunicationTargetLink.CreatedBy = loginState.Username;
            sendCommunication.CommunicationTargetLink.CreatedDate = DateTime.Now;
            sendCommunication.CommunicationTargetLink.IsDeleted = false;
            sendCommunication.CommunicationTargetLink.CommunicationId = sendCommunication.Communication.CommunicationId;

           
            PopulateLinkTargets();
        }
        #endregion

        #region Calculate Communication Type
        protected async Task CalculateCommunicationType()
        {
            if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformation)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Request_For_More_Information";
                sendCommunication.Communication.Description = "Legal_Notification_Request_For_More_Information";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.RequestMoreInfo;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformationReminder)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Request_For_More_Information_Reminder";
                sendCommunication.Communication.Description = "Legal_Notification_Request_For_More_Information_Reminder";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.RequestForMoreInformationReminder;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.SaveAndCloseFile)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Save_And_Close_File";
                sendCommunication.Communication.Description = "Legal_Notification_Save_And_Close_File";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.SaveAndCloseFile;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.CaseRegistered)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Case_Registered";
                sendCommunication.Communication.Description = "Legal_Notification_Case_Registered";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.CaseRegistered;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.GeneralUpdate)
            {
                sendCommunication.Communication.Title = "Legal_Notification_General_Update";
                sendCommunication.Communication.Description = "Legal_Notification_General_Update";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.GeneralUpdate;
            }
        }
        #endregion

        #region File Upload
        protected async Task AddDocument()
        {
            var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                new Dictionary<string, object>()
                {
                        { "ReferenceGuid", sendCommunication.Communication.CommunicationId },
                        { "CommunicationId", sendCommunication.Communication.CommunicationId },
                        { "IsViewOnly", false },
                        { "IsUploadPopup", true },
                        { "FileTypes", new List<string>() { ".pdf" } },
                        { "MaxFileSize", systemSettingState.File_Maximum_Size },
                        { "Multiple", false },
                        { "UploadFrom", "CaseManagement" },
                        { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
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

                result = await fileUploadService.RemoveTempAttachement(file?.FileName, file?.UploadedBy, "CaseManagement", "G2G_WEB", file?.AttachmentTypeId);
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
    }
}

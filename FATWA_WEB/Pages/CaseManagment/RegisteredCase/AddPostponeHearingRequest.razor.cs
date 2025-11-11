using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Postpone Hearing of a Case</History>
    public partial class AddPostponeHearingRequest : ComponentBase
    {
        #region Parameter

        [Parameter]
        public Guid HearingId { get; set; }

        [Parameter]
        public Guid CaseId { get; set; }

        #endregion

        #region Variables

        public CmsRegisteredCaseDetailVM registeredCase { get; set; }
        public PostponeHearing postponeHearing { get; set; } = new PostponeHearing { Id = Guid.NewGuid(), CreatedDate = DateTime.Now, IsDeleted = false };
        protected string reasonValidationMsg = "";
        protected List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();

        #endregion

        #region Component Load

        //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateCaseDetails();
                spinnerService.Hide();
            }
            catch (Exception ex)
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

        #endregion


        #region Dropdown Change Events

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Judgement Statuses</History>
        protected async Task PopulateCaseDetails()
        {
            var response = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(CaseId);
            if (response.IsSuccessStatusCode)
            {
                registeredCase = (CmsRegisteredCaseDetailVM)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #endregion


        #region Redirect and Dialog Events

        //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master" >Submit Judgement Details</History>
        protected async Task Form0Submit(PostponeHearing item)
        {
            try
            {
                if (!String.IsNullOrEmpty(postponeHearing.Reason))
                {
                    if (await dialogService.Confirm(translationState.Translate("Sure_Save_Changes"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        postponeHearing.CreatedBy = loginState.Username;
                        postponeHearing.HearingId = HearingId;
                        spinnerService.Show();
                        postponeHearing.HearingId = HearingId;
                        var response = await cmsRegisteredCaseService.AddPostponeHearingRequest(postponeHearing);
                        if (response.IsSuccessStatusCode)
                        {
                            // Save TempAttachement To Upload Documents
                            await SaveTempAttachementToUploadedDocument();
                            await MoveAttachmentsFromSourceToDestination(postponeHearing);
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Hearing_Postpone_Requested_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close(postponeHearing);
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();

                        // var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
                        //new Dictionary<string, object>()
                        //    {
                        //         { "ReferenceId", CaseId.ToString() },
                        //         { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                        //         { "DraftEntityType",  (int)DraftEntityTypeEnum.PostPoneHearing},
                        //         { "Payload", Newtonsoft.Json.JsonConvert.SerializeObject(postponeHearing) },
                        //         { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(postponeHearing,(int)DraftEntityTypeEnum.PostPoneHearing, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0) }
                        //    }
                        //    ,
                        //    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                        // 
                    }
                }
                else
                {
                    reasonValidationMsg = String.IsNullOrEmpty(postponeHearing.Reason) ? translationState.Translate("Required_Field") : "";
                }
            }
            catch (Exception ex)
            {
                spinnerService.Show();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region upload Documents
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid> { postponeHearing.Id };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = postponeHearing.CreatedBy,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
                });

                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }
        protected async Task MoveAttachmentsFromSourceToDestination(PostponeHearing postponeHearing)
        {
            try
            {
                copyAttachments.Add(
                    new CopyAttachmentVM()
                    {
                        SourceId = postponeHearing.Id,
                        DestinationId = CaseId,
                        CreatedBy = postponeHearing.CreatedBy
                    });

                var docResponse = await fileUploadService.UpdateExistingDocument(copyAttachments);
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }

        #endregion

    }
}

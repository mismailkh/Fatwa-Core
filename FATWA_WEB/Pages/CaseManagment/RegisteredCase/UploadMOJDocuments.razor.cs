using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class UploadMOJDocuments : ComponentBase
    {
        #region Paramter

        [Parameter]
        public string FileId { get; set; }
        [Parameter]
        public string MojRegistrationRequestId { get; set; }
        [Parameter]
        public string DocumentId { get; set; }
        [Parameter]
        public string AttachmentTypeId { get; set; }
        #endregion

        #region Variables
        public int MandatoryAttachmentTypeId { get; set; }
        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if(int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.OrderOnPetitionNotes)
            {
                MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.OrderOnPetitionCourtDecision;
            } 
            else if (int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.PerformOrderNotes)
            {
                MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.PerformOrderCourtDecision;
            }
            spinnerService.Hide();
        }

        #endregion

        protected async Task Form0Submit()
        {
            try
            {
                var attachments = await fileUploadService.GetTempAttachements(Guid.Parse(FileId));
                if (!attachments.Where(t => t.AttachmentTypeId == MandatoryAttachmentTypeId).Any())
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Upload_Claim_Statement"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    return;
                }

                if (await dialogService.Confirm(translationState.Translate("Sure_Submit_Case"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                   
                    var response = await cmsCaseFileService.ProcessCaseFile(Guid.Parse(MojRegistrationRequestId),loginState.Username);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Case_Submitted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await SaveTempAttachementToUploadedDocument();
                        // Update Draft Document Status
                        var documents = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(FileId));
                        var VersionId = documents.Where(x => x.UploadedDocumentId == int.Parse(DocumentId)).Select(x => x.VersionId).FirstOrDefault();
                        CmsDraftedTemplateVersions draftedTemplateVersion = new CmsDraftedTemplateVersions();
                        draftedTemplateVersion.VersionId = Guid.NewGuid();
                        draftedTemplateVersion.OldVersionId = (Guid)VersionId;
                        draftedTemplateVersion.StatusId = (int)CaseDraftDocumentStatusEnum.RegisteredInMOJ;
                        draftedTemplateVersion.CreatedBy = loginState.UserDetail.UserName;
                        draftedTemplateVersion.CreatedDate = DateTime.Now;
                        await cmsCaseTemplateService.UpdateDraftDocumentStatus(draftedTemplateVersion);

                        await Task.Delay(1500);
                        navigationManager.NavigateTo("/moj-registration-requests");
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }

                    spinnerService.Hide();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {
                    Guid.Parse(FileId)
                };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = loginState.Username,
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

        #region Redirect Function
        protected async Task RedirectBack()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm_Cancel"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                navigationManager.NavigateTo("moj-registration-requests/");
            }
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

    }
}

using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.Consultation.Request
{
    public partial class ViewResponseForRequestMoreInformation : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic ConsultationRequestId { get; set; }
        #endregion
        #region Varriable
        public ComsConsultationRequestResponseVM consultationRequestResponseVM { get; set; } = new ComsConsultationRequestResponseVM();
        public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        protected bool isRecordFound = true;
        public SfPdfViewerServer pdfViewer;

        #endregion
        #region ON Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateAttachmentTypes();
            var response = await consultationRequestService.GetConsultationRequestResponseById(Guid.Parse(ConsultationRequestId));
            if (response.IsSuccessStatusCode)
            {
                consultationRequestResponseVM = (ComsConsultationRequestResponseVM)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            await LoadAuthorityLetter();
            spinnerService.Hide();
        }
        #endregion

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task LoadAuthorityLetter()
        {
            try
            {
                if (isRecordFound)
                {
                    OfficialAttachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(ConsultationRequestId));
                    var attachment = OfficialAttachments?.FirstOrDefault();
                    if (attachment != null)
                    {
                        var physicalPath = string.Empty;
#if DEBUG
                        {
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                        }
#else
                        {

                            // Construct the physical path of the file on the server
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                            // Remove the wwwroot/Attachments part of the path to get the actual file path
                            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
}
#endif

                        if (!string.IsNullOrEmpty(physicalPath))
                        {
                            FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachment.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                            string base64String = Convert.ToBase64String(FileData);
                            DocumentPath = "data:application/pdf;base64," + base64String;
                            ShowDocumentViewer = true;
                            StateHasChanged();
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
            }
            catch
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
                var physicalPath = string.Empty;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileData);
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    ShowDocumentViewer = true;
                    StateHasChanged();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes((int)WorkflowModuleEnum.CaseManagement);
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

        #region Redirect Function
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
    }
}


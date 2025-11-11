using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.PdfViewerServer;
using Telerik.Blazor.Components;

namespace FATWA_WEB.Pages.Dms
{
    public partial class ViewDetailDocument : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic UploadedDocumentId { get; set; }
        #endregion

        #region Varriable
        public DMSDocumentDetailVM dMSDocumentDetailVM { get; set; } = new DMSDocumentDetailVM();
        //public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        protected bool isRecordFound = true;
        public SfPdfViewerServer pdfViewer;

        #endregion

        #region ON Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            var response = await fileUploadService.GetDocumentDetailById(Convert.ToInt32(UploadedDocumentId));
            if (response.IsSuccessStatusCode)
            {
                dMSDocumentDetailVM = (DMSDocumentDetailVM)response.ResultData;
                await LoadAuthorityLetter();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            spinnerService.Hide();
        }
        #endregion

        #region Load Attachment

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task LoadAuthorityLetter()
        {
            try
            {
                if (isRecordFound)
                {
                    if (dMSDocumentDetailVM != null)
                    {
                        var physicalPath = string.Empty;
#if DEBUG
                        {
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + dMSDocumentDetailVM.StoragePath).Replace(@"\\", @"\");

                        }
#else
                        {

                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + dMSDocumentDetailVM.StoragePath).Replace(@"\\", @"\");
                            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                           
                        }
#endif

                        if (!string.IsNullOrEmpty(physicalPath))
                        {
                            FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, dMSDocumentDetailVM.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
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
        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
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

        #region Button 
        protected async Task ShareDocument(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<ShareDocumentPopup>(
                translationState.Translate("Share_Document"),
                new Dictionary<string, object>() {
                    { "DocumentId", UploadedDocumentId },
                    {"IsConfidential" , dMSDocumentDetailVM.IsConfidential}

                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });

            if (dialogResult != null)
                navigationManager.NavigateTo("/document-list");
        }
        #endregion
    }
}


using FATWA_DOMAIN.Models.ViewModel.PACIVM;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Text;
using Telerik.Blazor.Components;
using System.Security.Cryptography;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using MsgReader.Outlook;
using PdfSharp.Drawing.Layout;
using static FATWA_GENERAL.Helper.Response;
using MsgKit;
using FATWA_GENERAL.Helper;
using Syncfusion.Blazor.PdfViewerServer;
namespace FATWA_WEB.Pages.PACI
{
    public partial class PACIDocumentView : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        [Parameter]
        public dynamic Id { get; set; }
        [Parameter]
        public bool tag { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        public SfPdfViewerServer pdfViewer;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!tag)
            {
                await LoadResponse();
            }
            else
            {
                await LoadRequest();
            }
        }

        protected async System.Threading.Tasks.Task LoadRequest()
        {
            try
            {


                PACIRequestListVM pACIRequestVM = new PACIRequestListVM();
                pACIRequestVM.RequestId = Id;

                var response = await pACIRequestService.GetAllPACIRequestbyRequestId(Convert.ToString(pACIRequestVM.RequestId));
                if (response.IsSuccessStatusCode)
                {
                    var res = (List<PACIRequestListVM>)response.ResultData;

                    foreach (var attachments in res)
                    {
                        if (attachments.RequestDocument != null)
                        {
                            var physicalPath = string.Empty;
#if DEBUG
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("Dms_file_path") + attachments.RequestDocument).Replace(@"\\", @"\");

                            }
#else
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachments.RequestDocument).Replace(@"\\", @"\");
                                physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\"); 
                            }
#endif
                            if (!String.IsNullOrEmpty(physicalPath))
                            {
                                FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, ".pdf", _config.GetValue<string>("DocumentEncryptionKey"));
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

                    }
                }

                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        protected async System.Threading.Tasks.Task LoadResponse()
        {
            try
            {



                PACIRequestListVM pACIRequestVM = new PACIRequestListVM();
                pACIRequestVM.RequestId = Id;

                var response = await pACIRequestService.GetAllPACIRequestbyRequestId(Convert.ToString(pACIRequestVM.RequestId));
                if (response.IsSuccessStatusCode)
                {
                    var res = (List<PACIRequestListVM>)response.ResultData;
                    foreach (var attachments in res)
                    {
                        if (attachments.ResponseDocument != null)
                        {

                            var physicalPath = string.Empty;
#if DEBUG
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("Dms_file_path") + attachments.RequestDocument).Replace(@"\\", @"\");

                            }
#else
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachments.RequestDocument).Replace(@"\\", @"\");
                                physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\"); 
                            }
#endif
                            if (!String.IsNullOrEmpty(physicalPath))
                            {
                                FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, ".pdf", _config.GetValue<string>("DocumentEncryptionKey"));
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

                    }

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

        #region Badrequest Notification

        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Handle bad request and display error messages in whole class</History>
        protected async System.Threading.Tasks.Task ReturnBadRequestNotification(ApiCallResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await System.Threading.Tasks.Task.Delay(5000);
                    await BrowserStorage.RemoveItemAsync("User");
                    await BrowserStorage.RemoveItemAsync("Token");
                    await BrowserStorage.RemoveItemAsync("RefreshToken");
                    await BrowserStorage.RemoveItemAsync("UserDetail");
                    await BrowserStorage.RemoveItemAsync("SecurityStamp");
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
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
    }
}

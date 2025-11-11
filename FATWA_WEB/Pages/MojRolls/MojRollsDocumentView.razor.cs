using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.MojRolls
{

    public partial class MojRollsDocumentView : ComponentBase
    {
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }



        [Parameter]
        public dynamic Id { get; set; }
        public string SessionDate { get; set; }
        public string Chamber_Name { get; set; }
        public string ChamberTypeCode_Name { get; set; }
        public string Court_Name { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await LoadAttachements();
        }
        protected async System.Threading.Tasks.Task Load()
        {

            try
            {

#if DEBUG
                {

                    var Result = await mojRollsService.GetRMSRequestsDetailById(Id);
                    if (Result.IsSuccessStatusCode)
                    {
                        var res = (MOJRollsRequestDetailsList)Result.ResultData;
                        DateTime SDate = (DateTime)res.SessionDate;
                        SessionDate = SDate.ToString("dd/MM/yyyy");
                        Chamber_Name = res.ChamberType_LookUp_Value;
                        ChamberTypeCode_Name = res.ChamberTypeCode_LookUp_Value;
                        Court_Name = res.CourtType_LookUp_Value;

                        if (res.FilePath != null)
                        {
                            var physicalPath = Path.Combine(Directory.GetCurrentDirectory() + res.FilePath).Replace(@"\\", @"\");
                            if (System.IO.File.Exists(physicalPath))
                            {
                                FileData = File.ReadAllBytes(physicalPath);
                                DocumentPath = "data:application/pdf;base64," + Convert.ToBase64String(FileData);
                                ShowDocumentViewer = true;
                                StateHasChanged();
                            }

                        }

                    }

                }
#else
                            {

                                var Result = await mojRollsService.GetRMSRequestsDetailById(Id);
                                if (Result.IsSuccessStatusCode)
                                {
                                    var res = (MOJRollsRequestDetailsList)Result.ResultData;              
                                     DateTime SDate = (DateTime)res.SessionDate;
                                     SessionDate = SDate.ToString("dd-MM-yyyy");
                                     Chamber_Name = res.ChamberType_LookUp_Value;
                                    ChamberTypeCode_Name = res.ChamberTypeCode_LookUp_Value;
                                    Court_Name = res.CourtType_LookUp_Value;
                                    // Construct the physical path of the file on the server
                                    var physicalPath = Path.Combine(_config.GetValue<string>("file_path") + res.FilePath).Replace(@"\\", @"\");
                                    // Remove the wwwroot/Attachments part of the path to get the actual file path
                                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", @"\");
                                     // Create a new HttpClient instance to download the file
                                    using var httpClient = new HttpClient();
                                    var httpresponse = await httpClient.GetAsync(physicalPath);
                                    // Check if the file was downloaded successfully
                                    if (httpresponse.IsSuccessStatusCode)
                                    {
                                        // Read the file content as a byte array
                                        var fileData = await httpresponse.Content.ReadAsByteArrayAsync();
                                        FileData = fileData;
                                        DocumentPath = "data:application/pdf;base64," + Convert.ToBase64String(FileData);
                                        ShowDocumentViewer = true;
                                        StateHasChanged();
                                    }


                                }

                            }
#endif

            }
            catch (Exception)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "لقد حدث خطأ ما. يرجى المحاولة مرة أخرى لاحقا",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task LoadAttachements()
        {

            try
            {
                var Result = await mojRollsService.GetRMSRequestsDetailById(Id);
                if (Result.IsSuccessStatusCode)
                {
                    var res = (MOJRollsRequestDetailsList)Result.ResultData;
                    DateTime SDate = (DateTime)res.SessionDate;
                    SessionDate = SDate.ToString("dd-MM-yyyy");
                    Chamber_Name = res.ChamberType_LookUp_Value;
                    ChamberTypeCode_Name = res.ChamberTypeCode_LookUp_Value;
                    Court_Name = res.CourtType_LookUp_Value;
                    var response = new ApiCallResponse();
                    response = await mojRollsService.GetMojRollAttachements(res.DocumentId);
                    if (response.IsSuccessStatusCode)
                    {
                        var attachments = (TempAttachementVM)response.ResultData;

                        if (attachments != null)
                        {
                            var physicalPath = string.Empty;
#if DEBUG
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachments.StoragePath).Replace(@"\\", @"\");
                            }
#else
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachments.StoragePath).Replace(@"\\", @"\");
                                physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\"); 
                            }
#endif
                            if (!String.IsNullOrEmpty(physicalPath))
                            {
                                FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachments.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
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
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}


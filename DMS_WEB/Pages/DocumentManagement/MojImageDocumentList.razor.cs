using Microsoft.AspNetCore.Components;
using DMS_WEB.Services;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;
using Radzen;
using Radzen.Blazor;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using static FATWA_GENERAL.Helper.Response;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Dynamic.Core;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_GENERAL.Helper;
using Syncfusion.Blazor.PdfViewerServer;

namespace DMS_WEB.Pages.DocumentManagement
{
    public partial class MojImageDocumentList : ComponentBase
    {
        #region Varible

        protected MojDocumentAdvanceSearchVM advanceSearchVM { get; set; } = new MojDocumentAdvanceSearchVM();
        protected RadzenDataGrid<MojDocumentVM>? grid;
        protected RadzenDataGrid<MojDocumentVM>? mojdocumentgridpVM = new RadzenDataGrid<MojDocumentVM>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        public bool allowRowSelectOnRowClick = true;
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;  
        #endregion
        #region Full property declaration
        protected List<AttachmentType> attachmentTypes { get; set; } = new List<AttachmentType>();
        IEnumerable<MojDocumentVM> _mojImagedocumentList;
        IEnumerable<MojDocumentVM> FilteredMojImageDocumentList { get; set; } = new List<MojDocumentVM>();
        protected IEnumerable<MojDocumentVM> mojImagedocumentList
        {
            get
            {
                return _mojImagedocumentList;
            }
            set
            {
                if (!object.Equals(_mojImagedocumentList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "mojImagedocumentList", NewValue = value, OldValue = _mojImagedocumentList };
                    _mojImagedocumentList = value;

                    Reload();
                }
            }

        }
        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    Reload();
                }
            }
        }
        #endregion
        #region On Initialize
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            translationState.TranslateGridFilterLabels(grid);
            await PopulateAttachmentTypes();
            await Load();
            spinnerService.Hide();
        }
        #endregion
        #region On Load
        protected async Task Load()
        {

            try
            {
                var response = await fileUploadService.MojImageDocumentList(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    mojImagedocumentList = (IEnumerable<MojDocumentVM>)response.ResultData;
                    FilteredMojImageDocumentList = (IEnumerable<MojDocumentVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredMojImageDocumentList = await gridSearchExtension.Filter(mojImagedocumentList, new Query()
                    {
                        Filter = $@"i => (i.CANNumber != null && i.CANNumber.ToString().Contains(@0)) || (i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    FilteredMojImageDocumentList = await gridSearchExtension.Filter(mojImagedocumentList, new Query()
                    {
                        Filter = $@"i => (i.CANNumber != null && i.CANNumber.ToString().Contains(@0)) || (i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search }
                    });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Advance Search
        public async void ResetForm()
        {

            advanceSearchVM = new MojDocumentAdvanceSearchVM();

            await Load();
            Keywords = false;
            StateHasChanged();
        }
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }

        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    //Summary = $"خطأ!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(advanceSearchVM.CANNumber) && string.IsNullOrWhiteSpace(advanceSearchVM.CANNumber)
                 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
            {

            }
            else
            {
                Keywords = true;

                await Load();
                //await grid.Reload();
                StateHasChanged();
            }
        }

        #endregion
        #region View and Download Attachments

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Download Attachment using stream</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then download</History>
        protected async Task DownloadAttachement(MojDocumentVM theUpdatedItem)
        {
            try
            {
                //Encryption/Descyption Key
                string password = _config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                MemoryStream fsOut = new MemoryStream();
                int data;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#if DEBUG
                {
                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    if (!String.IsNullOrEmpty(physicalPath))
                    {
                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Open);
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        await blazorDownloadFileService.DownloadFile(theUpdatedItem.FileName, fsOut, "application/octet-stream");
                        fsOut.Close();
                        cs.Close();
                        fsCrypt.Close();
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
#else
                {
                    
				    var RleasephysicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    RleasephysicalPath = RleasephysicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
					if (!String.IsNullOrEmpty(RleasephysicalPath))
					{ 
                        var httpClient = new HttpClient();
						var response = await httpClient.GetAsync(RleasephysicalPath);
						if (response.IsSuccessStatusCode)
						{
							var fsCrypt = await response.Content.ReadAsStreamAsync();
							CryptoStream cs = new CryptoStream(fsCrypt,
								RMCrypto.CreateDecryptor(key, key),
								CryptoStreamMode.Read);

							while ((data = cs.ReadByte()) != -1)
								fsOut.WriteByte((byte)data);

							await blazorDownloadFileService.DownloadFile(theUpdatedItem.FileName, fsOut, "application/octet-stream");
							fsOut.Close();
							cs.Close();
							fsCrypt.Close();
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
#endif
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

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task ViewAttachement(MojDocumentVM theUpdatedItem)
        {
            try
            {
                var physicalPath = string.Empty;
                DisplayDocumentViewer = false;
                StateHasChanged();
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
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, ".pdf", _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileData);
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
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
        protected void OnGroupRowRender(GroupRowRenderEventArgs args)
        {
            if (args.FirstRender) { args.Expanded = false; }
        }


        #endregion
        #region Redirect Function
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
        #region grid button
        IEnumerable<MojDocumentVM> _mojexpandImagedocumentList;
        protected IEnumerable<MojDocumentVM> mojexpandImagedocumentList
        {
            get
            {
                return _mojexpandImagedocumentList;
            }
            set
            {
                if (!object.Equals(_mojexpandImagedocumentList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "_mojexpandImagedocumentList", NewValue = value, OldValue = _mojexpandImagedocumentList };
                    _mojexpandImagedocumentList = value;

                    Reload();
                }
            }

        }
        protected async Task ExpendMojDocument(MojDocumentVM mojDocument)
        {
            translationState.TranslateGridFilterLabels(mojdocumentgridpVM);
            var response = await fileUploadService.GetMojDocumentByCaseNumber(mojDocument.CaseNumber);
            if (response.IsSuccessStatusCode)
            {
                mojexpandImagedocumentList = (List<MojDocumentVM>)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await ReturnBadRequestNotification(response);
            }
        }

        protected async Task PopulateAttachmentTypes()
        {
            var response = await fileUploadService.GetAllAttachmentTypes();
            if (response.IsSuccessStatusCode)
            {
                attachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        #endregion
        #region Badrequest Notification
        protected async Task ReturnBadRequestNotification(ApiCallResponse response)
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
                    await Task.Delay(5000);
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
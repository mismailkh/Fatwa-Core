using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Upload;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Shared
{
    public partial class FileUpload : ComponentBase
    {
        #region Parameters
        [Parameter]
        public Guid? ReferenceGuid { get; set; }
        [Parameter]
        public Guid? CommunicationId { get; set; } = Guid.Empty;
        [Parameter]
        public bool IsViewOnly { get; set; } = false;
        [Parameter]
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();

        [Parameter]
        public List<string> FileTypes { get; set; }

        [Parameter]
        public int? MaxFileSize { get; set; }

        [Parameter]
        public bool Multiple { get; set; }

        [Parameter]
        public string? UploadFrom { get; set; }

        [Parameter]
        public int? ModuleId { get; set; }

        [Parameter]
        public EventCallback<List<int>> DeletedAttachementIdsChanged { get; set; }

        [Parameter]
        public int? MandatoryAttachmentTypeId { get; set; }
        [Parameter]
        public bool? IsUploadPopup { get; set; } = false;
        [Parameter]
        public bool? AutoSave { get; set; } = false;
        [Parameter]
        public bool? AutoDelete { get; set; } = false;

        [Parameter]
        public bool? HideView { get; set; } = false;
        #endregion

        #region Variables
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        public bool Enabled { get; set; } = true;
        private string SelectFileButton { get; set; } = string.Empty;
        public string SaveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Upload";
        public string RemoveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Remove";
        public ObservableCollection<TempAttachementVM> AdditionalTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> TempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> TempFiles2 { get; set; } = new ObservableCollection<TempAttachementVM>();
        Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();
        protected TempAttachementVM TempAttachement { get; set; } = new TempAttachementVM();
        protected RadzenDataGrid<TempAttachementVM>? additionalAttachGrid;
        public string DocumentPath { get; set; }
        public int? PreviewedDocumentId { get; set; }
        public int? PreviewedAttachementId { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public bool DisplayDocumentViewer { get; set; }
        public IList<TempAttachementVM> selectedFiles;
        public bool allowRowSelectOnRowClick = true;
        protected RadzenDataGrid<TempAttachementVM> grid;
        public bool EnableMandatoryDocumentDeleteButton { get; set; } = false;
        public CommunicationListVM communicationDetail = new CommunicationListVM();
        #endregion

        #region Component Load
        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Populate Grid Temp and Uplaoded Attachments</History>
        //<History Author = 'Hassan Abbas' Date='2022-9-24' Version="2.0" Branch="master"> Populate Attachment Types dropdown based on Module</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            if ((bool)IsUploadPopup)
            {
                await PopulateAttachmentTypes();
                if (MandatoryAttachmentTypeId != null)
                {
                    TempAttachement.AttachmentTypeId = MandatoryAttachmentTypeId;
                }
            }
            else
            {
                await PopulateAttachmentTypes();
                await PopulateAttachmentGrid();
            }
        }

        #endregion

        #region Grid Data Population Events

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Populating Attachment Grid based on Module and Concatinate Temporary and Uploaded Documents</History>
        private async Task PopulateAttachmentGrid()
        {
            if (ReferenceGuid != null)
            {
                TempFiles = await fileUploadService.GetUploadedAttachements(false, 0, ReferenceGuid);
                TempFiles2 = await fileUploadService.GetTempAttachements(ReferenceGuid);
            }
            AdditionalTempFiles = TempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Concat(TempFiles2).ToList());
            Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
            AdditionalTempFiles = TempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Select(f => { f.FileName = regex.Replace(f.FileName, "", 1); return f; }).ToList());
            if (!Multiple)
            {
                if (TempFiles != null && TempFiles.Count() > 0)
                    Enabled = false;
            }
        }

        #endregion

        #region File Uploader Handlers

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> On File select check if file already uploaded</History>
        protected void OnSelectHandler(UploadSelectEventArgs e)
        {
            foreach (var item in e.Files)
            {
                if (TempFiles != null && TempFiles.Where(x => x.FileName == item.Name).Count() > 0)
                {
                    e.IsCancelled = true;
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Already_Uploaded"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                if (!FilesValidationInfo.Keys.Contains(item.Id))
                {
                    // nothing is assumed to be valid until the server returns an OK
                    FilesValidationInfo.Add(item.Id, IsSelectedFileValid(item));
                }
            }
            if (!Multiple && FilesValidationInfo.Keys.Count > 0)
            {
                SelectFileButton = "disabled-upload-button";
                StateHasChanged();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> On File Cancel remove from List</History>
        protected void OnCancelHandler(UploadCancelEventArgs e)
        {
            RemoveFailedFilesFromList(e.Files);
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Remove file from list</History>
        protected void OnRemoveHandler(UploadEventArgs e)
        {
            e.RequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Bearer", loginState.Token));
            e.RequestData.Add("_userName", loginState.Username);
            e.RequestData.Add("_uploadFrom", UploadFrom);
            e.RequestData.Add("_project", "FATWA_ADMIN");

            RemoveFailedFilesFromList(e.Files);
            if (!Multiple && FilesValidationInfo.Keys.Count == 0)
            {
                SelectFileButton = string.Empty;
            }
            //UpdateValidationModel();
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Remove uploaded document from Database if temporary else bind value to model and delete in repository</History>
        protected async Task RemoveTempAttachement(TempAttachementVM theUpdatedItem, bool isMandatory = false)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_File"), translationState.Translate("Remove_File"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var result = false;
                if (theUpdatedItem.UploadedDocumentId > 0)
                {
                    if ((bool)AutoDelete)
                    {
                        //Remove the Uploaded attachment from db & Folder
                        var docResponse = await fileUploadService.RemoveDocument(theUpdatedItem.UploadedDocumentId.ToString(), false);
                        if (docResponse.IsSuccessStatusCode)
                        {
                            await PopulateAttachmentGrid();
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
                        return;
                    }
                    else
                    {
                        DeletedAttachementIds.Add((int)theUpdatedItem.UploadedDocumentId);
                        await DeletedAttachementIdsChanged.InvokeAsync(DeletedAttachementIds);
                    }
                    result = true;
                }
                else
                {
                    result = await fileUploadService.RemoveTempAttachement(theUpdatedItem.FileName, theUpdatedItem.UploadedBy, UploadFrom, "FATWA_ADMIN", theUpdatedItem.AttachmentTypeId);
                }
                if (result)
                {
                    if (isMandatory)
                    {

                    }
                    else
                    {

                        AdditionalTempFiles.Remove(theUpdatedItem);
                        if (theUpdatedItem.UploadedDocumentId != null)
                        {
                            var docResponse = await fileUploadService.RemoveDocument(theUpdatedItem.UploadedDocumentId.ToString(), false);
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

                        await additionalAttachGrid.Reload();
                    }
                    EnableMandatoryDocumentDeleteButton = false;
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

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Add necessary parameters like Uploading Source, username, Token etc etc</History>
        protected async Task OnUploadHandler(UploadEventArgs e)
        {
            if (TempAttachement.AttachmentTypeId > 0)
            {
                e.RequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Bearer", loginState.Token));
                e.RequestData.Add("_pEntityIdentifierGuid", ReferenceGuid);
                e.RequestData.Add("_pCommunicationGuid", CommunicationId);
                e.RequestData.Add("_userName", loginState.Username);
                e.RequestData.Add("_typeId", TempAttachement.AttachmentTypeId);
                e.RequestData.Add("_uploadFrom", UploadFrom);
                e.RequestData.Add("_project", "FATWA_ADMIN");
                if (TempAttachement.OtherAttachmentType != null)
                    e.RequestData.Add("_otherAttachmentType", TempAttachement.OtherAttachmentType);
                if (TempAttachement.Description != null)
                    e.RequestData.Add("_description", TempAttachement.Description);
                if (TempAttachement.ReferenceNo != null)
                    e.RequestData.Add("_referenceNo", TempAttachement.ReferenceNo);
                if (TempAttachement.ReferenceDate != null)
                    e.RequestData.Add("_referenceDate", TempAttachement.ReferenceDate);
                if (TempAttachement.DocumentDate != null)
                    e.RequestData.Add("_documentDate", TempAttachement.DocumentDate);
                if (TempAttachement.FileName != null)
                    e.RequestData.Add("_FileTitle", TempAttachement.FileName);
            }
            else
            {
                e.IsCancelled = true;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Must_Attachment_Type"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Add uploaded document to grid list</History>
        //<History Author = 'Hassan Abbas' Date='2022-11-15' Version="1.0" Branch="master"> Auto save the document in UPLOADED_DOCUMENT table after success</History>
        protected async Task OnSuccessHandler(UploadSuccessEventArgs e)
        {
            var pathResponse = JsonConvert.DeserializeObject<FileUploadSuccessResponse>(e.Request.ResponseText);
            if (!(bool)IsUploadPopup)
            {
                var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
                TempAttachementVM tempAttachement = new TempAttachementVM();
                tempAttachement.DocType = e.Files[0].Extension;
                tempAttachement.FileName = String.IsNullOrEmpty(TempAttachement.FileName) ? e.Files[0].Name : TempAttachement.FileName + e.Files[0].Extension;
                tempAttachement.UploadedBy = loginState.Username;
                tempAttachement.DocDateTime = DateTime.Now;
                tempAttachement.DocumentDate = DateTime.Now;
                var type = AttachmentTypes?.Where(t => t.AttachmentTypeId == TempAttachement.AttachmentTypeId).FirstOrDefault();
                tempAttachement.AttachmentTypeId = type?.AttachmentTypeId;
                tempAttachement.Type_En = type?.Type_En;
                tempAttachement.Type_Ar = type?.Type_Ar;
                tempAttachement.StoragePath = pathResponse?.StoragePath;
                tempAttachement.AttachementId = pathResponse?.AttachementId;
                if (TempFiles == null)
                {
                    TempFiles = new ObservableCollection<TempAttachementVM>();
                }
                TempFiles.Add(tempAttachement);
                if (!Multiple)
                {
                    if (TempFiles.Count() > 0)
                        Enabled = false;
                }
            }
            else
            {
                TempAttachement.DocType = e.Files[0].Extension;
                TempAttachement.FileName = String.IsNullOrEmpty(TempAttachement.FileName) ? e.Files[0].Name : TempAttachement.FileName + e.Files[0].Extension;
                TempAttachement.UploadedBy = loginState.Username;
                TempAttachement.DocDateTime = DateTime.Now;
                TempAttachement.DocumentDate = DateTime.Now;
                TempAttachement.StoragePath = pathResponse?.StoragePath;
                TempAttachement.AttachementId = pathResponse?.AttachementId;
                if (!Multiple)
                {
                    if (TempFiles.Count() > 0)
                        Enabled = false;
                }
                if ((bool)AutoSave)
                {
                    var response = await fileUploadService.UploadTempAttachmentToUploadedDocument((Guid)ReferenceGuid);
                    if (response.IsSuccessStatusCode)
                    {
                        dialogService.Close(TempAttachement);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    dialogService.Close(TempAttachement);
                }
            }
            spinnerService.Hide();
        }

        protected async Task OnErrorHandler(Telerik.Blazor.Components.UploadErrorEventArgs e)
        {
            var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
            notificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                Summary = translationState.Translate("Error"),
                Style = "position: fixed !important; left: 0; margin: auto; "
            });
        }
        protected void RemoveFailedFilesFromList(List<UploadFileInfo> files)
        {
            foreach (var file in files)
            {
                if (FilesValidationInfo.Keys.Contains(file.Id))
                {
                    FilesValidationInfo.Remove(file.Id);
                }
            }
        }

        protected bool IsSelectedFileValid(UploadFileInfo file)
        {
            return !(file.InvalidExtension || file.InvalidMaxFileSize || file.InvalidMinFileSize);
        }

        #endregion

        #region View and Download Attachments

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Download Attachment using stream</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then download</History>
        protected async Task DownloadAttachement(TempAttachementVM theUpdatedItem)
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
                    //var physicalPath = Path.Combine(Directory.GetCurrentDirectory() + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
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
					var httpClient = new HttpClient();
					var RleasephysicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
					RleasephysicalPath = RleasephysicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
					if (!String.IsNullOrEmpty(RleasephysicalPath))
					{
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

        /*<History Author = 'Hassan Abbas' Date='2024-03-14' Version="1.0" Branch="master"> Append Custom Action into the Pdf Viewer Toolbar through Html</History>*/
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await jSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }

        //<History Author = 'Hassan Abbas' Date='2024-03-21' Version="1.0" Branch="master"> Open Document in New Window</History>
        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            var url = PreviewedDocumentId > 0 ? $"/preview-document/{ReferenceGuid}/{PreviewedDocumentId}" : $"/preview-attachement/{ReferenceGuid}/{PreviewedAttachementId}";
            await jSRuntime.InvokeVoidAsync("openNewWindow", url);
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
                ToolbarItems = new List<ToolbarItem>()
                {
                    ToolbarItem.PageNavigationTool,
                    ToolbarItem.MagnificationTool,
                    ToolbarItem.SelectionTool,
                    ToolbarItem.PanTool,
                    ToolbarItem.SearchOption,
                    ToolbarItem.PrintOption,
                    ToolbarItem.DownloadOption
                };
                DisplayDocumentViewer = false;
                StateHasChanged();
                PreviewedDocumentId = theUpdatedItem.UploadedDocumentId != null ? theUpdatedItem.UploadedDocumentId : 0;
                PreviewedAttachementId = theUpdatedItem.AttachementId != null ? theUpdatedItem.AttachementId : 0;
                var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
#if !DEBUG
				{
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
				}
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                    StateHasChanged();
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

        #endregion

        #region Add Document Popup

        //<History Author = 'Hassan Abbas' Date='2022-11-15' Version="1.0" Branch="master"> Open popup for uploading document in the same component</History>
        protected async Task AddDocument(TempAttachementVM? tempAttachement)
        {
            if (tempAttachement != null)
            {
                var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", ReferenceGuid },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "MandatoryAttachmentTypeId", MandatoryAttachmentTypeId},

                    }
                    ,
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false }
                );
                var document = (TempAttachementVM)result;
                if (document != null)
                {
                    MandatoryTempFiles.Remove(tempAttachement);
                    MandatoryTempFiles.Add(document);
                }
                await additionalAttachGrid.Reload();
            }
            else
            {
                TempAttachementVM document = new TempAttachementVM();
                var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                new Dictionary<string, object>()
                {
                        { "ReferenceGuid", ReferenceGuid },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "MandatoryAttachmentTypeId", MandatoryAttachmentTypeId},
                }
                ,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false }
                );
                document = (TempAttachementVM)result;
                if (document != null)
                {
                    AdditionalTempFiles.Add(document);
                    await additionalAttachGrid?.Reload();
                }
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Dropdown Data and Change Events

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateAttachmentTypes()
        {
            ApiCallResponse response;
            if ((bool)IsViewOnly)
                response = await fileUploadService.GetAttachmentTypes(0);
            else
                response = await fileUploadService.GetAttachmentTypes(ModuleId);

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

        #region Pupup Events

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Invoke Hidden File Uploader button on Form submit click to upload file and submit form at the same time</History>
        protected async Task Form0Submit(TempAttachementVM args)
        {
            try
            {
                spinnerService.Show();
                if (FilesValidationInfo.Where(f => f.Value == true).Any())
                {
                    await jSRuntime.InvokeVoidAsync("UploadFile");
                }
                else
                {
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Select_File"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Format File Max Size
        protected string FormatFileMaxSize(int fileMaxSizeBytes)
        {
            var asdasf = $"{fileMaxSizeBytes / (1024.0 * 1024 * 1024):F2} GB";
            if (fileMaxSizeBytes < 1024)
            {
                return $"{fileMaxSizeBytes} Bytes";
            }
            if (fileMaxSizeBytes < 1024.0 * 1024)
            {
                return $"{fileMaxSizeBytes / 1024:F2} KB";
            }
            if (fileMaxSizeBytes < 1024.0 * 1024 * 1024)
            {
                return $"{fileMaxSizeBytes / (1024 * 1024):F2} MB";
            }
            return $"{fileMaxSizeBytes / (1024.0 * 1024 * 1024):F2} GB";
        }
        #endregion
    }
}

using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using System.Collections.ObjectModel;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using static FATWA_DOMAIN.Enums.DmsEnums;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegalLegislationDialog : ComponentBase
    {
        #region Constructor
        public AddLegalLegislationDialog()
        {
            legalLegislation = new LegalLegislation();
            ShowResultFileNameInGrid = new List<TempAttachement>();
            LegislationIdAsEdit = Guid.Empty;
        }
        #endregion

        #region Get LegislationId For Edit Attachment Case
        [Parameter]
        public Guid LegislationIdForEditAttachment { get; set; }
        #endregion

        #region Variables declaration
        public LegalLegislation legalLegislation { get; set; }
        public List<TempAttachement> ShowResultFileNameInGrid { get; set; }
        public RadzenDataGrid<DMSKayPublicationDocumentListVM> KayFileGrid { get; set; } = new RadzenDataGrid<DMSKayPublicationDocumentListVM>();
        public IList<DMSKayPublicationDocumentListVM> kayselectedDocuments { get; set; } = new List<DMSKayPublicationDocumentListVM>();
        LegalLegislationSourceDocSearchVM fileSearch = new LegalLegislationSourceDocSearchVM();

        public bool allowRowSelectOnRowClick = true;
        public bool IsKuwaitAlyawm = true;
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public bool busyPreviewBtn { get; set; }
        public string KayDocumentBase64 { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        public bool DisplayFileUploader { get; set; }
        private bool IsGridKayRefreshed = false;
        public string SaveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/SingleUpload";
        public string RemoveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Remove";
        //public ObservableCollection<TempAttachement> TempFiles { get; set; } = new ObservableCollection<TempAttachement>();
        public ObservableCollection<TempAttachementVM> TempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();
        protected string pathImg = null;
        protected string pathOldImg = null;
        public Guid LegislationIdAsEdit { get; set; }
        protected KayDocumentListAdvanceSearchVM advanceSearchVM { get; set; } = new KayDocumentListAdvanceSearchVM { IsFullEdition = true };
        protected bool Keywords = false;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => KayFileGrid.CurrentPage + 1;
        private int CurrentPageSize => KayFileGrid.PageSize;
        public bool isVisible { get; set; }
        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            LegislationIdAsEdit = LegislationIdForEditAttachment;
            if (LegislationIdAsEdit != Guid.Empty)
            {
                legalLegislation.LegislationId = LegislationIdAsEdit;
            }
            else
            {
                legalLegislation.LegislationId = Guid.NewGuid();
            }
        }
        #endregion

        #region Full property declaration of Kay publication Documents
        IEnumerable<DMSKayPublicationDocumentListVM> DocumentList;
        IEnumerable<DMSKayPublicationDocumentListVM> _FilteredDocumentList;
        protected IEnumerable<DMSKayPublicationDocumentListVM> FilteredDocumentList
        {
            get
            {
                return _FilteredDocumentList;
            }
            set
            {
                if (!object.Equals(_FilteredDocumentList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredDocumentList", NewValue = value, OldValue = _FilteredDocumentList };
                    _FilteredDocumentList = value;
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

        #region Kay Document List & Search/Advance Search
        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search;
                FilteredDocumentList = await gridSearchExtension.Filter(DocumentList, new Query()
                {
                    Filter = $@"i => (i.EditionNumber != null && i.EditionNumber.ToString().ToLower().Contains(@0)) || (i.PublicationDate != null && i.PublicationDate.ToString().ToLower().Contains(@1)) || (i.PublicationDateHijri != null && i.PublicationDateHijri.ToString().ToLower().Contains(@2)) || (i.DocumentTitle != null && i.DocumentTitle.ToString().ToLower().Contains(@3))",
                    FilterParameters = new object[] { search.ToLower(), search.ToLower(), search.ToLower(), search.ToLower() }
                });
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

        #region Advance Search
        public async void ResetForm()
        {
            advanceSearchVM = new KayDocumentListAdvanceSearchVM { PageSize = KayFileGrid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            KayFileGrid.Reset();
            await KayFileGrid.Reload();
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
            if (advanceSearchVM.PublicationFrom > advanceSearchVM.PublicationTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = advanceSearchVM.isDataSorted = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(advanceSearchVM.DocumentTitle) && string.IsNullOrWhiteSpace(advanceSearchVM.EditionNumber) && !advanceSearchVM.PublicationFrom.HasValue && !advanceSearchVM.PublicationTo.HasValue)
            {

            }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (KayFileGrid.CurrentPage > 0)
                {
                    await KayFileGrid.FirstPage();
                }
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await KayFileGrid.Reload();
                }
                StateHasChanged();
            }
        }

        #endregion

        #region View And Download Attachment
        protected async Task ViewKayAttachement(DMSKayPublicationDocumentListVM theUpdatedItem)
        {
            try
            {
                if (theUpdatedItem != null)
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
                    PreviewedDocumentId = theUpdatedItem.Id;
                    spinnerService.Show();
                    DisplayDocumentViewer = false;
                    busyPreviewBtn = true;
                    await Task.Run(() => DecryptDocument(theUpdatedItem));
                    await Task.Delay(1000);
                    busyPreviewBtn = false;
                    spinnerService.Hide();
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

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task DecryptDocument(DMSKayPublicationDocumentListVM theUpdatedItem)
        {
            try
            {
                string physicalPath = string.Empty;

#if DEBUG
                {
                    physicalPath = Path.Combine(theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                }
#else
				{
                     physicalPath = Path.Combine(_config.GetValue<string>("kay_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace(_config.GetValue<string>("KayPublicationsPath"), "");
				}
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {

                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, ".pdf", _config.GetValue<string>("DocumentEncryptionKey"));
                    KayDocumentBase64 = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
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

        protected async Task DownloadAttachement(DMSKayPublicationDocumentListVM theUpdatedItem)
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
                    var physicalPath = Path.Combine(theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    if (!String.IsNullOrEmpty(physicalPath))
                    {
                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Open);
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        await blazorDownloadFileService.DownloadFile(theUpdatedItem.FileTitle, fsOut, "application/octet-stream");
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
					var RleasephysicalPath = Path.Combine(_config.GetValue<string>("kay_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    RleasephysicalPath = RleasephysicalPath.Replace(_config.GetValue<string>("KayPublicationsPath"), "");
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

							await blazorDownloadFileService.DownloadFile(theUpdatedItem.FileTitle, fsOut, "application/octet-stream");
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

        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        KayFileGrid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    advanceSearchVM.FromLegalLegislationForm = true;
                    var response = await fileUploadService.GetkayDocumentsListForDms(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredDocumentList = DocumentList = (IEnumerable<DMSKayPublicationDocumentListVM>)response.ResultData;
                        await InvokeAsync(StateHasChanged);
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput();
                        if (!string.IsNullOrEmpty(dataArgs.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredDocumentList = await gridSearchExtension.Sort(FilteredDocumentList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = DocumentList.Any() ? (DocumentList.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = DocumentList.Any() ? (DocumentList.First().TotalCount) / (KayFileGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    KayFileGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchVM.PageNumber == 1 || (advanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchVM.PageNumber = CurrentPage;
            advanceSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<DMSKayPublicationDocumentListVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredDocumentList = await gridSearchExtension.Sort(FilteredDocumentList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }

        #endregion

        #region Dialog Buttons

        //<History Author = 'Hassan Abbas' Date='2023-07-10' Version="1.0" Branch="master"> Submit Document Seelction and Navigate to legal principles form</History>
        protected async Task SubmitDocumentSelection(MouseEventArgs args)
        {
            try
            {
                spinnerService.Show();
                //var externalAttachments = LegislationIdForEditAttachment == Guid.Empty ? await fileUploadService.GetTempAttachements(legalLegislation.LegislationId) : null;
                var externalAttachments = await fileUploadService.GetTempAttachements(legalLegislation.LegislationId);
                if ((externalAttachments != null && externalAttachments.Any()) && kayselectedDocuments != null && kayselectedDocuments.Any())
                {
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Must_Select_Atleast_One_Source_Either_from_Kuwait"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else if ((externalAttachments != null && externalAttachments.Any()) || (kayselectedDocuments != null && kayselectedDocuments.Any()))
                {
                    spinnerService.Hide();
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("Yes"),
                        CancelButtonText = translationState.Translate("No")
                    }) == true)
                    {
                        spinnerService.Show();
                        if ((externalAttachments != null && externalAttachments.Any()) || kayselectedDocuments != null && kayselectedDocuments.Any())
                        {
                            var response = await fileUploadService.CopyLegalLegislationSourceAttachments(
                                new CopyLegalLegislationSourceAttachmentsVM
                                {
                                    KayselectedDocumentsIds = kayselectedDocuments.Select(d => (int)d.Id).ToList(),
                                    CreatedBy = loginState.UserDetail.UserName,
                                    DestinationId = legalLegislation.LegislationId
                                });
                            if (response.IsSuccessStatusCode)
                            {
                                navigationManager.NavigateTo("/add-legislation/" + legalLegislation.LegislationId + "/" + (LegislationIdForEditAttachment != Guid.Empty ? true : false));
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
                        else
                        {
                            navigationManager.NavigateTo("/add-legislation/" + legalLegislation.LegislationId + "/" + (LegislationIdForEditAttachment != Guid.Empty ? true : false));
                        }
                        spinnerService.Hide();
                    }
                }
                else
                {
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Must_Select_Atleast_One_Source"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
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

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

        #region Tab Change
        protected async Task OnTabChange(int value)
        {
            try
            {
                if (value == 0)
                {
                    DisplayFileUploader = false;
                    var response = await fileUploadService.RemoveTempAttachementsByReferenceId(legalLegislation.LegislationId);
                }
                else
                {
                    kayselectedDocuments.Clear();
                    DisplayFileUploader = true;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Document Preview 
        //<History Author = 'Hassan Abbas' Date='2024-03-21' Version="1.0" Branch="master"> Open Document in New Window</History>
        public int? PreviewedDocumentId { get; set; }
        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            await JsInterop.InvokeVoidAsync("openNewWindow", "/preview-legislation-document/" + PreviewedDocumentId + "/" + (int)DocumentLinkModuleEnum.LegalDocument);
        }
        #endregion

    }
}

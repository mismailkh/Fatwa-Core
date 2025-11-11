using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Security.Cryptography;
using System.Text;
using Telerik.Blazor.Components;

namespace FATWA_WEB.Pages.Dms
{
    public partial class ListKayPublication : ComponentBase
    {
        #region Varriable
        protected KayDocumentListAdvanceSearchVM advanceSearchVM { get; set; } = new KayDocumentListAdvanceSearchVM();
        protected RadzenDataGrid<DMSKayPublicationDocumentListVM>? grid = new RadzenDataGrid<DMSKayPublicationDocumentListVM>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        protected List<AttachmentType> attachmentTypes { get; set; } = new List<AttachmentType>();
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public SfPdfViewerServer viewer { get; set; } = new SfPdfViewerServer();
        public byte[] FileData { get; set; }
        public string DocumentPath { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        public bool busyPreviewBtn { get; set; }
        protected bool ShowReferenceFields { get; set; }
        public bool allowRowSelectOnRowClick = true;

        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;


        #endregion

        #region Full property declaration

        IEnumerable<DMSKayPublicationDocumentListVM> FullEditionDocumentList;
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

        #region On Initialize
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
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
                        grid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    advanceSearchVM.FromLegalLegislationForm = false;
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
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
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
                int TotalPages = DocumentList.Any() ? (DocumentList.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
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

        #region OnSearchInput 
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                FilteredDocumentList = await gridSearchExtension.Filter(DocumentList, new Query()
                {
                    Filter = $@"i => (i.EditionNumber != null && i.EditionNumber.ToString().ToLower().Contains(@0)) || (i.PublicationDate != null && i.PublicationDate.ToString().ToLower().Contains(@1)) || (i.PublicationDateHijri != null && i.PublicationDateHijri.ToString().ToLower().Contains(@2)) || (i.DocumentTitle != null && i.DocumentTitle.ToString().ToLower().Contains(@3)) || (i.EditionType != null && i.EditionType.ToString().ToLower().Contains(@4))",
                    FilterParameters = new object[] { search.ToLower(), search.ToLower(), search.ToLower(), search.ToLower(), search.ToLower() }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredDocumentList = await gridSearchExtension.Sort(FilteredDocumentList, ColumnName, SortOrder);

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
            advanceSearchVM = new KayDocumentListAdvanceSearchVM { PageSize = grid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
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
            if (string.IsNullOrWhiteSpace(advanceSearchVM.PublicationDateHijri) && string.IsNullOrWhiteSpace(advanceSearchVM.EditionNumber)
                 && !advanceSearchVM.PublicationFrom.HasValue && !advanceSearchVM.PublicationTo.HasValue)
            {

            }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                {
                    await grid.FirstPage();
                }
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                StateHasChanged();
            }
        }

        #endregion

        public async Task<DMSKayPublicationDocumentListVM> GetEditionDocument(string editionNumber)
        {
            var response = await fileUploadService.GetkayDocumentAccordingEditionNumber(editionNumber);
            if (response.IsSuccessStatusCode)
            {
                return (DMSKayPublicationDocumentListVM)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                return null;
            }
        }

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

        #region View and Download Attachments

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Download Attachment using stream</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then download</History>
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



        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task ViewAttachement(DMSKayPublicationDocumentListVM theUpdatedItem, bool isViewForEditionNumber = false)
        {
            try
            {
                if (isViewForEditionNumber)
                    theUpdatedItem = await GetEditionDocument(theUpdatedItem.EditionNumber);
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
                var physicalPath = string.Empty;
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
                    DocumentPath = "data:application/pdf;base64," + base64String;
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
        #endregion
    }
}

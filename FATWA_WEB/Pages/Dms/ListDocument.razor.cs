using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Security.Cryptography;
using System.Text;

namespace FATWA_WEB.Pages.Dms
{
    //<History Author = 'Muhammad Zaeem' Date='2023-06-06' Version="1.0" Branch="master">Document List Page</History>

    public partial class ListDocument : ComponentBase
    {

        #region Varriable

        protected DocumentListAdvanceSearchVM advanceSearchVM { get; set; } = new DocumentListAdvanceSearchVM();
        protected RadzenDataGrid<DMSDocumentListVM> grid = new RadzenDataGrid<DMSDocumentListVM>();
        protected RadzenDataGrid<DMSDocumentListVM> gridFav = new RadzenDataGrid<DMSDocumentListVM>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected List<AttachmentType> attachmentTypes { get; set; } = new List<AttachmentType>();
        public bool IsFavourite { get; set; } = false;

        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage { get; set; }
        private int CurrentPageSize { get; set; }
        protected int selectedTabIndex { get; set; } = 0;


        #endregion

        #region Full property declaration

        IEnumerable<DMSDocumentListVM> documentList = new List<DMSDocumentListVM>();
        protected IEnumerable<DMSDocumentListVM> FilteredDocumentList { get; set; }

        protected string search { get; set; }

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
            translationState.TranslateGridFilterLabels(gridFav);
            await PopulateAttachmentTypes();
            spinnerService.Hide();
        }
        #endregion

        #region OnSearchInput 
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();
                FilteredDocumentList = await gridSearchExtension.Filter(documentList, new Query()
                {
                    Filter = $@"i => 
                    (i.FileName != null && i.FileName.ToString().ToLower().Contains(@0)) 
                    || (i.AttachmentTypeAr != null && i.AttachmentTypeAr.ToString().ToLower().Contains(@1)) 
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) 
                    || (i.SharedByAr != null && i.SharedByAr.ToString().ToLower().Contains(@0)) 
                    || (i.SharedByEn != null && i.SharedByEn.ToString().ToLower().Contains(@0)) 
                    || (i.StatusEn != null && i.StatusEn.ToString().ToLower().Contains(@0)) 
                    || (i.StatusAr != null && i.StatusAr.ToString().ToLower().Contains(@0)) 
                    || (i.DocumentName != null && i.DocumentName.ToString().ToLower().Contains(@0)) 
                    || (i.VersionNumber.HasValue && i.VersionNumber.Value.ToString().Contains(@0))  
                    || (i.AttachmentTypeEn != null && i.AttachmentTypeEn.ToString().ToLower().Contains(@1))",
                    FilterParameters = new object[] { search.ToLower(), search.ToLower() }
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

        #region On Load Grid Data
        private async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (selectedTabIndex == 0)
                {
                    CurrentPage = grid.CurrentPage + 1;
                    CurrentPageSize = grid.PageSize;
                }
                else
                {
                    CurrentPage = gridFav.CurrentPage + 1;
                    CurrentPageSize = gridFav.PageSize;
                }

                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        if (selectedTabIndex == 0)
                            grid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        else
                            gridFav.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    if (selectedTabIndex == 0)
                        advanceSearchVM.isFavourite = false;
                    else
                        advanceSearchVM.isFavourite = true;
                    advanceSearchVM.UserId = loginState.UserDetail.UserId;
                    advanceSearchVM.RoleId = loginState.UserDetail.RoleId;
                    advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    advanceSearchVM.CreatedBy = loginState.UserDetail.Email;
                    spinnerService.Show();
                    var response = await fileUploadService.GetDocumentsList(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredDocumentList = documentList = (IEnumerable<DMSDocumentListVM>)response.ResultData;
                        await InvokeAsync(StateHasChanged);
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput();
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
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
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = documentList.Any() ? (documentList.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = documentList.Any() ? (documentList.First().TotalCount) / (selectedTabIndex == 0 ? grid.PageSize : gridFav.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    if (selectedTabIndex == 0)
                        grid.CurrentPage = TotalPages;
                    else
                        gridFav.CurrentPage = TotalPages;
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
        private async Task OnSortData(DataGridColumnSortEventArgs<DMSDocumentListVM> args)
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

        #region Grid Buttons
        protected async Task DocumentDetail(DMSDocumentListVM args)
        {
            if (args.IsDocumentAddedStatus == false)
            {
                navigationManager.NavigateTo("document-detail/" + Convert.ToInt32(args.UploadedDocumentId));
            }
            else
            {
                navigationManager.NavigateTo("document-view/" + args.AddedDocumentId.ToString() + '/' + args.VersionId?.ToString());
            }
        }
        protected async Task AddFavouriteDocument(DMSDocumentListVM args)
        {
            bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Add_Favourite_Document"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });
            if (dialogResponse == true)
            {
                args.UserId = loginState.UserDetail.UserId;
                var response = await fileUploadService.AddDocumentToFavourite(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Document_Added_To_Favourite"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    dialogService.Close();
                    await grid.Reload();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

            }

        }
        protected async Task RemoveFavouriteDocument(DMSDocumentListVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Remove_Favourite_Document"), translationState.Translate("delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var response = await fileUploadService.RemoveFavouriteDocument(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Remove_Favourite_Document_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await grid.Reload();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Advance Search
        public async void ResetForm()
        {
            advanceSearchVM = new DocumentListAdvanceSearchVM { PageSize = selectedTabIndex == 0 ? grid.PageSize : gridFav.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            if (selectedTabIndex == 0)
            {
                grid.Reset();
                await grid.Reload();
            }
            else
            {
                gridFav.Reset();
                await gridFav.Reload();
            }
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
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = advanceSearchVM.isDataSorted = true;
                return;
            }
            if (advanceSearchVM.AttachmentTypeId == 0 && string.IsNullOrWhiteSpace(advanceSearchVM.Filename)
                 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
            {

            }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (selectedTabIndex == 0)
                {
                    if (grid.CurrentPage > 0)
                    {
                        await grid.FirstPage();
                    }
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await grid.Reload();
                    }
                }
                else
                {
                    if (gridFav.CurrentPage > 0)
                    {
                        await gridFav.FirstPage();
                    }
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await gridFav.Reload();
                    }
                }
            }
        }

        #endregion

        #region Redirect Function


        protected async Task AddDocument(MouseEventArgs args)
        {
            try
            {
                navigationManager.NavigateTo("/document-add/");
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

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

        #region populate grids
        //<History Author = 'Muhammad Zaeem' Date='2023-03-14' Version="1.0" Branch="master">Populate Count data</History>
        protected async Task PopulateAttachmentTypes()
        {
            var response = await fileUploadService.GetAllAttachmentTypes();
            if (response.IsSuccessStatusCode)
            {
                attachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region View and Download Attachments

        //<History Author = 'Hassan Abbas' Date='2023-06-24' Version="1.0" Branch="master"> Download Attachment using stream</History>
        protected async Task DownloadAttachement(DMSDocumentListVM theUpdatedItem)
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
                    if (!string.IsNullOrEmpty(physicalPath))
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

        //<History Author = 'Hassan Abbas' Date='2023-06-24' Version="1.0" Branch="master"> Convert And Download Attachment using stream</History>
        protected async Task ConvertAndDownloadAttachement(DMSDocumentListVM theUpdatedItem)
        {
            try
            {
                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter optionsr
                converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;
                converter.Options.PdfPageOrientation = SelectPdf.PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;

                // create a new pdf document converting an url
                SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(theUpdatedItem.Content);
                pdfDocument.Save(stream);
                pdfDocument.Close();
                await blazorDownloadFileService.DownloadFile(theUpdatedItem.DocumentName + ".pdf", stream, "application/octet-stream");
                stream.Close();
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
        private Stream GetFileStream(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);
            return ms;
        }
        #endregion

        #region TabChange

        protected async Task OnTabChange(int index)
        {
            if (index == selectedTabIndex) { return; }
            selectedTabIndex = index;
            search = ColumnName = string.Empty;
            isVisible = Keywords = false;
            advanceSearchVM = new DocumentListAdvanceSearchVM();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            await Task.Delay(100);
            if (selectedTabIndex == 0)
            {
                grid.Reset();
                await grid.Reload();
            }
            else
            {
                gridFav.Reset();
                await gridFav.Reload();
            }
            StateHasChanged();
        }
        #endregion
    }
}

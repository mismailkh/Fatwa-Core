using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using static FATWA_DOMAIN.Models.ViewModel.LiteratureAdvancedSearchVM;
using Enum = System.Enum;
using Query = Radzen.Query;

namespace FATWA_WEB.Pages.Lms
{
    /*<History Author = 'Ammaar Naveed' Date='2024-09-17' Version="1.0" Branch="master">View and download IsViewable literature</History>*/

    public partial class ListViewableLiteratures : ComponentBase
    {
        #region Variables Declaration
        public bool isVisible { get; set; }
        protected LiteratureAdvancedSearchVM advancedSearchVM = new LiteratureAdvancedSearchVM();
        protected RadzenDataGrid<LmsViewableLiteratureVM> grid = new RadzenDataGrid<LmsViewableLiteratureVM>();
        protected bool Keywords = false;
        protected List<object> AdvancedSearchOptions { get; set; } = new List<object>();
        protected bool CheckKeywords = false;
        protected UserDetailVM userDetails { get; set; } = new UserDetailVM();
        protected IEnumerable<LmsLiteratureIndex> LiteratureIndexeDetails { get; set; } = new List<LmsLiteratureIndex>();
        public IEnumerable<LmsLiteratureAuthor> LmsLiteratureAuthor { get; set; } = new List<LmsLiteratureAuthor>();
        protected string search { get; set; }
        public bool allowRowSelectOnRowClick = true;
        protected int count;
        public IEnumerable<LmsViewableLiteratureVM> getLmsLiteraturesResult { get; set; } = new List<LmsViewableLiteratureVM>();
        protected IEnumerable<LmsViewableLiteratureVM> FilteredViewableLiterature { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        private bool DisplayDocumentViewer { get; set; }
        private int? LiteratureId { get; set; }
        private int? PreviewedDocumentId { get; set; }
        public byte[] FileData { get; set; }
        private string DocumentPath { get; set; }
        private string SaveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Upload";
        public bool busyPreviewBtn { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion     

        #region Initialized\Load
        protected override async Task OnInitializedAsync()
        {
            userDetails = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(grid);
            await PopulateAdvancedSearchOptions();
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advancedSearchVM.PageNumber || CurrentPageSize != advancedSearchVM.PageSize || (Keywords && advancedSearchVM.isDataSorted))
                {
                    if (advancedSearchVM.isGridLoaded && advancedSearchVM.PageSize == CurrentPageSize && !advancedSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advancedSearchVM.PageNumber - 1;
                        advancedSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    var response = await lmsLiteratureService.GetLmsViewableLiteratures(advancedSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        getLmsLiteraturesResult = (IEnumerable<LmsViewableLiteratureVM>)response.ResultData;
                        FilteredViewableLiterature = (IEnumerable<LmsViewableLiteratureVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredViewableLiterature = await gridSearchExtension.Sort(FilteredViewableLiterature, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
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
            if (advancedSearchVM.PageSize != null && advancedSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getLmsLiteraturesResult.Any() ? (getLmsLiteraturesResult.First().TotalCount) / ((int)advancedSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advancedSearchVM.PageNumber - 1;
                advancedSearchVM.isGridLoaded = true;
                advancedSearchVM.PageNumber = CurrentPage;
                advancedSearchVM.PageSize = args.Top;
                int TotalPages = getLmsLiteraturesResult.Any() ? (getLmsLiteraturesResult.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advancedSearchVM.PageNumber = TotalPages + 1;
                    advancedSearchVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((advancedSearchVM.PageNumber == 1 || (advancedSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advancedSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advancedSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advancedSearchVM.PageNumber = CurrentPage;
            advancedSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<LmsViewableLiteratureVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredViewableLiterature = await gridSearchExtension.Sort(FilteredViewableLiterature, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advancedSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Get Author & Literature Index Details
        private async Task GetLiteratureIndexDetails()
        {
            LiteratureIndexeDetails = await lmsLiteratureIndexService.GetLiteratureIndexDetails();
        }

        private async Task GetAuthors()
        {
            try
            {
                var response = await lmsLiteratureService.GetAuthorItems();
                if (response.IsSuccessStatusCode)
                {
                    LmsLiteratureAuthor = (IEnumerable<LmsLiteratureAuthor>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Grid Search
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {

                        FilteredViewableLiterature = await gridSearchExtension.Filter(getLmsLiteraturesResult, new Query()
                        {
                            Filter = $@"i => (i.LiteratureName != null && i.LiteratureName.ToLower().Contains(@0)) || (i.EditionNumber != null && i.EditionNumber.ToLower().Contains(@1)) || (i.IndexName_En != null && i.IndexName_En.ToLower().Contains(@2)) || (i.LiteratureAuthor_En != null && i.LiteratureAuthor_En.ToLower().Contains(@3)) || (i.ISBN != null && i.ISBN.ToLower().Contains(@4)) || (i.EditionYear.HasValue && i.EditionYear.Value.ToString(""dd/MM/yyyy"").Contains(@5))",
                            FilterParameters = new object[] { search, search, search, search, search, search }
                        });
                    }
                    else
                    {
                        FilteredViewableLiterature = await gridSearchExtension.Filter(getLmsLiteraturesResult, new Query()
                        {
                            Filter = $@"i => (i.LiteratureName != null && i.LiteratureName.ToLower().Contains(@0)) || (i.EditionNumber != null && i.EditionNumber.ToLower().Contains(@1)) || (i.IndexName_Ar != null && i.IndexName_Ar.ToLower().Contains(@2)) || (i.LiteratureAuthor_Ar != null && i.LiteratureAuthor_Ar.ToLower().Contains(@3)) || (i.ISBN != null && i.ISBN.ToLower().Contains(@4)) || (i.EditionYear.HasValue && i.EditionYear.Value.ToString(""dd/MM/yyyy"").Contains(@5))",
                            FilterParameters = new object[] { search, search, search, search, search, search }
                        });
                    }
                    if (!string.IsNullOrEmpty(ColumnName))
                    {
                        FilteredViewableLiterature = await gridSearchExtension.Sort(FilteredViewableLiterature, ColumnName, SortOrder);
                    }
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion        

        #region Advance Search
        public class AdvancedSearchEnumTypes
        {
            public LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum advancedSearchEnumValue { get; set; }
            public string advancedSearchEnumName { get; set; }
        }
        protected async Task PopulateAdvancedSearchOptions()
        {
            foreach (LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum item in Enum.GetValues(typeof(LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum)))
            {
                AdvancedSearchOptions.Add(new AdvancedSearchEnumTypes { advancedSearchEnumName = translationState.Translate(item.ToString()), advancedSearchEnumValue = item });
            }
            StateHasChanged();
        }
        protected async Task SubmitAdvanceSearch()
        {
            if (advancedSearchVM.FromDate > advancedSearchVM.ToDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            else if (advancedSearchVM.LiteratureId == 0
                && advancedSearchVM.ClassificationId == 0
                && advancedSearchVM.IndexId == 0
                && string.IsNullOrEmpty(advancedSearchVM.KeywordsType)
                && advancedSearchVM.GenericsIntergerKeyword == 0
                && advancedSearchVM.FromDate == null
                && advancedSearchVM.ToDate == null) { }
            else
            {
                Keywords = advancedSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                    await grid.FirstPage();
                else
                {
                    advancedSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                StateHasChanged();
            }
        }
        public async void ResetForm()
        {
            advancedSearchVM = new LiteratureAdvancedSearchVM { PageSize = grid.PageSize };
            Keywords = advancedSearchVM.isDataSorted = false;
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
        protected async Task OnChangeSearchValue()
        {
            spinnerService.Show();
            advancedSearchVM.KeywordsType = null;
            advancedSearchVM.GenericsIntergerKeyword = 0;
            switch ((int)advancedSearchVM.EnumSearchValue)
            {
                case (int)AdvancedSearchDropDownEnum.Barcode:
                    break;

                case (int)AdvancedSearchDropDownEnum.Author_Name:
                    await GetAuthors();
                    break;

                case (int)AdvancedSearchDropDownEnum.Book_Index:
                    await GetLiteratureIndexDetails();
                    break;
                default:
                    break;
            }
            spinnerService.Hide();
            StateHasChanged();
        }
        #endregion

        #region Redirect Functions
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Document View And Download

        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }

        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            await JsInterop.InvokeVoidAsync("openNewWindow", "/preview-literature-document/" + LiteratureId + "/" + PreviewedDocumentId);
        }
        protected async Task OnGridViewClick(LmsViewableLiteratureVM args)
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

            LiteratureId = args.LiteratureId;
            PreviewedDocumentId = args.UploadedDocumentId;
            string physicalPath;
#if DEBUG
            {
                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + args.StoragePath).Replace(@"\\", @"\");

            }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + args.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
            var byteArray = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, args.DocType, _config.GetValue<string>("DocumentEncryptionKey"), true);

            if (byteArray != null && byteArray.Length > 0)
            {
                string base64String = Convert.ToBase64String(byteArray);
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
        protected async Task DownloadAttachement(LmsViewableLiteratureVM args, bool useDeprecatedService = false)
        {
            try
            {
                string physicalPath;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + args.StoragePath).Replace(@"\\", @"\");

                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + args.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif

                var byteArray = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, args.DocType, _config.GetValue<string>("DocumentEncryptionKey"), true);

                if (byteArray != null && byteArray.Length > 0)
                {
                    await blazorDownloadFileService.DownloadFile(args.FileName, byteArray, "application/octet-stream");

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

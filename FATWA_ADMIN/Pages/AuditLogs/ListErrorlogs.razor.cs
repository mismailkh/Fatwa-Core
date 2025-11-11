using FATWA_ADMIN.Services;
using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.AuditLogs
{
    public partial class ListErrorlogs : ComponentBase
    {
        #region Variable Declaration

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        protected int count { get; set; }
        protected ErrorLogAdvanceSearchVM errorLogAdvanceSearchVM = new ErrorLogAdvanceSearchVM();
        protected bool Keywords = false;
        protected bool AdvancedSearchResultGrid;
        public bool isVisible { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        protected RadzenDataGrid<ErrorLogVM> grid = new RadzenDataGrid<ErrorLogVM>();
        protected string search { get; set; }
        IEnumerable<ErrorLogVM> getErrorlogsResult { get; set; } = new List<ErrorLogVM>();
        protected IEnumerable<ErrorLogVM> getSortedErrorlogsResult { get; set; } = new List<ErrorLogVM>();
        protected IEnumerable<ErrorLogVM>? FilteredErrorlogsResult { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;

        #endregion
        #region Service Injections
        [Inject]
        protected ErrorLogService errorLogService { get; set; }

        #endregion▬

        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            errorLogAdvanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<ErrorLogVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredErrorlogsResult = await gridSearchExtension.Sort(FilteredErrorlogsResult, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                errorLogAdvanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region On Load Grid Data
        private async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != errorLogAdvanceSearchVM.PageNumber || CurrentPageSize != errorLogAdvanceSearchVM.PageSize || (Keywords && errorLogAdvanceSearchVM.isDataSorted))
                {
                    if (errorLogAdvanceSearchVM.isGridLoaded && errorLogAdvanceSearchVM.PageSize == CurrentPageSize && !errorLogAdvanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)errorLogAdvanceSearchVM.PageNumber - 1;
                        errorLogAdvanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var result = await errorLogService.GetErrorLogAdvanceSearch(errorLogAdvanceSearchVM);
                    if (result.IsSuccessStatusCode)
                    {
                        FilteredErrorlogsResult = getErrorlogsResult = (IEnumerable<ErrorLogVM>)result.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);

                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredErrorlogsResult = await gridSearchExtension.Sort(FilteredErrorlogsResult, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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
            if (errorLogAdvanceSearchVM.PageSize != null && errorLogAdvanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getErrorlogsResult.Any() ? (getErrorlogsResult.First().TotalCount) / ((int)errorLogAdvanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)errorLogAdvanceSearchVM.PageNumber - 1;
                errorLogAdvanceSearchVM.isGridLoaded = true;
                errorLogAdvanceSearchVM.PageNumber = CurrentPage;
                errorLogAdvanceSearchVM.PageSize = args.Top;
                int TotalPages = getErrorlogsResult.Any() ? (getErrorlogsResult.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    errorLogAdvanceSearchVM.PageNumber = TotalPages + 1;
                    errorLogAdvanceSearchVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((errorLogAdvanceSearchVM.PageNumber == 1 || (errorLogAdvanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    errorLogAdvanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    errorLogAdvanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            errorLogAdvanceSearchVM.PageNumber = CurrentPage;
            errorLogAdvanceSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Search Input
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();
                    FilteredErrorlogsResult = await gridSearchExtension.Filter(getErrorlogsResult, new Query()
                    {
                        Filter = $@"i => i.Subject.ToString().ToLower().Contains(@0) 
                        || i.Body.ToString().ToLower().Contains(@0) 
                        || i.UserName.ToString().ToLower().Contains(@0)
                        || i.IPDetails.ToString().ToLower().Contains(@0)
                        || (i.LogDate.HasValue && i.LogDate.Value.ToString(""dd/MM/yyyy"").Contains(@0))
                        || i.Source.ToString().ToLower().Contains(@0)",
                        FilterParameters = new object[] { search.ToLower() }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredErrorlogsResult = await gridSearchExtension.Sort(FilteredErrorlogsResult, ColumnName, SortOrder);
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
        #endregion

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (string.IsNullOrEmpty(errorLogAdvanceSearchVM.Category)
                && string.IsNullOrEmpty(errorLogAdvanceSearchVM.Subject)
                && !(errorLogAdvanceSearchVM.FromDate.HasValue)
                && !(errorLogAdvanceSearchVM.ToDate.HasValue)
                && string.IsNullOrEmpty(errorLogAdvanceSearchVM.ComputerName)
                && string.IsNullOrEmpty(errorLogAdvanceSearchVM.UserName)) { }
            else
            {
                Keywords = errorLogAdvanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                {
                    await grid.FirstPage();
                }
                else
                {
                    errorLogAdvanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                StateHasChanged();
            }
        }
        public async void ResetForm()
        {
            errorLogAdvanceSearchVM = new ErrorLogAdvanceSearchVM { PageSize = grid.PageSize };
            Keywords = errorLogAdvanceSearchVM.isDataSorted = false;
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
        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task ViewErrorLogDetail(ErrorLogVM args)
        {
            navigationManager.NavigateTo("/lds-ErrorLog-view/" + args.ErrorLogId);
        }

        #endregion
    }
}

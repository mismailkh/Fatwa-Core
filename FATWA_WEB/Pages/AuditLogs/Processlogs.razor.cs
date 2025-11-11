using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.AuditLogs
{
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master">Literature Classification Component</History>
    public partial class Processlogs : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        protected ProcessLogAdvanceSearchVM processLogAdvanceSearchVM;
        protected ProcessLogAdvanceSearchVM args;
        protected bool Keywords = false;
        protected bool AdvancedSearchResultGrid;
        public bool isVisible { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #region Service Injections
        [Inject]
        protected ProcessLogService processLogService { get; set; }

        [Inject]
        protected ErrorLogService errorLogService { get; set; }

        #endregion

        protected RadzenDataGrid<ProcessLogVM> grid;
        public bool isLoading { get; set; }
        public int count { get; set; }

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
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<ProcessLogVM> _getProcesslogsResult;
        protected IEnumerable<ProcessLogVM> FilteredGetProcesslogsResult { get; set; }
        protected IEnumerable<ProcessLogVM> getProcesslogsResult
        {
            get
            {
                return _getProcesslogsResult;
            }
            set
            {
                if (!object.Equals(_getProcesslogsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getProcesslogsResult", NewValue = value, OldValue = _getProcesslogsResult };
                    _getProcesslogsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            processLogAdvanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        private async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != processLogAdvanceSearchVM.PageNumber || CurrentPageSize != processLogAdvanceSearchVM.PageSize || (Keywords && processLogAdvanceSearchVM.isDataSorted))
                {
                    if (processLogAdvanceSearchVM.isGridLoaded && processLogAdvanceSearchVM.PageSize == CurrentPageSize && !processLogAdvanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)processLogAdvanceSearchVM.PageNumber - 1;
                        processLogAdvanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var result = await errorLogService.GetProcessLogAdvanceSearch(processLogAdvanceSearchVM);
                    if (result.IsSuccessStatusCode)
                    {
                        FilteredGetProcesslogsResult = getProcesslogsResult = (IEnumerable<ProcessLogVM>)result.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();

                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredGetProcesslogsResult = await gridSearchExtension.Sort(FilteredGetProcesslogsResult, ColumnName, SortOrder);
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
            if (processLogAdvanceSearchVM.PageSize != null && processLogAdvanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getProcesslogsResult.Any() ? (getProcesslogsResult.First().TotalCount) / ((int)processLogAdvanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)processLogAdvanceSearchVM.PageNumber - 1;
                processLogAdvanceSearchVM.isGridLoaded = true;
                processLogAdvanceSearchVM.PageNumber = CurrentPage;
                processLogAdvanceSearchVM.PageSize = args.Top;
                int TotalPages = getProcesslogsResult.Any() ? (getProcesslogsResult.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    processLogAdvanceSearchVM.PageNumber = TotalPages + 1;
                    processLogAdvanceSearchVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((processLogAdvanceSearchVM.PageNumber == 1 || (processLogAdvanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    processLogAdvanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    processLogAdvanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            processLogAdvanceSearchVM.PageNumber = CurrentPage;
            processLogAdvanceSearchVM.PageSize = args.Top;
        }
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();
                FilteredGetProcesslogsResult = await gridSearchExtension.Filter(getProcesslogsResult, new Query()
                {
                    Filter = $@"i => (i.Process != null && i.Process.ToLower().Contains(@0) ) || (i.Task != null && i.Task.ToLower().Contains(@1) ) || (i.Description != null && i.Description.ToLower().Contains(@2) ) || (i.UserName != null &&  i.UserName.ToLower().Contains(@3) )",
                    FilterParameters = new object[] { search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))

                    FilteredGetProcesslogsResult = await gridSearchExtension.Sort(FilteredGetProcesslogsResult, ColumnName, SortOrder);

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
        protected async Task ViewProcessLogDetail(ProcessLogVM args)
        {

            navigationManager.NavigateTo("/lds-ProcessLog-view/" + args.ProcessLogId);
        }
        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<ProcessLogVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredGetProcesslogsResult = await gridSearchExtension.Sort(FilteredGetProcesslogsResult, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                processLogAdvanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        protected async Task ExportButtonClick(RadzenSplitButtonItem item)
        {
            try
            {
                if (item is null)

                {
                    await processLogService.ExportProcessLogToExcel(new Query()
                    {
                        Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
                        OrderBy = $"{grid.Query.OrderBy}",
                        Expand = "",
                        Select = "ProcessLogId, ProcessLogTypeId, ProcessLogEventId, Process, Task, StartDate, EndDate, Description, Computer"
                    }, $"Process Logs");

                }

                if (item != null && item.Value == "csv")
                {
                    await processLogService.ExportProcessLogToCSV(new Query()
                    {
                        Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
                        OrderBy = $"{grid.Query.OrderBy}",
                        Expand = "",
                        Select = "ProcessLogId, ProcessLogTypeId, ProcessLogEventId, Process, Task, StartDate, EndDate, Description, Computer"
                    }, $"Process Logs");
                }

                if (item != null && item.Value == "xlsx")
                {
                    await processLogService.ExportProcessLogToExcel(new Query()
                    {
                        Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
                        OrderBy = $"{grid.Query.OrderBy}",
                        Expand = "",
                        Select = "ProcessLogId, ProcessLogTypeId, ProcessLogEventId, Process, Task, StartDate, EndDate, Description, Computer"
                    }, $"Process Logs");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (processLogAdvanceSearchVM.StartDate > processLogAdvanceSearchVM.EndDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = processLogAdvanceSearchVM.isDataSorted = true;
                return;
            }
            if (string.IsNullOrEmpty(processLogAdvanceSearchVM.Process)
                && string.IsNullOrEmpty(processLogAdvanceSearchVM.Task)
                && !processLogAdvanceSearchVM.StartDate.HasValue
                && !processLogAdvanceSearchVM.EndDate.HasValue
                && string.IsNullOrEmpty(processLogAdvanceSearchVM.UserName))
            {

            }
            else
            {
                Keywords = processLogAdvanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                {
                    await grid.FirstPage();
                }
                else
                {
                    processLogAdvanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                StateHasChanged();
            }
        }
        public async void ResetForm()
        {
            processLogAdvanceSearchVM = new ProcessLogAdvanceSearchVM { PageSize = grid.PageSize };
            Keywords = processLogAdvanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }
        public Processlogs()
        {
            processLogAdvanceSearchVM = new ProcessLogAdvanceSearchVM();
            args = new ProcessLogAdvanceSearchVM();
            AdvancedSearchResultGrid = false;
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
        #endregion

    }
}

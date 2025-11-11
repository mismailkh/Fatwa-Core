using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;

namespace FATWA_ADMIN.Pages.WorkerService
{
    public partial class DetailWSExecution : ComponentBase
    {
        #region Properties
        protected RadzenDataGrid<WSExecutionDetailVM>? grid1 = new RadzenDataGrid<WSExecutionDetailVM>();
        public bool isVisible { get; set; }
        protected bool Keywords = false;
        protected WSExecutionAdvanceSearchVM advanceSearchVM = new WSExecutionAdvanceSearchVM();
        protected static IEnumerable<WSExecutionStatus> WSExecutionStatuses = new List<WSExecutionStatus>();
        protected IEnumerable<WSExecutionDetailVM> FilteredExecutionDetailResult { get; set; }
        protected List<WSWorkerServices> WSWorkerServices { get; set; } = new List<WSWorkerServices>();
        protected string search { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid1.CurrentPage + 1;
        private int CurrentPageSize => grid1.PageSize;
        protected IEnumerable<WSExecutionDetailVM> WSExecutionDetailVMs = new List<WSExecutionDetailVM>();


        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid1);
            await LoadStatuses();
            await GetWorkerServices();
            spinnerService.Hide();
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
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();

                    FilteredExecutionDetailResult = await gridSearchExtension.Filter(WSExecutionDetailVMs, new Query()
                    {
                        Filter = $@"i => (i.WorkerServiceId != null && i.WorkerServiceId.ToString().ToLower().Contains(@0)) 
                    || (i.WorkerServiceEn != null && i.WorkerServiceEn.ToString().ToLower().Contains(@0)) 
                    || (i.WorkerServiceAr != null && i.WorkerServiceAr.ToString().ToLower().Contains(@0)) 
                    || (i.ExecutionStatusAr != null && i.ExecutionStatusAr.ToString().ToLower().Contains(@0)) 
                    || (i.ExecutionStatusEn != null && i.ExecutionStatusEn.ToString().ToLower().Contains(@0))
                    || (i.ReattemptCount != null && i.ReattemptCount.ToString().ToLower().Contains(@0))
                    || (i.StartDateTime.HasValue && i.StartDateTime.Value.ToString(""dd/MM/yyyy"").Contains(@0))
                    || (i.EndDateTime.HasValue && i.EndDateTime.Value.ToString(""dd/MM/yyyy"").Contains(@0)) 
                    || (i.ExecutionDetails != null && i.ExecutionDetails.ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });

                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredExecutionDetailResult = await gridSearchExtension.Sort(FilteredExecutionDetailResult, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        #endregion
        #region On Load Grid Data 
        private async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid1.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var response = await timeIntervalService.GetWorkerServiceExecutionDetail(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredExecutionDetailResult = WSExecutionDetailVMs = (IEnumerable<WSExecutionDetailVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredExecutionDetailResult = await gridSearchExtension.Sort(FilteredExecutionDetailResult, ColumnName, SortOrder);
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
                int oldPageCount = WSExecutionDetailVMs.Any() ? (WSExecutionDetailVMs.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = WSExecutionDetailVMs.Any() ? (WSExecutionDetailVMs.First().TotalCount) / (grid1.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    grid1.CurrentPage = TotalPages;
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
        private async Task OnSortData(DataGridColumnSortEventArgs<WSExecutionDetailVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredExecutionDetailResult = await gridSearchExtension.Sort(FilteredExecutionDetailResult, args.Column.Property, (SortOrder)args.SortOrder);
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

        #region Row Cell Render based on status highlight color 
        protected void RowCellRender(RowRenderEventArgs<WSExecutionDetailVM> WSExecutionDetailVMs)
        {
            if (WSExecutionDetailVMs.Data.ExecutionStatusEn == WorkerServiceExecutionStatusEnums.Failed.ToString()
                || WSExecutionDetailVMs.Data.ExecutionStatusAr == WorkerServiceExecutionStatusEnums.Failed.ToString()
                || WSExecutionDetailVMs.Data.ExecutionStatusEn == WorkerServiceExecutionStatusEnums.Exception.ToString()
                || WSExecutionDetailVMs.Data.ExecutionStatusAr == WorkerServiceExecutionStatusEnums.Exception.ToString())
            {
                WSExecutionDetailVMs.Attributes.Add("style", $"background-color: #FF7F7F;");
            }
        }
        #endregion

        #region Advance Search
        protected void ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new WSExecutionAdvanceSearchVM { PageSize = grid1.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            grid1.Reset();
            await grid1.Reload();
            StateHasChanged();
        }
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.FromDate > advanceSearchVM.ToDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (advanceSearchVM.StatusId == 0 && advanceSearchVM.WorkerServiceId == 0 /*&& string.IsNullOrWhiteSpace(advanceSearchVM.SearchKeywords*/
            && !advanceSearchVM.FromDate.HasValue && !advanceSearchVM.ToDate.HasValue)
            {
            }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (grid1.CurrentPage > 0)
                {
                    await grid1.FirstPage();
                }
                else
                {
                    advanceSearchVM.isGridLoaded = false;

                    await grid1.Reload();
                }
                StateHasChanged();
            }
        }
        protected async Task LoadStatuses()
        {
            WSExecutionStatuses = await timeIntervalService.GetWSExecutionStatuses();
        }

        protected async Task GetWorkerServices()
        {

            WSWorkerServices = await timeIntervalService.GetWorkerServices();

        }
        #endregion

        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

    }
}

using FATWA_ADMIN.Services;
using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.AuditLogs
{
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master">Literature Classification Component</History>
    public partial class Processlogs : ComponentBase
    {
        #region Variables
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        protected ProcessLogAdvanceSearchVM processLogAdvanceSearchVM { get; set; }
        protected ProcessLogAdvanceSearchVM args;
        protected bool Keywords = false;
        protected bool AdvancedSearchResultGrid;
        public bool isVisible { get; set; }
        protected RadzenDataGrid<ProcessLogVM> grid = new RadzenDataGrid<ProcessLogVM>();
        public bool isLoading { get; set; }
        public int count { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
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
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion 

        #region Service Injections
        [Inject]
        protected ProcessLogService processLogService { get; set; }

        [Inject]
        protected ErrorLogService errorLogService { get; set; }

        #endregion

        #region Functions 
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredGetProcesslogsResult = await gridSearchExtension.Filter(getProcesslogsResult, new Query()
                    {
                        Filter = $@"i => (i.Process != null && i.Process.ToString().ToLower().Contains(@0)) || 
                    (i.Task != null && i.Task.ToString().ToLower().Contains(@1)) || 
                    (i.Description != null && i.Description.ToString().ToLower().Contains(@2)) || 
                    (i.UserName != null &&  i.UserName.ToString().ToLower().Contains(@3)) ||
                    (i.IPDetails != null && i.IPDetails.ToString().ToLower().Contains(@4)) ||
                    (i.StartDate.HasValue && i.StartDate.Value.ToString(""dd/MM/yyyy hh:mm tt"").Contains(@5))",
                        FilterParameters = new object[] { search, search, search, search, search, search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredGetProcesslogsResult = await gridSearchExtension.Sort(FilteredGetProcesslogsResult, ColumnName, SortOrder);
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
        protected async Task ViewProcessLogDetail(ProcessLogVM args)
        {

            navigationManager.NavigateTo("/lds-ProcessLog-view/" + args.ProcessLogId);
        }
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            processLogAdvanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data
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
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);

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
        #endregion

        #region On Sort Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-03' Version="1.0" Branch="master">Sort grid data</History>*/
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

        #region Advance Search
        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (processLogAdvanceSearchVM.StartDate > processLogAdvanceSearchVM.EndDate)
            {
                Keywords = true;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
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

        #region Badrequest Notiication
        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Handle bad request and display error messages in whole class</History>
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

                }
                else
                {
                    var badRequestResponse = (BadRequestResponse)response.ResultData;
                    if (badRequestResponse.InnerException != null && badRequestResponse.InnerException.ToLower().Contains("violation of unique key"))
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Record_Already_Exists"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
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

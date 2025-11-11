using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.AuditLogs
{
    public partial class ListErrorlogs : ComponentBase
    {
        #region Service Injections
        [Inject]
        protected ErrorLogService errorLogService { get; set; }
        #endregion

        #region Variables Declaration
        protected RadzenDataGrid<ErrorLogVM> grid = new RadzenDataGrid<ErrorLogVM>();
        protected IEnumerable<ErrorLogVM> getErrorlogsResult { get; set; } = new List<ErrorLogVM>();
        protected IEnumerable<ErrorLogVM>? FilteredErrorlogsResult { get; set; }
        protected string search { get; set; }
        protected ErrorLogAdvanceSearchVM errorLogAdvanceSearchVM = new ErrorLogAdvanceSearchVM();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            errorLogAdvanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
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
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput();
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
        #endregion
        #region Grid Pagination Calculation
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

        #region Grid Search
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();
                FilteredErrorlogsResult = await gridSearchExtension.Filter(getErrorlogsResult, new Query()
                {
                    Filter = $@"i => i.Subject.ToString().ToLower().Contains(@0)
                                || i.Body != null && i.Body.ToString().ToLower().Contains(@1) 
                                || i.UserName != null && i.UserName.ToString().ToLower().Contains(@2) 
                                || i.LogDate.HasValue && i.LogDate.Value.ToString(""dd/MM/yyyy"").Contains(@3) 
                                || i.Source != null && i.Source.ToString().ToLower().Contains(@4)
                                || i.IPDetails != null && i.IPDetails.ToString().ToLower().Contains(@5)",
                    FilterParameters = new object[] { search, search, search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredErrorlogsResult = await gridSearchExtension.Sort(FilteredErrorlogsResult, ColumnName, SortOrder);

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

        #region Button Clicks & Redirections
        protected async Task ViewErrorLogDetail(ErrorLogVM args)
        {
            navigationManager.NavigateTo("/lds-ErrorLog-view/" + args.ErrorLogId);
        }

        protected async Task ExportButtonClick(RadzenSplitButtonItem args)
        {
            try
            {
                if (args?.Value == "csv")
                {
                    await errorLogService.ExportErrorLogToCSV(new Query()
                    {
                        Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
                        OrderBy = $"{grid.Query.OrderBy}",
                        Expand = "",
                        Select = "ErrorLogId, ErrorLogTypeId, ErrorLogEventId, Subject, Body, LogDate, Category, Source, Type, Computer "
                    },
                    $"Error Logs");
                }

                if (args == null || args.Value == "xlsx")
                {
                    await errorLogService.ExportErrorLogToExcel(new Query()
                    {
                        Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
                        OrderBy = $"{grid.Query.OrderBy}",
                        Expand = "",
                        Select = "ErrorLogId, ErrorLogTypeId, ErrorLogEventId, Subject, Body, LogDate, Category, Source, Type, Computer "
                    },
                    $"Error Logs");
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

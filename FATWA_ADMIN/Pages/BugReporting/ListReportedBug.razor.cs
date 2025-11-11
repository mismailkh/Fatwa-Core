using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class ListReportedBug : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<ReportedBugListVM>? bugGrid { get; set; } = new RadzenDataGrid<ReportedBugListVM>();
        protected AdvanceSearchBugListVM advanceSearchBugList = new AdvanceSearchBugListVM();
        protected List<ReportedBugListVM> FilteredBugListVM { get; set; }
        public bool isVisible { get; set; }
        protected bool Keywords = false;
        protected IEnumerable<BugApplication> Applications { get; set; }
        protected IEnumerable<BugModule> Modules { get; set; }
        protected IEnumerable<BugStatus> BugStatuses { get; set; }
        protected IEnumerable<BugIssueType> IssueTypes { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => bugGrid.CurrentPage + 1;
        private int CurrentPageSize => bugGrid.PageSize;

        IEnumerable<ReportedBugListVM> _getBugs;
        string _search;
        protected IEnumerable<ReportedBugListVM> GetBugs = new List<ReportedBugListVM>();
        #endregion

        #region On Component Load
        protected async override Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(bugGrid);
            await PopulateAllApplications();
            await PopulateBugStatuses();
            await PopulateIssuesTypes();
            await PopulateBugModule();
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-06' Version="1.0" Branch="master">Load grid data based on pagination</History>*/
        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchBugList.PageNumber || CurrentPageSize != advanceSearchBugList.PageSize || (Keywords && advanceSearchBugList.isDataSorted))
                {
                    if (advanceSearchBugList.isGridLoaded && advanceSearchBugList.PageSize == CurrentPageSize && !advanceSearchBugList.isPageSizeChangeOnFirstLastPage)
                    {
                        bugGrid.CurrentPage = (int)advanceSearchBugList.PageNumber - 1;
                        advanceSearchBugList.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var response = await bugReportingService.GetReprotedBugList(advanceSearchBugList);
                    if (response.IsSuccessStatusCode)
                    {
                        GetBugs = (IEnumerable<ReportedBugListVM>)response.ResultData;
                        FilteredBugListVM = (List<ReportedBugListVM>)GetBugs;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredBugListVM = await gridSearchExtension.Sort(FilteredBugListVM, ColumnName, SortOrder);
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
            if (advanceSearchBugList.PageSize != null && advanceSearchBugList.PageSize != CurrentPageSize)
            {
                int oldPageCount = GetBugs.Any() ? (GetBugs.First().TotalCount) / ((int)advanceSearchBugList.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchBugList.PageNumber - 1;
                advanceSearchBugList.isGridLoaded = true;
                advanceSearchBugList.PageNumber = CurrentPage;
                advanceSearchBugList.PageSize = args.Top;
                int TotalPages = GetBugs.Any() ? (GetBugs.First().TotalCount) / (bugGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchBugList.PageNumber = TotalPages + 1;
                    advanceSearchBugList.PageSize = args.Top;
                    bugGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchBugList.PageNumber == 1 || (advanceSearchBugList.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchBugList.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchBugList.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchBugList.PageNumber = CurrentPage;
            advanceSearchBugList.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-06' Version="1.0" Branch="master">Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<ReportedBugListVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredBugListVM = await gridSearchExtension.Sort(FilteredBugListVM, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchBugList.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion


        #region Button click Events
        protected async Task EditBug(ReportedBugListVM bug)
        {
            navigationManager.NavigateTo("/add-reportedbug/" + bug.Id);
        }
        protected async Task ViewBug(ReportedBugListVM bug)
        {
            navigationManager.NavigateTo("/reportedbug-view/" + bug.Id);
        }
        protected async Task ButtonAddClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/add-reportedbug");
        }
        private void GoBackHomeScreen()
        {

            navigationManager.NavigateTo("/index");
        }
        protected async Task AddBugTicket(ReportedBugListVM args)
        {
            try
            {
                await dialogService.OpenAsync<AddBugTicket>(
                               translationState.Translate("Raise_A_Ticket"),
                               new Dictionary<string, object>() { { "TicketId", null }, { "BugId", args.Id.ToString() } },
                               new DialogOptions() { Width = "70%", Height = "85%", CloseDialogOnOverlayClick = false });
                bugGrid.Reset();
                await bugGrid.Reload();
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
        protected string search { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredBugListVM = await gridSearchExtension.Filter(GetBugs, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ?
                    $@"i => (i.PrimaryBugId != null && i.PrimaryBugId.ToString().ToLower().Contains(@0)) || 
                    (i.ApplicationEn != null && i.ApplicationEn.ToString().ToLower().Contains(@1)) || 
                    (i.StatusEn != null && i.StatusEn.ToString().ToLower().Contains(@2)) || 
                    (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3)) ||
                    (i.Subject != null && i.Subject.ToString().ToLower().Contains(@4)) ||
                    (i.IssueTypeEn != null && i.IssueTypeEn.ToString().ToLower().Contains(@5)) ||
                    (i.ModuleEn != null && i.ModuleEn.ToString().ToLower().Contains(@6))"
                    :
                    $@"i => (i.PrimaryBugId != null && i.PrimaryBugId.ToString().ToLower().Contains(@0)) || 
                    (i.ApplicationAr != null && i.ApplicationAr.ToString().ToLower().Contains(@1)) || 
                    (i.StatusAr != null && i.StatusAr.ToString().ToLower().Contains(@2)) || 
                    (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))
                    (i.Subject != null && i.Subject.ToString().ToLower().Contains(@4)) ||
                    (i.IssueTypeAr != null && i.IssueTypeAr.ToString().ToLower().Contains(@5)) ||
                    (i.ModuleAr != null && i.ModuleAr.ToString().ToLower().Contains(@6))",
                        FilterParameters = new object[] { search, search, search, search, search, search, search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                    {
                        FilteredBugListVM = await gridSearchExtension.Sort(FilteredBugListVM, ColumnName, SortOrder);
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
        #endregion

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchBugList.CreatedDateFrom > advanceSearchBugList.CreatedDateTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            if (string.IsNullOrWhiteSpace(advanceSearchBugList.PrimaryBugId) && advanceSearchBugList.ApplicationId == 0 && advanceSearchBugList.StatusId == 0 && string.IsNullOrWhiteSpace(advanceSearchBugList.ModifiedBy)
                 && !advanceSearchBugList.CreatedDateFrom.HasValue && !advanceSearchBugList.CreatedDateTo.HasValue && string.IsNullOrWhiteSpace(advanceSearchBugList.ModifiedBy))
            {
            }
            else
            {
                spinnerService.Show();
                Keywords = advanceSearchBugList.isDataSorted = true;
                if (bugGrid.CurrentPage > 0)
                {
                    await bugGrid.FirstPage();
                }
                else
                {
                    advanceSearchBugList.isGridLoaded = false;
                    await bugGrid.Reload();
                }
                spinnerService.Hide();
                StateHasChanged();
            }
        }
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        public async void ResetForm()
        {
            advanceSearchBugList = new AdvanceSearchBugListVM { PageSize = bugGrid.PageSize };
            Keywords = advanceSearchBugList.isDataSorted = false;
            bugGrid.Reset();
            await bugGrid.Reload();
            StateHasChanged();
        }
        #endregion

        #region Dropdowns
        protected async Task PopulateAllApplications()
        {
            try
            {
                var response = await lookupService.GetAllApplications();
                if (response.IsSuccessStatusCode)
                {
                    Applications = (List<BugApplication>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task PopulateBugStatuses()
        {
            try
            {
                var response = await lookupService.GetBugStatuses();
                if (response.IsSuccessStatusCode)
                {
                    BugStatuses = (List<BugStatus>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            StateHasChanged();
        }
        protected async Task PopulateIssuesTypes()
        {
            try
            {
                var response = await lookupService.GetIssuesTypes();
                if (response.IsSuccessStatusCode)
                {
                    IssueTypes = (List<BugIssueType>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            StateHasChanged();
        }
        protected async Task PopulateBugModule()
        {
            try
            {
                var response = await lookupService.GetBugModules();
                if (response.IsSuccessStatusCode)
                {
                    Modules = (List<BugModule>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            StateHasChanged();
        }

        #endregion
    }
}

using FATWA_DOMAIN.Models.OrganizingCommittee;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.OrganizingCommittee
{
    public partial class ListCommittee : ComponentBase
    {
        #region Variables
        protected AdvanceSearchCommitteListVm advanceSearchCommitteeList = new AdvanceSearchCommitteListVm();
        protected RadzenDataGrid<CommitteeListVm>? committeeGrid { get; set; } = new RadzenDataGrid<CommitteeListVm>();
        protected IEnumerable<CommitteeListVm> FilteredCommitteeListVM;
        protected IEnumerable<CommitteeStatus> CommitteeStatus { get; set; }
        IEnumerable<CommitteeListVm> _getCommittee;
        public List<string> committeeNumbers { get; set; } = new List<string>();
        public bool isVisible { get; set; }
        protected bool Keywords = false;
        string _search;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => committeeGrid.CurrentPage + 1;
        private int CurrentPageSize => committeeGrid.PageSize;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region OnInitializedAsync
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearchCommitteeList.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(committeeGrid);
            await GetStatus();
            spinnerService.Hide();
        }
        #endregion

        #region Add Committee
        protected async Task AddCommittee()
        {
            navigationManager.NavigateTo("/add-committee");
        }
        #endregion


        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {

            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchCommitteeList.PageNumber || CurrentPageSize != advanceSearchCommitteeList.PageSize || (Keywords && advanceSearchCommitteeList.isDataSorted))
                {
                    if (advanceSearchCommitteeList.isGridLoaded && advanceSearchCommitteeList.PageSize == CurrentPageSize && !advanceSearchCommitteeList.isPageSizeChangeOnFirstLastPage)
                    {
                        committeeGrid.CurrentPage = (int)advanceSearchCommitteeList.PageNumber - 1;
                        advanceSearchCommitteeList.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    advanceSearchCommitteeList.UserId = loginState.UserDetail.UserId;
                    spinnerService.Show();
                    var response = await organizingCommitteeService.GetCommittee(advanceSearchCommitteeList);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredCommitteeListVM = GetCommittee = (IEnumerable<CommitteeListVm>)response.ResultData;
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(dataArgs.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredCommitteeListVM = await gridSearchExtension.Sort(FilteredCommitteeListVM, ColumnName, SortOrder);
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
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchCommitteeList.PageSize != null && advanceSearchCommitteeList.PageSize != CurrentPageSize)
            {
                int oldPageCount = GetCommittee.Any() ? (GetCommittee.First().TotalCount) / ((int)advanceSearchCommitteeList.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchCommitteeList.PageNumber - 1;
                advanceSearchCommitteeList.isGridLoaded = true;
                advanceSearchCommitteeList.PageNumber = CurrentPage;
                advanceSearchCommitteeList.PageSize = args.Top;
                int TotalPages = GetCommittee.Any() ? (GetCommittee.First().TotalCount) / (committeeGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchCommitteeList.PageNumber = TotalPages + 1;
                    advanceSearchCommitteeList.PageSize = args.Top;
                    committeeGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchCommitteeList.PageNumber == 1 || (advanceSearchCommitteeList.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchCommitteeList.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchCommitteeList.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchCommitteeList.PageNumber = CurrentPage;
            advanceSearchCommitteeList.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<CommitteeListVm> args)
        {
            if (args.SortOrder != null)
            {
                FilteredCommitteeListVM = await gridSearchExtension.Sort(FilteredCommitteeListVM, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchCommitteeList.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region GetStatus
        protected async Task GetStatus()
        {
            try
            {
                var response = await lookupService.GetStatus();
                if (response.IsSuccessStatusCode)
                {
                    CommitteeStatus = (List<CommitteeStatus>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
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
        #endregion

        #region ViewCommittee
        protected async Task ViewCommittee(CommitteeListVm committee)
        {
            navigationManager.NavigateTo("/committee-view/" + committee.Id);
        }
        #endregion 

        #region Edit Committee
        protected async Task EditCommittee(CommitteeListVm committee)
        {
            navigationManager.NavigateTo("/add-committee/" + committee.Id);
        }
        #endregion

        #region AdvanceSearch
        protected async Task SubmitAdvanceSearch()
        {
            if (!string.IsNullOrWhiteSpace(advanceSearchCommitteeList.CommitteeNumber) || !string.IsNullOrWhiteSpace(advanceSearchCommitteeList.Subject) || advanceSearchCommitteeList.Duration != null || advanceSearchCommitteeList.StatusId != null
               || advanceSearchCommitteeList.CreatedDateFrom.HasValue || advanceSearchCommitteeList.CreatedDateTo.HasValue)
            {

                if (advanceSearchCommitteeList.CreatedDateFrom > advanceSearchCommitteeList.CreatedDateTo)
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
                else
                {
                    Keywords = advanceSearchCommitteeList.isDataSorted = true;
                    if (committeeGrid.CurrentPage > 0)
                    {
                        await committeeGrid.FirstPage();
                    }
                    else
                    {
                        advanceSearchCommitteeList.isGridLoaded = false;
                        await committeeGrid.Reload();
                    }
                    StateHasChanged();
                }
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
            advanceSearchCommitteeList = new AdvanceSearchCommitteListVm { PageSize = committeeGrid.PageSize };
            Keywords = advanceSearchCommitteeList.isDataSorted = false;
            await Task.Delay(100);
            committeeGrid.Reset();
            await committeeGrid.Reload();
            StateHasChanged();
        }
        #endregion

        #region Search
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }
        protected IEnumerable<CommitteeListVm> GetCommittee
        {
            get
            {
                return _getCommittee;
            }
            set
            {
                if (!Equals(_getCommittee, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetCommittee", NewValue = value, OldValue = _getCommittee };
                    _getCommittee = value;

                    Reload();
                }

            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region OnSearchInput
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredCommitteeListVM = await gridSearchExtension.Filter(GetCommittee, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.CommitteeNumber != null && i.CommitteeNumber.ToString().Contains(@0)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@1)) || (i.Duration != null && i.Duration.ToString().ToLower().Contains(@2)) || (i.StatusEn != null && i.StatusEn.ToString().ToLower().Contains(@3)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@4))"
                    : $@"i => (i.CommitteeNumber != null && i.CommitteeNumber.ToString().Contains(@0)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@1)) || (i.Duration != null && i.Duration.ToString().ToLower().Contains(@2)) || (i.StatusAr != null && i.StatusAr.ToString().ToLower().Contains(@3)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@4))",

                    FilterParameters = new object[] { search, search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredCommitteeListVM = await gridSearchExtension.Sort(FilteredCommitteeListVM, ColumnName, SortOrder);
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

        #region Button Click Event
        private void GoBackHomeScreen()
        {

            navigationManager.NavigateTo("/index");
        }
        #endregion
    }
}

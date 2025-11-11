using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //<!--<History Author = "Hassan Abbas" Date="2023-03-23" Version="1.0" Branch="master"> List of Moj Execution Requests</History>-->
    public partial class ListExecutionRequests : ComponentBase
    {
        #region Variables
        protected List<MojExecutionRequestVM> MojExecutionRequests = new List<MojExecutionRequestVM>();
        private Timer debouncer;
        private const int debouncerDelay = 500;
        protected List<MojExecutionRequestVM> FilteredMojExecutionRequests { get; set; }
        protected RadzenDataGrid<MojExecutionRequestVM> ExecReqGrid = new RadzenDataGrid<MojExecutionRequestVM>();
        protected RadzenDataGrid<CmsJugdmentDecisionVM> RegenExecReqGrid = new RadzenDataGrid<CmsJugdmentDecisionVM>();
        public IList<CmsJugdmentDecisionVM> jugdmentDecisionVMs;
        public string UserId { get; set; }
        public int selectedIndex { get; set; } = 0;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int? PageNumber { get; set; } = 1;
        private int? PageSize { get; set; }
        private int CurrentPage { get; set; }
        private int CurrentPageSize { get; set; }
        private bool isGridLoaded { get; set; }
        private bool isPageSizeChangeOnFirstLastPage { get; set; }
        #endregion
        #region Full property variable
        IEnumerable<CmsJugdmentDecisionVM> _jugdmentDecisionVMs;
        IEnumerable<CmsJugdmentDecisionVM> FilteredGetjugdmentDecisionVMs { get; set; }
        protected IEnumerable<CmsJugdmentDecisionVM> getjugdmentDecisionVMs
        {
            get
            {
                return _jugdmentDecisionVMs;
            }
            set
            {
                if (!Equals(_jugdmentDecisionVMs, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getjugdmentDecisionVMs", NewValue = value, OldValue = getjugdmentDecisionVMs };
                    _jugdmentDecisionVMs = value;

                    Reload();
                }

            }
        }
        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2023-03-28' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            UserId = loginState.UserDetail.UserId;
            PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(ExecReqGrid);
            translationState.TranslateGridFilterLabels(RegenExecReqGrid);
            spinnerService.Hide();
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }


        protected async Task OnLoadExecutionRequestData(LoadDataArgs args)
        {
            try
            {
                CurrentPage = ExecReqGrid.CurrentPage + 1;
                CurrentPageSize = ExecReqGrid.PageSize;
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != PageNumber || CurrentPageSize != PageSize)
                {
                    if (isGridLoaded && PageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        ExecReqGrid.CurrentPage = (int)PageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args, true);
                    spinnerService.Show();
                    var response = await cmsCaseFileService.GetMojExecutionRequests(PageNumber, PageSize);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredMojExecutionRequests = MojExecutionRequests = (List<MojExecutionRequestVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredMojExecutionRequests = await gridSearchExtension.Sort(FilteredMojExecutionRequests, ColumnName, SortOrder);
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
        protected async Task OnLoadInterpretationExecutionRequestsData(LoadDataArgs args)
        {
            try
            {
                CurrentPage = RegenExecReqGrid.CurrentPage + 1;
                CurrentPageSize = RegenExecReqGrid.PageSize;
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != PageNumber || CurrentPageSize != PageSize)
                {
                    if (isGridLoaded && PageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        RegenExecReqGrid.CurrentPage = (int)PageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args, false);
                    spinnerService.Show();
                    var responseJudgment = await cmsRegisteredCaseService.GetJudgmentDecisionList(Guid.Parse(UserId), PageNumber, PageSize);
                    if (responseJudgment.IsSuccessStatusCode)
                    {
                        FilteredGetjugdmentDecisionVMs = getjugdmentDecisionVMs = (List<CmsJugdmentDecisionVM>)responseJudgment.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);

                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredGetjugdmentDecisionVMs = await gridSearchExtension.Sort(FilteredGetjugdmentDecisionVMs, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(responseJudgment);
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
        private void SetPagingProperties(LoadDataArgs args, bool IsExecutionGrid)
        {
            if (PageSize != CurrentPageSize)
            {
                int oldPageCount = IsExecutionGrid ? MojExecutionRequests.Any() ? (MojExecutionRequests.First().TotalCount) / ((int)PageSize) : 1 : getjugdmentDecisionVMs.Any() ? (getjugdmentDecisionVMs.First().TotalCount) / ((int)PageSize) : 1;
                int oldPageNumber = (int)PageNumber - 1;
                isGridLoaded = true;
                PageNumber = CurrentPage;
                PageSize = args.Top;
                int TotalPages = IsExecutionGrid ? MojExecutionRequests.Any() ? (MojExecutionRequests.First().TotalCount) / (ExecReqGrid.PageSize) : 1 : getjugdmentDecisionVMs.Any() ? (getjugdmentDecisionVMs.First().TotalCount) / (RegenExecReqGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PageNumber = TotalPages + 1;
                    PageSize = args.Top;
                    if (IsExecutionGrid)
                        ExecReqGrid.CurrentPage = TotalPages;
                    else
                        RegenExecReqGrid.CurrentPage = TotalPages;

                }
                if ((PageNumber == 1 || (PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            PageNumber = CurrentPage;
            PageSize = args.Top;
        }

        #endregion

        #region On Sort Grids Data
        /*<History Author='Ammaar Naveed' Date='2025-02-04' Version="1.0" Branch="master">Sort grid data</History>*/
        private async Task OnSortExecutionRequestData(DataGridColumnSortEventArgs<MojExecutionRequestVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredMojExecutionRequests = await gridSearchExtension.Sort(FilteredMojExecutionRequests, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        private async Task OnSortInterpretationExecutionRequestsData(DataGridColumnSortEventArgs<CmsJugdmentDecisionVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredGetjugdmentDecisionVMs = await gridSearchExtension.Sort(FilteredGetjugdmentDecisionVMs, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Badrequest Notification

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
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
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
        //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> redirect to View Detail</History>
        protected async Task ViewDetail(MojExecutionRequestVM args)
        {
            spinnerService.Show();
            var taskDetailVM = new TaskDetailVM();
            var getTaskDetail = await taskService.GetTaskDetailByReferenceAndUserId(args.Id, loginState.UserDetail.UserId);
            if (getTaskDetail.IsSuccessStatusCode)
            {
                taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
                navigationManager.NavigateTo("/executionrequest-detail/" + args.Id + "/" + taskDetailVM.TaskId);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(getTaskDetail);
            }
            spinnerService.Hide();
        }
        #endregion

        #region Grid Buttons
        protected async Task DetailJudgmentDecision(CmsJugdmentDecisionVM args)
        {
            navigationManager.NavigateTo("/judgmentdecision-detail/" + args.Id);
        }
        #endregion

        #region Functions
        protected string search { get; set; }

        public async void OnTabChange(int index)
        {
            if (index == selectedIndex) { return; }
            search = ColumnName = string.Empty;
            selectedIndex = index;
            await Task.Delay(100);
            ExecReqGrid.Reset();
            if (index == 0)
            {
                ExecReqGrid.Reset();
                await ExecReqGrid.Reload();
            }
            else
            {
                RegenExecReqGrid.Reset();
                await RegenExecReqGrid.Reload();
            }
            InitializePagingProperties();
            StateHasChanged();
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (selectedIndex == 0)
                {
                    FilteredMojExecutionRequests = await gridSearchExtension.Filter(MojExecutionRequests, new Query()
                    {
                        Filter = $@"i => (i.CANNumber != null && i.CANNumber.ToString().ToLower().Contains(@0)) ||
                        (i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@1)) ||
                        (i.AssignedDate.HasValue && i.AssignedDate.Value.ToString(""dd/MM/yyyy hh:mm tt"").Contains(@2)) ||
                        (i.Remarks != null && i.Remarks.ToString().ToLower().Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredMojExecutionRequests = await gridSearchExtension.Sort(FilteredMojExecutionRequests, ColumnName, SortOrder);

                }
                else
                {
                    if (selectedIndex == 1)
                    {
                        FilteredGetjugdmentDecisionVMs = await gridSearchExtension.Filter(getjugdmentDecisionVMs, new Query()
                        {
                            Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ?
                            $@"i => ( i.Title != null && i.Title.ToString().ToLower().Contains(@0))||
                            (i.Description != null && i.Description.ToString().ToLower().Contains(@1))||
                            (i.DecisionTypeEn != null && i.DecisionTypeEn.ToString().ToLower().Contains(@2))"
                            :
                            $@"i => ( i.Title != null && i.Title.ToString().ToLower().Contains(@0))||
                            (i.Description != null && i.Description.ToString().ToLower().Contains(@1))||
                            (i.DecisionTypeAr != null && i.DecisionTypeAr.ToString().ToLower().Contains(@2))",

                            FilterParameters = new object[] { search, search, search }
                        });
                        if (!string.IsNullOrEmpty(ColumnName))
                            FilteredGetjugdmentDecisionVMs = await gridSearchExtension.Sort(FilteredGetjugdmentDecisionVMs, ColumnName, SortOrder);

                    }
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
        #region Initlizaing the Grid Pagination Properties 
        protected void InitializePagingProperties()
        {
            ColumnName = string.Empty;
            PageNumber = 1;
            PageSize = systemSettingState.Grid_Pagination;
            CurrentPage = selectedIndex == 0 ? ExecReqGrid.CurrentPage + 1 : RegenExecReqGrid.CurrentPage + 1;
            CurrentPageSize = selectedIndex == 0 ? ExecReqGrid.PageSize : RegenExecReqGrid.PageSize;
            isGridLoaded = false;
            isPageSizeChangeOnFirstLastPage = false;
        }
        #endregion

    }
}

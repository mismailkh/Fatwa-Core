using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;

namespace FATWA_WEB.Pages.BugReporting
{
    public partial class ListBugTicket
    {
        #region Variables
        protected RadzenDataGrid<TicketListVM>? ticketGrid { get; set; }
        protected AdvanceSearchTicketListVM advanceSearchTicketList = new AdvanceSearchTicketListVM();
        protected List<TicketListVM> FilteredTicketListVM { get; set; }
        public IList<TicketListVM> SelectedTicketList = new List<TicketListVM>();
        protected List<DecisionStatusVM> SelectedTickets { get; set; } = new List<DecisionStatusVM>();
        public bool allowRowSelectOnRowClick = true;
        protected BugTicket bugTicket { get; set; }
        public bool isVisible { get; set; }
        protected bool Keywords = false;
        protected IEnumerable<BugApplication> Applications { get; set; }
        protected IEnumerable<BugModule> Modules { get; set; }
        protected IEnumerable<BugStatus> BugStatuses { get; set; }
        protected IEnumerable<Priority> Priorities { get; set; }
        protected IEnumerable<BugSeverity> Severities { get; set; }
        public DecisionStatusVM decisionStatus = new DecisionStatusVM();
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => ticketGrid.CurrentPage + 1;
        private int CurrentPageSize => ticketGrid.PageSize;
        IEnumerable<TicketListVM> _getTicket;
        string _search;
        protected IEnumerable<TicketListVM> GetTicket = new List<TicketListVM>();
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region OnInitializedAsync
        protected async override Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(ticketGrid);
            await PopulateAllApplications();
            await PopulateBugModule();
            await PopulatePriorityList();
            await PopulateBugStatuses();
            await PopulateSeverity();
            bugTicketEventService.OnBugTicketAdded += ReloadGrid;

            spinnerService.Hide();
        }
        #endregion 
        #region Reload Grid
        private void ReloadGrid()
        {
            ticketGrid.Reload();
        }
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchTicketList.PageNumber || CurrentPageSize != advanceSearchTicketList.PageSize || (Keywords && advanceSearchTicketList.isDataSorted))
                {
                    if (advanceSearchTicketList.isGridLoaded && advanceSearchTicketList.PageSize == CurrentPageSize && !advanceSearchTicketList.isPageSizeChangeOnFirstLastPage)
                    {
                        ticketGrid.CurrentPage = (int)advanceSearchTicketList.PageNumber - 1;
                        advanceSearchTicketList.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    advanceSearchTicketList.UserId = loginState.UserDetail.UserId;
                    var response = await bugReportingService.GetTickets(advanceSearchTicketList);
                    if (response.IsSuccessStatusCode)
                    {
                        GetTicket = (IEnumerable<TicketListVM>)response.ResultData;
                        FilteredTicketListVM = (List<TicketListVM>)GetTicket;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredTicketListVM = await gridSearchExtension.Sort(FilteredTicketListVM, ColumnName, SortOrder);
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
            if (advanceSearchTicketList.PageSize != null && advanceSearchTicketList.PageSize != CurrentPageSize)
            {
                int oldPageCount = GetTicket.Any() ? (GetTicket.First().TotalCount) / ((int)advanceSearchTicketList.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchTicketList.PageNumber - 1;
                advanceSearchTicketList.isGridLoaded = true;
                advanceSearchTicketList.PageNumber = CurrentPage;
                advanceSearchTicketList.PageSize = args.Top;
                int TotalPages = GetTicket.Any() ? (GetTicket.First().TotalCount) / (ticketGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchTicketList.PageNumber = TotalPages + 1;
                    advanceSearchTicketList.PageSize = args.Top;
                    ticketGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchTicketList.PageNumber == 1 || (advanceSearchTicketList.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchTicketList.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchTicketList.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchTicketList.PageNumber = CurrentPage;
            advanceSearchTicketList.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<TicketListVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredTicketListVM = await gridSearchExtension.Sort(FilteredTicketListVM, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchTicketList.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region On Select List
        protected async Task OnSelectList(bool isChecked)
        {
            if (isChecked)
            {
                advanceSearchTicketList.StatusId = (int)BugStatusEnum.Resolved;
                ticketGrid.Reset();
                await ticketGrid.Reload();
                SelectedTicketList = FilteredTicketListVM;
            }
            else
            {
                SelectedTicketList = new List<TicketListVM>();
                advanceSearchTicketList.StatusId = 0;
                ticketGrid.Reset();
                await ticketGrid.Reload();
            }
        }
        #endregion

        #region Button click Events
        protected async Task EditBugTicket(TicketListVM ticket)
        {
            try
            {
                await dialogService.OpenAsync<AddBugTicket>(
                           translationState.Translate("Raise_A_Ticket"),
                           new Dictionary<string, object>() { { "TicketId", ticket.Id.ToString() } },
                           new DialogOptions() { Width = "70%", CloseDialogOnOverlayClick = true });
                ticketGrid.Reset();
                await ticketGrid.Reload();

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
        protected async Task ViewBugTicket(TicketListVM ticket)
        {
            navigationManager.NavigateTo("/bugticket-view/" + ticket.Id + "/" + true);
        }
        protected async Task AddTicket()
        {
            try
            {
                await dialogService.OpenAsync<AddBugTicket>(
                           translationState.Translate("Raise_A_Ticket"),
                           new Dictionary<string, object>() { { "TicketId", null } },
                           new DialogOptions() { Width = "70%", CloseDialogOnOverlayClick = true });
                ticketGrid.Reset();
                await ticketGrid.Reload();
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
        protected async Task CloseTicket(TicketListVM ticket)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit_Close_Ticket"),
                    translationState.Translate("Submit"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        spinnerService.Show();
                        decisionStatus.ReferenceId = ticket.Id;
                        decisionStatus.StatusId = (int)BugStatusEnum.Closed;
                        var response = await bugReportingService.UpdateTicketStatus(decisionStatus);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Ticket_Closed_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        SelectedTicketList = new List<TicketListVM>();
                        ticketGrid.Reset();
                        await ticketGrid.Reload();
                        spinnerService.Hide();
                    }
                }
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
        protected async Task CloseTicketList()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                                  translationState.Translate("Sure_Submit_Close_Ticket"),
                                  translationState.Translate("Submit"),
                                  new ConfirmOptions()
                                  {
                                      OkButtonText = translationState.Translate("OK"),
                                      CancelButtonText = translationState.Translate("Cancel")
                                  });
                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        spinnerService.Show();
                        var response = await bugReportingService.UpdateAllSelectedTicketStatus(SelectedTicketList);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = SelectedTicketList.Count().ToString() + " " + translationState.Translate("Ticket_Closed_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            SelectedTicketList = new List<TicketListVM>();
                            advanceSearchTicketList = new AdvanceSearchTicketListVM();
                            ticketGrid.Reset();
                            await ticketGrid.Reload();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }

                        spinnerService.Hide();
                    }
                }
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

        private void GoBackHomeScreen()
        {

            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region OnSearch
        protected string search { get; set; }
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
                        FilteredTicketListVM = await gridSearchExtension.Filter(GetTicket, new Query()
                        {
                            Filter = $@"i => (i.TicketId != null && i.TicketId.ToString().ToLower().Contains(@0)) || 
                        (i.ApplicationAr != null && i.ApplicationEn.ToString().ToLower().Contains(@1))|| 
                        (i.ModuleEn != null && i.ModuleEn.ToString().ToLower().Contains(@2)) || 
                        (i.IssueTypeEn != null && i.IssueTypeEn.ToString().ToLower().Contains(@3)) || 
                        (i.StatusEn != null && i.StatusEn.ToString().ToLower().Contains(@4)) || 
                        (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@5)) ||
                        (i.Subject != null && i.Subject.ToString().ToLower().Contains(@6))",
                            FilterParameters = new object[] { search, search, search, search, search, search, search }
                        });
                    }
                    else
                    {
                        FilteredTicketListVM = await gridSearchExtension.Filter(GetTicket, new Query()
                        {
                            Filter = $@"i => (i.TicketId != null && i.TicketId.ToString().ToLower().Contains(@0)) || 
                        (i.ApplicationAr != null && i.ApplicationAr.ToString().ToLower().Contains(@1))||
                        (i.ModuleAr != null && i.ModuleAr.ToString().ToLower().Contains(@2)) || 
                        (i.IssueTypeAr != null && i.IssueTypeAr.ToString().ToLower().Contains(@3)) ||
                        (i.StatusAr != null && i.StatusAr.ToString().ToLower().Contains(@4)) || 
                        (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@5))||
                        (i.Subject != null && i.Subject.ToString().ToLower().Contains(@6))",
                            FilterParameters = new object[] { search, search, search, search, search, search, search }
                        });
                    }
                    if (!string.IsNullOrEmpty(ColumnName))
                    {
                        FilteredTicketListVM = await gridSearchExtension.Sort(FilteredTicketListVM, ColumnName, SortOrder);
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

        #region AdvanceSearch
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchTicketList.CreatedDateFrom > advanceSearchTicketList.CreatedDateTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            if (string.IsNullOrWhiteSpace(advanceSearchTicketList.TicketId) && advanceSearchTicketList.ApplicationId == 0 &&
                advanceSearchTicketList.ModuleId == 0 && advanceSearchTicketList.StatusId == 0 &&
                advanceSearchTicketList.PriorityId == 0 && advanceSearchTicketList.SeverityId == 0 &&
                string.IsNullOrWhiteSpace(advanceSearchTicketList.AssignTo) && string.IsNullOrWhiteSpace(advanceSearchTicketList.ModifiedBy)
                && !advanceSearchTicketList.CreatedDateFrom.HasValue && !advanceSearchTicketList.CreatedDateTo.HasValue &&
                string.IsNullOrWhiteSpace(advanceSearchTicketList.AssignTo) && string.IsNullOrWhiteSpace(advanceSearchTicketList.ModifiedBy))
            {
            }
            else
            {
                spinnerService.Show();
                Keywords = advanceSearchTicketList.isDataSorted = true;
                if (ticketGrid.CurrentPage > 0)
                {
                    await ticketGrid.FirstPage();
                }
                else
                {
                    advanceSearchTicketList.isGridLoaded = false;
                    await ticketGrid.Reload();
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
            advanceSearchTicketList = new AdvanceSearchTicketListVM { PageSize = ticketGrid.PageSize };
            Keywords = advanceSearchTicketList.isDataSorted = false;
            ticketGrid.Reset();
            await ticketGrid.Reload();
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
        protected async Task PopulatePriorityList()
        {
            try
            {
                var response = await lookupService.GetCasePriorities();
                if (response.IsSuccessStatusCode)
                {
                    Priorities = (List<Priority>)response.ResultData;
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
        protected async Task PopulateSeverity()
        {
            try
            {
                var response = await lookupService.GetSeverity();
                if (response.IsSuccessStatusCode)
                {
                    Severities = (List<BugSeverity>)response.ResultData;
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

        #region Redirect
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        #region Dispose
        public void Dispose()
        {
            bugTicketEventService.OnBugTicketAdded -= ReloadGrid;
        }
        #endregion
    }
}

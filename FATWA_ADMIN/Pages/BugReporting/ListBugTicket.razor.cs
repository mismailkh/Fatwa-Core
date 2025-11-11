using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class ListBugTicket : ComponentBase
    {
        #region Variables
        protected RadzenDataGrid<TicketListVM>? ticketGrid { get; set; } = new RadzenDataGrid<TicketListVM>();
        protected AdvanceSearchTicketListVM advanceSearchTicketList = new AdvanceSearchTicketListVM();
        protected List<TicketListVM> FilteredTicketListVM { get; set; }
        public bool isVisible { get; set; }
        protected bool Keywords = false;
        protected IEnumerable<BugApplication> Applications { get; set; }
        protected IEnumerable<BugModule> Modules { get; set; }
        protected IEnumerable<BugStatus> BugStatuses { get; set; }
        protected IEnumerable<Priority> Priorities { get; set; }
        protected IEnumerable<BugSeverity> Severities { get; set; }
        protected IEnumerable<BugIssueType> IssueTypes { get; set; }
        public DecisionStatusVM decisionStatus = new DecisionStatusVM();
        public IList<TicketListVM> SelectedTicketList = new List<TicketListVM>();
        public bool allowRowSelectOnRowClick = true;
        IEnumerable<TicketListVM> _getTicket;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => ticketGrid.CurrentPage + 1;
        private int CurrentPageSize => ticketGrid.PageSize;
        protected string search { get; set; }
        protected IEnumerable<TicketListVM> GetTicket = new List<TicketListVM>(); private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region On Component Load
        protected async override Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(ticketGrid);
            await PopulateAllApplications();
            await PopulateBugModule();
            await PopulatePriorityList();
            await PopulateBugStatuses();
            await PopulateSeverity();
            await PopulateIssuesTypes();
            spinnerService.Hide();
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
                SelectedTicketList = FilteredTicketListVM = FilteredTicketListVM.Where(x => x.CreatedBy == loginState.UserDetail.UserName).ToList();
            }
            else
            {
                SelectedTicketList = new List<TicketListVM>();
                advanceSearchTicketList.StatusId = 0;
                advanceSearchTicketList.UserId = string.Empty;
                ticketGrid.Reset();
                await ticketGrid.Reload();
            }
        }
        #endregion

        #region Raise Ticket
        protected async Task RaiseATicket()
        {
            try
            {
                await dialogService.OpenAsync<AddBugTicket>(
                           translationState.Translate("Raise_A_Ticket"),
                           new Dictionary<string, object>() { { "TicketId", null } },
                           new DialogOptions() { Width = "70%", Height = "85%", CloseDialogOnOverlayClick = false });
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
        #endregion

        #region Button click Events
        protected async Task EditBugTicket(TicketListVM ticket)
        {
            try
            {
                await dialogService.OpenAsync<AddBugTicket>(
                               translationState.Translate("Raise_A_Ticket"),
                               new Dictionary<string, object>() { { "TicketId", ticket.Id.ToString() }, { "BugId", ticket.BugId.ToString() } },
                               new DialogOptions() { Width = "70%", Height = "85%", CloseDialogOnOverlayClick = false });
                ticketGrid.Reset();
                await ticketGrid.Reload();
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
        protected async Task ViewBugTicket(TicketListVM ticket)
        {
            navigationManager.NavigateTo("/bugticket-view/" + ticket.Id + "/" + true);
        }

        private void GoBackHomeScreen()
        {

            navigationManager.NavigateTo("/index");
        }
        #endregion

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #region Search
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredTicketListVM = await gridSearchExtension.Filter(GetTicket, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i =>
                                         (i.TicketId != null && i.TicketId.ToString().ToLower().Contains(@0)) ||
                                         (i.ApplicationEn != null && i.ApplicationEn.ToString().ToLower().Contains(@1)) ||
                                         (i.ModuleEn != null && i.ModuleEn.ToString().ToLower().Contains(@2)) ||
                                         (i.IssueTypeEn != null && i.IssueTypeEn.ToString().ToLower().Contains(@3)) ||
                                         (i.ReportedBy != null && i.ReportedBy.ToString().ToLower().Contains(@4)) ||
                                         (i.StatusEn != null && i.StatusEn.ToString().ToLower().Contains(@5)) ||
                                         (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@6)) ||
                                         (i.Subject != null && i.Subject.ToString().ToLower().Contains(@7)) ||
                                         (i.UserNameEn != null && i.UserNameEn.ToString().ToLower().Contains(@8))"

                    : $@"i =>            (i.TicketId != null && i.TicketId.ToString().ToLower().Contains(@0)) || 
                                         (i.ApplicationAr != null && i.ApplicationAr.ToString().ToLower().Contains(@1)) || 
                                         (i.ModuleAr != null && i.ModuleAr.ToString().ToLower().Contains(@2)) || 
                                         (i.IssueTypeAr != null && i.IssueTypeAr.ToString().ToLower().Contains(@3))  || 
                                         (i.ReportedBy != null && i.ReportedBy.ToString().ToLower().Contains(@4)) || 
                                         (i.StatusAr != null && i.StatusAr.ToString().ToLower().Contains(@5)) || 
                                         (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@6)) ||
                                         (i.Subject != null && i.Subject.ToString().ToLower().Contains(@7)) ||
                                         (i.UserNameAr != null && i.UserNameAr.ToString().ToLower().Contains(@8))",
                    FilterParameters = new object[] { search, search, search, search, search, search, search, search, search }
                });
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
            if (string.IsNullOrWhiteSpace(advanceSearchTicketList.TicketId) &&
                advanceSearchTicketList.ApplicationId == 0
                && advanceSearchTicketList.ModuleId == 0
                && advanceSearchTicketList.StatusId == 0
                && advanceSearchTicketList.PriorityId == 0
                && advanceSearchTicketList.SeverityId == 0
                && string.IsNullOrWhiteSpace(advanceSearchTicketList.AssignTo)
                && string.IsNullOrWhiteSpace(advanceSearchTicketList.ModifiedBy)
                && !advanceSearchTicketList.CreatedDateFrom.HasValue
                && !advanceSearchTicketList.CreatedDateTo.HasValue
                && string.IsNullOrWhiteSpace(advanceSearchTicketList.AssignTo)
                && string.IsNullOrWhiteSpace(advanceSearchTicketList.ModifiedBy))
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
        #endregion

        #region Close Ticket
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
        #endregion
    }
}

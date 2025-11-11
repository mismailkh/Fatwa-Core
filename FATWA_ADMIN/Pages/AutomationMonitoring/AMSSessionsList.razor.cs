using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSSessionsList : ComponentBase
    {
        #region Varriables
        [Parameter]
        public dynamic ProcessId { get; set; }
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected RadzenDataGrid<AMSSessionListVM>? SessionGrid;
        protected IEnumerable<AMSSessionListVM> SessionLists { get; set; }
        public AMSSessionListVM SessionListVM { get; set; }
        protected AdvanceSearchSessionVM advanceSearchVM = new AdvanceSearchSessionVM();
        protected List<AMSSessionStatus> Statuses { get; set; } = new List<AMSSessionStatus>();
        protected bool isLoading { get; set; }
        public string? ProcessName { get; set; }
        public int count { get; set; }
        IEnumerable<AMSSessionListVM> _Sessionlist;
        IEnumerable<AMSSessionListVM> FilteredSession { get; set; } = new List<AMSSessionListVM>();
        protected IEnumerable<AMSSessionListVM> Sessionlist
        {
            get
            {
                return _Sessionlist;
            }
            set
            {
                if (!object.Equals(_Sessionlist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _Sessionlist };
                    _Sessionlist = value;

                    Reload();
                }

            }
        }
      

        protected string search {  get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;


        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            await PopulateStatuses();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;
            var response = await automationmonitoringService.GetSessionList(advanceSearchVM, Convert.ToInt32(ProcessId));
            if (response.IsSuccessStatusCode)
            {
                _Sessionlist = (IEnumerable<AMSSessionListVM>)response.ResultData;
                FilteredSession = (IEnumerable<AMSSessionListVM>)response.ResultData;
                var PrcessName = _Sessionlist.FirstOrDefault();
                if (PrcessName != null)
                {
                    ProcessName = PrcessName.ProcessName;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            StateHasChanged();
        }
        protected async Task PopulateStatuses()
        {
            var response = await automationmonitoringService.GetSessionStatus();
            if (response.IsSuccessStatusCode)
            {
                Statuses = (List<AMSSessionStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region  List Filtration -> SearchFunctionality
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
                        FilteredSession = await gridSearchExtension.Filter(_Sessionlist, new Query()
                        {
                            Filter = $@"i => (i.processName != null && i.processName.ToLower().Contains(@0))",
                            FilterParameters = new object[] { search }
                        });
                    }
                    else
                    {
                        FilteredSession = await gridSearchExtension.Filter(_Sessionlist, new Query()
                        {
                            Filter = $@"i => (i.processName != null && i.processName.ToLower().Contains(@0))",
                            FilterParameters = new object[] { search }
                        });
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
        protected async Task SessionLogsDetails(AMSSessionListVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSSessionLogList>(
                            translationState.Translate("Session_Log"),
                            new Dictionary<string, object>() { { "SessionId", item.SessionId } },
                            new DialogOptions()
                            {
                                Width = "60% !important",
                                CloseDialogOnOverlayClick = false,
                            });
                await Task.Delay(400);
                await Load();

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        protected async Task UpdateSessionStatus(AMSSessionListVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSSessionAction>(
                            translationState.Translate("Session_Action"),
                            new Dictionary<string, object>() { { "SessionId", item.SessionId } },
                            new DialogOptions()
                            {
                                Width = "30% !important",
                                CloseDialogOnOverlayClick = false,
                            });
                await Task.Delay(400);
                await Load();

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
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
            if (!advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue && advanceSearchVM.StatusId == 0)
            {

            }
            else
            {
                Keywords = true;
                await Load();
                //await grid.Reload();
                StateHasChanged();
            }



        }

        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchSessionVM();
            await Load();
            Keywords = false;
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();

            }
        }

        #endregion

    }
}

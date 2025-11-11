using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSSessionLogList : ComponentBase
    {
        #region Varriables
        [Parameter]
        public dynamic SessionId { get; set; }
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected RadzenDataGrid<AMSSessionLogsVM>? SessionGrid;
        protected IEnumerable<AMSSessionLogsVM>  SessionLists { get; set; }
        public AMSSessionLogsVM  SessionLogsVM { get; set; }
        protected AdvanceSearchProcessVM advanceSearchVM = new AdvanceSearchProcessVM();
        protected bool isLoading { get; set; }
        public int count { get; set; }
        IEnumerable<AMSSessionLogsVM> _Sessionlist;
        IEnumerable<AMSSessionLogsVM> FilteredSession { get; set; } = new List<AMSSessionLogsVM>();
        protected IEnumerable<AMSSessionLogsVM> Sessionlist
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
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;
            var response = await automationmonitoringService.GetSessionLogsList(Convert.ToInt32(SessionId));
            if (response.IsSuccessStatusCode)
            {
                _Sessionlist = (IEnumerable<AMSSessionLogsVM>)response.ResultData;
                FilteredSession = (IEnumerable<AMSSessionLogsVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            StateHasChanged();
        }
        #endregion

        #region  List Filtration -> SearchFunctionality
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
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
    }
}

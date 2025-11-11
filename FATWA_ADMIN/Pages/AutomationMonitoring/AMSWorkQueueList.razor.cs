using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSWorkQueueList : ComponentBase
    {
        #region Varriables
        [Parameter]
        public dynamic ProcessId { get; set; }
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected List<AMSQueueItemStatus> Statuses { get; set; } = new List<AMSQueueItemStatus>();
        protected AdvanceSearchQueueVM advanceSearchVM = new AdvanceSearchQueueVM();
        protected RadzenDataGrid<AutomationMonitoringQueueVM>? QueueGrid;
        protected bool isLoading { get; set; }
        public string ProcessName { get; set; }
        IEnumerable<AutomationMonitoringQueueVM> _Queuelist;
        IEnumerable<AutomationMonitoringQueueVM> FilteredQueue { get; set; } = new List<AutomationMonitoringQueueVM>();
        protected IEnumerable<AutomationMonitoringQueueVM> Queuelist
        {
            get
            {
                return _Queuelist;
            }
            set
            {
                if (!object.Equals(_Queuelist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _Queuelist };
                    _Queuelist = value;

                    Reload();
                }

            }
        }

        protected string search { get; set; }
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
            var response = await automationmonitoringService.GetQueueList(Convert.ToInt32(ProcessId), advanceSearchVM);
            if (response.IsSuccessStatusCode)
            {
                _Queuelist = (IEnumerable<AutomationMonitoringQueueVM>)response.ResultData;
                FilteredQueue = (List<AutomationMonitoringQueueVM>)response.ResultData;
                var result = _Queuelist.FirstOrDefault();
                if (result != null)
                {
                    ProcessName = result.ProcessName;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            StateHasChanged();
        }
        #endregion
        protected async Task PopulateStatuses()
        {
            var response = await automationmonitoringService.GetQueueItemStatuses();
            if (response.IsSuccessStatusCode)
            {
                Statuses = (List<AMSQueueItemStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
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
                        FilteredQueue = await gridSearchExtension.Filter(_Queuelist, new Query()
                        {
                            Filter = $@"i => (i.QueueName != null && i.QueueName.ToLower().Contains(@0))",
                            FilterParameters = new object[] { search }
                        });
                    }
                    else
                    {
                        FilteredQueue = await gridSearchExtension.Filter(_Queuelist, new Query()
                        {
                            Filter = $@"i => (i.QueueName != null && i.QueueName.ToLower().Contains(@0))",
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

        protected async Task DetailsQueueItem(AutomationMonitoringQueueVM item)
        {
            try
            {
                navigationManager.NavigateTo("/queueitem-list/" + item.QueueId);
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
                    //Summary = $"???!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (advanceSearchVM.StatusId == 0 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
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
            advanceSearchVM = new AdvanceSearchQueueVM();
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

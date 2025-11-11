using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSProcessDetails : ComponentBase
    {
        #region Varriables
        [Parameter]
        public dynamic ProcessId { get; set; }
        protected bool Keywords = false;
        protected RadzenDataGrid<AMSSessionLogsVM>? SessionLogsGrid;
        protected RadzenDataGrid<AMSResourcesVM> grid;
        protected RadzenDataGrid<AutomationMonitoringQueueVM>? QueueGrid;
        protected RadzenDataGrid<AMSQueueListVM>? QueueDetailsLitGrid;
        protected AMSProcesses aMSProcesses = new AMSProcesses();
        protected bool isLoading { get; set; }
        public bool allowRowSelectOnRowClick = true;
        protected int? TotalPendingItems { get; set; }
        protected int? TotalItems { get; set; }

        IEnumerable<AMSResourcesVM> _getAMSResourcesVM;
        IEnumerable<AMSResourcesVM> FilteredgetAMSResourcesVM;
        protected IEnumerable<AMSResourcesVM> getAMSResourcesVM
        {
            get
            {
                return _getAMSResourcesVM;
            }
            set
            {
                if (!Equals(_getAMSResourcesVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getAMSResourcesVM", NewValue = value, OldValue = _getAMSResourcesVM };
                    _getAMSResourcesVM = value;

                    Reload();
                }

            }
        }
        IEnumerable<AMSQueueListVM> _getAMSQueueListVMs;
        IEnumerable<AMSQueueListVM> FilteredgetAMSQueueListVM;
        protected IEnumerable<AMSQueueListVM> getAMSQueueListVMs
        {
            get
            {
                return _getAMSQueueListVMs;
            }
            set
            {
                if (!Equals(_getAMSQueueListVMs, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getAMSQueueListVMs", NewValue = value, OldValue = _getAMSQueueListVMs };
                    _getAMSQueueListVMs = value;

                    Reload();
                }

            }
        }
        protected AdvanceSearchQueueVM advanceSearchVM = new AdvanceSearchQueueVM();
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

        IEnumerable<AMSSessionLogsVM> _SessionLogslist;
        IEnumerable<AMSSessionLogsVM> FilteredSessionLogs { get; set; } = new List<AMSSessionLogsVM>();
        protected IEnumerable<AMSSessionLogsVM> SessionLogslist
        {
            get
            {
                return _SessionLogslist;
            }
            set
            {
                if (!object.Equals(_SessionLogslist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredSessionLogs", NewValue = value, OldValue = _SessionLogslist };
                    _SessionLogslist = value;

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

            await Load();
            await PapulateResources();
            await PapulateQueueList();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;
            var response = await automationmonitoringService.GetProcessesById(Convert.ToInt32(ProcessId));
            if (response.IsSuccessStatusCode)
            {
                aMSProcesses = (AMSProcesses)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            StateHasChanged();
        }
        public async Task PapulateResources()
        {
            var response = await automationmonitoringService.GetResourcesByProcessId(Convert.ToInt32(ProcessId));
            if (response.IsSuccessStatusCode)
            {
                getAMSResourcesVM = (IEnumerable<AMSResourcesVM>)response.ResultData;
                FilteredgetAMSResourcesVM = (IEnumerable<AMSResourcesVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task PapulateQueueList()
        {
            var response = await automationmonitoringService.GetQueueDetialsByProcessId(Convert.ToInt32(ProcessId));
            if (response.IsSuccessStatusCode)
            {
                getAMSQueueListVMs = (IEnumerable<AMSQueueListVM>)response.ResultData;
                FilteredgetAMSQueueListVM = (IEnumerable<AMSQueueListVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        protected async Task ExpandResources(AMSResourcesVM aMSResourcesVM)
        {
            var response = await automationmonitoringService.GetSessionLogsList(Convert.ToInt32(aMSResourcesVM.SessionId));
            if (response.IsSuccessStatusCode)
            {
                SessionLogslist = (IEnumerable<AMSSessionLogsVM>)response.ResultData;
                FilteredSessionLogs = (IEnumerable<AMSSessionLogsVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task ExpandQueue(AMSQueueListVM automationMonitoringQueue)
        {
            var response = await automationmonitoringService.GetQueueListByQueueId(automationMonitoringQueue.QueueId);
            if (response.IsSuccessStatusCode)
            {
                _Queuelist = (IEnumerable<AutomationMonitoringQueueVM>)response.ResultData;
                FilteredQueue = (List<AutomationMonitoringQueueVM>)response.ResultData;
                var result = _Queuelist.FirstOrDefault();
                if (FilteredQueue != null)
                {
                    TotalPendingItems = FilteredQueue.Sum(x => x.PendingItems ?? 0);
                    TotalItems = FilteredQueue.Sum(x => x.ItemCount ?? 0);
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task ActiveAndInActiveProcess(AutomationMonitoringProcessVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSProcessAction>(
                            translationState.Translate("Process_Action"),
                            new Dictionary<string, object>() { { "ProcessId", item.Id } },
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
        protected async Task ViewCompletedItemList(AutomationMonitoringQueueVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSQueueItemList>(
                          translationState.Translate("Queue_Item_List") + "<br/>Date: " + (item.CreatedDate?.ToString("dd/MM/yyyy") ?? string.Empty),
                           new Dictionary<string, object>() { { "QueueId", item.QueueId }, { "gridshowonly", true }, { "statuscode", 3 },{ "ProcessId", ProcessId } },
                            new DialogOptions()
                            {
                                Width = "80% !important",
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
        protected async Task ViewNewPendingItemList(AutomationMonitoringQueueVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSQueueItemList>(
                          translationState.Translate("Queue_Item_List") + "<br/>Date: " + (item.CreatedDate?.ToString("dd/MM/yyyy") ?? string.Empty),
                           new Dictionary<string, object>() { { "QueueId", item.QueueId }, { "gridshowonly", true }, { "statuscode", 1 }, { "ProcessId", ProcessId }, { "CreatedDate", item.CreatedDate?.ToString("dd/MM/yyyy") } },
                            new DialogOptions()
                            {
                                Width = "80% !important",
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
        protected async Task ViewExceptionItemList(AutomationMonitoringQueueVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSQueueItemList>(
                          translationState.Translate("Queue_Item_List") + "<br/>Date: " + (item.CreatedDate?.ToString("dd/MM/yyyy") ?? string.Empty),
                            new Dictionary<string, object>() { { "QueueId", item.QueueId}, { "gridshowonly", true }, { "statuscode", 4 }, { "ProcessId", ProcessId } },
                            new DialogOptions()
                            {
                                Width = "80% !important",
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
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
    }



}

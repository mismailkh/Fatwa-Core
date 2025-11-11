using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSQueueItemList : ComponentBase
    {
        #region Varriables
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        [Parameter]
        public dynamic QueueId { get; set; }
        [Parameter]
        public dynamic gridshowonly { get; set; }
        [Parameter]
        public dynamic statuscode { get; set; }
        [Parameter]
        public dynamic ProcessId { get; set; }
        [Parameter]
        public dynamic CreatedDate { get; set; }
        protected RadzenDataGrid<AutomationMonitoringQueueItemVM>? QueueItemGrid;
        protected AdvanceSearchQueueVM advanceSearchVM = new AdvanceSearchQueueVM();
        protected string RedirectURL { get; set; }
        public string TransKeyHeader = string.Empty;
        [Inject]
        protected IJSRuntime JsInterop { get; set; }
        protected List<AMSQueueItemStatus> Statuses { get; set; } = new List<AMSQueueItemStatus>();
        protected bool isLoading { get; set; }
        protected string? QueueName { get; set; }

        protected IEnumerable<AutomationMonitoringQueueItemVM> _QueueItem;
        IEnumerable<AutomationMonitoringQueueItemVM> FilteredQueueItem { get; set; } = new List<AutomationMonitoringQueueItemVM>();
        protected IEnumerable<AutomationMonitoringQueueItemVM> QueueItem
        {
            get
            {
                return _QueueItem;
            }
            set
            {
                if (!object.Equals(_QueueItem, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredQueueItem", NewValue = value, OldValue = _QueueItem };
                    _QueueItem = value;

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
            try
            {
                isLoading = true;
                var response = await automationmonitoringService.GetQueueItemList(advanceSearchVM, Convert.ToInt32(QueueId));
                if (response.IsSuccessStatusCode)
                {
                    _QueueItem = (IEnumerable<AutomationMonitoringQueueItemVM>)response.ResultData;
                    FilteredQueueItem = (List<AutomationMonitoringQueueItemVM>)response.ResultData;
                    if (Convert.ToInt32(statuscode) == 1)
                    {
                         FilteredQueueItem = FilteredQueueItem.Where(x => x.StatusCode == Convert.ToInt32(statuscode) &&  x.CreatedDate?.ToString("dd/MM/yyyy") == CreatedDate).ToList();
                        _QueueItem = FilteredQueueItem;
                    }
                    else
                    {
                        FilteredQueueItem = FilteredQueueItem.Where(x => x.StatusCode == Convert.ToInt32(statuscode)).ToList();
                    }

                    var queueName = _QueueItem.FirstOrDefault();

                    if (queueName != null)
                    {
                        QueueName = queueName.QueueName;
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

                isLoading = false;

                StateHasChanged();
            }
            catch (Exception ex)
            {

                throw;
            }

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
                    FilteredQueueItem = await gridSearchExtension.Filter(_QueueItem, new Query()
                    {
                        Filter = $@"i => (i.ItemName != null && i.ItemName.ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                }
                else
                {
                    FilteredQueueItem = await gridSearchExtension.Filter(_QueueItem, new Query()
                    {
                        Filter = $@"i => (i.ItemName != null && i.ItemName.ToLower().Contains(@0))",
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
        protected async Task AMSExceptionAction(AutomationMonitoringQueueItemVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSExceptionAction>(
                            translationState.Translate("Exception_Action"),
                            new Dictionary<string, object>() { { "ItemId", item.ItemId } },
                            new DialogOptions()
                            {
                                Width = "50% !important",
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
        protected async Task AMSExceptionDetails(AutomationMonitoringQueueItemVM item)
        {
            try
            {
                navigationManager.NavigateTo("/exception-details/" + item.ItemId + "/" + ProcessId);
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
        protected async Task ItemLogDetails(AutomationMonitoringQueueItemVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSItemLogList>(
                            translationState.Translate("Item_Log"),
                            new Dictionary<string, object>() { { "ItemId", item.ItemId } },
                            new DialogOptions()
                            {
                                Width = "100% !important",
                                Height = "100% !important",
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
    }
}

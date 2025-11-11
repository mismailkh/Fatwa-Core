using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSCaseDataaExtraction : ComponentBase
    {
        #region Varriables
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        [Parameter]
        public dynamic QueueId { get; set; }
        public string CANumber { get; set; }
        public string CaseNumber { get; set; }
        protected RadzenDataGrid<AMSCaseDataExtractionVM>? QueueItemGrid;
        protected AdvanceSearchQueueVM advanceSearchVM = new AdvanceSearchQueueVM();
        protected List<AMSQueueItemStatus> Statuses { get; set; } = new List<AMSQueueItemStatus>();
        AMSCaseDataExtractionVM aMSCaseDataExtraction = new AMSCaseDataExtractionVM { StartDate = DateTime.Now };
        protected string RedirectURL { get; set; }
        public string TransKeyHeader = string.Empty;
        [Inject]
        protected IJSRuntime JsInterop { get; set; }
        protected bool isLoading { get; set; }
        protected IEnumerable<AMSCaseDataExtractionVM> _QueueItem;
        IEnumerable<AMSCaseDataExtractionVM> FilteredQueueItem { get; set; } = new List<AMSCaseDataExtractionVM>();
        protected IEnumerable<AMSCaseDataExtractionVM> QueueItem
        {
            get
            {
                return _QueueItem;
            }
            set
            {
                if (!object.Equals(_QueueItem, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _QueueItem };
                    _QueueItem = value;

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
            var response = await automationmonitoringService.GetCaseDataExtraction(advanceSearchVM);
            if (response.IsSuccessStatusCode)
            {
                _QueueItem = (IEnumerable<AMSCaseDataExtractionVM>)response.ResultData;
                FilteredQueueItem = (List<AMSCaseDataExtractionVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            isLoading = false;

            StateHasChanged();
        }
        #endregion

        private async Task FormSubmit(AMSCaseDataExtractionVM aMSCaseDataExtractionVM)
        {

            // Validation logic for CANumber and CaseNumber
            if (string.IsNullOrEmpty(CANumber) || string.IsNullOrEmpty(CaseNumber))
            {
            }
            else
            {
                AMSWorkQueueItem queueItem = new AMSWorkQueueItem();
                queueItem.CreatedBy = loginState.UserDetail.UserName;
                queueItem.Data = CANumber + "," + CaseNumber;
                bool exists = await CheckIfAlreadyPushedToCMS(queueItem.Data);
                if (exists)
                {
                    var response = await automationmonitoringService.SaveAMSCaseDateExtraction(queueItem);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Queue_Item_Save_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        CANumber = string.Empty;
                        CaseNumber = string.Empty;
                        await Load();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Queue_Item_Not_Save"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("CAN_And_Case_Number_is_already_pushed_to_Queue"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }

        private async Task<bool> CheckIfAlreadyPushedToCMS(string Data)
        {
            var response = await automationmonitoringService.CheckIfAlreadyPushedToCMS(Data);
            if (response)
            {
                return false;
            }
            else
            {
                return true;
            }

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
                    FilteredQueueItem = await gridSearchExtension.Filter(_QueueItem, new Query()
                    {
                        Filter = $@"i => ((i.CaseNumber != null && i.CaseNumber.ToLower().Contains(@0))
                        || (i.CANNumber != null && i.CANNumber.ToLower().Contains(@0))
                        || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0))
                        || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@0)))",
                        FilterParameters = new object[] { search }
                    }); await InvokeAsync(StateHasChanged);
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
        protected async Task AMSExceptionDetails(AMSCaseDataExtractionVM item)
        {
            try
            {
                navigationManager.NavigateTo("/exception-details/" + item.ItemId);
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
        protected async Task ItemLogDetails(AMSCaseDataExtractionVM item)
        {
            try
            {
                navigationManager.NavigateTo("/workqueuelog-list/" + item.ItemId);
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

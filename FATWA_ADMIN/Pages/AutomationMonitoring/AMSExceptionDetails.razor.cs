using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.AutomationMonitoringInterface.AutomationMonitoringInterfaceEnum;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSExceptionDetails : ComponentBase
    {
        [Parameter]
        public dynamic ItemId { get; set; }
        [Parameter]
        public dynamic ProcessId { get; set; }
        public AMSExceptionsDetailsVM AMSExceptionsDetails { get; set; } = new AMSExceptionsDetailsVM();
        protected RadzenDataGrid<AMSItemLogVM>? ItemLogsGrid;
        [Inject]
        protected IJSRuntime JsInterop { get; set; }
        public bool tryException;
        public bool IsView { get; set; } = false;
        public string ExceptionComment;
        public string TransKeyHeader = string.Empty;
        protected string RedirectURL { get; set; }
        public string detailsprocessViewPath = "";
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        AMSWorkQueueItem _workQueueItem;
        protected AMSWorkQueueItem workQueueItem
        {
            get
            {
                return _workQueueItem;
            }
            set
            {
                if (!object.Equals(_workQueueItem, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "workQueueItem", NewValue = value, OldValue = _workQueueItem };
                    _workQueueItem = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<AMSItemLogVM> _Itemloglist;
        IEnumerable<AMSItemLogVM> FilteredItemLog { get; set; } = new List<AMSItemLogVM>();
        protected IEnumerable<AMSItemLogVM> Itemloglist
        {
            get
            {
                return _Itemloglist;
            }
            set
            {
                if (!object.Equals(_Itemloglist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredItemLog", NewValue = value, OldValue = _Itemloglist };
                    _Itemloglist = value;

                    Reload();
                }

            }
        }
        protected override async Task OnInitializedAsync()
        {
            workQueueItem = new AMSWorkQueueItem() { };
            workQueueItem.Id = new int();
            await Load();
        }
        protected async Task Load()
        {

            var response = await automationmonitoringService.GetExceptionDetails(Convert.ToInt32(ItemId));
            if (response.IsSuccessStatusCode)
            {
                AMSExceptionsDetails = (AMSExceptionsDetailsVM)response.ResultData;
                detailsprocessViewPath = $"/AmsProcess-Details/{ProcessId}";
                if (AMSExceptionsDetails.ItemId != null)
                {
                    await PapulateItemLog((int)AMSExceptionsDetails.ItemId);
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);

            }
        }

        protected async Task SaveChanges(MouseEventArgs args)
        {
            try
            {
                if (tryException)
                {
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        workQueueItem.Id = Convert.ToInt32(ItemId);
                        workQueueItem.StatusId = (int)QueueItemsEnum.ReattemptException;
                        workQueueItem.ExceptionComment = ExceptionComment;
                        var response = await automationmonitoringService.UpdateQueueItem(workQueueItem);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Exception_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Exception_could_not_be_updated"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }

                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Checked_Box"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
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
        public async Task PapulateItemLog(int ItemId)
        {

            var response = await automationmonitoringService.GetItemLogsList(Convert.ToInt32(ItemId));
            if (response.IsSuccessStatusCode)
            {
                _Itemloglist = (IEnumerable<AMSItemLogVM>)response.ResultData;
                FilteredItemLog = (IEnumerable<AMSItemLogVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();
        }
        protected async Task ItemLogDetails(int? ItemId)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSItemLogList>(
                            translationState.Translate("Item_Log"),
                            new Dictionary<string, object>() { { "ItemId", ItemId } },
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

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
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
    }
}

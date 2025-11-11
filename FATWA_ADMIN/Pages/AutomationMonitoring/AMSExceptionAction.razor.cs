using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.AutomationMonitoringInterface.AutomationMonitoringInterfaceEnum;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSExceptionAction : ComponentBase
    {
        [Parameter]
        public dynamic ItemId { get; set; }
        public AMSExceptionsDetailsVM AMSExceptionsDetails { get; set; } = new AMSExceptionsDetailsVM();
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
                
            }
            else
            {
               await invalidRequestHandlerService.ReturnBadRequestNotification(response);
             
            }
        }

        protected async Task SaveChanges(AMSWorkQueueItem args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    workQueueItem.Id = ItemId;
                    workQueueItem.StatusId = (int)QueueItemsEnum.ReattemptException;
                    var response = await automationmonitoringService.UpdateQueueItem(workQueueItem);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Exception_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(workQueueItem);
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
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}

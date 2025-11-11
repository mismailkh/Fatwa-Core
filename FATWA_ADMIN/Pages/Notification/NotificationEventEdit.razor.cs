using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_GENERAL.Helper.Enum;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.Notification
{
    public partial class NotificationEventEdit : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic EventId { get; set; }
        #endregion

        #region variables
        public NotificationEvent Event { get; set; } = new NotificationEvent();
        public List<NotificationReceiverType> ReceiverType { get; set; } = new List<NotificationReceiverType>();
        public List<Role> roles { get; set; } = new List<Role>();
        public List<Group> groups { get; set; } = new List<Group>();
        bool IsRole;
        bool IsGroup;
        string roleId;
        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            await GetReceiverType();
            await GetRoles();
            await GetGroups();
            spinnerService.Hide();
        }
        #endregion

        #region On Load 
        private async Task Load()
        {
            if (EventId != null)
            {
                var response = await notificationsService.GetNotificationEventById(int.Parse(EventId));
                if (response.IsSuccessStatusCode)
                {
                    Event = (NotificationEvent)response.ResultData;
                    //if (Event.ReceiverTypeId == (int)NotificationReceiverTypeEnum.Role)
                    //{
                    //    IsRole = true;
                    //    roleId = Event.ReceiverTypeRefId.ToString();
                    //}
                    //else if (Event.ReceiverTypeId == (int) NotificationReceiverTypeEnum.Group)
                    //{
                    //    IsGroup = true;
                    //}
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Save And Cancel Button
        protected async Task SaveChanges()
        {

            if (await dialogService.Confirm(
                translationState.Translate("Sure_update_Event"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = @translationState.Translate("OK"),
                    CancelButtonText = @translationState.Translate("Cancel")
                }) == true)
            {
                spinnerService.Show();

                var response = await notificationsService.EditNotificationEvent(Event);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Event_Updated_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await Task.Delay(2000);

                    await jSRuntime.InvokeVoidAsync("history.back");

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
                 }


        protected async Task CancelChanges(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Cancel"),
            translationState.Translate("Confirm"),
            new ConfirmOptions()
            {
                OkButtonText = @translationState.Translate("OK"),
                CancelButtonText = @translationState.Translate("Cancel")
            });

            if (dialogResponse == true)
            {
                navigationManager.NavigateTo("/notification-eventlist");
            }
        }

        #endregion

        #region Populate Drop Downs
        public async Task GetReceiverType()
        {
            var response = await notificationsService.GetReceiverType();
            if (response.IsSuccessStatusCode)
            {
                ReceiverType = (List<NotificationReceiverType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        public async Task GetRoles()
        {
            var result = await lookupService.GetRoles();
            if (result.IsSuccessStatusCode)
            {
                roles = (List<Role>)result.ResultData;
            }
        }

        public async Task GetGroups()
        {
            var result = await lookupService.GetGroups();
            if (result.IsSuccessStatusCode)
            {
                groups = (List<Group>)result.ResultData;
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

        #region On Receiver type Change
        public void OnReceivertypeChange()
        {
            if (Event.ReceiverTypeId == (int)NotificationReceiverTypeEnum.Role)
            {
                IsRole = true;
                IsGroup = false;

            }
            else if (Event.ReceiverTypeId == (int)NotificationReceiverTypeEnum.Group)
            {
                IsRole = false;
                IsGroup = true;
            }
            else
            {
                IsRole = false;
                IsGroup = false;
            }
        }
        #endregion
    }
}

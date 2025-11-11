using FATWA_DOMAIN.Models.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.Notification
{
    public partial class AddNotificationEvent : ComponentBase
    {
        public AddNotificationEvent()
        {
            Template = new NotificationTemplate();
        }
        #region Parameter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion
        #region Varriables
        public NotificationTemplate? Template { get; set; }
        public bool FieldDisable { get; set; }
        public List<NotificationEvent>? Events { get; set; }
        public List<NotificationChannel>? Channels { get; set; }
        public IEnumerable<NotificationEventPlaceholders>? PlaceHolder { get; set; }
        public RadzenDataGrid<NotificationEventPlaceholders> PlaceHolderGrid = new RadzenDataGrid<NotificationEventPlaceholders>();
        #endregion
        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (Id is not null)
            {
                FieldDisable = true;
            }
            else
            {
                FieldDisable = false;
            }
            await PopulateChannels();
            await PopulateEvents();
            await Load();
            spinnerService.Hide();

        }
        private async Task Load()
        {
            if (Id != null)
            {
                var response = await notificationsService.GetNotificationEventTemplateById(Guid.Parse(Id));
                if (response.IsSuccessStatusCode)
                {
                    Template = (NotificationTemplate)response.ResultData;
                    await OnEventsChange();
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }

        #endregion
        #region Populate Drop Downs
        public async Task PopulateEvents()
        {
            var response = await notificationsService.GetEvent();
            if (response.IsSuccessStatusCode)
            {
                Events = (List<NotificationEvent>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task PopulateChannels()
        {
            var response = await notificationsService.GetChannel();
            if (response.IsSuccessStatusCode)
            {
                Channels = (List<NotificationChannel>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        #endregion
        #region On Change Event
        public async Task OnEventsChange()
        {
            var response = await notificationsService.GetPlaceHoldersByEventId(Template.EventId);
            if (response.IsSuccessStatusCode)
            {
                PlaceHolder = (IEnumerable<NotificationEventPlaceholders>)response.ResultData;
                if (Id == null)
                {
                    Template.BodyEn = string.Empty;
                    Template.BodyAr = string.Empty;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion


        protected async Task SaveChanges()
        {
            bool? dialogResponse = await dialogService.Confirm(
                translationState.Translate("Sure_Save_Event"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = @translationState.Translate("OK"),
                    CancelButtonText = @translationState.Translate("Cancel")
                });

            if (dialogResponse == true)
            {
                spinnerService.Show();

                try
                {
                    Template.TemplateId = Id == null ? Guid.NewGuid() : Template.TemplateId;
                    var response = Id == null
                        ? await notificationsService.CreateNotificationEventTemplate(Template)
                        : await notificationsService.UpdateNotificationEventTemplate(Template);

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate(Id == null ? "Event_Save_Successfully" : "Event_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Task.Delay(2000);
                        await jSRuntime.InvokeVoidAsync("history.back");
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                finally
                {
                    spinnerService.Hide();
                }
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

        #region Redirect Function

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        #endregion Redirect Functions     

    }
}

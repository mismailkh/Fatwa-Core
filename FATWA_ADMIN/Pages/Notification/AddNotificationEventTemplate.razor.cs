using FATWA_DOMAIN.Models.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.Notification
{
    public partial class AddNotificationEventTemplate : ComponentBase
    {
        public AddNotificationEventTemplate()
        {
            Template = new NotificationTemplate();
        }
        #region Parameter
        [Parameter]
        public dynamic Id { get; set; }
        [Parameter]
        public dynamic EventId { get; set; }
        [Parameter]
        public dynamic ChannelId { get; set; }
        #endregion
        #region Varriables
        public NotificationTemplate? Template { get; set; }
        public List<NotificationEvent>? Events { get; set; }
        public List<NotificationChannel>? Channels { get; set; }
        public IEnumerable<NotificationEventPlaceholders>? PlaceHolder { get; set; }
        public RadzenDataGrid<NotificationEventPlaceholders> PlaceHolderGrid = new RadzenDataGrid<NotificationEventPlaceholders>();
        protected string nameEnValidationMsg = "";
        protected string nameArValidationMsg = "";
        protected string channelValidationMsg = "";
        protected string bodyEnValidationMsg = "";
        protected string bodyArValidationMsg = "";
        protected string eventValidationMsg = "";
        #endregion
        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            Template.EventId = int.Parse(EventId);
            Template.ChannelId = int.Parse(ChannelId);
            await OnEventsChange();
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


            if ((Template.ChannelId != 0) && (Template.EventId != 0) && (Template.BodyEn is not null) && (Template.BodyAr is not null) && (Template.NameEn is not null) && (Template.NameAr is not null))
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
                    if (Id == null)
                    {
                        Template.TemplateId = Guid.NewGuid();
                        Template.isActive = true;
                        var response = await notificationsService.CreateNotificationEventTemplate(Template);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Event_Save_Successfully"),
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
                    else
                    {

                        var response = await notificationsService.UpdateNotificationEventTemplate(Template);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Event_Updated_Successfully"),
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
                    spinnerService.Hide();
                }
            }
            else
            {
                nameEnValidationMsg = String.IsNullOrEmpty(Template.NameEn) ? translationState.Translate("Required_Field") : "";
                nameArValidationMsg = String.IsNullOrEmpty(Template.NameAr) ? translationState.Translate("Required_Field") : "";
                bodyEnValidationMsg = String.IsNullOrEmpty(Template.BodyEn) ? translationState.Translate("Required_Field") : "";
                bodyArValidationMsg = String.IsNullOrEmpty(Template.BodyAr) ? translationState.Translate("Required_Field") : "";
                channelValidationMsg = Template.ChannelId <= 0 ? translationState.Translate("Required_Field") : "";
                eventValidationMsg = Template.EventId <= 0 ? translationState.Translate("Required_Field") : "";

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


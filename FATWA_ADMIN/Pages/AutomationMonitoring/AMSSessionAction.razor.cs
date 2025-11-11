using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSSessionAction : ComponentBase
    {
        [Parameter]
        public dynamic SessionId { get; set; }
        protected IEnumerable<StatusOption> statusOptions { get; set; }
        protected List<AMSSessionStatus> Statuses { get; set; } = new List<AMSSessionStatus>();
        public class StatusOption
        {
            public bool Id { get; set; }
            public string Status { get; set; }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        AMSSession _session;
        protected AMSSession session
        {
            get
            {
                return _session;
            }
            set
            {
                if (!object.Equals(_session, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "session", NewValue = value, OldValue = _session };
                    _session = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected override async Task OnInitializedAsync()
        {
            session = new AMSSession() { };
            session.SessionId = new int();
            await Load();
            await PopulateStatuses();
        }
        protected async Task Load()
        {


        }

        protected async Task SaveChanges(AMSSession args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    session.SessionId = SessionId;
                    var response = await automationmonitoringService.UpdateSession(session);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Session_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(session);
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("session_could_not_be_updated"),
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
        protected async Task PopulateStatuses()
        {
            var response = await automationmonitoringService.GetSessionStatus();
            if (response.IsSuccessStatusCode)
            {
                Statuses = (List<AMSSessionStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}

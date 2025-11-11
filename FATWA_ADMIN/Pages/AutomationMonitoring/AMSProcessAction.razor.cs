using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSProcessAction : ComponentBase
    {
        [Parameter]
        public dynamic ProcessId { get; set; }
        protected IEnumerable<StatusOption> statusOptions { get; set; }
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
        AMSProcesses _process;
        protected AMSProcesses process
        {
            get
            {
                return _process;
            }
            set
            {
                if (!object.Equals(_process, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "process", NewValue = value, OldValue = _process };
                    _process = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected override async Task OnInitializedAsync()
        {
            process = new AMSProcesses() { };
            process.Id = new int();
            await Load();
        }
        protected async Task Load()
        {
            statusOptions = new List<StatusOption>
            {
            new StatusOption { Id = true, Status = "Active" },
            new StatusOption { Id = false, Status = "Inactive" }
            };
            statusOptions = (IEnumerable<StatusOption>)statusOptions;
      
        }

        protected async Task SaveChanges(AMSProcesses args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    process.Id = ProcessId;
                    var response = await automationmonitoringService.UpdateProcess(process);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Process_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(process);
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Process_could_not_be_updated"),
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

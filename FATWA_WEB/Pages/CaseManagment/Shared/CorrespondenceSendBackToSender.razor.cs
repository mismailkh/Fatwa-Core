using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;


namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class CorrespondenceSendBackToSender : ComponentBase
    {

        [Parameter]
        public dynamic CommunicationId { get; set; }
        public CommunicationHistory? communicationHistory { get; set; } = new();

        protected async Task FormSubmit()
        {
            bool? dialogResponse = false;
            string successMessage = translationState.Translate("Assined_Back_Hos_Done");


            dialogResponse = await dialogService.Confirm(
                translationState.Translate("Sure_Submit"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                });
            if (dialogResponse == true)
            {
                communicationHistory.SentBy = Guid.Parse(loginState.UserDetail.UserId);
                communicationHistory.ReferenceId = Guid.Parse(CommunicationId);
                var response = await communicationService.SendBackToSender(communicationHistory);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = successMessage,
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    navigationManager.NavigateTo("/inboxOutbox-list");

                }

            }
        }
        protected async Task ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

    }
}

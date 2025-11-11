using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class CorrespondenceReturnToHos : ComponentBase
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
                communicationHistory.SectorId = loginState.UserDetail.SectorTypeId;
                communicationHistory.ReferenceId = Guid.Parse(CommunicationId);
                var response = await communicationService.AssignBackToHos(communicationHistory);
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

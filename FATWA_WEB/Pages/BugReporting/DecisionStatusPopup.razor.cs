using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;

namespace FATWA_WEB.Pages.BugReporting
{
    public partial class DecisionStatusPopup : ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid ReferenceId { get; set; }
        #endregion
        #region Variable Declaration 
        public string reasonValidationMessage { get; set; } = "";
        public string StatusValidationMessage { get; set; } = "";
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        public class StautsEnumTemp
        {
            public int StatusEnumValue { get; set; }
            public string StatusEnumName { get; set; }
        }
        public List<StautsEnumTemp> bugStatuses { get; set; } = new List<StautsEnumTemp>();
        public DecisionStatusVM decisionStatus = new DecisionStatusVM();

        #endregion
        #region Load Componenet
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await PopulateStatuses();
        }
        #endregion
        #region Populate 
        protected async Task PopulateStatuses()
        {
            try
            {
                try
                {
                    foreach (BugStatusEnum item in Enum.GetValues(typeof(BugStatusEnum)))
                    {
                        if (item == BugStatusEnum.Closed)
                        {
                            bugStatuses.Add(new StautsEnumTemp { StatusEnumName = translationState.Translate("Ticket_Close"), StatusEnumValue = (int)item });
                        }
                        if (item == BugStatusEnum.Reopened)
                        {
                            bugStatuses.Add(new StautsEnumTemp { StatusEnumName = translationState.Translate("Ticket_Reopen"), StatusEnumValue = (int)item });
                        }
                    }
                    decisionStatus.StatusId = (int)BugStatusEnum.Closed;
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
        #region Submit
        protected async Task SubmitStatus()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit"),
                    translationState.Translate("Submit"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                    if (dialogResponse != null)
                    {
                        if ((bool)dialogResponse)
                        {
                            decisionStatus.ReferenceId = ReferenceId;
                            var response = await bugReportingService.UpdateTicketStatus(decisionStatus);
                            if (response.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate(decisionStatus.StatusId == (int)BugStatusEnum.Closed ? "Ticket_Closed_Successfully" : "Ticket_Reopened_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                dialogService.Close();
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
                    }
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
        #region Redirect and Dialog Events
        protected async void ButtonCancelClick(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Cancel"),
                   translationState.Translate("Submit"),
                   new ConfirmOptions()
                   {
                       OkButtonText = translationState.Translate("OK"),
                       CancelButtonText = translationState.Translate("Cancel")
                   });
            if (dialogResponse != null)
            {
                if ((bool)dialogResponse)
                {
                    dialogService.Close();
                }
            }
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

    }
}

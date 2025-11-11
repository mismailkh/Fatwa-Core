using FATWA_DOMAIN.Models.BugReporting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class AddBugType : ComponentBase
    {
        #region variable Declaration
        protected BugIssueType bugType;
        #endregion

        #region Load Component

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        ApiCallResponse response = new ApiCallResponse();
        protected async Task Load()
        {
            bugType = new BugIssueType() { };
            bugType.Id = new int();

        }
        #endregion

        #region Save Changes
        protected async Task SaveChanges(BugIssueType args)
        {

            if (await dialogService.Confirm(
            translationState.Translate("Sure_Submit"),
            translationState.Translate("Submit"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                var response = await bugReportingService.SaveBugType(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Bug_Type_Added_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    dialogService.Close(args);
                    spinnerService.Hide();

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
            }
        }
        #endregion

        #region Cancel Button
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        #endregion
    }
}

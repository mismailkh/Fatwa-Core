using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
namespace FATWA_ADMIN.Pages.UserManagement.Translations
{
    public partial class SaveSystemTranslation : ComponentBase
    {
        #region Parameters
        [Parameter]
        public Translation Translation { get; set; }
        #endregion

        #region On Component Load
        //Jo ap dhondh rhy ho Wo yha nhi ha
        #endregion

        #region Button Events
        protected async Task CancelClick(MouseEventArgs args)
        {
            dialogService.Close();
        }
        protected async Task SaveTranslationSubmit(Translation args)
        {

            if (await dialogService.Confirm(translationState.Translate("Sure_Submit"),
                translationState.Translate("Submit"),
                new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
            {
                spinnerService.Show();
                var response = await translationService.UpdateTranslation(args);
                spinnerService.Hide();
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    dialogService.Close(true);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    dialogService.Close(false);
                }
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
    }
}

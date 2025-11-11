using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class GenderAdd : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        protected Gender Genderadd;
        ApiCallResponse response = new ApiCallResponse();
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            if (Id == null)
                Genderadd = new Gender() { };
            else
            {
                response = await lookupService.GetGenderById(Id);
                if (response.IsSuccessStatusCode)
                {
                    Genderadd = (Gender)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            spinnerService.Hide();

        }
        #endregion

        #region Save Changes
        protected async Task SaveChanges(Gender args)
        {

            if (await dialogService.Confirm(
                                 translationState.Translate("Sure_Submit"),
                                 translationState.Translate("Confirm"),
                                 new ConfirmOptions()
                                 {
                                     OkButtonText = translationState.Translate("OK"),
                                     CancelButtonText = translationState.Translate("Cancel")
                                 }) == true)
            {
                spinnerService.Show();
                if (Id != null)
                {
                    var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateGender(Genderadd);
                    if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Gender_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(true);
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                spinnerService.Hide();
            }
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        #endregion
    }
}

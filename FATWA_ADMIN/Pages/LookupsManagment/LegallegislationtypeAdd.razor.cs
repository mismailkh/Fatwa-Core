using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class LegallegislationtypeAdd : ComponentBase
    {
        #region Paramter

        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        protected LegalLegislationType legalLegislationType;

        ApiCallResponse response = new ApiCallResponse();
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                legalLegislationType = new LegalLegislationType() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetLegalLegislationtypeById(Id);
                if (response.IsSuccessStatusCode)
                {
                    legalLegislationType = (LegalLegislationType)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }

        #endregion

        #region Form Submit
        protected async Task Form0Submit(LegalLegislationType args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                                     translationState.Translate("Sure_Submit"),
                                     translationState.Translate("Confirm"),
                                     new ConfirmOptions()
                                     {
                                         OkButtonText = translationState.Translate("OK"),
                                         CancelButtonText = translationState.Translate("Cancel")
                                     });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (Id == null)
                    {
                        var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SavelegislationType(legalLegislationType);
                        if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Legislation_Type_Added_Successfully"),
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
                    else
                    {
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdatelegislationType(legalLegislationType);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Legislation_Type_Updated_Successfully"),
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

                    dialogService.Close(true);
                    StateHasChanged();
                    spinnerService.Hide();
                }

            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_literature") : translationState.Translate("Literature_type_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
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

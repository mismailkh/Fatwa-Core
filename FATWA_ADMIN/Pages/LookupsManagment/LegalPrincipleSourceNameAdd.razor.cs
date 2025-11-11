using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class LegalPrincipleSourceNameAdd : ComponentBase
    {
        #region Paramter

        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        protected LegalPublicationSourceName legalPublicationSourceName;
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
                legalPublicationSourceName = new LegalPublicationSourceName() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetLegalPublicationSourceNameById(Id);
                if (response.IsSuccessStatusCode)
                {
                    legalPublicationSourceName = (LegalPublicationSourceName)response.ResultData;
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
        protected async Task Form0Submit(LegalPublicationSourceName args)
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
                        var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SavelegalPublicationSourceName(legalPublicationSourceName);
                        if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Successfully_added_legal_publication_source"),
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
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdatePublicationSourceName(legalPublicationSourceName);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Successfully_edited_legal_publication_source"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_Legal_Publication_Source") : translationState.Translate("Legal_Publication_Source_could_not_be_updated"),
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

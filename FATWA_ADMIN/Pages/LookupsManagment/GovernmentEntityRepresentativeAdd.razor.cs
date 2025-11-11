using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class GovernmentEntityRepresentativeAdd : ComponentBase
    {

        #region Paramter

        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variable
        public List<GovernmentEntity> GovernmentEntitiesName { get; set; } = new List<GovernmentEntity>();
        protected GovernmentEntityRepresentative GovernmentEntityRepresentatives;
        ApiCallResponse response = new ApiCallResponse();
        #endregion

	    #region On Load
			
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await GetGovernmentEntities();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                GovernmentEntityRepresentatives = new GovernmentEntityRepresentative() { };
                GovernmentEntityRepresentatives.Id = new Guid();
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetGovernmentRepresentativeById(Id);
                if (response.IsSuccessStatusCode)
                {
                    GovernmentEntityRepresentatives = (GovernmentEntityRepresentative)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region populate Government Entity Name 
        protected async Task GetGovernmentEntities()
        {

            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovernmentEntitiesName = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();

        }
        #endregion

        #region Form submit

        protected async Task SaveChanges(GovernmentEntityRepresentative args)
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
                        var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SaveGovernmentEntityRepresentative(GovernmentEntityRepresentatives);
                        if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Government_Entity_Representative_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }

                    }
                    else
                    {
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateGovernmentEntityRepresentative(GovernmentEntityRepresentatives);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Government_Entity_Representative_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Government_Entity_Representative") : translationState.Translate("Government_Entity_Representative_could_not_be_updated"),
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

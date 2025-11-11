using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    //< History Author = 'Hassan Abbas' Date = '2022-02-27' Version = "1.0" Branch = "master" >Add Ge Representative</History>
    public partial class AddGeRepresentative : ComponentBase
    {
        #region Parameters

        [Parameter]
        public dynamic GovtEntityId { get; set; }
        public int EntityId { get { return Convert.ToInt32(GovtEntityId); } set { GovtEntityId = value; } }

        #endregion

        #region Variables 

        GovernmentEntityRepresentative geRepresentative = new GovernmentEntityRepresentative();
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();

        #endregion

        #region Component Load
        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateGovernmentEntities();
                geRepresentative.GovtEntityId = EntityId;
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Dialog Events
        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Submit Form</History>
        //< History Author = 'Aqeel' Date = '2022-09-29' Version = "1.0" Branch = "master" >Fix Civil id issue that is generic but it was not working</History>
        protected async Task Form0Submit(GovernmentEntityRepresentative args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Submit"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await cmsSharedService.CreateGeRepresentative(geRepresentative);
                    if (response.IsSuccessStatusCode)
                    {
                        geRepresentative = (GovernmentEntityRepresentative)response.ResultData;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Representative_Added_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(geRepresentative);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region Dropdown Change Events

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Case Templates</History>
        protected async Task PopulateGovernmentEntities()
        {
            var govtEntityResponse = await lookupService.GetGovernmentEntities();
            if (govtEntityResponse.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)govtEntityResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(govtEntityResponse);
            }
        }

        #endregion

    }
}

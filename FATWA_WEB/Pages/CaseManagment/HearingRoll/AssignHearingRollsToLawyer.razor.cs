using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.HearingRoll
{
    public partial class AssignHearingRollsToLawyer:ComponentBase
    {
        #region parameter
        [Parameter]
        public int ChamberNumberId { get; set; }
        [Parameter]
        public DateTime HearingDate { get; set; }
        #endregion
        #region variable declaration
        public CmsAssignLawyerToCourt assignLawyerToCourt { get; set; } = new CmsAssignLawyerToCourt();
        protected IEnumerable<LawyerVM> lawyers { get; set; }
        public string LawyerId { get; set; }
        protected CaseAssignment caseRequestLawyerAssignment = new CaseAssignment();
        #endregion
        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            await PopulateLawyersList(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0, ChamberNumberId); 
        }
        #endregion
        #region Remote Dropdown Data  
        protected async Task PopulateLawyersList(int sectorTypeId, int chamberNumberId = 0)
        {
            var userresponse = await lookupService.GetLawyersBySectorAndChamber(sectorTypeId, chamberNumberId);
            if (userresponse.IsSuccessStatusCode)
            {
                lawyers = (IEnumerable<LawyerVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
     
        #endregion
        #region Submit button click
        protected async Task Form0Submit(CmsAssignLawyerToCourt args)
        {
            try
            {
               
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Submit"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("Yes"),
                       CancelButtonText = @translationState.Translate("No")
                   });
                if (dialogResponse == true)
                {
                    ApiCallResponse response = null;
                    AssignHearingRollToLawyerVM hearingRollToLawyerVM = new AssignHearingRollToLawyerVM
                    {
                        LawyerId = LawyerId,
                        HearingDate = HearingDate,
                        ChamberNumberid = ChamberNumberId
                    };
                        response = await mojRollsService.AssignHearingRollsToLawyer(hearingRollToLawyerVM);
                    if (response.IsSuccessStatusCode)
                    {
                        var UpdateMojRollsReqeuest = await mojRollsService.UpdateMojRollsReqeust(hearingRollToLawyerVM);
                        if (UpdateMojRollsReqeuest.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Changes_Saved_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });

                        }
                       
                        navigationManager.NavigateTo("/upcominghearings-rolls-list");
                    }
                    else
                    {
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_Went_Wrong"),
                                //Summary = translationState.Translate("Error"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
        #region BACK BUTTON

        protected async Task ButtonCancel(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}

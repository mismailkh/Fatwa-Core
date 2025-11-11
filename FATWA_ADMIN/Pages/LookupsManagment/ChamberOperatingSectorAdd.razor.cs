using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class ChamberOperatingSectorAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public List<ChamberDetailVM>? SelectedChambers { get; set; }
        #endregion

        #region Variable 
        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        ChamberOperatingSector ChamberSectorType = new ChamberOperatingSector();
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateSectorTypes();
            spinnerService.Hide();
        }
        protected async Task PopulateSectorTypes()
        {

            var response = await lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)response.ResultData;
                SectorTypes = SectorTypes.Where(s => s.Id >= (int)OperatingSectorTypeEnum.AdministrativeRegionalCases && s.Id <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases && s.Id != (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases).ToList();
            }
            StateHasChanged();
        }
        #endregion

        #region Save Changes / form Submit

        protected async Task SaveChanges(ChamberOperatingSector args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    ChamberSectorType.SelectedChamberIds = SelectedChambers.Select(x => x.Id).ToList();
                    var response = await lookupService.SaveChamberOperatingSector(ChamberSectorType);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(1);
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
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}

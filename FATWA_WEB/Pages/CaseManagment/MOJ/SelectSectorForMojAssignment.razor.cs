using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //<!-- <History Author = 'Hassan Abbas' Date='2024-02-28' Version="1.0" Branch="master">Assign MOJ Case Files to Sector</History> -->
    public partial class SelectSectorForMojAssignment : ComponentBase
    {
        [Parameter]
        public List<Guid> SelectedFileIds { get; set; }
        #region Variable 
        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        AssignMojCaseFileToSectorVM sectorAssignemnt = new AssignMojCaseFileToSectorVM();
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateSectorTypes();
            spinnerService.Hide();
        }

        #endregion

        #region Data Population Events
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

        #region Dialog Events
        protected async Task SaveChanges(AssignMojCaseFileToSectorVM args)
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
                    sectorAssignemnt.FileIds = SelectedFileIds.ToList();
                    var response = await cmsCaseFileService.AssignUnassignedFilesToSector(sectorAssignemnt);
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
        protected async Task ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

    }
}

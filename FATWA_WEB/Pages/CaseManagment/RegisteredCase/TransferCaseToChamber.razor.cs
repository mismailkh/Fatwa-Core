using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class TransferCaseToChamber
    {
        [Parameter]
        public int ChamberId { get; set; }
        [Parameter]
        public int ChamberNumberId { get; set; }
        [Parameter]
        public Guid CaseId { get; set; }
        [Parameter]
        public int CourtId { get; set; }
        [Parameter]
        public Guid OutComeId { get; set; }
        public int SelectedChamberNumber { get; set; }
        public List<ChamberNumber> ChamberNumbers { get; set; } = new List<ChamberNumber>();
        public List<Chamber> allChambers { get; set; } = new List<Chamber>();
        public CMSRegisteredCaseTransferHistoryVM cMSRegisteredCaseTransferHistoryVM { get; set; } = new CMSRegisteredCaseTransferHistoryVM();
        protected override async Task OnInitializedAsync()
        {
            cMSRegisteredCaseTransferHistoryVM.ChamberFromId = ChamberId;
            cMSRegisteredCaseTransferHistoryVM.ChamberNumberFromId = ChamberNumberId;
            cMSRegisteredCaseTransferHistoryVM.OutcomeId = OutComeId;
            await PopulateChambers();
            await PopulateChamberNumbers();
        }

        #region Data Population and Change Events
        public async Task PopulateChamberNumbers()
        {
            var response = await lookupService.GetChamberNumbersByChamberId(ChamberId);
            if (response.IsSuccessStatusCode)
            {
                ChamberNumbers = (List<ChamberNumber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateChambers()
        {
            var response = await lookupService.GetChamberByCourtId(CourtId);
            if (response.IsSuccessStatusCode)
            {
                allChambers = (List<Chamber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async void OnChangeChamber()
        {
            var response = await lookupService.GetChamberNumbersByChamberId(ChamberId);
            if (response.IsSuccessStatusCode)
            {
                ChamberNumbers = (List<ChamberNumber>)response.ResultData;
                StateHasChanged();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        public async Task UpdateRegisteredCaseChamberNumber()
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
                    cMSRegisteredCaseTransferHistoryVM.ChamberToId = ChamberId;
                    cMSRegisteredCaseTransferHistoryVM.ChamberNumberToId = SelectedChamberNumber;
                    cMSRegisteredCaseTransferHistoryVM.CaseId = CaseId;
                    var response = await cmsRegisteredCaseService.UpdateRegisteredCaseChamberNumber(cMSRegisteredCaseTransferHistoryVM);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Case_Transfered_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(true);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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
    }
}

using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Radzen;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class CreateRegisteredCaseTransferRequest : ComponentBase
    {
        #region Parameteres
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
        [Parameter]
        public string? ChamberNameEn { get; set; }
        [Parameter]
        public string? ChamberNameAr { get; set; }
        [Parameter]
        public string? ChamberNumber { get; set; }
        #endregion
        public List<ChamberNumber> ChamberNumbers { get; set; } = new List<ChamberNumber>();
        public List<Chamber> allChambers { get; set; } = new List<Chamber>();
        public CmsRegisteredCaseTransferRequest cMSRegisteredCaseTransferRequest { get; set; } = new CmsRegisteredCaseTransferRequest();
        protected override async Task OnInitializedAsync()
        {
            cMSRegisteredCaseTransferRequest.ChamberFromId = ChamberId;
            cMSRegisteredCaseTransferRequest.ChamberNumberFromId = ChamberNumberId;
            cMSRegisteredCaseTransferRequest.OutcomeId = OutComeId;
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
        #region Redirect and Dialog Events
        protected async Task Form0Submit()
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
                    cMSRegisteredCaseTransferRequest.Id = Guid.NewGuid();
                    cMSRegisteredCaseTransferRequest.ChamberToId = ChamberId;
                    cMSRegisteredCaseTransferRequest.ChamberNumberToId = ChamberNumberId;
                    cMSRegisteredCaseTransferRequest.StatusId = (int)RegisteredCaseTransferRequestStatusEnum.Submitted;
                    var mappedRegisteredCaseTransferRequest = mapper.Map<CmsRegisteredCaseTransferRequestVM>(cMSRegisteredCaseTransferRequest);
                    mappedRegisteredCaseTransferRequest.ChamberFromNameEn = ChamberNameEn;
                    mappedRegisteredCaseTransferRequest.ChamberFromNameAr = ChamberNameAr;
                    mappedRegisteredCaseTransferRequest.ChamberNumberFrom = ChamberNumber;
                    mappedRegisteredCaseTransferRequest.ChamberToNameEn = allChambers.Where(x => x.Id == cMSRegisteredCaseTransferRequest.ChamberToId).FirstOrDefault().Name_En;
                    mappedRegisteredCaseTransferRequest.ChamberToNameAr = allChambers.Where(x => x.Id == cMSRegisteredCaseTransferRequest.ChamberToId).FirstOrDefault().Name_Ar;
                    mappedRegisteredCaseTransferRequest.ChamberNumberTo = ChamberNumbers.Where(x => x.Id == cMSRegisteredCaseTransferRequest.ChamberNumberToId).FirstOrDefault().Number;
                    mappedRegisteredCaseTransferRequest.Remarks = cMSRegisteredCaseTransferRequest.Remarks;
                    mappedRegisteredCaseTransferRequest.CreatedBy = loginState.UserDetail.UserName;
                    mappedRegisteredCaseTransferRequest.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                    if (dataCommunicationService.outcomeHearing == null)
                    {
                        dataCommunicationService.outcomeHearing = new OutcomeHearing();
                    }
                    dataCommunicationService.outcomeHearing.caseTransferRequestsVM.Add(mappedRegisteredCaseTransferRequest);
                    dialogService.Close(true);
                }
            }
            catch (Exception ex)
            {
                spinnerService.Show();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}

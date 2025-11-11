using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<--<History Author = 'Hassan Abbas' Date='2023-11-27' Version="1.0" Branch="master">Outcome Hearing page showing the Outcome details, atachments</History> -->
    public partial class DetailOutcomeHearing : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic OutcomeId { get; set; }
        #endregion

        #region Variables 
        public OutcomeHearingDetailVM outcomeDetail { get; set; } = new OutcomeHearingDetailVM();
        protected List<JudgementVM> Judgements = new List<JudgementVM>();
        protected List<TransferHistoryVM> TransferHistoryVMs = new List<TransferHistoryVM>();
        protected RadzenDataGrid<CaseOutcomePartyLinkHistoryVM> PartiesGrid;
        protected List<CaseOutcomePartyLinkHistoryVM> CasePartyLinks;
      
        #endregion

        #region ON Load Component
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateOutcomeDetails();
            await PopulateJudgements();
            await PopulateTransferHistoryDetail();
            await PopulatePartiesGrid();
            spinnerService.Hide();
        }
        #endregion

        #region Dropdown and Grid Population Events

        protected async Task PopulateOutcomeDetails()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetOutcomeDetail(Guid.Parse(OutcomeId));
                if (response.IsSuccessStatusCode)
                {
                    outcomeDetail = (OutcomeHearingDetailVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task PopulateJudgements()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetJudgementsByOutcome(Guid.Parse(OutcomeId));
                if (response.IsSuccessStatusCode)
                {
                    Judgements = (List<JudgementVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task PopulateTransferHistoryDetail()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetTransferHistoryByOutcome(Guid.Parse(OutcomeId), Guid.Empty);
                if (response.IsSuccessStatusCode)
                {
                    TransferHistoryVMs = (List<TransferHistoryVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task PopulatePartiesGrid()
        {
            var partyResponse = await cmsRegisteredCaseService.GetCMSCaseOutcomePartyHistoryDetailById(Guid.Parse(OutcomeId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CaseOutcomePartyLinkHistoryVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; c.CasePartyAction = (CaseOutcomePartyActionEnum)c.ActionId; return c; }).ToList();
                foreach (var casePartyLink in CasePartyLinks)
                {
                    casePartyLink.CasePartyActionName = translationState.Translate(casePartyLink.CasePartyAction.ToString());
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }
        public void PartyRowRender(RowRenderEventArgs<CaseOutcomePartyLinkHistoryVM> args)
        {
            try
            {
                if (args.Data.AttachmentCount <= 0)
                {
                    args.Attributes.Add("class", "no-party-attachment");
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task BtnBack(MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected void DetailJudgement(JudgementVM args)
        {
            navigationManager.NavigateTo("/judgement-view/" + args.Id);
        }
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion
        
    }
}

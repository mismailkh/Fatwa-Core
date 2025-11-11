using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class DetailJudgement : ComponentBase
	{
        #region Parameter
        [Parameter]
        public dynamic JudgementId { get; set; }
        #endregion

        #region Variables
        public CmsJudgementDetailVM JudgementDetail { get; set; } = new CmsJudgementDetailVM();

        #endregion 

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateJudgementDetails();
            spinnerService.Hide();
        }
        #endregion

        #region Redirect Functions
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected async Task BtnBack(MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
        #endregion

        #region Populate Judgement Detail
        public async Task PopulateJudgementDetails()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetJudgementDetailById(Guid.Parse(JudgementId));
                if (response.IsSuccessStatusCode)
                {
                    if(response.ResultData != null)
                    {
                        JudgementDetail = (CmsJudgementDetailVM)response.ResultData;

                    }
                    else
                    {
                        var newJudgement = dataCommunicationService.outcomeHearing.outcomeJudgement.Where(x => x.Id == Guid.Parse(JudgementId)).FirstOrDefault();
                       JudgementDetail = mapper.Map<CmsJudgementDetailVM>(newJudgement);
                        
                    }

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}


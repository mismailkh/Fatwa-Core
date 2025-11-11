using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class ListExecutionDecisionRequests : ComponentBase
    {
        #region Variables
        protected RadzenDataGrid<CmsJugdmentDecisionVM> grid;
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        public string UserId { get; set; }

        public IList<CmsJugdmentDecisionVM> jugdmentDecisionVMs ;

        #endregion

        #region Full property variable
        IEnumerable<CmsJugdmentDecisionVM> _jugdmentDecisionVMs;
        protected IEnumerable<CmsJugdmentDecisionVM> getjugdmentDecisionVMs
        {
            get
            {
                return _jugdmentDecisionVMs;
            }
            set
            {
                if (!Equals(_jugdmentDecisionVMs, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getjugdmentDecisionVMs", NewValue = value, OldValue = getjugdmentDecisionVMs };
                    _jugdmentDecisionVMs = value;

                    Reload();
                }

            }
        }
        #endregion

        #region ON Initialized
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            UserId = loginState.UserDetail.UserId;
            await Load();
            translationState.TranslateGridFilterLabels(grid);

            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            try
            {
                var responseJudgment = await cmsRegisteredCaseService.GetJudgmentDecisionList(Guid.Parse(UserId), null, null);
                if (responseJudgment.IsSuccessStatusCode)
                {
                    getjugdmentDecisionVMs = (List<CmsJugdmentDecisionVM>)responseJudgment.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(responseJudgment);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Redirect Function
        //<History Author = 'Ijaz Ahmad' Date='2022-12-13' Version="1.0" Branch="master"> Redirect Function </History>

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RequestedDocumentDetails(CmsRequestDocumentsVM args)
        {
            navigationManager.NavigateTo("requested-documents-list-detail/" + args.CaseId);
        }
        #endregion

        #region Grid Buttons
        protected async Task DetailJudgmentDecision(CmsJugdmentDecisionVM args)
        {
            navigationManager.NavigateTo("/judgmentdecision-detail/" + args.Id);
        }
        #endregion

    }
}

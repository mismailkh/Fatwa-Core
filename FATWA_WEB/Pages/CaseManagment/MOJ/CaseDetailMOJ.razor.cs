using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_WEB.Pages.Meet;
using FATWA_WEB.Services.CaseManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    public partial class CaseDetailMOJ : ComponentBase
    {
        #region Parameter 
        [Parameter]
        public dynamic CaseId { get; set; }
        #endregion

        #region Variable
        protected CaseDetailMOJVM caseDetailMOJVM { get; set; } = new CaseDetailMOJVM();
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            await PopulateCaseDetail();
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }



        #endregion

        #region Populate Case Detail 
        private async Task PopulateCaseDetail()
        {
            try
            {
                spinnerService.Show();
                var response = await cmsRegisteredCaseService.GetCaseDetailForMOJ(Guid.Parse(CaseId));
                if (response.IsSuccessStatusCode)
                {
                    caseDetailMOJVM = (CaseDetailMOJVM)response.ResultData;
                    Reload();
                }
                spinnerService.Hide();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Redirect Function
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion
    }
}

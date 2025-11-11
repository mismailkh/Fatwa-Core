using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class RegisteredCaseMetaDataGrid : ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid CaseId { get; set; }
        #endregion

        #region Variables
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM() { IsAssigned = false };
        protected CmsCaseFileDetailVM caseFile { get; set; }
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        #endregion
        protected async Task Load()
        {
            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(CaseId);
            if (result.IsSuccessStatusCode)
            {
                registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                await PopulateCaseFileGrid();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task PopulateCaseFileGrid()
        {
            var response = await cmsCaseFileService.GetCaseFileDetailByIdVM((Guid)registeredCase.FileId);
            if (response.IsSuccessStatusCode)
            {
                caseFile = (CmsCaseFileDetailVM)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
    }
}

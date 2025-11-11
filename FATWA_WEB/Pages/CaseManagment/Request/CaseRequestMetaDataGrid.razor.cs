using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace FATWA_WEB.Pages.CaseManagment.Request
{
    public partial class CaseRequestMetaDataGrid : ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid RequestId { get; set; }
        #endregion

        #region Variables
        protected CaseRequestDetailVM caseRequest { get; set; } = new CaseRequestDetailVM();
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
            var response = await caseRequestService.GetCaseRequestDetailById(RequestId);
            if (response.IsSuccessStatusCode)
            {
                caseRequest = JsonConvert.DeserializeObject<CaseRequestDetailVM>(response.ResultData.ToString());
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
    }
}

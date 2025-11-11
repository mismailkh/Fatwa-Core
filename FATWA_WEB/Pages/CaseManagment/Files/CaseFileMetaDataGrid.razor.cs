using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    public partial class CaseFileMetaDataGrid : ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid FileId { get; set; }
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
            var result = await cmsCaseFileService.GetCaseFileDetailByIdVM(FileId);
            if (result.IsSuccessStatusCode)
            {
                dataCommunicationService.caseFile = (CmsCaseFileDetailVM)result.ResultData;
                await PopulateCaseRequestGrid();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task PopulateCaseRequestGrid()
        {
            var caseRequestResponse = await caseRequestService.GetCaseRequestDetailById(dataCommunicationService.caseFile.RequestId);
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseRequest = JsonConvert.DeserializeObject<CaseRequestDetailVM>(caseRequestResponse.ResultData.ToString());
                dataCommunicationService.caseFile.CaseRequest.Add(caseRequest);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }
    }
}

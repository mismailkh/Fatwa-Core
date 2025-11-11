using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.DS
{
    public partial class DigitalSignatureDocumentHistory : ComponentBase
    {
        [Parameter]
        public int DocumentId { get; set; }
        protected RadzenDataGrid<DsSigningRequestTaskLogVM>? grid = new RadzenDataGrid<DsSigningRequestTaskLogVM>();
        protected RadzenDataGrid<DsSigningRequestTaskLogVM>? grid1 = new RadzenDataGrid<DsSigningRequestTaskLogVM>();
        protected IEnumerable<DsSigningRequestTaskLogVM> FilterDsSigningRequestTaskLog = new List<DsSigningRequestTaskLogVM>();
        protected IEnumerable<DsSigningRequestTaskLogVM> signingHistoryList = new List<DsSigningRequestTaskLogVM>();

        protected override async Task OnInitializedAsync()
        {
            await GetDSDocumentHistory(DocumentId);
            translationState.TranslateGridFilterLabels(grid);
            translationState.TranslateGridFilterLabels(grid1);
        }


        protected async Task GetDSDocumentHistory(int DocumentId)
        {
            var response = await fileUploadService.GetAllTasksForSignature(DocumentId);
            if (response.IsSuccessStatusCode)
            {
                var result = (List<DsSigningRequestTaskLogVM>)response.ResultData;
                FilterDsSigningRequestTaskLog = result.Where(x => x.ReceiverId != null).ToList();
                signingHistoryList = result.Where(x => x.ReceiverId == null).ToList();
            }
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }
        
    }
}

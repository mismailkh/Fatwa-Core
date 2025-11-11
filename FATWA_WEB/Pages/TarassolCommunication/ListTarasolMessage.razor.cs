using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.TarassolCommunication
{
    public partial class ListTarasolMessage : ComponentBase
    {

        protected RadzenDataGrid<CommunicationTarasolRPAPayloadOutput>? communicationPayloadGrid = new RadzenDataGrid<CommunicationTarasolRPAPayloadOutput>();
        protected RadzenDataGrid<CommunicationDocumentPayloadOutput>? documentPayloadGrid = new RadzenDataGrid<CommunicationDocumentPayloadOutput>();

        protected IEnumerable<CommunicationTarasolRPAPayloadOutput> TarasolRPAPayload = new List<CommunicationTarasolRPAPayloadOutput>();
        protected IEnumerable<CommunicationDocumentPayloadOutput> DocumentPayload = new List<CommunicationDocumentPayloadOutput>();

        protected TarasolRPAPayloadWithDocuments tarasolRPAPayloadWithDocuments { get; set; }


        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(communicationPayloadGrid);
            translationState.TranslateGridFilterLabels(documentPayloadGrid);
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            var response = await communicationTarasolService.GetRPAFaultyMessages();
            if (response.IsSuccessStatusCode)
            {
                var tarasolRPAPayloadWithDocuments = (TarasolRPAPayloadWithDocuments)response.ResultData;
                TarasolRPAPayload = tarasolRPAPayloadWithDocuments.CommunicationPayload;
                DocumentPayload = tarasolRPAPayloadWithDocuments.DocumentPayload;
                DocumentPayload = DocumentPayload.Select(x => 
                {
                    x.FileName = x.FileName == null ? translationState.Translate("FileName_missing_key") : x.FileName;
                    return x;
                }).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region OnTab Change
        protected async Task OnTabChange(int index)
        {
            await Task.Delay(100);
            if (index == 0)
            {
            }
            else
            {
            }
            StateHasChanged();
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
        #endregion
    }
}

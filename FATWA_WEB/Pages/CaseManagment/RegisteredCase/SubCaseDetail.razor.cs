using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Data;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Syncfusion.Blazor.PdfViewerServer;
using Syncfusion.Blazor.PdfViewer;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Response;
using Telerik.Blazor.Components.Editor;
using DocumentFormat.OpenXml.Office2010.Word;
using FATWA_DOMAIN.Interfaces;
using DocumentFormat.OpenXml.Bibliography;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.JSInterop;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class SubCaseDetail : ComponentBase
    {

        #region Parameter

        [Parameter]
        public dynamic CaseId { get; set; }

        #endregion

        #region Model full property Instance

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region Legal Legislation Detail View Grid Load Properties Load

        DetailSubCaseVM _detailSubCaseVM;
        protected DetailSubCaseVM detailSubCaseVM
        {
            get
            {
                return _detailSubCaseVM;
            }
            set
            {
                if (!Equals(_detailSubCaseVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "DetailSubCase", NewValue = value, OldValue = _detailSubCaseVM };
                    _detailSubCaseVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {

            await GetSubCaseByIdLoad();

            spinnerService.Hide();
        }

        protected async Task GetSubCaseByIdLoad()
        {
            try
            {
                spinnerService.Show();
                // legislationsVM.LegislationId = Guid.Parse(LegislationId);
                var response = await caseRequestService.GetSubCaseByCaseId(Guid.Parse(CaseId));
                if (response.IsSuccessStatusCode)
                {
                    detailSubCaseVM = (DetailSubCaseVM)response.ResultData;
                }
                await InvokeAsync(StateHasChanged);
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Redirect Button
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
        #endregion

    }
}

using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lms
{
    public partial class DetailStockTakingReport : ComponentBase
    {
        #region Parameter
        [Parameter]
        public string StockTakingId { get; set; }
        #endregion

        #region Variables
        protected RadzenDataGrid<LmsStockTakingBooksReportListVm> StockTakingReportGridRef { get; set; } = new RadzenDataGrid<LmsStockTakingBooksReportListVm>();
        protected LmsStockTakingDetailVM lmsStockTakingDetails { get; set; } = new LmsStockTakingDetailVM();
        protected List<LmsStockTakingBooksReportListVm> lmsStockTakingBooksReport { get; set; } = new List<LmsStockTakingBooksReportListVm>();
        protected List<StockTakingPerformerVm> stockTakingPerformers { get; set; } = new List<StockTakingPerformerVm>();
        #endregion

        #region On Initializated
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            await GetStockTakingDetailById();
            await GetStockTakingReportById();
            await GetPerformersByStockTakingId();
        }

        protected async Task GetStockTakingDetailById()
        {
            try
            {
                var response = await lmsLiteratureService.GetStockTakingDetailById(Guid.Parse(StockTakingId));
                if (response.IsSuccessStatusCode)
                {
                    lmsStockTakingDetails = (LmsStockTakingDetailVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task GetStockTakingReportById()
        {
            try
            {
                var response = await lmsLiteratureService.GetLmsBookStockTakingReportList(Guid.Parse(StockTakingId));
                if (response.IsSuccessStatusCode)
                {
                    lmsStockTakingBooksReport = (List<LmsStockTakingBooksReportListVm>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Get StockTaking Performers by StocktakingId
        protected async Task GetPerformersByStockTakingId()
        {
            try
            {
                var response = await lmsLiteratureService.GetPerformersByStockTakingId(Guid.Parse(StockTakingId));
                if (response.IsSuccessStatusCode)
                {
                    stockTakingPerformers = (List<StockTakingPerformerVm>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #endregion

        #region Option Row
        protected async Task EditOptionRow(LmsStockTakingBooksReportListVm option)
        {
            await StockTakingReportGridRef.EditRow(option);
        }
        protected async Task SaveRow(LmsStockTakingBooksReportListVm remarkValue)
        {

            if (!string.IsNullOrEmpty(remarkValue.Remarks))
            {
                await StockTakingReportGridRef.UpdateRow(remarkValue);
            }
            else
            {
                await StockTakingReportGridRef.UpdateRow(remarkValue);
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

    }
}

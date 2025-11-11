using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lms
{
    public partial class LmsStockTakingHistoryDialog : ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid StockTakingId { get; set; }

        #endregion
        #region Variable Declaration
        public List<LmsStockTakingHistoryVm> lmsstocktakinghistoryList = new List<LmsStockTakingHistoryVm>();
        public RadzenDataGrid<LmsStockTakingHistoryVm> grid0;

        #endregion

        #region Load Component 
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(grid0);
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            try
            {
                var response = await lmsLiteratureService.GetLmsStockTakingHistoryById(StockTakingId);
                if (response.IsSuccessStatusCode)
                {
                    lmsstocktakinghistoryList = (List<LmsStockTakingHistoryVm>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch(Exception ex)
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

       
        #region Dialog Close
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
      
    }
}

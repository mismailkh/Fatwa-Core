using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;


namespace FATWA_WEB.Pages.ServiceRequests.InventoryRequests
{
    public partial class DetailInventoryRequest : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic ServiceRequestId { get; set; }

        #endregion

        #region Variable
        protected RadzenDataGrid<ServiceRequestItemsDetailVM>? grid = new RadzenDataGrid<ServiceRequestItemsDetailVM>();
        protected RadzenDataGrid<RejectReasonVM>? grid1 = new RadzenDataGrid<RejectReasonVM>();
        protected RadzenDataGrid<IssueItemsVM>? grid3 = new RadzenDataGrid<IssueItemsVM>();
        protected ServiceRequestVM ServiceRequestDetail { get; set; } = new ServiceRequestVM();
        protected List<ServiceRequestItemsDetailVM> ServiceRequestItemsList { get; set; } = new List<ServiceRequestItemsDetailVM>();
        protected List<RejectReasonVM> RejectReasonsVM { get; set; } = new List<RejectReasonVM>();
        protected List<IssueItemsVM> IssueItemsList { get; set; } = new List<IssueItemsVM>();

        #endregion

        #region On Initialize
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
            //await GetStoreIdByUserId();
            await GetServiceRequestDetailById();
            await GetInventoryRequestItems();
            await GetRejectReasons();
            await GetIssueItemsById(Guid.Parse(ServiceRequestId));
        }
        #endregion

        #region Functions

        private async Task GetServiceRequestDetailById()
        {
            var response = await serviceRequestSharedService.GetServiceRequestDetailById(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
                ServiceRequestDetail = (ServiceRequestVM)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetInventoryRequestItems()
        {
            var response = await inventoryRequestService.GetInventoryRequestItems(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
                ServiceRequestItemsList = (List<ServiceRequestItemsDetailVM>)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetRejectReasons()
        {
            var response = await inventoryRequestService.GetRejectReasons(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
                RejectReasonsVM = (List<RejectReasonVM>)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetIssueItemsById(Guid id)
        {
            var response = await inventoryRequestService.GetIssueItemsById(id);

            if (response.IsSuccessStatusCode && response.ResultData is not null)
                IssueItemsList = (List<IssueItemsVM>)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        #endregion

        #region Buttons
        protected async Task ServiceRequestItemDetail(ServiceRequestItemsDetailVM args)
        {
            navigationManager.NavigateTo("detail-serviceRequestItem/" + args.ServiceRequestId + "/" + args.ServiceRequestItemId + "/" + ServiceRequestDetail.ServiceRequestStatusId);
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

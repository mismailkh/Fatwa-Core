using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.ServiceRequests.InventoryRequests
{
    public partial class DetailInventoryRequestItem : ComponentBase
	{
		#region Parameter
		[Parameter]
		public dynamic ServiceRequestId { get; set; }
		[Parameter]
		public dynamic ServiceRequestStatusId { get; set; }
		[Parameter]
		public dynamic ServiceRequestItemId { get; set; }
		#endregion

		#region Variables
		public ServiceRequestItemsDetailVM ServiceRequestItemDetailVM = new ServiceRequestItemsDetailVM();
		public RequestDetailVM requestDetail = new RequestDetailVM();
		public StoreDetailVM storeDetailVM = new StoreDetailVM();
		protected RadzenDataGrid<StoreDetailVM>? CaseGrid;
		public List<StoreDetailVM> storedetaillist = new List<StoreDetailVM>();
		protected RequestDetailVM RequestDetails { get; set; } = new RequestDetailVM();
		//private bool IsRejected { get; set; }
		#endregion

		#region On Initiallize
		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();
			await Load();

			spinnerService.Hide();
		}
		#endregion

		#region Load

		private async Task Load()
		{
			spinnerService.Show();
			await GetServiceRequestItemDetailById();
			spinnerService.Hide();
		}



		#endregion

		#region Functions

		private async Task GetServiceRequestItemDetailById()
		{
			try
			{
				var response = await inventoryRequestService.GetInventoryRequestItemDetailById(Guid.Parse(ServiceRequestItemId));

				if (response.IsSuccessStatusCode)
					ServiceRequestItemDetailVM = (ServiceRequestItemsDetailVM)response.ResultData;
				else
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
			catch (Exception)
			{

				throw;
			}
		}
		//protected async Task PopulateStore()
		//{
		//    var User = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
		//    var result = await invInventoryService.GetStoreDetail(Guid.Parse(User.UserId), Guid.Parse(ItemId));
		//    if (result.IsSuccessStatusCode)
		//    {
		//        storedetaillist = (List<StoreDetailVM>)result.ResultData;

		//    }
		//}
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

        #region Buttons

        //protected async Task ApproveServiceRequestItem(ServiceRequestItemsDetailVM args)
        //{
        //    var dialogResponse = await dialogService.OpenAsync<ApproveRequestItem>(
        //       translationState.Translate("Approve_Quantity"),

        //        new Dictionary<string, object>()
        //            {
        //                {"ItemsDetailVM",(args)}
        //            },
        //        new DialogOptions() { CloseDialogOnOverlayClick = true });
        //    if (dialogService != null)
        //        await Load();
        //    StateHasChanged();

        //}

        //protected async Task RejectItemReason(ServiceRequestItemsDetailVM args)
        //{
        //    try
        //    {
        //        var dialogResult = await dialogService.OpenAsync<RequestRejectReason>(
        //       translationState.Translate("Reject_Reason"),

        //        new Dictionary<string, object>()
        //            {
        //                {"ServiceRequestId",(Guid.Parse(ServiceRequestId)) },
        //                { "ServiceRequestItemId", (args.ServiceRequestItemId) },
        //            },
        //        new DialogOptions() { CloseDialogOnOverlayClick = true });
        //        await Load();
        //    }
        //    catch (Exception ex)
        //    {
        //        notificationService.Notify(new NotificationMessage()
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Detail = ex.Message,
        //            Style = "position: fixed !important; left: 0; margin: auto; "
        //        });
        //    }
        //}



        public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		#endregion
	}
}

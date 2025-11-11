using FATWA_DOMAIN.Models.ServiceRequestModels.ComplaintRequestModels;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using FATWA_WEB.Pages.ServiceRequests.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;

namespace FATWA_WEB.Pages.ServiceRequests.ComplaintRequests
{
    public partial class DetailComplaintRequest : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic ServiceRequestId { get; set; }

        #endregion

        #region Constructor
        public DetailComplaintRequest()
        {
            ComplaintRequestDetailVM = new ComplaintRequestDetailVM();
            RequestRemarksDetailVM = new List<RequestRemarksDetailVM>();
        }
        #endregion

        #region Variable
        protected RadzenDataGrid<ServiceRequestItemsDetailVM> grid = new RadzenDataGrid<ServiceRequestItemsDetailVM>();
        protected RadzenDataGrid<RejectReasonVM> grid1 = new RadzenDataGrid<RejectReasonVM>();
        protected RadzenDataGrid<IssueItemsVM> grid3 = new RadzenDataGrid<IssueItemsVM>();
        protected ServiceRequestVM ServiceRequestDetail { get; set; } = new ServiceRequestVM();
        protected List<ServiceRequestItemsDetailVM> ServiceRequestItemsList { get; set; } = new List<ServiceRequestItemsDetailVM>();
        protected List<RejectReasonVM> RejectReasonsVM { get; set; } = new List<RejectReasonVM>();
        protected List<IssueItemsVM> IssueItemsList { get; set; } = new List<IssueItemsVM>();
        protected ComplaintRequestDetailVM ComplaintRequestDetailVM { get; set; }
        protected IEnumerable<RequestRemarksDetailVM> RequestRemarksDetailVM { get; set; }


        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            ServiceRequestId = Guid.Parse(ServiceRequestId);
            await Load();
            await GetComplaintRequestRemarksById();
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            //await GetStoreIdByUserId();
            await GetServiceRequestDetailById();
            await GetComplaintRequestDetailById();

            //await GetRejectReasons();
        }
        #endregion

        #region Functions

        private async Task GetServiceRequestDetailById()
        {
            var response = await serviceRequestSharedService.GetServiceRequestDetailById(ServiceRequestId);

            if (response.IsSuccessStatusCode)
                ServiceRequestDetail = (ServiceRequestVM)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetComplaintRequestDetailById()
        {
            var response = await complaintRequestService.GetComplaintRequestDetailById(ServiceRequestId);

            if (response.IsSuccessStatusCode)
                ComplaintRequestDetailVM = (ComplaintRequestDetailVM)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetComplaintRequestRemarksById()
        {
            var response = await serviceRequestSharedService.GetServiceRequestRemarks(ComplaintRequestDetailVM.Id);

            if (response.IsSuccessStatusCode)
                RequestRemarksDetailVM = (IEnumerable<RequestRemarksDetailVM>)response.ResultData!;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        #endregion

        #region Buttons

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


        #region Close Request
        protected async Task CloseServiceRequest()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                         translationState.Translate("Are_you_sure_you_want_to_close_complaint_request"),
                         translationState.Translate("Confirm"),
                         new ConfirmOptions()
                         {
                             OkButtonText = @translationState.Translate("Yes"),
                             CancelButtonText = @translationState.Translate("No")
                         });

                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        spinnerService.Show();
                        var response = await serviceRequestSharedService.UpdateServiceRequestStatus(ServiceRequestId, (int)ServiceRequestStatusEnum.Closed);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Service_Request_Closed_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Load();
                            spinnerService.Hide();

                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
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

        #region ReOpen Request
        protected async Task ReOpenComplaintRequest()
        {
            try
            {
                var dialogResponse = await dialogService.OpenAsync<RequestDecisionPopUp>(translationState.Translate("Re_Open_Complaint_Request"),
                new Dictionary<string, object>() { { "ReferenceId", ComplaintRequestDetailVM.Id }, { "RemarkType", (int)RemarkTypeEnum.ReOpen }, { "ServiceRequestId", ServiceRequestId } },
                new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = true });
                if (dialogResponse is not null)
                {
                    await Load();
                    await GetComplaintRequestRemarksById();
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
    }
}

using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.ServiceRequest
{
    public partial class ServiceRequestApprovalDeatil : ComponentBase
    {
        #region Parameters

        [Parameter]
        public int Id { get; set; }

        #endregion

        #region Varibles

        protected RadzenDataGrid<ServiceRequestApprovalHistoryVm>? HistoryGrid = new RadzenDataGrid<ServiceRequestApprovalHistoryVm>();
        protected List<ServiceRequestApprovalHistoryVm> ServiceReqApprovalHistory = new List<ServiceRequestApprovalHistoryVm>();
        protected ServiceRequestApprovalDetailVm serviceRequestApprovalDetailVm = new ServiceRequestApprovalDetailVm();
        
        #endregion

        #region On Initialize 
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            spinnerService.Show();
            await GetServiceRequestApprovalById();
            await GetServiceRequestApprovalHistory();
            spinnerService.Hide();
        }
        #endregion

        #region Functions
        protected async Task GetServiceRequestApprovalById()
        {
            var response = await lookupService.GetServiceRequestApprovalDetail(Id);
            if (response.IsSuccessStatusCode)
            {
                serviceRequestApprovalDetailVm = (ServiceRequestApprovalDetailVm)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetServiceRequestApprovalHistory()
        {
            var response = await lookupService.GetServiceRequestApprovalHistory(Id);
            if (response.IsSuccessStatusCode)
            {
                ServiceReqApprovalHistory = (List<ServiceRequestApprovalHistoryVm>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

    }
}

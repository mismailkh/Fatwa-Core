using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;

namespace FATWA_WEB.Pages.CaseManagment.Users
{
    //< History Author = "Ammaar Naveed" Date = "02/10/2024" Version = "1.0" Branch = "master" >Enhanced dialog to show both permanent and termporary delegations.</ History >
    //< History Author = "Ammaar Naveed" Date = "01/07/2024" Version = "1.0" Branch = "master" >Dialog grid implementation for employee leave delegation history.</ History >
    public partial class EmployeeDelegationHistoryDialog : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string UserId { get; set; }
        #endregion

        #region Variables Declaration
        protected List<EmployeeDelegationHistoryVM> EmployeeDelegationHistories;
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            await GetEmployeeLeaveDelegationInformation();
        }
        #endregion

        #region Get Employee Leave Delegation Information
        protected async Task GetEmployeeLeaveDelegationInformation()
        {
            var response = await userService.GetEmployeeDelegationsInformation(UserId);
            if (response.IsSuccessStatusCode)
            {
                EmployeeDelegationHistories = (List<EmployeeDelegationHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

    }
}

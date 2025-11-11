using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class EmployeeAddedDialog : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string? userId { get; set; }
        [Parameter]
        public int EmployeeTypeId { get; set; }
        [Parameter]
        public string EmployeeId { get; set; }
        [Parameter]
        public string? LoginId { get; set; }
        [Parameter]
        public string? UserName { get; set; }
        #endregion

        public async Task NavigateToUsersList()
        {
            navigationManager.NavigateTo("/employee-list");
        }
        protected async Task NavigateToEditPage(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/add-employee/" + userId + "/" + EmployeeTypeId, true);
        }

    }
}

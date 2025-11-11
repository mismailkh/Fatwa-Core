using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_ADMIN.Dialogs.UserManagement
{
    public partial class SaveUserRole : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string UserId { get; set; }
        [Parameter]
        public string LoggedInUser { get; set; }
        [Parameter]
        public string CurrentRoleId { get; set; }
        [Parameter]
        public bool IsUserHasAnyTask { get; set; }
        #endregion

        #region Variables and Fields Declarations
        private UserRoleAssignmentVM UserRoleAssignmentVM { get; set; } = new UserRoleAssignmentVM();
        AddEmployeeVM EmployeeVM = new AddEmployeeVM();
        protected List<Role> SystemRoles { get; set; }
        protected bool isEditEmployeeRole = false;
        #endregion

        #region Oninitialzied/On Component Load
        protected override async Task OnInitializedAsync()
        {
            await PopulateRolesDropdown();
            if (CurrentRoleId != null)
            {
                await GetEmployeeDetails();
            }
        }
        #endregion

        #region Populate Dropdown
        protected async Task PopulateRolesDropdown()
        {
            var response = await userService.GetRoles();
            if (response.IsSuccessStatusCode)
            {
                SystemRoles = (List<Role>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Save User Role
        private async Task SaveEmployeeRole()
        {
            bool? dialogResponse = await dialogService.Confirm(CurrentRoleId == null? translationState.Translate("Confirm_Save_Role"): translationState.Translate("Confirm_Update_Role"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("OK"),
                        CancelButtonText = @translationState.Translate("Cancel")
                    });
            if (dialogResponse == true)
            {
                spinnerService.Show();
                UserRoleAssignmentVM.CreatedBy = LoggedInUser;
                UserRoleAssignmentVM.SelectedRoleId = EmployeeVM.RoleId;
                UserRoleAssignmentVM.SelectedUserId = UserId;
                var response = await userService.SaveEmployeeRole(UserRoleAssignmentVM);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = CurrentRoleId == null ? translationState.Translate("Employee_Role_Added_Successfully") : translationState.Translate("Employee_Role_Updated_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; width: 25% !important;",
                        Duration = 5000
                    });
                    navigationManager.NavigateTo("/employees-list");
                    await InvokeAsync(StateHasChanged);
                    spinnerService.Hide();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }
        #endregion

        #region Get Employee Details
        private async Task GetEmployeeDetails()
        {
            spinnerService.Show();
            isEditEmployeeRole = true;
            var response = await userService.GetEmployeeDetailById(Guid.Parse(UserId));
            if (response.IsSuccessStatusCode)
            {
                EmployeeVM = (AddEmployeeVM)response.ResultData;
                spinnerService.Hide();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Redirect Functions
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/employees-list");
        }
        #endregion

    }
}

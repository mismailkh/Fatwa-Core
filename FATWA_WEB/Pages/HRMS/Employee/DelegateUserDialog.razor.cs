using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class DelegateUserDialog : ComponentBase
    {
        //< History Author = "Ammaar Naveed" Date = "30/09/2024" Version = "1.0" Branch = "master" >Enhanced for deactivated employees delegation</ History >
        //< History Author = "Ammaar Naveed" Date = "23/04/2024" Version = "1.0" Branch = "master" > Dialog for Vice HOS delegation</ History >

        #region Parameters
        [Parameter]
        public int SectorTypeId { get; set; }
        [Parameter]
        public string RoleId { get; set; }
        [Parameter]
        public string SelectedUserId { get; set; }
        [Parameter]
        public string UserName { get; set; }
        [Parameter]
        public int EmployeeTypeId { get; set; }
        [Parameter]
        public int EmployeeStatusId { get; set; }
        [Parameter]
        public int DesignationId { get; set; }
        #endregion

        #region Variables & Fields
        EmployeeLeaveDelegationInformation EmployeeLeaveDelegation = new EmployeeLeaveDelegationInformation();
        protected bool isDateChangeRequested = false;
        EmployeeDelegationVM EmployeeDelegation = new EmployeeDelegationVM();
        protected IEnumerable<EmployeeDelegationVM> DelegatedEmployeesList { get; set; } = new List<EmployeeDelegationVM>();
        protected List<EmployeeDelegationVM> AlternateEmployeesList { get; set; }

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            await GetEmployeesByRoleSectorDesignation();
        }

        #endregion

        #region Employee Existing Leave Check
        //< History Author = "Ammaar Naveed" Date = "29/04/2024" Version = "1.0" Branch = "master" >Check existence of selected employee leave on current date selection before submission.</ History >
        protected async Task CheckExistingLeave()
        {
            if (EmployeeLeaveDelegation.ToDate < EmployeeLeaveDelegation.FromDate )
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("ToDate_NotGreate_FromDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                EmployeeLeaveDelegation.FromDate = null;
                EmployeeLeaveDelegation.ToDate = null;
                return;
            }
            if (EmployeeLeaveDelegation.FromDate != null && EmployeeLeaveDelegation.ToDate != null)
            {
                bool isEmployeeOnLeave = await userService.CheckEmployeeLeaveStatus(SelectedUserId, EmployeeLeaveDelegation.FromDate, EmployeeLeaveDelegation.ToDate);
                if (isEmployeeOnLeave)
                {
                    await dialogService.OpenAsync<EmployeeLeaveStatusDialog>
                   (
                       translationState.Translate("Notice"),
                       new Dictionary<string, object>() { },
                       new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false }                       
                   );
                    EmployeeLeaveDelegation.FromDate = null;
                    EmployeeLeaveDelegation.ToDate = null;
                    AlternateEmployeesList = null;
                }
                if (isEmployeeOnLeave == false)
                {
                   await GetEmployeesByLeaveDelegationInformation();
                }
            }
        }
        #endregion

        #region Populate Delegated User Dropdown
        #region Task Delegation Case
        /*<History Author='Ammaar Naveed' Date='01-10-2024'>Get users by role sector and designation</History>*/

        private async Task GetEmployeesByRoleSectorDesignation()
        {
            var response = await userService.GetEmployeesByRoleSectorAndDesignation(SectorTypeId, RoleId, DesignationId);
            if (response.IsSuccessStatusCode)
            {
                DelegatedEmployeesList = (List<EmployeeDelegationVM>)response.ResultData;
                DelegatedEmployeesList = DelegatedEmployeesList.Where(x => x.UserId != SelectedUserId).ToList();
                if (EmployeeTypeId == (int)EmployeeTypeEnum.Internal)
                    DelegatedEmployeesList = DelegatedEmployeesList.Where(x => x.EmployeeTypeId == (int)EmployeeTypeEnum.Internal);
                else if (EmployeeTypeId == (int)EmployeeTypeEnum.External)
                    DelegatedEmployeesList = DelegatedEmployeesList.Where(x => x.EmployeeTypeId == (int)EmployeeTypeEnum.External);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion
        #region Leave Delegation Case
        protected async Task GetEmployeesByLeaveDelegationInformation()
        {            
            var response = await userService.GetAlternateEmployeesList(SectorTypeId, RoleId, EmployeeLeaveDelegation.FromDate, EmployeeLeaveDelegation.ToDate);
            if (response.IsSuccessStatusCode)

            {
                AlternateEmployeesList = (List<EmployeeDelegationVM>)response.ResultData;
                AlternateEmployeesList = AlternateEmployeesList.Where(user => user.UserId != SelectedUserId && user.IsAbsent != true).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion
        #endregion

        #region Save Employee Delegation Information

        #region Save Employee Leave Delegation Information
        public async Task SaveEmployeeLeaveDelegationInformation()
        {            
            bool? dialogResponse = await dialogService.Confirm(
                 translationState.Translate("Sure_Submit"),
                 translationState.Translate("Confirm"),
                 new ConfirmOptions()
                 {
                     OkButtonText = @translationState.Translate("OK"),
                     CancelButtonText = @translationState.Translate("Cancel")
                 });

            if (dialogResponse == true)
            {
                spinnerService.Show();
                ApiCallResponse response = null;
                EmployeeLeaveDelegation.UserId = SelectedUserId;
                EmployeeLeaveDelegation.CreatedBy = UserName;
                response = await userService.SaveDelegatedEmployee(EmployeeLeaveDelegation);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Delegated_Employee_Added_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    dialogService.Close();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
            else
            {
                dialogService.Close();
                dialogService.Close(1);
            }
        }
        #endregion

        #region Save Employee Task Delegation Information
        public async Task SaveEmployeeTaskDelegationInformation()
        {
            bool? dialogResponse = await dialogService.Confirm(
                 translationState.Translate("Sure_Submit"),
                 translationState.Translate("Confirm"),
                 new ConfirmOptions()
                 {
                     OkButtonText = @translationState.Translate("OK"),
                     CancelButtonText = @translationState.Translate("Cancel")
                 });

            if (dialogResponse == true)
            {
                spinnerService.Show();
                ApiCallResponse response = null;
                EmployeeDelegation.UserId = SelectedUserId;
                EmployeeDelegation.LoggedInUsername = UserName;
                response = await userService.SaveDelegatedEmployeeForDeactivatedEmployee(EmployeeDelegation);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Tasks_Assigned_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    dialogService.Close();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
            else
            {
                dialogService.Close();
                dialogService.Close(1);
            }
        }
        #endregion
        #endregion

        #region Dialog Buttons
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

    }
}

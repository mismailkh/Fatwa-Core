using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using static FATWA_DOMAIN.Enums.UserEnum;
using Radzen;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class DeactivateEmployees : ComponentBase
    {
        #region Paramweters
        [Parameter]
        public string EmployeeId { get; set; }

        [Parameter]
        public int EmployeeStatusId { get; set; }

        [Parameter]
        public string RoleId { get; set; }

        [Parameter]
        public int SectorTypeId { get; set; }
        [Parameter]
        public bool IsUserHasAnyTask { get; set; }

        [Parameter]
        public List<EmployeeVM> SelectedUsersForDeactivation { get; set; }

        [Parameter]
        public int DesignationId { get; set; }

        [Parameter]
        public int EmployeeTypeId { get; set; }
        #endregion

        #region Variables
        public int? EmployeeDeactivationReason = 0;
        public bool IsInternal = false;
        AddEmployeeVM AddEmployeeVM = new AddEmployeeVM()
        {
            UserEmploymentInformation = new UserEmploymentInformation()
        };

        protected DeactivateEmployeesModel deactivateModel { get; set; } = new DeactivateEmployeesModel();
        protected IEnumerable<DeactivateEmployeesVM> DelegatedEmployeesList { get; set; } = new List<DeactivateEmployeesVM>();
        protected List<EmployeeStatus> employeeStatuses { get; set; } = new List<EmployeeStatus>();
        #endregion

        #region On Component Load   

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            var employeeType = SelectedUsersForDeactivation.FirstOrDefault();

            if (employeeType != null)
            {
                if (employeeType.EmployeeTypeId == 1)
                {
                    IsInternal = true;
                    EmployeeDeactivationReason = employeeType.EmployeeStatusId;
                }
                else
                {
                    EmployeeDeactivationReason = employeeType.EmployeeStatusId;
                }
                if (SelectedUsersForDeactivation.Count > 1)
                { EmployeeDeactivationReason = 0; }
            }
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            //await GetEmployeesByRoleAndSector();
            await GetEmployeeStatus();
            deactivateModel.StatusDate = DateTime.Today;
        }
        #endregion

        protected async Task DeactivateEmployee(MouseEventArgs args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    EmployeeStatusId == 1 ? translationState.Translate("Deactivate_Confirm_Message") : translationState.Translate("Activate_Confirm_Message"),
                                 translationState.Translate("Confirm"),
                                 new ConfirmOptions()
                                 {
                                     OkButtonText = @translationState.Translate("OK"),
                                     CancelButtonText = @translationState.Translate("Cancel")
                                 });

                if (dialogResponse == true)
                {
                    if (SelectedUsersForDeactivation != null && SelectedUsersForDeactivation.Count > 0)
                    {
                        spinnerService.Show();
                        var deactivationEmployeesVM = new DeactivateEmployeesVM()
                        {
                            EmployeesList = SelectedUsersForDeactivation.ToList(),
                            DeactivationReason = EmployeeDeactivationReason,
                            StatusReason = deactivateModel.StatusReason,
                            StatusDate = deactivateModel.StatusDate,
                            UserId = deactivateModel.DelegatedEmployeId
                        };
                        string loggedInUser = loginState.UserDetail.Email;
                        var response = await userService.DeactivateEmployee(deactivationEmployeesVM, EmployeeId, loggedInUser);
                        if (response.IsSuccessStatusCode)
                        {
                            spinnerService.Hide();
                            dialogService.Close();
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = deactivateModel.DelegatedEmployeId != null ? translationState.Translate("Employee_Deactivated_Sucess_Tasks_Reassigned") : translationState.Translate("Employee_Status_Updated_Success"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(3000);
                            navigationManager.NavigateTo("/employee-list");
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task FormInvalidSubmit()
        {
            try
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /*private async Task GetEmployeesByRoleAndSector()
        {
            var response = await userService.GetEmployeesByRoleAndSector(SectorTypeId, RoleId, DesignationId);
            if (response.IsSuccessStatusCode)
            {
                DelegatedEmployeesList = (IEnumerable<DeactivateEmployeesVM>)response.ResultData;
                DelegatedEmployeesList = DelegatedEmployeesList.Where(x => x.UserId != EmployeeId).ToList();
                if (EmployeeTypeId == (int)EmployeeTypeEnum.Internal)
                    DelegatedEmployeesList = DelegatedEmployeesList.Where(x => x.EmployeeTypeId == (int)EmployeeTypeEnum.Internal);
                else if(EmployeeTypeId == (int)EmployeeTypeEnum.External)
                    DelegatedEmployeesList = DelegatedEmployeesList.Where(x => x.EmployeeTypeId == (int)EmployeeTypeEnum.External);
            }
        }*/
        private async Task GetEmployeeStatus()
        {
            var response = await userService.GetEmployeeStatus();

            if (response.IsSuccessStatusCode)
            {
                employeeStatuses = (List<EmployeeStatus>)response.ResultData;

                if (IsInternal)
                {
                    if (EmployeeStatusId != 1)
                    {
                        employeeStatuses = employeeStatuses.Where(x => x.Id != EmployeeDeactivationReason && (x.Id == (int)EmployeeStatusEnum.Active)).ToList();
                    }
                    else
                    {
                        employeeStatuses = employeeStatuses.Where(x => x.Id != EmployeeDeactivationReason && x.Id != (int)EmployeeStatusEnum.InActive).ToList();
                    }
                }
                else
                {
                    employeeStatuses = employeeStatuses.Where(x => x.Id != EmployeeDeactivationReason && (x.Id == (int)EmployeeStatusEnum.Active || x.Id == (int)EmployeeStatusEnum.InActive)).ToList();
                }
            }
            deactivateModel.employeeStatuses = employeeStatuses;
        }
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        /*protected async Task DeactivateSelectedEmployees(MouseEventArgs args )
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    AddEmployeeVM.UserEmploymentInformation.EmployeeStatusId == 1? translationState.Translate("Deactivate_Confirm_Message") : translationState.Translate("Activate_Confirm_Message"),
                                 translationState.Translate("Confirm"),
                                 new ConfirmOptions()
                                 {
                                     OkButtonText = @translationState.Translate("OK"),
                                     CancelButtonText = @translationState.Translate("Cancel")
                                 });

                if (dialogResponse == true)
                {
                    if (SelectedUsersForDeactivation != null && SelectedUsersForDeactivation.Count > 0)
                    {
                        spinnerService.Show();
                        var deactivationEmployeesVM = new DeactivateEmployeesVM()
                        {
                            EmployeesList = SelectedUsersForDeactivation.ToList(),
                            DeactivationReason = EmployeeDeactivationReason,
                            StatusReason = deactivateModel.StatusReason,
                            StatusDate = deactivateModel.StatusDate,
                            UserId = deactivateModel.DelegatedEmployeId
                        };
                        string loggedInUser = loginState.UserDetail.Email;
                        var response = await userService.DeactivateEmployee(deactivationEmployeesVM, EmployeeId, loggedInUser);
                        var responseContent = (DeactivateEmployeesResponse)response.ResultData;
                        if (response.IsSuccessStatusCode)
                        {
                            var successMessage = translationState.Translate("Employee status has been updated.");
                            if (!string.IsNullOrEmpty(responseContent.FailedDeactivations.ToString()))
                            {
                                if (SelectedUsersForDeactivation.Count > 1)
                                {
                                    successMessage = $"{successMessage}. {Environment.NewLine} Following users could not be deactivated {responseContent.FailedDeactivations} ";

                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = successMessage,
                                        Style = "position: fixed !important; left: 0; margin: auto; "

                                    });
                                }

                                else
                                {
                                    successMessage = $"User could not be deactivated";

                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = successMessage,
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                }


                            }
                            else
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = successMessage,
                                    Style = "position: fixed !important; left: 0; margin: auto; "

                                });
                            }


                            navigationManager.NavigateTo("/employee-list");
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }*/

    }
}

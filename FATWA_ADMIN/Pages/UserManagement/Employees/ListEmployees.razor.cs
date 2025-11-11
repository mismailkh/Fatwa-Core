using FATWA_ADMIN.Dialogs.UserManagement;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_ADMIN.Pages.UserManagement.Employees
{
    public partial class ListEmployees : ComponentBase
    {
        #region Variables and Fields Declarations
        public bool allowRowSelectOnRowClick = false;
        protected IEnumerable<EmployeesListVM> EmployeesList = new List<EmployeesListVM>();
        protected bool isAdvanceSearchForInternal = true;
        protected string search;
        protected IEnumerable<UserClaimsVM> UMSClaimsList = new List<UserClaimsVM>();
        AddEmployeeVM EmployeeVM = new AddEmployeeVM();
        private UserClaimsVM UserClaimsVM = new UserClaimsVM();
        public IList<EmployeesListVM> SelectedUsers { get; set; } = new List<EmployeesListVM>();
        protected IEnumerable<EmployeesListVM> FilteredEmployeesList { get; set; } = new List<EmployeesListVM>();
        protected bool isVisible { get; set; }
        protected int SelectedParentTabIndex { get; set; } = 0;
        protected int SelectedChildTabIndex { get; set; } = 0;
        protected List<Role> SystemRoles { get; set; }
        private IEnumerable<Designation> DesignationsList { get; set; }
        IEnumerable<OperatingSectorType> OperatingSectorTypesList { get; set; }
        private UserRoleAssignmentVM UserRoleAssignmentVM { get; set; } = new UserRoleAssignmentVM();
        private int? SectorTypeId { get; set; }
        private int? DesignationId { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Grid and Dropdown Variables
        protected RadzenDropDown<int?> DesignationDropdown = new RadzenDropDown<int?>();
        protected RadzenDataGrid<EmployeesListVM>? employeeGrid = new RadzenDataGrid<EmployeesListVM>();
        protected RadzenDropDown<int?> SectorDropdown = new RadzenDropDown<int?>();
        protected RadzenDropDown<string?> RolesDropdown = new RadzenDropDown<string?>();
        private Dictionary<string, bool> UserDigitalSignMapping = new Dictionary<string, bool>();

        #endregion        

        #region Oninitialized/On Component Load
        protected async Task Load()
        {
            await GetEmployeesList();
            await GetUmsClaims();
            await PopulateRolesDropdown();
            await GetDesignationsList();
            await GetSectorTypes();
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(employeeGrid);
            spinnerService.Hide();
        }
        #endregion

        #region Claims Block
        #region Assign Claims To Bulk Users
        private async Task SaveUserClaims()
        {
            bool? dialogResponse = await dialogService.Confirm(translationState.Translate("Update_allow_digital_signature_message"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("OK"),
                        CancelButtonText = @translationState.Translate("Cancel")
                    });
            if (dialogResponse == true)
            {
                spinnerService.Show();
                EmployeeVMForDropDown data = new EmployeeVMForDropDown();
                data.UserIds = UserDigitalSignMapping;
                var response = await userService.AllowBulkDigitalSign(data);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    navigationManager.NavigateTo("/employees-list");
                    await Load();
                    spinnerService.Hide();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            else
            {
                spinnerService.Show();
                await GetEmployeesList();
                spinnerService.Hide();
            }
        }
        #endregion

        #region Assign Claims To Individual User
        protected async Task AssignClaimsToUser(MouseEventArgs args, string userId)
        {
            await dialogService.OpenAsync<AssignUserClaims>(translationState.Translate("Assign_Digital_Signature"),
            new Dictionary<string, object>()
                            {
                             {"LoggedInUser",loginState.Username},
                             {"UserId", userId},
                            },
                            new DialogOptions() { Width = "35% !important", CloseDialogOnOverlayClick = false });
            await Load();
        }
        #endregion
        #endregion

        #region Roles Block
        #region Assign Role To Single User 
        protected async Task AssignRoleToUser(MouseEventArgs args, string userId, string roleId)
        {

            await GetEmployeeDetails(userId);
            await dialogService.OpenAsync<SaveUserRole>(roleId == null ? translationState.Translate("Assign_Role_To_Employee") : translationState.Translate("Update_Employee_Role"),
                            new Dictionary<string, object>()
                            {
                             {"LoggedInUser",loginState.Username},
                             {"UserId", userId},
                             {"CurrentRoleId", roleId},
                             {"IsUserHasAnyTask", EmployeeVM.IsUserHasAnyTask },
                            },
                            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });
            await Load();
        }
        #endregion

        #region Assign Role To Multiple Users
        protected async Task AssignRoleToBulkUsers()
        {
            if (SelectedUsers.Count < 2)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Warning,
                    Detail = translationState.Translate("Please_Select_Users"),
                    Style = "position: fixed !important; left: 0; margin: auto; width: 30% !important;"
                });
            }
            else if (UserRoleAssignmentVM.SelectedRoleId == null)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Warning,
                    Detail = translationState.Translate("Please_Select_Role"),
                    Style = "position: fixed !important; left: 0; margin: auto; width: 25% !important;"
                });
            }
            else
            {
                bool? dialogResponse = await dialogService.Confirm(translationState.Translate("Confirm_Save_Role_Bulk_Users"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("OK"),
                        CancelButtonText = @translationState.Translate("Cancel")
                    });
                if (dialogResponse == true)
                {
                    UserRoleAssignmentVM.SelectedUsersIdsList = SelectedUsers.Select(user => user.UserId).ToList();
                    UserRoleAssignmentVM.ExistingRoleIds = SelectedUsers.Select(user => user.RoleId).ToList();
                    UserRoleAssignmentVM.CreatedBy = loginState.Username;
                    spinnerService.Show();
                    var response = await userService.SaveEmployeeRole(UserRoleAssignmentVM);

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Role_For_Selected_Employees_Updated_Successfully"),
                            Style = (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "position: fixed !important; left: 0; margin: auto; width: 30% !important;" : "position: fixed !important; left: 0; margin: auto; width: 23% !important;"),
                            Duration = 5000
                        });
                        SelectedUsers = new List<EmployeesListVM>();
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
        #endregion
        #endregion

        #region Get Employee List/Details Block
        private async Task GetEmployeesList()
        {
            int EmployeeTypeId = SelectedChildTabIndex == 0 ? (int)EmployeeTypeEnum.Internal : (int)EmployeeTypeEnum.External;
            spinnerService.Show();
            var response = await userService.GetEmployeesListForAdmin(EmployeeTypeId, SectorTypeId, DesignationId);
            if (response.IsSuccessStatusCode)
            {
                EmployeesList = (IEnumerable<EmployeesListVM>)response.ResultData;
                FilteredEmployeesList = EmployeesList;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            spinnerService.Hide();
        }

        #region Get Employee Details
        private async Task<bool> GetEmployeeDetails(string userId)
        {
            var response = await userService.GetEmployeeDetailById(Guid.Parse(userId));
            if (response.IsSuccessStatusCode)
            {
                EmployeeVM = (AddEmployeeVM)response.ResultData;
                return EmployeeVM.IsUserHasAnyTask;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                return false;
            }
        }
        #endregion
        #endregion

        #region Update User Default Receiver Status
        //<History Author='Ammaar Naveed' Date='30-07-2024'>Update default correspondence receiver status.</History>// 
        private async Task UpdateDefaultCorrespondenceReceiverStatus(EmployeesListVM employeeVM)
        {
            try
            {
                if (
                await dialogService.Confirm(translationState.Translate("Update_Default_Receiver_Status"),
                translationState.Translate("Update_Deault_Receiver"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel"),
                    Width = "30% !important",
                    Style = "text-align: center;"
                })
                == true)
                {
                    bool isDefaultCorrespondenceReceiver = employeeVM.IsDefaultCorrespondenceReceiver;
                    string userId = employeeVM.UserId;
                    spinnerService.Show();
                    var response = await userService.UpdateDefaultReceiverStatus(isDefaultCorrespondenceReceiver, userId);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Default_Correspondence_Receiver_Updated"),
                            Style = (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "position: fixed !important; left: 0; margin: auto; width: 34% !important;" : "position: fixed !important; left: 0; margin: auto; width: 20% !important;")
                        });
                        navigationManager.NavigateTo("/employees-list");
                        spinnerService.Hide();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        spinnerService.Hide();
                    }
                }
                else
                {
                    employeeVM.IsDefaultCorrespondenceReceiver = !employeeVM.IsDefaultCorrespondenceReceiver;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Populate Dropdowns
        #region Populate UMS Claims List
        private async Task GetUmsClaims()
        {
            int moduleId = (int)ModuleEnum.DigitalSignature;
            var response = await userService.GetUmsClaimsByModuleId(moduleId);
            if (response.IsSuccessStatusCode)
            {
                UMSClaimsList = ((IEnumerable<UserClaimsVM>)response.ResultData).Where(c => c.ClaimValue != "Permissions.DS.DigitalSignature");
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Populate Sector Types List
        public async Task GetSectorTypes()
        {
            var response = await userService.GetSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                OperatingSectorTypesList = (IEnumerable<OperatingSectorType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Populate Designations List
        private async Task GetDesignationsList()
        {
            var response = await userService.GetDesignationsList();
            if (response.IsSuccessStatusCode)
            {
                DesignationsList = (IEnumerable<Designation>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        #endregion

        #region Populate System Roles List
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

        #endregion

        #region Grid Search
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {
                        FilteredEmployeesList = await gridSearchExtension.Filter(EmployeesList, new Query()
                        {
                            Filter = $@"i => (i.EmployeeNameEn != null && i.EmployeeNameEn.ToLower().Contains(@0)) || (i.RoleNameEn != null && i.RoleNameEn.ToLower().Contains(@1)) || (i.EmployeeId != null && i.EmployeeId.ToLower().Contains(@2)) || (i.SectorTypeNameEn != null && i.SectorTypeNameEn.ToLower().Contains(@3))|| (i.DesignationEn != null && i.DesignationEn.ToLower().Contains(@4))",
                            FilterParameters = new object[] { search, search, search, search, search }
                        });
                    }
                    else
                    {
                        FilteredEmployeesList = await gridSearchExtension.Filter(EmployeesList, new Query()
                        {
                            Filter = $@"i => (i.EmployeeNameAr != null && i.EmployeeNameAr.ToLower().Contains(@0)) || (i.RoleNameAr != null && i.RoleNameAr.ToLower().Contains(@1)) || (i.EmployeeId != null && i.EmployeeId.ToLower().Contains(@2)) || (i.SectorTypeNameAr != null && i.SectorTypeNameAr.ToLower().Contains(@3))|| (i.DesignationEn != null && i.DesignationEn.ToLower().Contains(@4))",
                            FilterParameters = new object[] { search, search, search, search, search }
                        });
                    }
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        #region OnTab Change
        protected async Task OnParentTabChange(int index)
        {
            if (index == SelectedParentTabIndex)
            {
                return;
            }

            SelectedParentTabIndex = index;

            employeeGrid.Reset(true, true);
            SectorDropdown.Reset();
            DesignationDropdown.Reset();

            await Task.Delay(100);

            SelectedUsers = new List<EmployeesListVM>();
            isAdvanceSearchForInternal = (index == 0);

            StateHasChanged();
        }
        protected async Task OnChildTabChange(int index)
        {
            if (index == SelectedChildTabIndex)
            {
                return;
            }

            SelectedChildTabIndex = index;

            employeeGrid.Reset(true, true);
            SectorDropdown.Reset();
            DesignationDropdown.Reset();

            if (UserRoleAssignmentVM.SelectedRoleId != null)
            {
                RolesDropdown.Reset();
            }

            await Task.Delay(100);

            SelectedUsers = new List<EmployeesListVM>();
            isAdvanceSearchForInternal = (index == 0);

            await GetEmployeesList();

            StateHasChanged();
        }
        #endregion

        #region Redirect Functions

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Functions
        void TogglePageClaims(bool isChecked)
        {
            var currentPageData = employeeGrid.PagedView.ToList();
            foreach (var employee in currentPageData)
            {
                employee.AllowDigitalSign = isChecked;
                if (isChecked)
                {
                    if (!SelectedUsers.Contains(employee))
                    {
                        SelectedUsers.Add(employee);
                    }
                }
                else
                {
                    if (SelectedUsers.Contains(employee))
                    {
                        SelectedUsers.Remove(employee);
                    }
                }
                if (UserDigitalSignMapping.ContainsKey(employee.UserId))
                {
                    UserDigitalSignMapping[employee.UserId] = isChecked;
                }
                else
                {
                    UserDigitalSignMapping.Add(employee.UserId, isChecked);
                }
            }
            employeeGrid.Reload();
        }
        void ToggleRowSelection(bool isChecked, EmployeesListVM data)
        {
            if (UserDigitalSignMapping.ContainsKey(data.UserId))
            {
                UserDigitalSignMapping[data.UserId] = isChecked;
            }
            else
            {
                UserDigitalSignMapping.Add(data.UserId, isChecked);
            }
            /*  if (isChecked)
              {
                  //SelectedUsers.Add(data);
                  employeeGrid.SelectRow(data);
              }
              else
              {
                  //SelectedUsers.Remove(data);
                  employeeGrid.SelectRow(data);
              }*/
            employeeGrid.Reload();
        }
        #endregion
    }
}

using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.UserEnum;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class ListEmployee : ComponentBase
    {
        #region Varriables Declaration
        public bool allowRowSelectOnRowClick = false;
        public IList<EmployeeVM> SelectUsers = new List<EmployeeVM>();
        public IList<EmployeeVM> SelectUsersExternal = new List<EmployeeVM>();
        protected RadzenDataGrid<EmployeeVM>? employeeGrid = new RadzenDataGrid<EmployeeVM>();
        protected IEnumerable<EmployeeVM> getUsersResult = new List<EmployeeVM>();
        protected bool isAdvanceSearchForInternal = true;
        protected string search { get; set; }
        public List<Company> Companies { get; set; } = new List<Company>();
        public List<Department> Departments { get; set; } = new List<Department>();
        public List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        public IEnumerable<Designation> Designations { get; set; }
        public List<EmployeeStatus> EmployeeStatuses { get; set; } = new List<EmployeeStatus>();
        protected UserListAdvanceSearchVM advanceSearchVM { get; set; } = new UserListAdvanceSearchVM();
        protected IEnumerable<EmployeeVM> FilteredEmployeesList { get; set; }
        protected bool isVisible { get; set; }
        protected bool Keywords { get; set; }
        protected int selectedTabIndex { get; set; } = 0;
        protected int TabIndex { get; set; } = 0;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => employeeGrid.CurrentPage + 1;
        private int CurrentPageSize => employeeGrid.PageSize;

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(employeeGrid);
            await SectorTypesList();
            await CompanyList();
            await DesignationList();
            await EmployeeStatus();
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data
        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        employeeGrid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();

                    if (TabIndex == 0)
                        advanceSearchVM.EmployeeTypeId = (int)EmployeeTypeEnum.Internal;
                    else
                        advanceSearchVM.EmployeeTypeId = (int)EmployeeTypeEnum.External;
                    var response = await userService.GetEmployeeList(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredEmployeesList = getUsersResult = (IEnumerable<EmployeeVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredEmployeesList = await gridSearchExtension.Sort(FilteredEmployeesList, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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
            spinnerService.Hide();
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getUsersResult.Any() ? (getUsersResult.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = getUsersResult.Any() ? (getUsersResult.First().TotalCount) / (employeeGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    employeeGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchVM.PageNumber == 1 || (advanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchVM.PageNumber = CurrentPage;
            advanceSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSort(DataGridColumnSortEventArgs<EmployeeVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredEmployeesList = await gridSearchExtension.Sort(FilteredEmployeesList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Grid Search
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                FilteredEmployeesList = await gridSearchExtension.Filter(getUsersResult, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.EmployeeNameEn != null && i.EmployeeNameEn.ToLower().Contains(@0)) 
                            || (i.DesignationEn != null && i.DesignationEn.ToLower().Contains(@1)) 
                            || (i.Email != null && i.Email.ToLower().Contains(@2)) 
                            || (i.ADUserName != null && i.ADUserName.ToLower().Contains(@3)) 
                            || (i.CivilId != null && i.CivilId.ToLower().Contains(@4)) 
                            || (i.SectorTypeNameEn != null && i.SectorTypeNameEn.ToLower().Contains(@5))
                            || (i.AdUserName != null && i.AdUserName.ToLower().Contains(@6))
                            || (i.EmployeeId != null && i.EmployeeId.ToLower().Contains(@7))
                            || (i.StatusEn != null && i.StatusEn.ToLower().Contains(@8))"

                    : $@"i => (i.EmployeeNameAr != null && i.EmployeeNameAr.ToLower().Contains(@0)) 
                            || (i.DesignationAr != null && i.DesignationAr.ToLower().Contains(@1)) 
                            || (i.Email != null && i.Email.ToLower().Contains(@2)) 
                            || (i.ADUserName != null && i.ADUserName.ToLower().Contains(@3)) 
                            || (i.CivilId != null && i.CivilId.ToLower().Contains(@4)) 
                            || (i.SectorTypeNameAr != null && i.SectorTypeNameAr.ToLower().Contains(@5))
                            || (i.AdUserName != null && i.AdUserName.ToLower().Contains(@6))
                            || (i.EmployeeId != null && i.EmployeeId.ToLower().Contains(@7))
                            || (i.StatusAr != null && i.StatusAr.ToLower().Contains(@8))",

                    FilterParameters = new object[] { search, search, search, search, search, search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredEmployeesList = await gridSearchExtension.Sort(FilteredEmployeesList, ColumnName, SortOrder);
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

        #region Grid Navigations
        protected async Task AddEmployee(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<AddUserType>
                  (
                      translationState.Translate("Add_Employee_Type"),
                      null,
                      new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                  );
            if (dialogResult != null)
            {
                navigationManager.NavigateTo("/add-employee/" + dialogResult);
            }
        }
        protected async Task EditEmployee(MouseEventArgs args, string userId, int EmployeeType)
        {
            navigationManager.NavigateTo("/add-employee/" + userId + "/" + EmployeeType);
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected async Task ViewEmployeeDetails(MouseEventArgs args, EmployeeVM employee)
        {
            navigationManager.NavigateTo("/employee-view/" + employee.UserId);

        }
        #endregion        

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {

            if (string.IsNullOrWhiteSpace(advanceSearchVM.CivilId)
                 && !advanceSearchVM.SectorId.HasValue && string.IsNullOrWhiteSpace(advanceSearchVM.Name)
                 && string.IsNullOrEmpty(advanceSearchVM.PassportNumber) && !advanceSearchVM.CompanyId.HasValue
                 && !advanceSearchVM.EmployeeStatusId.HasValue && !advanceSearchVM.DesignationId.HasValue) { }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (employeeGrid.CurrentPage > 0)
                    await employeeGrid.FirstPage();
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await employeeGrid.Reload();
                }
            }
        }
        public async void ResetForm()
        {

            advanceSearchVM = new UserListAdvanceSearchVM { PageSize = employeeGrid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            employeeGrid.Reset();
            await employeeGrid.Reload();
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        #region Advance Search Dropdowns
        protected async Task CompanyList()
        {
            var res = await userService.GetCompanies();
            if (res.IsSuccessStatusCode)
            {
                Companies = (List<Company>)res.ResultData;
            }
        }

        protected async Task DepartmentsList()
        {
            var res = await userService.DepartmentList();
            if (res.IsSuccessStatusCode)
            {
                Departments = (List<Department>)res.ResultData;
            }
        }
        protected async Task SectorTypesList()
        {
            var res = await lookupService.GetOperatingSectorTypes();
            if (res.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)res.ResultData;
            }
        }
        protected async Task DesignationList()
        {
            var res = await lookupService.GetDesignationList();
            if (res.IsSuccessStatusCode)
            {
                Designations = (IEnumerable<Designation>)res.ResultData;
            }
        }
        protected async Task EmployeeStatus()
        {
            var res = await userService.GetEmployeeStatus();
            if (res.IsSuccessStatusCode)
            {
                EmployeeStatuses = (List<EmployeeStatus>)res.ResultData;
            }
        }

        #endregion

        #endregion

        #region Tab Change Event
        protected async Task OnTabChange(int index)
        {
            if (index == selectedTabIndex) { return; }
            search = ColumnName = string.Empty;
            TabIndex = index;
            advanceSearchVM = new UserListAdvanceSearchVM { PageSize = employeeGrid.PageSize };
            isVisible = Keywords = false;
            await Task.Delay(100);
            search = string.Empty;

            if (index == 0)
            {
                employeeGrid.Reset();
                await employeeGrid.Reload();
                SelectUsersExternal = new List<EmployeeVM>();
                isAdvanceSearchForInternal = true;
            }
            else
            {
                employeeGrid.Reset();
                await employeeGrid.Reload();
                SelectUsers = new List<EmployeeVM>();
                isAdvanceSearchForInternal = false;
            }
            StateHasChanged();
        }
        #endregion

        #region Reset User Password
        public async Task resetPassword(MouseEventArgs args, string? Email, string? FullNameEn, string? FullNameAr, int EmployeeType, string? AdUserName)
        {
            if (Email != null)
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("reset_password_for") + " " +
                    (Thread.CurrentThread.CurrentCulture.Name == "en-US" ? FullNameEn : FullNameAr),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                bool IsTemporaryPassword = false;
                if (EmployeeType == (int)EmployeeTypeEnum.External)
                {
                    IsTemporaryPassword = true;
                }
                if (dialogResponse == true)
                {
                    var result = await dialogService.OpenAsync<ResetPassword>
                         (IsTemporaryPassword ? translationState.Translate("Set_Temporary_Password") : translationState.Translate("Reset_Password"),
                         new Dictionary<string, dynamic>
                         {
                              { "IsTemporaryPassword",IsTemporaryPassword},
                         },
                         new DialogOptions() { Width = "31% !important", CloseDialogOnOverlayClick = false });
                    if (result != null)
                    {
                        ResetPasswordVM ResetPasswordBody = new ResetPasswordVM
                        {
                            Email = Email,
                            NewPassword = result.Password,
                            CreatedBy = loginState.Username,
                            EmployeeType = EmployeeType,
                            AdUserName = AdUserName
                        };
                        var responsePasswordChange = await userService.ResetUserPassword(ResetPasswordBody);
                        if (responsePasswordChange.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Password_success_message") + " " + (Thread.CurrentThread.CurrentCulture.Name == "en-US" ? FullNameEn : FullNameAr),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                            await Task.Delay(500);
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(responsePasswordChange);
                        }
                    }
                    else
                    {
                        dialogService.Close();
                    }

                }
                else
                {
                    dialogService.Close();
                }
            }

        }
        #endregion

        #region Employee Leave Delegation
        //<History Author = 'Ammaar Naveed' Date='2024-04-29' Version="1.0" Branch="master">Delegate an alternate Vice HOS -> Dialog</History>
        protected async Task DelegateEmployeeDialog(MouseEventArgs args, int? SectorTypeId, string RoleId, string SelectedUserId, int EmployeeTypeId, int? EmployeeStatusId, int? DesignationId)
        {
            try
            {
                await dialogService.OpenAsync<DelegateUserDialog>
                  (
                      translationState.Translate("Delegate_An_Employee"),
                      new Dictionary<string, object>()
                           {
                             {"SectorTypeId",SectorTypeId },
                             {"RoleId",RoleId },
                             {"SelectedUserId",SelectedUserId },
                             {"Username",loginState.Username },
                             {"EmployeeTypeId",EmployeeTypeId },
                             {"EmployeeStatusId",EmployeeStatusId },
                             {"DesignationId",DesignationId },
                           },
                      new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                  );
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
        #endregion

        #region Employees Bulk Import
        protected async Task BulkImport()
        {
            var dialogResult = await dialogService.OpenAsync<ImportBulkEmployee>
            (
                translationState.Translate("Import_Bulk_Employee"),
                null,
                new DialogOptions() { Width = "40% !important", CloseDialogOnOverlayClick = true }
            );
            await employeeGrid.Reload();
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
    }
}

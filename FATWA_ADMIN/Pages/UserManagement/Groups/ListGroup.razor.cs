using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.UserManagement.Groups
{
    public partial class ListGroup : ComponentBase
    {
        #region Variable Declaration
        private Timer debouncer;
        private const int debouncerDelay = 500;
        protected bool isLoading { get; set; }
        private bool isVisible { get; set; }
        protected bool Keywords = false;
        protected UserListAdvanceSearchVM advanceSearchVM = new UserListAdvanceSearchVM();
        IEnumerable<OperatingSectorType> OperatingSectorTypes { get; set; }
        IEnumerable<EmployeeType> EmployeeTypes { get; set; }
        IEnumerable<Role> UserRoles { get; set; }
        protected IEnumerable<EmployeesListDropdownVM> EmployeesList = new List<EmployeesListDropdownVM>();
        protected string UserId { get; set; }
        private bool IsSearchDisable { get; set; }
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
        #endregion

        #region Radzen DataGrid Variables and Settings

        protected RadzenDataGrid<UserGroupListVM>? grid;
        protected string search { get; set; }
        protected int Count { get; set; }
        protected IEnumerable<UserGroupListVM> getUserGroupListVM = new List<UserGroupListVM>();
        protected IEnumerable<UserGroupListVM> FilterUserGroupListVM = new List<UserGroupListVM>();

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            await PopulateSectorTypeDropdown();
            await PopulateEmployeeTypesDropdown();
            await PopulateRolesDropdown();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            var response = await groupService.GetGroupDetails(advanceSearchVM);
            if (response.IsSuccessStatusCode)
            {
                FilterUserGroupListVM = getUserGroupListVM = (IEnumerable<UserGroupListVM>)response.ResultData;
                Count = FilterUserGroupListVM.Count();
                isLoading = false;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    isLoading = true;

                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilterUserGroupListVM = await gridSearchExtension.Filter(getUserGroupListVM, new Query()
                    {
                        Filter = $@"i => (i.GroupNameEn != null && i.GroupNameEn.ToLower().Contains(@0)) 
                    || (i.DescriptionEn != null && i.DescriptionEn.ToLower().Contains(@0))
                    || (i.GroupNameAr != null && i.GroupNameAr.ToLower().Contains(@0))
                    || (i.DescriptionAr != null && i.DescriptionAr.ToLower().Contains(@0))
                    || (i.GroupTypeNameAr != null && i.GroupTypeNameAr.ToLower().Contains(@0))
                    || (i.GroupTypeNameEn != null && i.GroupTypeNameEn.ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                    isLoading = false;

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

        #region Add User Group
        protected async Task ButtonAddClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/save-user-group");
        }

        protected async Task GridEditButtonClick(MouseEventArgs args, UserGroupListVM data)
        {
            navigationManager.NavigateTo("/save-user-group/" + data.GroupId + "/" + false);
        }

        protected async Task GridViewListButtonClick(MouseEventArgs args, UserGroupListVM data)
        {
            navigationManager.NavigateTo("/save-user-group/" + data.GroupId + "/" + true);
        }
        #endregion

        #region Populate Dropdowns
        //<History Author = 'Ammaar Naveed' Date='2024-10-16' Version="1.0" Branch="master">Get operating sector types.</History>
        public async Task PopulateSectorTypeDropdown()
        {
            var response = await userService.GetSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                OperatingSectorTypes = (IEnumerable<OperatingSectorType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task PopulateEmployeeTypesDropdown()
        {
            var response = await userService.GetEmployeeTypes();
            if (response.IsSuccessStatusCode)
            {
                EmployeeTypes = (IEnumerable<EmployeeType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateRolesDropdown()
        {
            var response = await userService.GetRoles();
            if (response.IsSuccessStatusCode)
            {
                UserRoles = (List<Role>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        private async Task PopulateEmployeesListDropdown()
        {
            var response = await userService.GetEmployeesListForUserGroup(advanceSearchVM);
            if (response.IsSuccessStatusCode)
            {
                EmployeesList = (IEnumerable<EmployeesListDropdownVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Advance Search
        //<History Author = 'Ammaar Naveed' Date='2024-10-16' Version="1.0" Branch="master">Advance search for group search by user.</History>
        protected async Task SubmitAdvanceSearch()
        {
            if (!string.IsNullOrEmpty(UserId) || !string.IsNullOrEmpty(advanceSearchVM.Name))
            {
                spinnerService.Show();
                Keywords = true;
                await Load();
                StateHasChanged();
                spinnerService.Hide();
            }
        }
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (UserId == null && string.IsNullOrWhiteSpace(advanceSearchVM.Name))
            {
                IsSearchDisable = true;
            }
            if (!isVisible)
            {
                ResetForm();
            }
        }
        public async void ResetForm()
        {
            spinnerService.Show();
            UserId = null;
            advanceSearchVM = new UserListAdvanceSearchVM();
            Keywords = false;
            await Load();
            StateHasChanged();
            spinnerService.Hide();
        }
        #endregion

        #region On input Search Value
        protected void OnInputSearchValue(ChangeEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value.ToString()))
                IsSearchDisable = true;
            else
                IsSearchDisable = false;

            if(!string.IsNullOrEmpty(UserId))
                IsSearchDisable = false;

        }
        #endregion
        #region On Change User
        protected async Task OnChangeUser(object args)
        {
            if (args == null)
                IsSearchDisable = true;
            else
                IsSearchDisable = false;

            if (!string.IsNullOrEmpty(advanceSearchVM.Name))
                IsSearchDisable = false;
        }
        #endregion
    }
}

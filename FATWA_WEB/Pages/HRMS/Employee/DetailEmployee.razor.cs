using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class DetailEmployee : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string? Id { get; set; }
        #endregion
        
        #region Variables
        AddEmployeeVM EmployeeVM = new AddEmployeeVM
        {
            UserId = Guid.NewGuid(),
            userPersonalInformation = new UserPersonalInformation()
            {
                Nationality = new Nationality(),
                Gender = new Gender(),
            },
            UserEmploymentInformation = new UserEmploymentInformation()
            {
                Designation = new Designation(),
                SectorType = new OperatingSectorType(),
            },
            UserEducationalInformation = new List<UserEducationalInformation>(),
            UserWorkExperiences = new List<UserWorkExperience>(),
            userTrainingAttendeds = new List<UserTrainingAttended>(),
            UserContactInformationList = new List<UserContactInformation>()
        };
        bool RefreshGrid = false;
        protected IEnumerable<GroupTypeWebSystemVM> groupAccessTypes { get; set; }
        protected GroupTypeWebSystemVM groupAccessType { get; set; }
        #endregion

        #region On Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            var response = await userService.GetEmployeeDetailById(Guid.Parse(Id));
            if (response.IsSuccessStatusCode)
            {
                EmployeeVM = (AddEmployeeVM)response.ResultData;
                await PopulateGroupTypes(EmployeeVM.GroupTypeId);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            await GetEmployeePasswordHistory();
            await GetEmployeeActivities();
            await GetEmployeeLeaveDelegationInformation();
        }


        #endregion

        private async Task PopulateGroupTypes(int GroupTypeId)
        {
            var response = await groupService.GetGroupAccessTypes();
            if (response.IsSuccessStatusCode)
            {
                groupAccessTypes = (IEnumerable<GroupTypeWebSystemVM>)response.ResultData;
                groupAccessType = groupAccessTypes.Where(x => x.GroupTypeId == GroupTypeId).FirstOrDefault();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        #region Get Employee Password History
        protected IEnumerable<UserPasswordHistory> getEmployeePasswordHistory;
        protected async Task GetEmployeePasswordHistory()
        {
            var response = await userService.GetEmployeePasswordHistory(Id);
            if (response.IsSuccessStatusCode)
            {
                getEmployeePasswordHistory = (IEnumerable<UserPasswordHistory>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Get Employee Login/Logout Activity
        protected IEnumerable<UserActivity> UserActivities;
        protected async Task GetEmployeeActivities()
        {
            var response = await userService.GetEmployeeActivities(Id);
            if (response.IsSuccessStatusCode)
            {
                UserActivities = (IEnumerable<UserActivity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Get Employee Leave Delegation Information
        protected List<EmployeeDelegationHistoryVM> EmployeeLeaveDelegationRecords;
        protected async Task GetEmployeeLeaveDelegationInformation()
        {
            var response = await userService.GetEmployeeDelegationsInformation(Id);
            if (response.IsSuccessStatusCode)
            {
                EmployeeLeaveDelegationRecords = (List<EmployeeDelegationHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Redirect Back
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }

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

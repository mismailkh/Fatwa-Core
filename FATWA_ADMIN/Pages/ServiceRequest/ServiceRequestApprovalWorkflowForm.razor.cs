using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_ADMIN.Pages.ServiceRequest
{
    public partial class ServiceRequestApprovalWorkflowForm : ComponentBase
    {
        #region Parametes

        [Parameter]
        public int Id { get; set; }

        #endregion

        #region Variables
        int ApprovalNo = 0;
        protected bool refreshApprovals { get; set; }
        public int LevelOfApproval { get; set; }
        public List<ServiceRequestType> ServiceRequestTypes { get; set; }
        public List<DepartmentDetailVM> Departments { get; set; }
        public List<OperatingSectorType> OperatingSectors { get; set; }
        public List<SectorRolesVM> Roles { get; set; }

        List<Guid> rolesIds = new List<Guid>();
        protected Dictionary<int, Guid> SelectedSectorAndRole = new Dictionary<int, Guid>();

        protected ServiceRequestFinalApprovalVM serviceRequestApprovalVm = new ServiceRequestFinalApprovalVM();
        protected ServiceRequestApprovalDetailVm serviceRequestApprovalDetailVm = new ServiceRequestApprovalDetailVm();

        #endregion

        #region Component OnLoad
        protected override async void OnInitialized()
        {
            spinnerService.Show();
            await Load();
            StateHasChanged();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            await GetServiceRequestTypes();
            await GetDepartmentList();
            if (Id > 0)
            {
                await GetServiceRequestApprovalById();
            }
        }
        #endregion

        #region Functions

        public async Task GetServiceRequestTypes()
        {
            var response = await serviceRequestService.GetServiceRequestTypes();
            if (response.IsSuccessStatusCode)
            {
                ServiceRequestTypes = (List<ServiceRequestType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetDepartmentList()
        {
            var response = await lookupService.GetDepartmentList();
            if (response.IsSuccessStatusCode)
            {
                Departments = (List<DepartmentDetailVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetOperatingSectorsByDepartmentId()
        {
            var response = await lookupService.GetOperatingSectorsByDepartmentId(serviceRequestApprovalVm.DepartmentId);
            if (response.IsSuccessStatusCode)
            {
                OperatingSectors = (List<OperatingSectorType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetRolesBySectorTypeId()
        {
            if (serviceRequestApprovalVm.SectorIds is null)
            {
                Roles = null;
                return;
            }
            // var response = await lookupService.GetRolesBySectorIds(serviceRequestApproval.SectorIds);
            var response = await lookupService.GetRolesBySectorTypeIds(serviceRequestApprovalVm.SectorIds);
            if (response.IsSuccessStatusCode)
            {
                Roles = (List<SectorRolesVM>)response.ResultData;
                // Roles = (List<Role>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task OnNumberOfApprovalChange()
        {
            SelectedSectorAndRole = new Dictionary<int, Guid>();
            refreshApprovals = false;

            for (int i = 1; i <= serviceRequestApprovalVm.NoOfApproval; i++)
            {
                SelectedSectorAndRole.Add(i, Guid.Empty);
            }
            refreshApprovals = true;

            if (serviceRequestApprovalVm.NoOfApproval < ApprovalNo && rolesIds.Count() == ApprovalNo)
            {
                rolesIds.RemoveAt(rolesIds.Count - 1);
            }
            ApprovalNo = serviceRequestApprovalVm.NoOfApproval;
        }

        protected async Task OnSectorAndRoleSelection(Guid Id, int index)
        {
            SelectedSectorAndRole[index] = Id;
            rolesIds.Add(Id);
        }

        protected async Task GetServiceRequestApprovalById()
        {
            var response = await lookupService.GetServiceRequestApprovalDetail(Id);
            if (response.IsSuccessStatusCode)
            {
                serviceRequestApprovalDetailVm = (ServiceRequestApprovalDetailVm)response.ResultData;
                serviceRequestApprovalVm.ApprovalId = serviceRequestApprovalDetailVm.Id;
                serviceRequestApprovalVm.ServiceRequestTypesId = new List<int>() { serviceRequestApprovalDetailVm.ServiceRequestTypeId };
                serviceRequestApprovalVm.DepartmentId = serviceRequestApprovalDetailVm.DepartmentId;
                serviceRequestApprovalVm.SectorIds = serviceRequestApprovalDetailVm.SectorIds.Split(',').Select(int.Parse).ToList();
                serviceRequestApprovalVm.NoOfApproval = serviceRequestApprovalDetailVm.NoOfApprovals;
                serviceRequestApprovalVm.SelectedSectorAndRoles = serviceRequestApprovalDetailVm.RoleIds.Split(",");

                await GetOperatingSectorsByDepartmentId();
                await GetRolesBySectorTypeId();
                refreshApprovals = true;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task AddServiceRequestFlow()
        {
            List<string> selectedRoles = new();
            foreach (var id in rolesIds)
            {
                SectorRolesVM sectorAndRole = Roles.Where(x => x.Id == id).FirstOrDefault();
                selectedRoles.Add($"{sectorAndRole.SectorId}, {sectorAndRole.RoleId}");
            }
            serviceRequestApprovalVm.SelectedSectorAndRoles = selectedRoles;
            var response = await lookupService.AddServiceRequestFinalApproval(serviceRequestApprovalVm);
            if (response.IsSuccessStatusCode)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Service_Request_Flow_Added_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task UpdateServiceRequestFlow()
        {
            List<string> selectedRoles = new();
            foreach (var id in rolesIds)
            {
                SectorRolesVM sectorAndRole = Roles.Where(x => x.Id == id).FirstOrDefault();
                selectedRoles.Add($"{sectorAndRole.SectorId}, {sectorAndRole.RoleId}");
            }
            serviceRequestApprovalVm.SelectedSectorAndRoles = selectedRoles;
            var response = await lookupService.UpdateServiceRequestApproval(serviceRequestApprovalVm);
            if (response.IsSuccessStatusCode)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Service_Request_Flow_Added_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Form Submit
        protected async Task Form0Submit()
        {
            try
            {
                var User = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                bool? dialogResponse = await dialogService.Confirm(
             translationState.Translate("Sure_Submit"),
             translationState.Translate("Confirm"),
             new ConfirmOptions()
             {
                 OkButtonText = @translationState.Translate("OK"),
                 CancelButtonText = @translationState.Translate("Cancel")
             });
                spinnerService.Show();
                if (dialogResponse == true)
                {
                    if (Id > 0)
                    {
                        await UpdateServiceRequestFlow();
                    }
                    else
                    {
                        await AddServiceRequestFlow();
                    }
                    navigationManager.NavigateTo("/service-request-flow-list");
                }
                spinnerService.Hide();
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Unable_to_Reject"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Cancel Form
        protected async Task CancelForm()
        {
            navigationManager.NavigateTo("/Service-Request-Flow-List");
        }

        #endregion
    }
}

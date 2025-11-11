using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Radzen;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_DOMAIN.Enums.WorkflowParameterEnums;

namespace FATWA_WEB.Pages.Workflows
{
    public partial class WorkflowActivityParameter : ComponentBase
    {
        [Parameter]
        public WorkflowParams ParameterType { get; set; }
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string ParamValue { get; set; }

        [Parameter]
        public EventCallback<string> ParamValueChanged { get; set; }
        [Parameter]
        public int SubModuleId { get; set; }

        protected List<IdentityRole> Roles { get; set; }
        protected IEnumerable<Department> department { get; set; }
        protected IEnumerable<UserVM> users { get; set; }
        public int DepartmentId { get; set; }


        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateDepartmentList();
            spinnerService.Hide();
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master"> Function for getting users records for the drop down dynamically from the API</History>
        protected async Task GetRemoteUsersData(DropDownListReadEventArgs args)
        {
            DataEnvelope<User> result = await legalLegislationService.GetRemoteUsersData(args.Request);
            args.Data = result.Data;
            args.Total = result.Total;

        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master"> Function for getting entity details of the selected item</History>
        protected async Task<User> GetUserModelFromValue(string selectedValue)
        {
            return await legalLegislationService.GetUserModelFromValue(selectedValue);
        }
        protected async Task PopulateDepartmentList()
        {
            var userresponse = await lookupService.GetDepartments();
            if (userresponse.IsSuccessStatusCode)
            {
                department = (IEnumerable<Department>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        protected async Task OnChangeDepId(object args)
        {
            if (args != null)
            {
                await PopulateUsersListByDepartment();
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("No_User_Of_Department"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }
        protected async Task PopulateUsersListByDepartment()
        {
            var userresponse = await lookupService.GetUsersByDepartment(DepartmentId);
            if (userresponse.IsSuccessStatusCode)
            {
                users = (IEnumerable<UserVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master"> Function for getting users records for the drop down dynamically from the API</History>
        protected async Task LoadRolesData(LoadDataArgs args)
        {
            Roles = await roleService.GetAllRoles();
            var query = Roles.AsQueryable();

            if (!string.IsNullOrEmpty(args.Filter))
            {
                query = query.Where(c => c.Name.ToLower().Contains(args.Filter.ToLower()));
            }

            Roles = query.ToList();
            if (SubModuleId == (int)WorkflowSubModuleEnum.LegalLegislations)
            {
                Roles = Roles.Where(x => SystemRoles.LegislationRoles.Contains(x.Id)).ToList();
            }
            else if (SubModuleId == (int)WorkflowSubModuleEnum.LegalPrinciples)
            {
                Roles = Roles.Where(x => SystemRoles.PrincipleRoles.Contains(x.Id)).ToList();
            }
            else if (SubModuleId >= (int)WorkflowSubModuleEnum.Administrative && SubModuleId <= (int)WorkflowSubModuleEnum.CivilCommercial)
            {
                Roles = Roles.Where(x => SystemRoles.CaseRoles.Contains(x.Id) && x.Id != SystemRoles.Messenger).ToList();
            }
            else if (SubModuleId >= (int)WorkflowSubModuleEnum.Contracts && SubModuleId <= (int)WorkflowSubModuleEnum.InternationalArbitration)
            {
                Roles = Roles.Where(x => SystemRoles.ConsultationRoles.Contains(x.Id)).ToList();
            }
            InvokeAsync(StateHasChanged);
        }

        protected async Task ValueChanged()
        {
            await ParamValueChanged.InvokeAsync(ParamValue);
        }
    }
}

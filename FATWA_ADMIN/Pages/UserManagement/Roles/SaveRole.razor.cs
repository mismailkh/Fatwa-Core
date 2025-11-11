using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Telerik.Blazor.Components;

namespace FATWA_ADMIN.Pages.UserManagement.Roles
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Save Role class for handling Create/Update role</History>
    //<History Author = 'Hassan Abbas' Date='2022-07-24' Version="1.0" Branch="master"> Updated permissions supervisorsAndManagersGrid with custom Grouping header and multiple selection based on Modules and Submodules</History>
    //<History Author = 'Hassan Abbas' Date='2022-07-27' Version="1.0" Branch="master"> added Role clone feature to copy all the assigned permissions from another role</History>

    public partial class SaveRole : ComponentBase
    {
        #region Parameters

        [Parameter]
        public string RoleId { get; set; }
        public string CloneRoleId { get; set; }

        #endregion

        #region Service Injection

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected RoleService roleService { get; set; }
        #endregion

        #region Variable Declaration
        public bool CreateAnother { get; set; } = false;
        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        Role Role = new Role();
        protected bool allowRowSelectOnRowClick = true;
        protected bool isLoading { get; set; }
        public int count { get; set; }
        string _search;
        protected string bodyEnLengthMsg = "";
        protected string bodyArLengthMsg = "";
        #endregion

        #region Radzen DataGrid Variables and Settings
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<ClaimVM> _getRoleClaimsResult;
        protected IEnumerable<ClaimVM> getRoleClaimsResult
        {
            get
            {
                return _getRoleClaimsResult;
            }
            set
            {
                if (!object.Equals(_getRoleClaimsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRoleClaimsResult", NewValue = value, OldValue = _getRoleClaimsResult };
                    _getRoleClaimsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Group claims based on Module and Submodules</History>
        protected void OnRender(DataGridRenderEventArgs<ClaimVM> args)
        {
            if (args.FirstRender)
            {
                args.Grid.Groups.Add(new GroupDescriptor() { Title = translationState.Translate("Module"), Property = "Module", SortOrder = SortOrder.Descending });
                args.Grid.Groups.Add(new GroupDescriptor() { Title = translationState.Translate("Submodule"), Property = "SubModule", SortOrder = SortOrder.Descending });
                StateHasChanged();
            }
        }

        protected void OnGroupRowRender(GroupRowRenderEventArgs args)
        {
            if (args.FirstRender) { args.Expanded = false; }
        }
        #endregion

        #region On Component Load

        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Laod permissions based on role and Role model if update operation</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (RoleId != null)
            {
                var response = await roleService.GetRoleById(RoleId);
                if (response.IsSuccessStatusCode)
                {
                    Role = (Role)response.ResultData;
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            await Load();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            isLoading = true;


            spinnerService.Hide();
        }

        #endregion

        #region Button Events
        //protected void OnBodyInput(string newValue, bool isEnglish)
        //{
        //    if (isEnglish)
        //    {
        //        Role.Description_En = newValue;
        //        bodyEnLengthMsg = newValue.Length > 500
        //            ? translationState.Translate("Max_Five_Hundred_Characters")
        //            : string.Empty;
        //    }
        //    else
        //    {
        //        Role.Description_Ar = newValue;
        //        bodyArLengthMsg = newValue.Length > 500
        //            ? translationState.Translate("Max_Five_Hundred_Characters")
        //            : string.Empty;
        //    }
        //}
        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Create / Update role</History>
        protected async Task SaveChanges()
        {
            if (RoleId == null)
            {
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Save_Role"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });

                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    var response = await roleService.SaveRole(Role);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Role_Created"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (CreateAnother)
                        {
                            await JSRuntime.InvokeVoidAsync("window.location.reload");
                        }
                        else
                        {
                            navigationManager.NavigateTo("roles");
                        }
                    }
                    else
                    {
                        Role.Id = null;
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response, "Role_Name_Exists");
                    }
                    spinnerService.Hide();
                }
            }
            else
            {
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Save_Changes"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });

                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    var response = await roleService.SaveRole(Role);
                    if (response.IsSuccessStatusCode)
                    {
                        spinnerService.Hide();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Role_Updated"),
                            Style = "position: fixed !important; left: 0; margin: auto; ",
                        });
                        navigationManager.NavigateTo("roles");
                    }

                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response, "Role_Name_Exists");
                    }
                    spinnerService.Hide();
                }
            }

            dialogService.Close(true);
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Cancel saving role</History>
        protected async Task CancelSavingRole()
        {
            dialogService.Close(false);
        }
        #endregion

        #region Toggle Create Another

        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Toggle CreateAnother flag</History>
        protected async Task ToggleCreateAnother()
        {
            if (CreateAnother) { CreateAnother = false; } else { CreateAnother = true; }
        }

        #endregion

        #region Role dropdown

        //<History Author = 'Hassan Abbas' Date='2022-07-27' Version="1.0" Branch="master"> Function for getting roles records for the drop down dynamically from the API</History>
        protected async Task GetRemoteRolesData(DropDownListReadEventArgs args)
        {
            DataEnvelope<Role> result = await roleService.GetRemoteRolesData(args.Request);
            args.Data = result.Data;
            args.Total = result.Total;
        }

        //<History Author = 'Hassan Abbas' Date='2022-07-27' Version="1.0" Branch="master"> Function for getting entity details of the selected item</History>
        protected async Task<Role> GetRoleModelFromValue(string selectedValue)
        {
            return await roleService.GetRoleModelFromValue(selectedValue);
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

    }
}

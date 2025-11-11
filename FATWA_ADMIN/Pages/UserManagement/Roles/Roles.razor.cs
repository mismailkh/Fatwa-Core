using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_ADMIN.Extensions;
using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.UserManagement.Roles
{
    public partial class RolesComponent : ComponentBase
    {
        #region Constructor
        public RolesComponent()
        {

        }
        #endregion

        #region Variable Declaration
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Service Inject
        [Inject]
        protected SpinnerService spinnerService { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Inject]
        protected DialogService dialogService { get; set; }
        [Inject]
        protected LoginState loginState { get; set; }
        [Inject]
        protected TranslationState translationState { get; set; }
        [Inject]
        protected RoleService roleServices { get; set; }
        [Inject]
        protected NotificationService notificationService { get; set; }
        [Inject]
        protected ILocalStorageService BrowserStorage { get; set; }
        [Inject]
        protected RadzenGridSearchExtension gridSearchExtension { get; set; }
        [Inject]
        protected InvalidRequestHandlerService invalidRequestHandlerService { get; set; }
        #endregion

        #region Radzen DataGrid Variables and Settings

        public List<Role> rolesList = new List<Role>();

        protected RadzenDataGrid<Role>? grid0 = new RadzenDataGrid<Role>();

        protected bool allowRowSelectOnRowClick = true;
        protected bool isLoading { get; set; }
        public int count { get; set; }
        string _search;
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

        IEnumerable<Role> _getRolesResult;
        protected IEnumerable<Role> FilteredGetRolesResult { get; set; } = new List<Role>();
        protected IEnumerable<Role> getRolesResult
        {
            get
            {
                return _getRolesResult;
            }
            set
            {
                if (!object.Equals(_getRolesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRolesResult", NewValue = value, OldValue = _getRolesResult };
                    _getRolesResult = value;
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

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            await Load();
            translationState.TranslateGridFilterLabels(grid0);
        }
        protected async Task Load()
        {
            var result = await roleServices.GetRoleDetails();
            if (result != null && result.Any())
            {
                getRolesResult = result;
                FilteredGetRolesResult = result;
                count = getRolesResult.Count();
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Info,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            isLoading = false;
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    isLoading = true;
                    if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    {
                        FilteredGetRolesResult = await gridSearchExtension.Filter(getRolesResult, new Query()
                        {
                            Filter = $@"i => (i.Name != null && i.Name.ToLower().Contains(@0)) || (i.Description_En != null && i.Description_En.ToLower().Contains(@1)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3)) || (i.ModifiedBy != null && i.ModifiedBy.ToLower().Contains(@4)) || (i.ModifiedDate.HasValue && i.ModifiedDate.Value.ToString(""dd/MM/yyyy"").Contains(@5))",
                            FilterParameters = new object[] { search, search, search, search, search, search }
                        });
                    }
                    else
                    {
                        FilteredGetRolesResult = await gridSearchExtension.Filter(getRolesResult, new Query()
                        {
                            Filter = $@"i => (i.Name != null && i.Name.ToLower().Contains(@0)) || (i.Description_Ar != null && i.Description_Ar.ToLower().Contains(@1)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3)) || (i.ModifiedBy != null && i.ModifiedBy.ToLower().Contains(@4)) || (i.ModifiedDate.HasValue && i.ModifiedDate.Value.ToString(""dd/MM/yyyy"").Contains(@5))",
                            FilterParameters = new object[] { search, search, search, search, search, search }
                        });
                    }
                    
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

        #region Add Role
        protected async Task Button0Click(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/save-role");

        }
        #endregion

        #region Update Role
        protected async Task GridEditButtonClick(MouseEventArgs args, Role data)
        {
            try
            {

                if (await dialogService.OpenAsync<SaveRole>(translationState.Translate("Update_Role"),
                new Dictionary<string, object>() { { "RoleId", data.Id } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Load();
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

        #region Delete Role (Soft delete status change)
        protected async Task GridDeleteButtonClick(MouseEventArgs args, Role data)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Are_you_Sure_You_Want_To_Delete_This_Role"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = translationState.Translate("OK"),
                       CancelButtonText = translationState.Translate("Cancel")
                   });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    // Get all role claims
                    var claimsResult = await roleServices.GetAllClaims(data.Id);
                    // data.RoleClaims = claimsResult.Where(c => c.IsAssigned).ToList();

                    var response = await roleServices.DeleteRole(data);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Role_Deleted"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                    await Task.Delay(300);
                    await Load();
                }
            }
            catch (System.Exception ex)
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
    }
}

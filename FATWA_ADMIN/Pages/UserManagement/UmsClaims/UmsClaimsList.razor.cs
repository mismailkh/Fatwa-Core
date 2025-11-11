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
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.UserManagement.UmsClaims
{
    public partial class UmsClaimsListComponent : ComponentBase
    {
        #region Constructor
        public UmsClaimsListComponent()
        {

        }
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
        protected NotificationService notificationService { get; set; }
        [Inject]
        protected ILocalStorageService BrowserStorage { get; set; }
        [Inject]
        protected UmsClaimService ClaimService { get; set; }
        [Inject]
        protected RadzenGridSearchExtension gridSearchExtension { get; set; }
        [Inject]
        protected InvalidRequestHandlerService invalidRequestHandlerService { get; set; }


        #endregion


        #region Radzen DataGrid Variables and Settings
        private Timer debouncer;
        private const int debouncerDelay = 500;
        public List<ClaimUms> claimsList = new List<ClaimUms>();

        protected RadzenDataGrid<ClaimUms>? grid = new RadzenDataGrid<ClaimUms>();

        protected bool allowRowSelectOnRowClick = true;
        public int count { get; set; }
        string _search;
        protected bool isLoading { get; set; }
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

        IEnumerable<ClaimUms> _getClaimsResult;
        protected IEnumerable<ClaimUms> FilteredGetClaimsResult { get; set; } = new List<ClaimUms>();
        protected IEnumerable<ClaimUms> getClaimsResult
        {
            get
            {
                return _getClaimsResult;
            }
            set
            {
                if (!object.Equals(_getClaimsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getClaimsResult", NewValue = value, OldValue = _getClaimsResult };
                    _getClaimsResult = value;
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
            translationState.TranslateGridFilterLabels(grid);
        }
        protected async Task Load()
        {
            isLoading = true;
            var response = await ClaimService.GetClaimUms();
            if (response.IsSuccessStatusCode)
            {
                getClaimsResult = (IEnumerable<ClaimUms>)response.ResultData;
                FilteredGetClaimsResult = (IEnumerable<ClaimUms>)response.ResultData;
                count = getClaimsResult.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
                    FilteredGetClaimsResult = await gridSearchExtension.Filter(getClaimsResult, new Query()
                    {
                        Filter = $@"i => ( (i.Module != null && i.Module.ToLower().Contains(@0)) || (i.Title_En != null && i.Title_En.ToLower().Contains(@1)) || (i.Title_Ar != null && i.Title_Ar.ToLower().Contains(@2))|| (i.SubModule != null && i.SubModule.ToLower().Contains(@3)))",
                        FilterParameters = new object[] { search, search, search, search }
                    });
                    isLoading = false;await InvokeAsync(StateHasChanged);
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

        #region Edit Claims 
        protected async Task GridEditButtonClick(ClaimUms data)
        {
            try
            {
                navigationManager.NavigateTo("/save-claims/" + data.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region hard delete claims
        protected async Task GridDeleteButtonClick(ClaimUms claimUms)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Delete_The_Record"), translationState.Translate("delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var response = await ClaimService.DeleteClaims(claimUms.Id);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await Load();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
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
                throw new Exception(ex.Message);
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

    }
}

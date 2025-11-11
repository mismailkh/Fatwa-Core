using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;


namespace FATWA_ADMIN.Pages.UserManagement.UmsClaims
{
    public partial class SaveUmsClaims : ComponentBase
    {
        #region Service Injection
        [Inject]
        protected UmsClaimService ClaimService { get; set; }

        #endregion

        #region Parameters

        [Parameter]
        public dynamic Id { get; set; }

        protected RadzenDataGrid<ClaimUms> grid;
        protected bool isLoading { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }


        #endregion

        #region Radzen DataGrid Variables and Settings
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


        ClaimUms _getClaimsResult;
        protected ClaimUms getClaimsResult
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
        #endregion

        #region On Component Load

        //<History Author = 'Zaeem' Date='2022-07-22' Version="1.0" Branch="master"> Laod permissions based on role and Role model if update operation</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            //   translationState.TranslateGridFilterLabels(supervisorsAndManagersGrid);
            spinnerService.Hide();
        }
        protected async Task Load()
        {

            if (Id != null)
            {
                var response = await ClaimService.GetClaimsById(Convert.ToInt32(Id));
                if (response.IsSuccessStatusCode)
                {
                    getClaimsResult = (ClaimUms)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            else
            {

                getClaimsResult = new ClaimUms() { };
            }
        }


        #endregion

        #region Button Events
        //<History Author = 'Muhammad Zaeem' Date='2022-07-22' Version="1.0" Branch="master"> Create / Update role</History>
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
            navigationManager.NavigateTo("claims");
        }
        protected async Task SaveClaimsSubmit(ClaimUms args)
        {
            try
            {
                if (Id == null)
                {
                    // check if parent number is already saved
                    var response = await ClaimService.SaveClaims(args);
                    if (response.IsSuccessStatusCode)
                    {
                        getClaimsResult = (ClaimUms)response.ResultData;
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    var fatwaDbCreateClaimsResult = await ClaimService.SaveClaims(args);
                    dialogService.Close(getClaimsResult);
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Claim_Updated"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });

                }
                navigationManager.NavigateTo("claims");
            }
            catch (Exception)
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

    }
}

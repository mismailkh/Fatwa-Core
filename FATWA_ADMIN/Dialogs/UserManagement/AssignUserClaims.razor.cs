using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_ADMIN.Dialogs.UserManagement
{
    //< History Author = "Ammaar Naveed" Date = "08/08/2024" Version = "1.0" Branch = "master" >Assign user claims</ History>

    public partial class AssignUserClaims : ComponentBase
    {
        #region Variables
        protected IEnumerable<UserClaimsVM> UMSClaimsList = new List<UserClaimsVM>();
        #endregion

        #region Parameters
        [Parameter]
        public string LoggedInUser { get; set; }
        [Parameter]
        public string UserId { get; set; }
        #endregion

        #region Constructors
        private UserClaimsVM UserClaimsVM = new UserClaimsVM();
        #endregion

        #region On Component Load/ On Initialized
        protected override async Task OnInitializedAsync()
        {
            await GetUmsClaims();
        }
        #endregion

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

        #region Save User Claims
        private async Task SaveUserClaims()
        {
                bool? dialogResponse = await dialogService.Confirm(translationState.Translate("Confirm_Add_Claims"),
                        translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = @translationState.Translate("OK"),
                            CancelButtonText = @translationState.Translate("Cancel")
                        });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    UserClaimsVM.CreatedBy = LoggedInUser;
                    UserClaimsVM.SelectedUserId = UserId;
                    UserClaimsVM.ModuleId = (int)ModuleEnum.DigitalSignature;
                    string parentActionClaimDigitalSignature = "Permissions.DS.DigitalSignature";
                    UserClaimsVM.SelectedUserClaims = UserClaimsVM.SelectedUserClaims.Concat(new[] { parentActionClaimDigitalSignature }).ToList();
                var response = await userService.SaveUserClaims(UserClaimsVM);

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("User_Claims_Added_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        navigationManager.NavigateTo("/employees-list");
                        await InvokeAsync(StateHasChanged);
                        spinnerService.Hide();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }

        }
        #endregion

        #region Redirect Functions
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/employees-list");
        }
        #endregion

    }
}

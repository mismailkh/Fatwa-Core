using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class SectorTypeAdd : ComponentBase
    {
        #region Variables
        protected IEnumerable<SectorBuilding> Buildings { get; set; }
        protected IEnumerable<SectorFloor> Floors { get; set; }
        public List<Role> SystemRoles { get; set; }
        protected OperatingSectorType SectorType;
        ApiCallResponse response = new ApiCallResponse();
        #endregion

        #region Parameter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            await GetSystemRoles();
            await GetSectorFloor();
            await GetSectorTypeById();
            if (Id == null)
            {
                SectorType = new OperatingSectorType() { };
            }
            else
            {
                await GetSectorBuildings();
                await GetSectorRolesBySectorId();
            }
            spinnerService.Hide();
        } 
        #endregion

        #region Functions 
        public async Task GetSectorBuildings()
        {
            var response = await lookupService.GetSectorBuilding();
            if (response.IsSuccessStatusCode)
            {
                Buildings = (IEnumerable<SectorBuilding>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        public async Task GetSectorFloor()
        {
            var response = await lookupService.GetSectorFloor();
            if (response.IsSuccessStatusCode)
            {
                Floors = (IEnumerable<SectorFloor>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task GetSystemRoles()
        {
            var response = await userService.GetRoles();
            if (response.IsSuccessStatusCode)
            {
                SystemRoles = (List<Role>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetSectorRolesBySectorId()
        {
            var response = await lookupService.GetSectorRolesBySectorId(Id);
            if (response.IsSuccessStatusCode)
            {
                List<string> roles = new List<string>();
                var result = (List<CmsOperatingSectorTypesRoles>)response.ResultData;
                foreach (var role in result)
                {
                    roles.Add(role.RoleId);
                }
                SectorType.RoleIds = roles;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetSectorTypeById()
        {
            response = await lookupService.GetSectorTypeById(Id);
            if (response.IsSuccessStatusCode)
            {
                SectorType = (OperatingSectorType)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Save changes

        protected async Task SaveChanges(OperatingSectorType args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                                     translationState.Translate("Sure_Submit"),
                                     translationState.Translate("Confirm"),
                                     new ConfirmOptions()
                                     {
                                         OkButtonText = translationState.Translate("OK"),
                                         CancelButtonText = translationState.Translate("Cancel")
                                     });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (Id != null)
                    {
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateSectorType(SectorType);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Sector_Type_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }

                    dialogService.Close(true);
                    StateHasChanged();
                    spinnerService.Hide();
                }

            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Sector_Type") : translationState.Translate("Sector_Type_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        #endregion
    }
}

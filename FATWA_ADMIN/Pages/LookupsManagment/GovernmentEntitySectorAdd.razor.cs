using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class GovernmentEntitySectorAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variable
        public List<GovernmentEntity> GovernmentEntitiesName { get; set; } = new List<GovernmentEntity>();
        ApiCallResponse response = new ApiCallResponse();
        protected GEDepartments GovernmentEntitySectors;
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await GetGovernmentEntities();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                GovernmentEntitySectors = new GEDepartments() { };
                GovernmentEntitySectors.Id = new int();
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetGovtEntityDepartmentById(Id);
                if (response.IsSuccessStatusCode)
                {
                    GovernmentEntitySectors = (GEDepartments)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region populate Government Entities  Name 
        protected async Task GetGovernmentEntities()
        {

            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovernmentEntitiesName = (List<GovernmentEntity>)response.ResultData;
            }

            StateHasChanged();

        }
        #endregion

        #region Form submit
        protected async Task SaveChanges(GEDepartments args)
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
                    if (Id == null)
                    {
                        if (GovernmentEntitySectors.DefaultReceiver)
                        {
                            var DepartmentId = GovernmentEntitySectors.Id;
                            var entityId = GovernmentEntitySectors.EntityId ?? 0;
                            var result = await lookupService.CheckDefaultReceiverAlreadyAttached(entityId, DepartmentId);
                            if (result.ResultData != null && (bool)result.ResultData == true)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Warning,
                                    Detail = translationState.Translate("Default_Receiver_Already_Attached_With_Government_Entity"),
                                    Style = "position: fixed !important; left: 0; margin: auto; Width:40px; "
                                });
                                await Task.Delay(2000);
                                return;
                            }
                        }
                        var response = await lookupService.SaveGovernmentEntityDepartment(GovernmentEntitySectors);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Government_Entity_Department_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "

                            });
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        if (GovernmentEntitySectors.DefaultReceiver)
                        {
                            var entityId = GovernmentEntitySectors.EntityId ?? 0;
                            var DepartmentId = GovernmentEntitySectors.Id;
                            var result = await lookupService.CheckDefaultReceiverAlreadyAttached(entityId, DepartmentId);
                            if (result.ResultData != null && (bool)result.ResultData == true)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Warning,
                                    Detail = translationState.Translate("Default_Receiver_Already_Attached_With_Government_Entity"),
                                    Style = "position: fixed !important; left: 0; margin: auto;  Width:40px;"
                                });
                                await Task.Delay(2000);
                                return;
                            }
                        }
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateGovernmentEntityDepartment(GovernmentEntitySectors);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Government_Entity_Department_Updated_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Government_Entity_Department") : translationState.Translate("Government_Entity_Department_could_not_be_updated"),
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

using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class EpGradeTypeAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        protected GradeType EpGradeType;
        ApiCallResponse response = new ApiCallResponse();
        IEnumerable<Department> Departments { get; set; }

        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            await GetEmployeeDepartment();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                EpGradeType = new GradeType() { };
            }
            else
            {
                response = await lookupService.GetEpGradeTypeById(Id);
                if (response.IsSuccessStatusCode)
                {
                    EpGradeType = (GradeType)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }

        #endregion

        #region Department DDL
        public async Task GetEmployeeDepartment()
        {
            var response = await lookupService.GetEmployeeDepartment();
            if (response.IsSuccessStatusCode)
            {
                Departments = (IEnumerable<Department>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Form Submit
        protected async Task SaveChanges(GradeType args)
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
                        var fatwaDbCreateGradeTypeResult = await lookupService.SaveGradeType(EpGradeType);
                        if (fatwaDbCreateGradeTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Grade_Type_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close(true);
                            StateHasChanged();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        var fatwaDbUpdateGradeTypeResult = await lookupService.UpdateGradeType(EpGradeType);
                        if (fatwaDbUpdateGradeTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Grade_Type_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close(true);
                            StateHasChanged();
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Grade_Type") : translationState.Translate("Grade_Type_could_not_be_updated"),
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

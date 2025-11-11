using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
using Department = FATWA_DOMAIN.Models.AdminModels.UserManagement.Department;


namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class DepartmentAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        protected Department departments;
        ApiCallResponse response = new ApiCallResponse();
        public bool ReturnDayValidation { get; set; } = false;
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                departments = new Department() { };
                departments.Id = new int();
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetDepartmentById(Id);
                if (response.IsSuccessStatusCode)
                {
                    departments = (Department)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region Form Submit

        protected async Task SaveChanges(Department args)
        {
            try
            {
                if (string.IsNullOrEmpty(departments.Borrow_Return_Day_Duration.ToString()) || departments.Borrow_Return_Day_Duration == 0)
                {
                    ReturnDayValidation = true;
                    return;
                }
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
                        response = await lookupService.SaveDepartment(departments);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Department_Added_Successfully"),
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
                        response = await lookupService.UpdateDepartment(departments);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Department_Updated_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Department") : translationState.Translate("Department_could_not_be_updated"),
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

        #region Check return day
        void CheckReturnDay()
        {
            if (string.IsNullOrEmpty(departments.Borrow_Return_Day_Duration.ToString()) || departments.Borrow_Return_Day_Duration == 0)
            {
                ReturnDayValidation = true;
            }
            else
            {
                ReturnDayValidation = false;
            }
        }
        #endregion
    }
}

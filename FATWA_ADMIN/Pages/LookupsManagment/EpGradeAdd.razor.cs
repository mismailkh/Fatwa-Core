using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class EpGradeAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variable
        IEnumerable<GradeType> GradeTypeList { get; set; }
        protected Grade EpGrades;
        ApiCallResponse response = new ApiCallResponse();
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await GetGradeTypes();
            await Load();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                EpGrades = new Grade() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetEpGradeById(Id);
                if (response.IsSuccessStatusCode)
                {
                    EpGrades = (Grade)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        public async Task GetGradeTypes()
        {
            var response = await lookupService.GetGradeTypes();
            if (response.IsSuccessStatusCode)
            {
                GradeTypeList = (IEnumerable<GradeType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();
        }
        #endregion

        #region Form Submit
        protected async Task SaveChanges(Grade args)
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
                        response = await lookupService.SaveGrade(EpGrades);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Grade_Added_Successfully"),
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
                        response = await lookupService.UpdateGrade(EpGrades);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Grade_Updated_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Grade") : translationState.Translate("Grade_could_not_be_updated"),
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

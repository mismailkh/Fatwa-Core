using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.TaskModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class TaskTypeAdd : ComponentBase
    {
        #region Paramter

        [Parameter]
        public dynamic TypeId { get; set; }
        #endregion

        #region Variables
        TaskType _taskType;
        protected TaskType taskType
        {
            get
            {
                return _taskType;
            }
            set
            {
                if (!object.Equals(_taskType, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "taskType", NewValue = value, OldValue = _taskType };
                    _taskType = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Load();
            }
            catch (Exception)
            {
                throw;
            }

        }
        ApiCallResponse response = new ApiCallResponse();
        protected async Task Load()
        {
            if (TypeId == null)
            {
                spinnerService.Show();
                taskType = new TaskType() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetTaskTypeById(TypeId);
                if (response.IsSuccessStatusCode)
                {
                    taskType = (TaskType)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region Form submit
        protected async Task SaveChanges(TaskType args)
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
                    if (TypeId != null)
                    {
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateTaskType(taskType);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Task_Type_Updated_Successfully"),
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
                    Detail = TypeId == null ? translationState.Translate("Could_not_create_a_new_Task_Type") : translationState.Translate("Task_Type_could_not_be_updated"),
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

        #region Grid Funtions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion
    }
}

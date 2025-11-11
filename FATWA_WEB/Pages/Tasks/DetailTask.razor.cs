using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.DropDowns;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Tasks
{
    public partial class DetailTask : ComponentBase
    {
        #region Parameters

        [Parameter]
        public dynamic taskId { get; set; }

        #endregion

        #region Variables

        protected TaskDetailVM taskDetailVM { get; set; }
        public bool isTaskRejected = true;
        List<Notification> notifications = new List<Notification>();

        #endregion

        #region Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            try
            {
                var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(taskId));
                if (getTaskDetail.IsSuccessStatusCode)
                {
                    taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
                    if (taskDetailVM is not null && taskDetailVM.TaskStatusId == (int)TaskStatusEnum.Done)
                    {
                        isTaskRejected = false;
                    }
                    else
                    {
                        isTaskRejected = true;
                    } 
                    //if(taskDetailVM.)
                }
                else
                {
                    taskDetailVM = new TaskDetailVM();
                }
            }
            catch (Exception ex)
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

        #region Functions

        protected async void ButtonCancel()
        {
            //if (taskDetailVM.TaskStatusId==(int)TaskStatusEnum.Pending)
            //{
            //	navigationManager.NavigateTo("/usertask-list/" + (int)TaskStatusEnum.Pending);
            //}
            //else if (taskDetailVM.TaskStatusId==(int)TaskStatusEnum.Approved)
            //{
            //	navigationManager.NavigateTo("/usertask-list/" + (int)TaskStatusEnum.Approved);
            //}
            //else if (taskDetailVM.TaskStatusId==(int)TaskStatusEnum.Rejected)
            //{
            //	navigationManager.NavigateTo("/usertask-list/" + (int)TaskStatusEnum.Rejected);
            //}
            //else if (taskDetailVM.TaskStatusId ==(int)TaskStatusEnum.InProgress)
            //{
            //	navigationManager.NavigateTo("/usertask-list/" + (int)TaskStatusEnum.InProgress);
            //}
            //else if (taskDetailVM.TaskStatusId == (int)TaskStatusEnum.Done)
            //{
            //	navigationManager.NavigateTo("/usertask-list/" + (int)TaskStatusEnum.Done);
            //}
            if (taskDetailVM.SystemGenTypeId > 0)
            {
                await RedirectBack();
            }
            else
            {
                navigationManager.NavigateTo("/usertask-list");
            }

        }

        protected async Task ButtonViewInformation()
        {
            navigationState.ReturnUrl = "usertask-list";
            if (taskDetailVM.Url.StartsWith("caserequest-transfer-review") || taskDetailVM.Url.StartsWith("caserequest-copy-review") || taskDetailVM.Url.StartsWith("casefile-copy-review") || taskDetailVM.Url.StartsWith("casefile-transfer-review") ||
                 taskDetailVM.Url.StartsWith("mergerequest-view") || taskDetailVM.Url.StartsWith("draftdocument-detail") || taskDetailVM.Url.StartsWith("executionrequest-detail") || taskDetailVM.Url.StartsWith("document-view"))
            {
                await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + taskDetailVM.Url + "/" + taskDetailVM.TaskId, "_blank");
            }
            else if (taskDetailVM.Url.StartsWith("consultationrequest-transfer-review") || taskDetailVM.Url.StartsWith("draftdocument-detail") || taskDetailVM.Url.StartsWith("/consultationfile-transfer-review"))
            {
                await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + taskDetailVM.Url + "/" + taskDetailVM.TaskId, "_blank");
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + taskDetailVM.Url, "_blank");
            }
        }

        protected async Task ButtonReject()
        {
            try
            {
                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;

                bool? dialogResponse = await dialogService.Confirm(
                  translationState.Translate("Sure_Reject_Task"),
                  translationState.Translate("Confirm"),
                  new ConfirmOptions()
                  {
                      OkButtonText = @translationState.Translate("OK"),
                      CancelButtonText = @translationState.Translate("Cancel")
                  });

                if (dialogResponse == true)
                {
                    var dialogResult = await dialogService.OpenAsync<RejectAndReason>(
                         translationState.Translate("Reject_Reason"),
                         new Dictionary<string, object>() { { "ReferenceId", taskDetailVM.TaskId } },
                         new DialogOptions() { CloseDialogOnOverlayClick = true });

                    if (dialogResult is not null)
                    {
                        ApiCallResponse response = null;

                        if ((taskDetailVM.ModuleId == (int)WorkflowModuleEnum.CaseManagement) && (taskDetailVM.TypeId == (int)TaskTypeEnum.Assignment))
                        {
                            response = await taskService.NotifyTaskAssignedBy(taskDetailVM);
                        }
                        if ((taskDetailVM.ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement) && (taskDetailVM.TypeId == (int)TaskTypeEnum.Assignment))
                        {
                            response = await taskService.ConsultationTaskRejection(taskDetailVM);
                        }
                        else
                        {
                            response = await taskService.DecisionTask(taskDetailVM);
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Task_Reject_Success"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                            await RedirectBack();
                        }
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task ButtonAccept()
        {
            try
            {
                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;

                bool? dialogResponse = await dialogService.Confirm(
                  translationState.Translate("Sure_Accept_Task"),
                  translationState.Translate("Confirm"),
                  new ConfirmOptions()
                  {
                      OkButtonText = @translationState.Translate("OK"),
                      CancelButtonText = @translationState.Translate("Cancel")
                  });

                if (dialogResponse == true)
                {
                    ApiCallResponse response = null;

                    if ((taskDetailVM.ModuleId == (int)WorkflowModuleEnum.CaseManagement) && (taskDetailVM.TypeId == (int)TaskTypeEnum.Assignment))
                    {
                        response = await taskService.SaveCaseAssignment(taskDetailVM);
                    }
                    else
                    {
                        response = await taskService.DecisionTask(taskDetailVM);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Task_Accept_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await RedirectBack();
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion
    }
}

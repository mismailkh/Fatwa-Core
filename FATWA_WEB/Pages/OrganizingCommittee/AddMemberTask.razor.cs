using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.OrganizingCommittee
{
    public partial class AddMemberTask:ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid CommitteeId { get; set; }
        [Parameter]
        public Guid TaskId { get; set; }
        [Parameter]
        public bool Update { get; set; }
        #endregion

        #region Variables
        public List<CommitteeMembersVMs> committeeMembers { get; set; } = new List<CommitteeMembersVMs>();
        public CommitteeMembersVMs committeeMember { get; set; } = new CommitteeMembersVMs();
        public CommitteeTaskVm committeeTasksVm { get; set; } = new CommitteeTaskVm { TaskDeadline = DateTime.Now };
        public RadzenDropDown<string?> addedMemberDropDown { get; set; } = new RadzenDropDown<string?>();
        public string memberValidationMessage { get; set; } = " ";
        public string taskValidationMessage { get; set; } = " ";
        public IEnumerable<UserDataVM> usersData { get; set; } = new List<UserDataVM>();
        public List<CommitteeTaskVm> committeeTasks { get; set; } = new List<CommitteeTaskVm>();
        public RadzenDataGrid<CommitteeTaskVm> committeeTaskGrid { get; set; } = new RadzenDataGrid<CommitteeTaskVm>();
        protected bool IsReadOnly = true;
        public List<SaveTaskVM> addedTaskList { get; set; } = new List<SaveTaskVM>();
        public TaskDetailVM taskDetail { get; set; } = new TaskDetailVM();
        public List<TempCommitteeTaskVm> tempCommitteeTask { get; set; } = new List<TempCommitteeTaskVm>();
        public List<FATWA_DOMAIN.Models.Notifications.Notification> sendNotifications { get; set; } = new List<FATWA_DOMAIN.Models.Notifications.Notification>();
        public Guid taskId { get; set; }
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            if (Update)
            {
                await GetTempCommitteeTask();

            }
            await GetCommitteeMembers();


        }
        #endregion

        #region Get Committee Members
        protected async Task GetCommitteeMembers()
        {
            try
            {
                var response = await organizingCommitteeService.GetCommitteeMembers(CommitteeId);
                if (response.IsSuccessStatusCode)
                {
                    committeeMembers = (List<CommitteeMembersVMs>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Committee Task
        protected async Task GetTempCommitteeTask()
        {
            try
            {
                var response = await organizingCommitteeService.GetTempCommitteeTasks(CommitteeId);
                if (response.IsSuccessStatusCode)
                {
                    tempCommitteeTask = (List<TempCommitteeTaskVm>)response.ResultData;
                    var tempTask = tempCommitteeTask.Where(x => x.Id == TaskId).FirstOrDefault();
                    committeeTasksVm.MemberId = tempTask.MemberId;
                    committeeTasksVm.Name = tempTask.TaskName;
                    committeeTasksVm.TaskDeadline = tempTask.TaskDeadline;
                    committeeTasksVm.Description = tempTask.Description;

                    await InvokeAsync(StateHasChanged);

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Task by TaskId
        protected async Task GetTaskByTaskId()
        {
            try
            {
                var response = await taskService.GetTaskDetailById(taskId);
                if (response.IsSuccessStatusCode)
                {
                    taskDetail = (TaskDetailVM)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Member Tasks
        protected async Task SaveMemberTasks(CommitteeTaskVm args)
        {
            try
            {

                if (string.IsNullOrEmpty(args.MemberId) || string.IsNullOrEmpty(args.Name))
                {
                    memberValidationMessage = string.IsNullOrEmpty(args.MemberId) ? translationState.Translate("Required_Field") : "";
                    taskValidationMessage = string.IsNullOrEmpty(args.Name) ? translationState.Translate("Required_Field") : "";
                }
                else
                {
                    bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("OK"),
                        CancelButtonText = @translationState.Translate("Cancel")
                    });
                    if (dialogResponse == true)
                    {
                        ApiCallResponse response;
                        committeeTasks.Add(args);
                        await FillAddedTaskList(committeeTasks);
                        if (Update)
                        {
                            args.TaskId = TaskId;
                            response = await organizingCommitteeService.UpdateTempTask(args);
                        }
                        else
                        {
                            response = await taskService.AddTaskList(addedTaskList);

                        }
                        if (response.IsSuccessStatusCode)
                        {
                            if (!Update)
                            {
                                await GetTaskByTaskId();
                                await SendNotification();
                            }
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate(Update ? "Task_Successfully_Updated" : "Task_Successfully_Added"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close();
                        }
                        else
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

        #region Fill Task Added List
        protected async Task FillAddedTaskList(List<CommitteeTaskVm> committeeTaskVms)
        {
            try
            {
                foreach (var task in committeeTaskVms)
                {
                    SaveTaskVM saveTask = new SaveTaskVM()
                    {
                        Task = new UserTask()
                        {
                            DueDate = DateTime.Now.Date,
                            Date = DateTime.Now.Date
                        },
                        TaskActions = new List<TaskAction>(),
                        DeletedTaskActionIds = new List<Guid>(),
                    };
                    saveTask.Task.TaskId = Guid.NewGuid();
                    saveTask.Task.TaskStatusId = (int)TaskStatusEnum.Pending;
                    saveTask.Task.TypeId = (int)TaskTypeEnum.Task;
                    saveTask.Task.PriorityId = (int)PriorityEnum.Medium;
                    saveTask.Task.Name = task.Name;
                    saveTask.Task.CreatedDate = DateTime.Now;
                    saveTask.Task.CreatedBy = loginState.UserDetail.UserName;
                    saveTask.Task.DueDate = task.TaskDeadline;
                    saveTask.Task.AssignedBy = loginState.UserDetail.UserId;
                    saveTask.Task.AssignedTo = task.MemberId;
                    saveTask.Task.Description = " ";
                    saveTask.Task.SectorId = (int)loginState.UserDetail.SectorTypeId;
                    saveTask.Task.DepartmentId = (int)DepartmentEnum.Administrative;
                    saveTask.Task.ModuleId = (int)WorkflowModuleEnum.OrganizingCommittee;
                    saveTask.Task.RoleId = SystemRoles.FatwaAdmin; //FATWA ADMIN
                    saveTask.Task.IsDeleted = false;
                    saveTask.Task.ReferenceId = CommitteeId;
                    saveTask.Task.Url = "detail-committee/" + CommitteeId;
                    saveTask.Task.Description = task.Description;
                    saveTask.TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Save Committee Action",
                            TaskId = saveTask.Task.TaskId,
                        }
                    };
                    taskId = saveTask.Task.TaskId;
                    addedTaskList.Add(saveTask);
                }
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

        #region Cancel
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

        #region Reload Page
        protected async Task ReloadPage()
        {
            await JsInterop.InvokeVoidAsync("refreshPage");
        }
        #endregion

        #region Send Notificatin
        protected async Task SendNotification()
        {
            try
            {
                foreach (var addTask in addedTaskList)
                {
                    FATWA_DOMAIN.Models.Notifications.Notification notification = new FATWA_DOMAIN.Models.Notifications.Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = addTask.Task.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = committeeTasksVm.MemberId, // Assign to each receiverId
                        ModuleId = (int)WorkflowModuleEnum.OrganizingCommittee,
                        Action = "detail",
                        EntityName = "usertask",
                        EntityId = addTask.Task.TaskId.ToString(),
                        EventId = (int)NotificationEventEnum.AddMemberTask,

                    }; ;
                    notification.NotificationParameter = new NotificationParameter
                    {
                        ReferenceNumber = taskDetail.TaskNumber.ToString()
                    };
                    sendNotifications.Add(notification);
                }
                var notificationResponse = await notificationDetailService.SendNotification(sendNotifications);
                if (notificationResponse.IsSuccessStatusCode)
                {

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(notificationResponse);
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}

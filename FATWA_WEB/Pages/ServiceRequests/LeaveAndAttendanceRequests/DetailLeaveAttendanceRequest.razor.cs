using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.ServiceRequests.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
namespace FATWA_WEB.Pages.ServiceRequests.LeaveAndAttendanceRequests
{
    public partial class DetailLeaveAttendanceRequest : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic ServiceRequestId { get; set; }
        [Parameter]
        public dynamic RequestTypeId { get; set; }
        [Parameter]
        public string TaskId { get; set; }

        #endregion

        #region Constructor
        public DetailLeaveAttendanceRequest()
        {
            LeaveAttendanceRequestDetail = new LeaveAttendanceRequestDetailVM();
            RequestRemarksDetailVM = new List<RequestRemarksDetailVM>();
            taskDetailVM = new TaskDetailVM();
        }
        #endregion

        #region Variable
        protected ServiceRequestVM ServiceRequestDetail { get; set; } = new ServiceRequestVM();
        protected LeaveAttendanceRequestDetailVM LeaveAttendanceRequestDetail { get; set; }
        protected IEnumerable<RequestRemarksDetailVM> RequestRemarksDetailVM { get; set; }
        protected TaskDetailVM taskDetailVM;
        List<Notification> notifications = new List<Notification>();
        public List<SaveTaskVM> AddedTaskList { get; set; } = new List<SaveTaskVM>();
        protected UserVM ManagerLeaveAndAttandance { get; set; } = new UserVM();
        protected string MangeerLeaveAndDutyId { get; set; }
        protected string NotificationUrl { get; set; }

        protected string PageHeading = string.Empty;
        protected int CreatedByNotificatiionEntityId { get; set; }
        protected int ReceiverNotificatiionEntityId { get; set; }
        protected string ServiceRequestTitle { get; set; }
        protected string TaskName { get; set; }
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            ServiceRequestId = Guid.Parse(ServiceRequestId);
            RequestTypeId = int.Parse(RequestTypeId);
            SetRequestInitialValues();
            await Load();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            spinnerService.Show();
            await GetServiceRequestDetailById();
            await GetLeaveAttendanceRequestDetailById();
            await GetLeaveAttendanceRequestRemarksById();
            await PopulateUsersListBySectorAndRoleId();
            if (TaskId != null)
            {
                await PopulateTaskDetails();
                await GetManagerTaskReminderData();
            }
            //await PopulateUsersListBySector(); //no more required need to delete this
            spinnerService.Hide();

        }
        #endregion

        #region Functions
        protected async Task PopulateUsersListBySectorAndRoleId()
        {
            var userresponse = await cmsSharedService.GetUsersByRoleAndSector(SystemRoles.Manager, (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment);
            if (userresponse.IsSuccessStatusCode)
            {
                List<SectorUsersVM> users = (List<SectorUsersVM>)userresponse.ResultData;
                MangeerLeaveAndDutyId = users.Select(u => u.UserId).FirstOrDefault();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        //protected async Task PopulateUsersListBySector()
        //{
        //    var userresponse = await lookupService.GetUsersBySector((int)OperatingSectorTypeEnum.LeaveAndDutyDepartment);
        //    if (userresponse.IsSuccessStatusCode)
        //    {
        //        IEnumerable<UserVM> users = (IEnumerable<UserVM>)userresponse.ResultData;
        //        ManagerLeaveAndAttandance = users.FirstOrDefault();
        //    }
        //    else
        //    {
        //        await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
        //    }
        //}
        protected async Task PopulateTaskDetails()
        {
            try
            {
                var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(TaskId));
                if (getTaskDetail.IsSuccessStatusCode && getTaskDetail.ResultData is not null)
                {
                    taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(getTaskDetail);
                    navigationManager.NavigateTo($"usertask-list/{(int)TaskStatusEnum.Pending}");
                    return;
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        private void SetRequestInitialValues()
        {
            switch (RequestTypeId)
            {
                case (int)ServiceRequestTypeEnum.SubmitaLeaveRequest:
                    PageHeading = translationState.Translate("Leave_Request_Detail");
                    NotificationUrl = "leave-request";
                    ServiceRequestTitle = "LeaveRequest";
                    TaskName = translationState.Translate("Leave_Request_Send_For_Approval");
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours:
                    PageHeading = translationState.Translate("Reduce_Working_Hours_Request_Detail");
                    NotificationUrl = "reduceworkinghours-request";
                    ServiceRequestTitle = "RequestToReduceWorkingHours";
                    TaskName = translationState.Translate("Reduce_Working_Hours_Send_For_Approval");
                    break;

                case (int)ServiceRequestTypeEnum.RequestForFingerprintExemption:
                    PageHeading = translationState.Translate("Fingerprint_Exemption_Request_Detail");
                    NotificationUrl = "fingerprintexemption-request";
                    ServiceRequestTitle = "RequestForFingerprintExemption";
                    TaskName = translationState.Translate("Fingerprint_Exemption_Send_For_Approval");
                    break;

                case (int)ServiceRequestTypeEnum.SubmitaRequestforPermission:
                    PageHeading = translationState.Translate("Permission_Request_Detail");
                    NotificationUrl = "permission-request";
                    ServiceRequestTitle = "RequestforPermission";
                    TaskName = translationState.Translate("Permission_Request_Send_For_Approval");
                    break;

                case (int)ServiceRequestTypeEnum.RequestforAppointmentwithMedicalCouncil:
                    PageHeading = translationState.Translate("Appointment_With_Medical_Council_Request_Detail");
                    NotificationUrl = "appointment-medicalcouncil-request";
                    ServiceRequestTitle = "RequestForAppointmentWithMedicalCouncil";
                    TaskName = translationState.Translate("Appointment_With_Medical_Council_Request_Send_For_Approval");
                    break;
            }
        }

        private async Task GetServiceRequestDetailById()
        {
            var response = await serviceRequestSharedService.GetServiceRequestDetailById(ServiceRequestId);

            if (response.IsSuccessStatusCode)
                ServiceRequestDetail = (ServiceRequestVM)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetLeaveAttendanceRequestDetailById()
        {
            var response = await leaveAndAttendanceRequestService.GetLeaveAttendanceRequestDetailById(ServiceRequestId);

            if (response.IsSuccessStatusCode)
                LeaveAttendanceRequestDetail = (LeaveAttendanceRequestDetailVM)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetLeaveAttendanceRequestRemarksById()
        {
            var response = await serviceRequestSharedService.GetServiceRequestRemarks(LeaveAttendanceRequestDetail.Id);

            if (response.IsSuccessStatusCode)
                RequestRemarksDetailVM = (IEnumerable<RequestRemarksDetailVM>)response.ResultData!;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task UpdateTaskStatus()
        {
            var taskResponse = await taskService.TaskUpdateWithService(ServiceRequestId);
            if (!taskResponse.IsSuccessStatusCode)
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
            }
        }

        #endregion

        #region Buttons

        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");

        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Approve Request
        protected async Task UpdateServiceRequestStatus(int statusId)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                         translationState.Translate("Sure_Approve"),
                         translationState.Translate("Confirm"),
                         new ConfirmOptions()
                         {
                             OkButtonText = @translationState.Translate("Yes"),
                             CancelButtonText = @translationState.Translate("No")
                         });

                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        var response = await serviceRequestSharedService.UpdateServiceRequestStatus(ServiceRequestId, statusId);
                        if (response.IsSuccessStatusCode)
                        {
                            await UpdateTaskStatus();

                            // notification to user who created request
                            var createdby = loginState.UserDetail.UserName;
                            var userId = loginState.UserDetail.UserId;
                            string entityId = $"{RequestTypeId}/{taskDetailVM.ReferenceId}";
                            await FillNotificationList(createdby, taskDetailVM.AssignedBy, (int)NotificationEventEnum.LeaveRequestApproved, "detail", NotificationUrl, entityId, ServiceRequestDetail.ServiceRequestNumber, ServiceRequestTitle);

                            // notification to Leave and attendance manager
                            var taskId = Guid.NewGuid();

                            await FillNotificationList(createdby, MangeerLeaveAndDutyId, (int)NotificationEventEnum.LeaveRequestApproved, "detail", NotificationUrl, $"{entityId}/{taskId}", ServiceRequestDetail.ServiceRequestNumber, ServiceRequestTitle);
                            string taskurl = $"{NotificationUrl}-detail/{entityId}";
                            await FillAddedTask(taskId, NotificationUrl, TaskName, createdby, new Guid(userId), MangeerLeaveAndDutyId, taskurl, (Guid)taskDetailVM.ReferenceId, "");

                            var notificationResponse = await notificationDetailService.SendNotification(notifications);
                            var taskResponse = await taskService.AddTaskList(AddedTaskList);
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Service_Request_Approved_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            navigationManager.NavigateTo("usertask-list");
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                }
            }
            catch (Exception ex)
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

        #region Add Service Request Remark
        protected async Task AddServcieRequestRemarks(int remarkType, int requestStatusId)
        {
            try
            {
                string title = "";
                switch (remarkType)
                {
                    case (int)RemarkTypeEnum.Rejected:
                        title = "Reject_Leave_Request";
                        break;

                    case (int)RemarkTypeEnum.NeedModification:
                        title = "Need_Modification";
                        break;
                }
                var dialogResponse = await dialogService.OpenAsync<RequestDecisionPopUp>(translationState.Translate(title),
                new Dictionary<string, object>()
                {
                    { "ReferenceId", LeaveAttendanceRequestDetail.Id },
                    { "RemarkType", remarkType },
                    { "ServiceRequestId", ServiceRequestId },
                    { "ServiceRequestStatus", requestStatusId },
                },
                new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = true });
                if (dialogResponse is not null)
                {
                    if (remarkType == (int)(int)RemarkTypeEnum.Rejected)
                    {
                        await FillNotificationList(loginState.UserDetail.UserName, taskDetailVM.AssignedBy, (int)NotificationEventEnum.LeaveRequestRejected, "detail", NotificationUrl, $"{RequestTypeId}/{taskDetailVM.ReferenceId}", ServiceRequestDetail.ServiceRequestNumber, ServiceRequestTitle);
                    }
                    else
                    {
                        await FillNotificationList(loginState.UserDetail.UserName, taskDetailVM.AssignedBy, (int)NotificationEventEnum.LeaveRequestNeedModification, "detail", NotificationUrl, $"{RequestTypeId}/{taskDetailVM.ReferenceId}", ServiceRequestDetail.ServiceRequestNumber, ServiceRequestTitle);
                    }
                    var notificationResponse = await notificationDetailService.SendNotification(notifications);
                    await UpdateTaskStatus();
                    navigationManager.NavigateTo("usertask-list");
                }
            }
            catch (Exception ex)
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

        #region  send Notification List
        protected async Task FillNotificationList(string ceatedBy, string receiverId, int eventId, string action, string entityName, string entityId, string referenceNumber, string NotificationTitle)
        {
            var notification = new Notification()
            {
                CreatedBy = ceatedBy,
                NotificationId = Guid.NewGuid(),
                DueDate = DateTime.Now.AddDays(5),
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                ReceiverId = receiverId,
                ModuleId = (int)WorkflowModuleEnum.LeaveAndAttendance,
                EventId = eventId, // (int)NotificationEventEnum.SubmitServiceRequest,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                NotificationParameter = new NotificationParameter
                {
                    Entity = NotificationTitle,
                    ServiceRequestNumber = referenceNumber
                }
            };
            notifications.Add(notification);
        }
        #endregion

        #region Fill Task Added List and process log
        protected async Task FillAddedTask(Guid taskId, string actionName, string taskName, string createdBy, Guid? assignedBy, string assignedTo, string url, Guid referenceId, string description = null)
        {
            //var taskId = Guid.NewGuid();
            var saveTask = new SaveTaskVM()
            {
                Task = new UserTask() { DueDate = DateTime.Now.Date, Date = DateTime.Now.Date },
                TaskActions = new List<TaskAction>(),
                DeletedTaskActionIds = new List<Guid>(),
            };
            saveTask.Task.Url = url + "/" + taskId;
            saveTask.Task.Name = taskName;
            saveTask.Task.TaskId = taskId;
            saveTask.Task.IsDeleted = false;
            saveTask.Task.CreatedBy = createdBy;
            saveTask.Task.AssignedTo = assignedTo;
            saveTask.Task.ReferenceId = referenceId;
            saveTask.Task.Description = description;
            saveTask.Task.IsSystemGenerated = true;
            saveTask.Task.CreatedDate = DateTime.Now;
            saveTask.Task.TypeId = (int)TaskTypeEnum.Task;
            saveTask.Task.AssignedBy = assignedBy.ToString();
            saveTask.Task.PriorityId = (int)PriorityEnum.Medium;
            saveTask.Task.TaskStatusId = (int)TaskStatusEnum.Pending;
            saveTask.Task.RoleId = SystemRoles.FatwaAdmin; //FATWA ADMIN
            saveTask.Task.DepartmentId = (int)DepartmentEnum.Operational;
            saveTask.Task.ModuleId = (int)WorkflowModuleEnum.ServiceRequest;
            saveTask.Task.SubModuleId = (int)SubModuleEnum.LeaveAndAttendance;
            saveTask.Task.SectorId = (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment;
            saveTask.Task.SystemGenTypeId = (int)TaskSystemGenTypeEnum.LeaveAttendanceRequest;
            saveTask.TaskActions = new List<TaskAction>() { new TaskAction() { ActionName = actionName, TaskId = taskId, } };
            AddedTaskList.Add(saveTask);
        }
        protected async Task CreateProcessLog(string requestTitle, string taskName, string Description, string message)
        {
            var processLog = new ProcessLog
            {
                //Process = requestTitle,
                //Task = taskName,
                //Description = Description,
                //ProcessLogEventId = (int)ProcessLogEnum.Processed,
                //Message = message,
                //IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                //ApplicationID = (int)PortalEnum.OSSPortal,
                //ModuleID = (int)WorkflowModuleEnum.OrganizingCommittee,
                //Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };
        }

        #endregion
        #region Get Manager Task Reminder Data
        protected async Task GetManagerTaskReminderData()
        {
            try
            {
                var response = await lookupService.GetManagerTaskReminderData(Guid.Parse(TaskId));
                if (response.IsSuccessStatusCode)
                {
                    managerTaskReminderData = (List<ManagerTaskReminderVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
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
    }
}

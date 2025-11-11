using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.InventoryManagement;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.ServiceRequests.Shared;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.ServiceRequests.InventoryRequests
{
    public partial class DetailReturnRequest : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic ServiceRequestId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        #endregion

        #region Inject Services
        [Inject]
        public ProcessLogService processLogService { get; set; }
        [Inject]
        public ErrorLogService errorLogService { get; set; }

        #endregion


        #region Variable

        protected IEnumerable<RequestRemarksDetailVM> RequestRemarksDetailVM { get; set; }  = new List<RequestRemarksDetailVM>();
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        protected ReturnItemVM ReturnItemVM { get; set; } = new ReturnItemVM();
        protected ServiceReqTaskNotificationDto sReqTaskNotifiDto { get; set; } = new ServiceReqTaskNotificationDto();

        protected List<ReturnItemVM> ReturnItemList { get; set; } = new List<ReturnItemVM>();
        protected RadzenDataGrid<ReturnItemVM>? grid1 = new RadzenDataGrid<ReturnItemVM>();


        protected RadzenDataGrid<ServiceRequestItemsDetailVM>? grid = new RadzenDataGrid<ServiceRequestItemsDetailVM>();
        protected RadzenDataGrid<IssueItemsVM>? grid2 = new RadzenDataGrid<IssueItemsVM>();
        protected ServiceRequestVM ServiceRequestDetail { get; set; } = new ServiceRequestVM();
        protected List<ServiceRequestItemsDetailVM> ServiceRequestItemsList { get; set; } = new List<ServiceRequestItemsDetailVM>();
        // protected List<IssueItemsVM> IssueItemsList { get; set; } = new List<IssueItemsVM>();

        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            if (TaskId is not null)
                await PopulateTaskDetails();

            await GetServiceRequestDetailById();
            //await GetInventoryRequestItems();
            //await GetIssueItemsById(serviceRequestId);
            await GetReturnItems();
            await GetRejectReasons();
        }
        #endregion

        #region Functions
        private async Task GetRejectReasons()
        {
            var response = await serviceRequestSharedService.GetServiceRequestRemarks(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
                RequestRemarksDetailVM = (IEnumerable<RequestRemarksDetailVM>)response.ResultData!;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }
        protected async Task GetReturnItems()
        {
            var response = await inventoryRequestService.GetReturnItem(Guid.Parse(ServiceRequestId));
            if (response.IsSuccessStatusCode)
            {
                ReturnItemList = (List<ReturnItemVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateTaskDetails()
        {
            if (TaskId != null)
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
        }
        private async Task GetServiceRequestDetailById()
        {
            var response = await serviceRequestSharedService.GetServiceRequestDetailById(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
                ServiceRequestDetail = (ServiceRequestVM)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        //private async Task GetInventoryRequestItems()
        //{
        //    var response = await inventoryRequestService.GetInventoryRequestItems(Guid.Parse(ServiceRequestId));

        //    if (response.IsSuccessStatusCode)
        //        ServiceRequestItemsList = (List<ServiceRequestItemsDetailVM>)response.ResultData;
        //    else
        //        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        //}

        //private async Task GetIssueItemsById(Guid id)
        //{
        //    var response = await inventoryRequestService.GetIssueItemsById(id);

        //    if (response.IsSuccessStatusCode && response.ResultData is not null)
        //        IssueItemsList = (List<IssueItemsVM>)response.ResultData;
        //    else
        //        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        //}

        #endregion

        #region Update Service Request Status
        //protected async Task UpdateServiceRequestStatus(int statusId)
        //{
        //    try
        //    {
        //        bool? dialogResponse = await dialogService.Confirm(
        //                 translationState.Translate("Sure_Approve"),
        //                 translationState.Translate("Confirm"),
        //                 new ConfirmOptions()
        //                 {
        //                     OkButtonText = @translationState.Translate("Yes"),
        //                     CancelButtonText = @translationState.Translate("No")
        //                 });

        //        if (dialogResponse != null)
        //        {
        //            if ((bool)dialogResponse)
        //            {
        //                // var response = await serviceRequestSharedService.UpdateServiceRequestStatus(Guid.Parse(ServiceRequestId), statusId);
        //                // fill Task For Custodain user
        //                if (statusId == (int)(int)ServiceRequestStatusEnum.ApprovedbyHOS)
        //                {
        //                    await FillTaskAndNotification(statusId, translationState.Translate("Approved"), ServiceRequestDetail.CustodianId);

        //                }
        //                else
        //                {
        //                    await FillTaskAndNotification(statusId, translationState.Translate("Rejected"), ServiceRequestDetail.RequestorId);

        //                }
        //                var response = await serviceRequestSharedService.UpdateServiceRequestStatus(sReqTaskNotifiDto);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    await UpdateTaskStatus();
        //                    var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;

        //                    if (apiResponse.addedTaskList.Count() > 0)
        //                    {
        //                        await SaveMemberTasks(apiResponse.addedTaskList);
        //                    }
        //                    if (apiResponse.sendNotifications.Count > 0)
        //                    {
        //                        await CreateSystemNotification(apiResponse.sendNotifications);
        //                    }
        //                    if (apiResponse.processLog != null)
        //                    {
        //                        await CreateProcessLog(apiResponse.processLog);
        //                    }

        //                    notificationService.Notify(new NotificationMessage()
        //                    {
        //                        Severity = NotificationSeverity.Success,
        //                        Detail = translationState.Translate("Service_Request_Approved_Successfully"),
        //                        Style = "position: fixed !important; left: 0; margin: auto; "
        //                    });
        //                    navigationManager.NavigateTo("usertask-list");
        //                }
        //                else
        //                {
        //                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        notificationService.Notify(new NotificationMessage()
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
        //            Style = "position: fixed !important; left: 0; margin: auto; "
        //        });
        //    }
        //}

        protected async Task ApproveReturnInventoryRequest(int statusId)
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
                await FillTaskAndNotification(statusId, translationState.Translate("Approved"), ServiceRequestDetail.CustodianId);
                var response = await serviceRequestSharedService.UpdateServiceRequestStatus(sReqTaskNotifiDto);
                if (response.IsSuccessStatusCode)
                {
                    await UpdateTaskStatus();
                    var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;

                    if (apiResponse.addedTaskList.Any())
                    {
                        await SaveMemberTasks(apiResponse.addedTaskList);
                    }
                    if (apiResponse.sendNotifications.Any())
                    {
                        await CreateSystemNotification(apiResponse.sendNotifications);
                    }
                    if (apiResponse.processLog != null)
                    {
                        await CreateProcessLog(apiResponse.processLog);
                    }

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
            else
            {
                return;
            }
        }
        protected async Task RejectReturnInventoryRequest(int statusId)
        {
            var dialogResponse = await dialogService.OpenAsync<RequestDecisionPopUp>(translationState.Translate("Rejection_Reason"),
               new Dictionary<string, object>()
               {
                    { "ReferenceId", Guid.Parse(ServiceRequestId)},
                    { "RemarkType", (int)RemarkTypeEnum.Rejected },
                    { "ServiceRequestId", Guid.Parse(ServiceRequestId)},
                    { "ServiceRequestStatus", statusId },
               },
               new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = true });

            //bool? dialogResponse = await dialogService.Confirm(
            //           translationState.Translate("Sure_Reject"),
            //           translationState.Translate("Confirm"),
            //           new ConfirmOptions()
            //           {
            //               OkButtonText = @translationState.Translate("Yes"),
            //               CancelButtonText = @translationState.Translate("No")
            //           });

            if (dialogResponse != null)
            {
                await FillTaskAndNotification(statusId, translationState.Translate("Rejected"), ServiceRequestDetail.RequestorId);
                var response = await serviceRequestSharedService.UpdateServiceRequestStatus(sReqTaskNotifiDto);
                if (response.IsSuccessStatusCode)
                {
                    await UpdateTaskStatus();
                    var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;
                    if (apiResponse.sendNotifications.Count > 0)
                    {
                        await CreateSystemNotification(apiResponse.sendNotifications);
                    }
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Service_Request_Rejected_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    navigationManager.NavigateTo("usertask-list");
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            else
            {
                return;
            }
        }
        #endregion

        private async Task UpdateTaskStatus()
        {
            var taskResponse = await taskService.TaskUpdateWithService(Guid.Parse(ServiceRequestId));
            if (!taskResponse.IsSuccessStatusCode)
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
            }
        }

        #region Fill task and notification Vm
        protected async Task FillTaskAndNotification(int statusId, string actionName, string assignToId)
        {
            if (taskDetailVM != null)
            {
                sReqTaskNotifiDto.Url = $"detail-return-servicerequest/{ServiceRequestId}/";
                sReqTaskNotifiDto.TaskName = taskDetailVM.Name;
                sReqTaskNotifiDto.AssignToId = assignToId;
                sReqTaskNotifiDto.AssignById = loginState.UserDetail.UserId;
                sReqTaskNotifiDto.CreatedBy = loginState.UserDetail.UserName;
                sReqTaskNotifiDto.ServiceReqId = ServiceRequestDetail.ServiceRequestId;
                sReqTaskNotifiDto.ActionName = actionName;
                sReqTaskNotifiDto.RequestStatusId = statusId;
                sReqTaskNotifiDto.ServiceReqNumber = ServiceRequestDetail.ServiceRequestNumber;
            }
        }
        #endregion
        protected async Task SaveMemberTasks(List<SaveTaskVM> requestTasks)
        {
            try
            {
                List<SaveTaskVM> tasks = new List<SaveTaskVM>();
                foreach (var task in requestTasks)
                {
                    task.Task.SubModuleId = null;
                    task.Task.SystemGenTypeId = null;
                    tasks.Add(task);
                }
                var response = await taskService.AddTaskList(tasks);
                if (!response.IsSuccessStatusCode)
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #region Buttons
        protected async Task ServiceRequestItemDetail(ServiceRequestItemsDetailVM args)
        {
            navigationManager.NavigateTo("detail-serviceRequestItem/" + args.ServiceRequestId + "/" + args.ServiceRequestItemId + "/" + ServiceRequestDetail.ServiceRequestStatusId);
        }
        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        protected async Task CreateProcessLog(ProcessLog processLog)
        {
            try
            {
                var response = await processLogService.CreateProcessLog(processLog);
                if (!response.IsSuccessStatusCode)
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        protected async Task CreateErrorLog(ErrorLog errorLog)
        {
            try
            {
                var response = await errorLogService.CreateErrorLog(errorLog);
                if (!response.IsSuccessStatusCode)
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        protected async Task CreateSystemNotification(List<Notification> notifications)
        {
            try
            {
                var notificationResponse = await notificationDetailService.SendNotification(notifications);
                if (!notificationResponse.IsSuccessStatusCode)
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
    }
}

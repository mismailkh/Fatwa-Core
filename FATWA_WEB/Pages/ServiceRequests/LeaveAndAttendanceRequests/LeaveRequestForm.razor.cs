using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendance;
using FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Syncfusion.Blazor.RichTextEditor;
using System.ComponentModel;
using System.Net;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_WEB.Pages.ServiceRequests.LeaveAndAttendanceRequests
{
    public partial class LeaveRequestForm : ComponentBase
    {
        #region Paramters
        [Parameter]
        public dynamic RequestTypeId { get; set; }
        [Parameter]
        public dynamic SectorId { get; set; }
        [Parameter]
        public dynamic ServiceRequestId { get; set; }

        [Inject]
        public ProcessLogService processLogService { get; set; }
        [Inject]
        public ErrorLogService errorLogService { get; set; }
        #endregion

        #region Variables
        IEnumerable<UserBasicDetailVM> Employees;
        List<LeaveType> LeaveTypes;
        List<WeekdaysSetting> WeekdaysSettings;
        List<WeekdaysSetting> WorkingDays;
        protected IEnumerable<PermissionType> permissionTypes;
        protected LeaveAttendanceRequest LeaveAttendanceRequest = new LeaveAttendanceRequest();
        LeaveAttendanceRequestDetailVM LeaveAttendanceRequestDetail;
        IEnumerable<ExemptionTime> ExemptionTimes;
        IEnumerable<ExemptionType> ExemptionTypes;
        IEnumerable<Reason> ReduceHourReasons;
        WorkingHour WorkingHour = new WorkingHour();
        DateTime EpStartTime { get; set; }
        DateTime EpEndTime { get; set; }
        string ManagerId = null;


        int? LeaveBalance = null;
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();
        protected string RequestNumber = null!;
        protected bool isSaved;
        public event PropertyChangedEventHandler PropertyChanged;
        protected string PageHeading = string.Empty;
        protected string action = string.Empty;
        int requestTypeId = 0;
        bool isOtherFieldDisabled = false;
        protected string DescriptionEnValidationMsg = "";
        protected string StartTimeValidationMsg = "";
        protected string EndTimeValidationMsg = "";
        protected string DaysOfAbsenceValidationMsg = "";
        public List<PublicHolidaysVM> PublicHolidays { get; set; }
        int daysDuration = 1;
        int PublicHolidayCount = 0;
        int WeekendsDaysCount = 0;
        #endregion

        #region Constructor
        public LeaveRequestForm()
        {
            ReduceHourReasons = new List<Reason>();
            Employees = new List<UserBasicDetailVM>();
            ExemptionTimes = new List<ExemptionTime>();
            ExemptionTypes = new List<ExemptionType>();
            LeaveAttendanceRequest = new LeaveAttendanceRequest();
            LeaveTypes = new List<LeaveType>();
            WeekdaysSettings = new List<WeekdaysSetting>();

            LeaveAttendanceRequest = new LeaveAttendanceRequest
            {
                Id = Guid.NewGuid(),
                ServiceRequest = new ServiceRequest
                {
                    ServiceRequestId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    ServiceRequestNumber = RequestNumber
                },
            };
            LeaveAttendanceRequest.ServiceRequestId = LeaveAttendanceRequest.ServiceRequest.ServiceRequestId;
            PublicHolidays = new List<PublicHolidaysVM>();
        }
        #endregion

        #region On Load Component 
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await GetManagerByuserId();
            if (ManagerId is null)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("No_Manager_Found_Contact_Administrator"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            requestTypeId = Convert.ToInt32(RequestTypeId);
            await Load();

            spinnerService.Hide();
        }

        #endregion

        #region DropDowns
        private async Task GetExemptionType()
        {
            var response = await leaveAndAttendanceRequestService.GetExemptionType();
            if (response.IsSuccessStatusCode)
            {
                ExemptionTypes = (List<ExemptionType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        private async Task GetExemptionTime()
        {
            var response = await leaveAndAttendanceRequestService.GetExemptionTime();
            if (response.IsSuccessStatusCode)
            {
                ExemptionTimes = (List<ExemptionTime>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetEmployeeList()
        {
            // var response = await userService.GetEmployeeList(new UserListAdvanceSearchVM());
            var response = await userService.GetActiveUsersBySectorTypeId();
            if (response.IsSuccessStatusCode)
            {
                Employees = (IEnumerable<UserBasicDetailVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetLeaveTypes()
        {
            var response = await leaveAndAttendanceRequestService.GetLeaveTypeLkps(new LeaveTypeAdvanceSearchVM());
            if (response.IsSuccessStatusCode)
            {
                LeaveTypes = (List<LeaveType>)response.ResultData;
                if (loginState.UserDetail.GenderId == (int)GenderEnum.Male)
                    LeaveTypes.Remove(LeaveTypes.Where(x => x.Id == (int)LeaveTypeEnum.MaternityLeave).FirstOrDefault());
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetWeekdaysSettings()
        {
            var response = await lookupService.GetWeekdaysSettings();
            if (response.IsSuccessStatusCode)
            {
                WeekdaysSettings = (List<WeekdaysSetting>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetPermissionTypes()
        {
            var response = await leaveAndAttendanceRequestService.GetPermissionTypes();
            if (response.IsSuccessStatusCode)
            {
                permissionTypes = (IEnumerable<PermissionType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetReduceHourReasons()
        {
            var response = await leaveAndAttendanceRequestService.GetReduceHourReasons();
            if (response.IsSuccessStatusCode)
            {
                ReduceHourReasons = (List<Reason>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #endregion

        #region Load
        protected async Task Load()
        {
            SetRequestInitialValues();
            await GetPublicHolidays();
            await PopulateDropDowns();
            if (ServiceRequestId is null)
            {
                await GetLatestServiceRequestNumber();
            }
            else
            {
                await GetLeaveAttendanceRequestDetailById();
            }
        }
        #endregion

        #region Functions
        private async Task GetEmployeeWorkingHours()
        {
            var response = await userService.GetEmployeeWorkingHours(loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                WorkingHour = (WorkingHour)response.ResultData;
                EpStartTime = DateTime.Today.Add(WorkingHour.StartTime);
                EpEndTime = DateTime.Today.Add(WorkingHour.EndTime);
                //EpEndTime = DateTime.Now + WorkingHour.EndTime;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetManagerByuserId()
        {
            var response = await userService.GetManagerByuserId(loginState.UserDetail.UserId);

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                ManagerId = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task CalculateWorkingDays(DateTime startDate, DateTime endDate, List<PublicHolidaysVM> publicHolidays)
        {
            int workingDays = 0;
            PublicHolidayCount = 0;
            WeekendsDaysCount = 0;
            for (DateTime date = startDate.Date; date <= endDate.Date;)
            {
                bool isPublicHoliday = publicHolidays.Any(publicHoliday => date >= publicHoliday.FromDate.Date && date <= publicHoliday.ToDate.Date);
                var weekday = WeekdaysSettings.FirstOrDefault(x => x.NameEn == date.DayOfWeek.ToString());
                bool isWeekend = weekday.IsWeekend || weekday.IsRestDay;

                if (isWeekend)
                {
                    WeekendsDaysCount++;
                }

                if (isPublicHoliday)
                {
                    PublicHolidayCount++;
                }

                if (isWeekend && isPublicHoliday)
                {
                    PublicHolidayCount--;
                }

                if (!(isWeekend || isPublicHoliday))
                {
                    workingDays++;
                }

                date = date.AddDays(1);
            }
            daysDuration = workingDays;
            StateHasChanged();
        }

        protected async Task GetPublicHolidays()
        {
            var response = await timeIntervalService.GetPublicHolidays();
            if (response.IsSuccessStatusCode)
            {
                PublicHolidays = (List<PublicHolidaysVM>)response.ResultData;
            }
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);

            await InvokeAsync(StateHasChanged);
        }

        private async void SetRequestInitialValues()
        {
            switch (requestTypeId)
            {
                case (int)ServiceRequestTypeEnum.SubmitaLeaveRequest:
                    LeaveAttendanceRequest.StartDate = DateTime.Now.Date;
                    LeaveAttendanceRequest.EndDate = DateTime.Now.Date;
                    LeaveAttendanceRequest.EntityId = (int)NotificationEventEnum.LeaveRequestSubmitted;
                    LeaveAttendanceRequest.NotificationTitle = "LeaveRequest";
                    PageHeading = ServiceRequestId is null ? translationState.Translate("Add_Leave_Request") : translationState.Translate("Edit_Leave_Request");
                    action = "leave-request";
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours:
                    LeaveAttendanceRequest.StartDate = DateTime.Now.Date;
                    LeaveAttendanceRequest.EndDate = DateTime.Now.Date;
                    LeaveAttendanceRequest.EntityId = (int)NotificationEventEnum.LeaveRequestSubmitted;
                    LeaveAttendanceRequest.NotificationTitle = "RequestToReduceWorkingHours";
                    LeaveAttendanceRequest.TaskName = translationState.Translate("Review_Reduce_Working_Hours_Request");
                    PageHeading = ServiceRequestId is null ? translationState.Translate("Add_Reduce_Working_Hours_Request") : translationState.Translate("Edit_Reduce_Working_Hours_Request");
                    action = "reduceworkinghours-request";
                    await GetEmployeeWorkingHours();
                    break;

                case (int)ServiceRequestTypeEnum.RequestForFingerprintExemption:
                    LeaveAttendanceRequest.StartDate = DateTime.Now.Date;
                    LeaveAttendanceRequest.EndDate = DateTime.Now.Date;
                    LeaveAttendanceRequest.EntityId = (int)NotificationEventEnum.LeaveRequestSubmitted;
                    LeaveAttendanceRequest.NotificationTitle = "RequestForFingerprintExemption";
                    LeaveAttendanceRequest.TaskName = translationState.Translate("Review_Fingerprint_Exemption_Request");
                    PageHeading = ServiceRequestId is null ? translationState.Translate("Add_Fingerprint_Exemption_Request") : translationState.Translate("Edit_Fingerprint_Exemption_Request");
                    action = "fingerprintexemption-request";
                    break;

                case (int)ServiceRequestTypeEnum.SubmitaRequestforPermission:
                    LeaveAttendanceRequest.NotificationTitle = "RequestforPermission";
                    LeaveAttendanceRequest.EntityId = (int)NotificationEventEnum.LeaveRequestSubmitted;
                    LeaveAttendanceRequest.TaskName = translationState.Translate("Review_Permission_Request");
                    PageHeading = ServiceRequestId is null ? translationState.Translate("Add_Permission_Request") : translationState.Translate("Edit_Permission_Request");
                    action = "permission-request";
                    await GetEmployeeWorkingHours();
                    break;

                case (int)ServiceRequestTypeEnum.RequestforAppointmentwithMedicalCouncil:
                    LeaveAttendanceRequest.NotificationTitle = "RequestForAppointmentWithMedicalCouncil";
                    LeaveAttendanceRequest.EntityId = (int)NotificationEventEnum.LeaveRequestSubmitted;
                    LeaveAttendanceRequest.TaskName = translationState.Translate("Review_Medical_Council_Request");
                    PageHeading = ServiceRequestId is null ? translationState.Translate("Add_Medical_Council_Request") : translationState.Translate("Edit_Medical_Council_Request");
                    action = "appointment-medicalcouncil-request";
                    break;
            }
        }



        public async Task AddSystemGeneratedTask()
        {
            var taskId = Guid.NewGuid();
            var taskResult = await taskService.AddSystemGeneratedTask(new SaveTaskVM
            {
                Task = new UserTask
                {
                    TaskId = taskId,
                    Name = "Leave_Attendance_Request_Created",
                    Description = "",
                    Date = DateTime.Now.Date,
                    AssignedBy = loginState.UserDetail.UserId,
                    AssignedTo = ManagerId,
                    IsSystemGenerated = true,
                    TaskStatusId = (int)TaskStatusEnum.Pending,
                    ModuleId = (int)WorkflowModuleEnum.ServiceRequest,
                    SectorId = (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment,
                    DepartmentId = (int)DepartmentEnum.Operational,
                    TypeId = (int)TaskTypeEnum.Task,
                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                    CreatedBy = loginState.Username,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReferenceId = LeaveAttendanceRequest.ServiceRequestId,
                    SystemGenTypeId = null,
                },
                TaskActions = new List<TaskAction>()
                {
                    new TaskAction()
                    {
                        ActionName = "Leave Request",
                        TaskId = taskId,
                    }
                }
            },
                action,
                "detail",
                    $"{requestTypeId + "/"}" + LeaveAttendanceRequest.ServiceRequestId.ToString() + "/" + taskId.ToString());
            if (!taskResult.IsSuccessStatusCode)
                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResult);
        }
        private async Task PopulateDropDowns()
        {
            switch (requestTypeId)
            {
                case (int)ServiceRequestTypeEnum.SubmitaLeaveRequest:
                    await GetLeaveTypes();
                    await GetWeekdaysSettings();
                    await GetEmployeeList();
                    await CalculateWorkingDays((DateTime)LeaveAttendanceRequest.StartDate, (DateTime)LeaveAttendanceRequest.EndDate, PublicHolidays);
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours:
                    //await GetWorkingDays();
                    await GetReduceHourReasons();
                    await GetWeekdaysSettings();
                    if (WeekdaysSettings is not null)
                        WorkingDays = WeekdaysSettings.Where(x => x.IsWeekend == false && x.IsRestDay == false).ToList();
                    await CalculateWorkingDays((DateTime)LeaveAttendanceRequest.StartDate, (DateTime)LeaveAttendanceRequest.EndDate, PublicHolidays);
                    break;


                case (int)ServiceRequestTypeEnum.RequestForFingerprintExemption:
                    await GetExemptionType();
                    await GetWeekdaysSettings();
                    await GetExemptionTime();
                    break;

                case (int)ServiceRequestTypeEnum.SubmitaRequestforPermission:
                    await GetPermissionTypes();
                    await GetEmployeeList();
                    break;

                case (int)ServiceRequestTypeEnum.RequestforAppointmentwithMedicalCouncil:
                    break;
            }
        }

        void SetTaskAndNotificationValues()
        {
            LeaveAttendanceRequest.TaskActionName = "View";
            // LeaveAttendanceRequest.TaskName = PageHeading;
            LeaveAttendanceRequest.TaskUrl = $"{action}-detail/{requestTypeId + "/"}" + LeaveAttendanceRequest.ServiceRequestId.ToString() + "/";
            LeaveAttendanceRequest.ServiceRequestTitle = action;
        }

        private async Task GetLatestServiceRequestNumber()
        {
            var response = await serviceRequestSharedService.GetLatestServiceRequestNumber(Convert.ToInt32(RequestTypeId));
            if (response.IsSuccessStatusCode)
            {
                RequestNumber = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetLeaveAttendanceRequestDetailById()
        {
            var response = await leaveAndAttendanceRequestService.GetLeaveAttendanceRequestDetailById(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
            {
                LeaveAttendanceRequestDetail = (LeaveAttendanceRequestDetailVM)response.ResultData;
                LeaveAttendanceRequest = mapper.Map<LeaveAttendanceRequest>(LeaveAttendanceRequestDetail);
                if (LeaveAttendanceRequest.LeaveTypeId is not null)
                    LeaveBalance = LeaveTypes.Where(x => x.Id == LeaveAttendanceRequest.LeaveTypeId).Select(x => x.LeaveBalance).FirstOrDefault();
                LeaveAttendanceRequest.ServiceRequest = new ServiceRequest
                {
                    CreatedDate = (DateTime)LeaveAttendanceRequestDetail.RequestCreatedDate,
                    ServiceRequestNumber = LeaveAttendanceRequestDetail.ServiceRequestNumber
                };
                if (LeaveAttendanceRequest.StartDate is not null && LeaveAttendanceRequest.EndDate is not null)
                    await CalculateWorkingDays((DateTime)LeaveAttendanceRequest.StartDate, (DateTime)LeaveAttendanceRequest.EndDate, PublicHolidays);
                RequestNumber = LeaveAttendanceRequestDetail.ServiceRequestNumber;
                if (!string.IsNullOrEmpty(LeaveAttendanceRequestDetail.SelectedDaysId))
                {
                    List<int> selectedDays = LeaveAttendanceRequestDetail.SelectedDaysId.Split(',').Select(int.Parse).ToList();
                    LeaveAttendanceRequest.SelectedDays = selectedDays;
                }
            }
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task GetServiceRequestDetailById()
        {
            var response = await serviceRequestSharedService.GetServiceRequestDetailById(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
            {
                LeaveAttendanceRequestDetail = (LeaveAttendanceRequestDetailVM)response.ResultData;
                LeaveAttendanceRequest = mapper.Map<LeaveAttendanceRequest>(LeaveAttendanceRequestDetail);

                LeaveAttendanceRequest.ServiceRequest = new ServiceRequest
                {
                    CreatedDate = (DateTime)LeaveAttendanceRequestDetail.RequestCreatedDate,
                    ServiceRequestNumber = LeaveAttendanceRequestDetail.ServiceRequestNumber
                };
                RequestNumber = LeaveAttendanceRequestDetail.ServiceRequestNumber;
            }
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { LeaveAttendanceRequest.Id },
                    CreatedBy = loginState.Username,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
                });

                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }

        protected async Task CreateSystemNotification(List<Notification> notifications)
        {

            try
            {
                var notificationResponse = await notificationDetailService.SendNotification(notifications);
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
                if (response.IsSuccessStatusCode)
                {

                }
                else
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

        protected async Task CreateProcessLog(ProcessLog processLog)
        {
            try
            {
                var response = await processLogService.CreateProcessLog(processLog);
                if (response.IsSuccessStatusCode)
                {

                }
                else
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
                if (response.IsSuccessStatusCode)
                {

                }
                else
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
        #endregion

        #region OnChange
        void OnChangeStartDate()
        {
            LeaveAttendanceRequest.EndDate = LeaveAttendanceRequest.StartDate;
            CalculateWorkingDays((DateTime)LeaveAttendanceRequest.StartDate, (DateTime)LeaveAttendanceRequest.EndDate, PublicHolidays);
        }

        void OnChangeDate()
        {
            CalculateWorkingDays((DateTime)LeaveAttendanceRequest.StartDate, (DateTime)LeaveAttendanceRequest.EndDate, PublicHolidays);
        }

        protected void OnChangeLeaveType()
        {
            if (LeaveAttendanceRequest.LeaveTypeId is not null)
                LeaveBalance = LeaveTypes.Where(x => x.Id == LeaveAttendanceRequest.LeaveTypeId).Select(x => x.LeaveBalance).Single();
            else
                LeaveBalance = null;
        }
        protected void OnChangeReasonType()
        {
            if (LeaveAttendanceRequest.ReduceHoursReasonId is not null)
            {
                if (LeaveAttendanceRequest.ReduceHoursReasonId == (int)ReasonReduceWorkingHoursEnum.Other)
                {
                    isOtherFieldDisabled = true;
                }
                else
                {
                    isOtherFieldDisabled = false;
                    LeaveAttendanceRequest.OtherLeaveReasonType = null;
                }
            }
            else
            {
                isOtherFieldDisabled = false;
                LeaveAttendanceRequest.OtherLeaveReasonType = null;
            }
        }
        #endregion

        #region Add and Update Leave Attendance Request

        private async Task AddLeaveAttendanceRequest(int statusId)
        {
            LeaveAttendanceRequest.ServiceRequest.ServiceRequestNumber = RequestNumber;
            LeaveAttendanceRequest.ServiceRequest.ServiceRequestTypeId = Convert.ToInt32(RequestTypeId);
            LeaveAttendanceRequest.ServiceRequest.ServiceRequestStatusId = statusId;
            LeaveAttendanceRequest.ServiceRequest.CreatedBy = loginState.UserDetail.UserName;
            LeaveAttendanceRequest.ServiceRequestId = LeaveAttendanceRequest.ServiceRequest.ServiceRequestId;
            if (ManagerId is not null)
                LeaveAttendanceRequest.AssignTaskUserId = ManagerId;
            LeaveAttendanceRequest.UserId = new Guid(loginState.UserDetail.UserId);
            LeaveAttendanceRequest.RequestTypeId = requestTypeId.ToString();
            if (LeaveAttendanceRequest.StartDate is not null && LeaveAttendanceRequest.EndDate is not null)
                LeaveAttendanceRequest.TotalDuration = daysDuration;

            if (LeaveAttendanceRequest.IsSubmit)
                SetTaskAndNotificationValues();
            var response = await leaveAndAttendanceRequestService.AddLeaveAttendanceRequest(LeaveAttendanceRequest);
            if (response.IsSuccessStatusCode)
            {
                if (LeaveAttendanceRequest.IsSubmit)
                {
                    await SaveTempAttachementToUploadedDocument();

                    var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;

                    if (apiResponse.addedTaskList.Count() > 0)
                    {
                        await SaveMemberTasks(apiResponse.addedTaskList);
                        //await AddSystemGeneratedTask();
                    }
                    if (apiResponse.sendNotifications.Count > 0)
                    {
                        await CreateSystemNotification(apiResponse.sendNotifications);
                    }
                    if (apiResponse.processLog != null)
                    {
                        await CreateProcessLog(apiResponse.processLog);
                    }
                }
                var message = LeaveAttendanceRequest.IsSubmit ? "Request_Submitted_Successfully" : "Draft_Saved_Successfully";
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate(message),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                navigationManager.NavigateTo("servicerequest-list");
            }
            else
            {
                var errorLog = (ErrorLog)response.ResultData;
                if (errorLog != null)
                {
                    await CreateErrorLog(errorLog);
                }
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task UpdateLeaveAttendanceRequest()
        {
            LeaveAttendanceRequest.ServiceRequest = null;
            if (ManagerId is not null)
                LeaveAttendanceRequest.AssignTaskUserId = ManagerId;
            LeaveAttendanceRequest.UserId = new Guid(loginState.UserDetail.UserId);
            LeaveAttendanceRequest.RequestTypeId = requestTypeId.ToString();
            if (LeaveAttendanceRequest.StartDate is not null && LeaveAttendanceRequest.EndDate is not null)
                LeaveAttendanceRequest.TotalDuration = daysDuration;

            if (LeaveAttendanceRequest.IsSubmit)
            {
                SetTaskAndNotificationValues();
                SetRequestInitialValues();
                if (LeaveAttendanceRequestDetail.ServiceRequestStatusId == (int)ServiceRequestStatusEnum.NeedModification)
                {
                    LeaveAttendanceRequest.IsReSubmit = true;
                    LeaveAttendanceRequest.EntityId = (int)NotificationEventEnum.ServiceRequestResubmitted;
                }
            }

            var response = await leaveAndAttendanceRequestService.UpdateLeaveAttendanceRequest(LeaveAttendanceRequest);
            LeaveAttendanceRequest.ServiceRequest = new ServiceRequest();
            if (response.IsSuccessStatusCode)
            {
                if (LeaveAttendanceRequest.IsSubmit)
                {
                    await SaveTempAttachementToUploadedDocument();
                    var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;
                    // await AddSystemGeneratedTask();
                    if (apiResponse.addedTaskList.Count() > 0)
                    {
                        await SaveMemberTasks(apiResponse.addedTaskList);
                        //await AddSystemGeneratedTask();
                    }
                    if (apiResponse.sendNotifications.Count > 0)
                    {
                        await CreateSystemNotification(apiResponse.sendNotifications);
                    }
                    if (apiResponse.processLog != null)
                    {
                        await CreateProcessLog(apiResponse.processLog);
                    }
                }
                var message = LeaveAttendanceRequest.IsSubmit ? "Request_Submitted_Successfully" : "Draft_Saved_Successfully";
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate(message),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                navigationManager.NavigateTo("servicerequest-list");
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Get Time Duration  
        private TimeSpan? TotalDuration { get; set; }

        // Method to calculate the total duration between StartTime and EndTime
        public void CalculateTotalDuration()
        {
            if (LeaveAttendanceRequest.StartTime.HasValue && LeaveAttendanceRequest.EndTime.HasValue)
            {
                var duration = LeaveAttendanceRequest.EndTime.Value - LeaveAttendanceRequest.StartTime.Value;

                if (duration < TimeSpan.Zero)
                {
                    // If EndTime is earlier than StartTime, add a day
                    duration += TimeSpan.FromDays(1);
                }

                TotalDuration = duration;
                LeaveAttendanceRequest.TotalTimeDuration = duration;
            }
            else
            {
                TotalDuration = null;
            }
        }

        // Method to get the total duration as a string
        public string GetTotalDuration()
        {
            CalculateTotalDuration();
            if (TotalDuration.HasValue)
            {
                var duration = TotalDuration.Value;
                return $"{duration.Hours} hours {duration.Minutes} minutes {duration.Seconds} seconds";
            }
            return string.Empty;
        }
        #endregion 

        #region Save As Draft
        protected async Task SaveAsDraft()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                            translationState.Translate("Are_You_Sure_You_Want_to_Save_Draft"),
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = @translationState.Translate("Yes"),
                                CancelButtonText = @translationState.Translate("No")
                            });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (ServiceRequestId == null)
                    {
                        await AddLeaveAttendanceRequest((int)ServiceRequestStatusEnum.Draft);
                    }
                    else
                        await UpdateLeaveAttendanceRequest();

                    spinnerService.Hide();

                    navigationManager.NavigateTo("servicerequest-list");
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

        #region Form Submit
        protected async Task FormSubmit()
        {
            try
            {
                if (requestTypeId == (int)ServiceRequestTypeEnum.SubmitaRequestforPermission)
                {
                    if (LeaveAttendanceRequest.StartTime is not null && LeaveAttendanceRequest.EndTime is not null)
                    {
                        var startTimeOnly = LeaveAttendanceRequest.StartTime.Value.TimeOfDay;
                        var endTimeOnly = LeaveAttendanceRequest.EndTime.Value.TimeOfDay;
                        if (startTimeOnly < endTimeOnly)
                        {
                            bool? dialogResponse = await dialogService.Confirm(
                                translationState.Translate("Sure_Submit"),
                                translationState.Translate("Confirm"),
                                new ConfirmOptions()
                                {
                                    OkButtonText = @translationState.Translate("Yes"),
                                    CancelButtonText = @translationState.Translate("No")
                                });
                            if (dialogResponse == true)
                            {
                                LeaveAttendanceRequest.IsSubmit = true;
                                spinnerService.Show();
                                if (ServiceRequestId is null)
                                {
                                    await AddLeaveAttendanceRequest((int)ServiceRequestStatusEnum.Submitted);
                                }
                                else
                                {
                                    await UpdateLeaveAttendanceRequest();
                                }
                                spinnerService.Hide();

                                navigationManager.NavigateTo("servicerequest-list");
                            }
                        }
                        else
                        {
                            InValidateStartAndEndTimes();
                        }
                    }
                    else
                    {
                        InValidateStartAndEndTimes();
                    }


                }
                else if (requestTypeId == (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours)
                {
                    if (LeaveAttendanceRequest.StartTime is not null && LeaveAttendanceRequest.EndTime is not null && LeaveAttendanceRequest.SelectedDays is not null)
                    {
                        var startTimeOnly = LeaveAttendanceRequest.StartTime.Value.TimeOfDay;
                        var endTimeOnly = LeaveAttendanceRequest.EndTime.Value.TimeOfDay;
                        if (startTimeOnly < endTimeOnly && ((endTimeOnly - startTimeOnly) <= TimeSpan.FromHours(4)) && LeaveAttendanceRequest.SelectedDays != null)
                        {
                            bool? dialogResponse = await dialogService.Confirm(
                                translationState.Translate("Sure_Submit"),
                                translationState.Translate("Confirm"),
                                new ConfirmOptions()
                                {
                                    OkButtonText = @translationState.Translate("Yes"),
                                    CancelButtonText = @translationState.Translate("No")
                                });
                            if (dialogResponse == true)
                            {
                                LeaveAttendanceRequest.IsSubmit = true;
                                spinnerService.Show();
                                if (ServiceRequestId is null)
                                {
                                    await AddLeaveAttendanceRequest((int)ServiceRequestStatusEnum.Submitted);
                                }
                                else
                                {
                                    await UpdateLeaveAttendanceRequest();
                                }
                                spinnerService.Hide();

                                navigationManager.NavigateTo("servicerequest-list");
                            }
                        }
                        else
                        {
                            InValidateStartAndEndTimes();
                        }
                    }
                    else
                    {
                        InValidateStartAndEndTimes();
                    }

                }
                else if (requestTypeId == (int)ServiceRequestTypeEnum.RequestforAppointmentwithMedicalCouncil)
                {
                    if (LeaveAttendanceRequest.Description != null)
                    {
                        bool? dialogResponse = await dialogService.Confirm(
                            translationState.Translate("Sure_Submit"),
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = @translationState.Translate("Yes"),
                                CancelButtonText = @translationState.Translate("No")
                            });
                        if (dialogResponse == true)
                        {
                            LeaveAttendanceRequest.IsSubmit = true;
                            spinnerService.Show();
                            if (ServiceRequestId is null)
                            {
                                await AddLeaveAttendanceRequest((int)ServiceRequestStatusEnum.Submitted);
                            }
                            else
                            {
                                await UpdateLeaveAttendanceRequest();
                            }
                            spinnerService.Hide();

                            navigationManager.NavigateTo("servicerequest-list");
                        }
                    }
                    else
                    {
                        DescriptionEnValidationMsg = LeaveAttendanceRequest.Description == null ? translationState.Translate("Required_Field") : "";
                    }
                }
                else if (requestTypeId == (int)ServiceRequestTypeEnum.SubmitaLeaveRequest || requestTypeId == (int)ServiceRequestTypeEnum.RequestForFingerprintExemption)
                {
                    bool? dialogResponse = await dialogService.Confirm(
                            translationState.Translate("Sure_Submit"),
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = @translationState.Translate("Yes"),
                                CancelButtonText = @translationState.Translate("No")
                            });
                    if (dialogResponse == true)
                    {
                        LeaveAttendanceRequest.IsSubmit = true;
                        spinnerService.Show();
                        if (ServiceRequestId is null)
                        {
                            await SetTaskName();
                            await AddLeaveAttendanceRequest((int)ServiceRequestStatusEnum.Submitted);
                        }
                        else
                        {
                            await UpdateLeaveAttendanceRequest();
                        }
                        spinnerService.Hide();

                        navigationManager.NavigateTo("servicerequest-list");
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

        #region Cancel Form
        protected async Task CancelForm()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                navigationManager.NavigateTo("servicerequest-list");
            }
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
        #endregion

        #region SyncFusion Rich Text Editor
        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
           new ToolbarItemModel() { Command = ToolbarCommand.Formats },
           new ToolbarItemModel() { Command = ToolbarCommand.Separator },
           new ToolbarItemModel() { Command = ToolbarCommand.Bold },
           new ToolbarItemModel() { Command = ToolbarCommand.Italic },
           new ToolbarItemModel() { Command = ToolbarCommand.Underline },
           new ToolbarItemModel() { Command = ToolbarCommand.Separator },
           new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
           new ToolbarItemModel() { Command = ToolbarCommand.Separator },
           new ToolbarItemModel() { Command = ToolbarCommand.NumberFormatList },
           new ToolbarItemModel() { Command = ToolbarCommand.BulletFormatList },
           new ToolbarItemModel() { Command = ToolbarCommand.Separator },
           new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
           new ToolbarItemModel() { Command = ToolbarCommand.Image },
           new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
           new ToolbarItemModel() { Command = ToolbarCommand.InsertCode },
           new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
           new ToolbarItemModel() { Command = ToolbarCommand.Separator },
           new ToolbarItemModel() { Command = ToolbarCommand.Undo },
           new ToolbarItemModel() { Command = ToolbarCommand.Redo }
        };
        private List<TableToolbarItemModel> TableQuickToolbarItems = new List<TableToolbarItemModel>()
        {
           new TableToolbarItemModel() { Command = TableToolbarCommand.TableHeader },
           new TableToolbarItemModel() { Command = TableToolbarCommand.TableRows },
           new TableToolbarItemModel() { Command = TableToolbarCommand.TableColumns },
           new TableToolbarItemModel() { Command = TableToolbarCommand.TableCell },
           new TableToolbarItemModel() { Command = TableToolbarCommand.HorizontalSeparator },
           new TableToolbarItemModel() { Command = TableToolbarCommand.TableRemove },
           new TableToolbarItemModel() { Command = TableToolbarCommand.BackgroundColor },
           new TableToolbarItemModel() { Command = TableToolbarCommand.TableCellVerticalAlign },
           new TableToolbarItemModel() { Command = TableToolbarCommand.Styles }
        };
        private List<ImageToolbarItemModel> ImageTools = new List<ImageToolbarItemModel>()
        {
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.Replace },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.Align },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.Caption },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.Remove },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.HorizontalSeparator },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.InsertLink },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.Display },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.AltText },
           new ImageToolbarItemModel() { Command = ImageToolbarCommand.Dimension }
        };
        private List<string> allowedTypes = new List<string> { ".png", ".jpg", ".jpeg" };
        #endregion

        #region Time Validations
        private string startTimeRangeValidationMsg;
        private string endTimeRangeValidationMsg;
        private string startTimeFourHourDifferenceValidationMsg;
        private string endTimeFourHourDifferenceValidationMsg;

        private void InValidateStartAndEndTimes()
        {
            startTimeRangeValidationMsg = "";
            endTimeRangeValidationMsg = "";
            startTimeFourHourDifferenceValidationMsg = "";
            endTimeFourHourDifferenceValidationMsg = "";

            if (requestTypeId == (int)ServiceRequestTypeEnum.SubmitaRequestforPermission)
            {
                StartTimeValidationMsg = LeaveAttendanceRequest.StartTime == null ? translationState.Translate("Required_Field") : "";
                EndTimeValidationMsg = LeaveAttendanceRequest.EndTime == null ? translationState.Translate("Required_Field") : "";
                if (LeaveAttendanceRequest.StartTime is not null && LeaveAttendanceRequest.EndTime is not null)
                {
                    var startTimeOnly = LeaveAttendanceRequest.StartTime.Value.TimeOfDay;
                    var endTimeOnly = LeaveAttendanceRequest.EndTime.Value.TimeOfDay;
                    if (startTimeOnly >= endTimeOnly)
                    {
                        startTimeRangeValidationMsg = translationState.Translate("Start_time_must_be_earlier_than_end_time");
                        endTimeRangeValidationMsg = translationState.Translate("End_time_must_be_later_than_start_time");
                    }
                }
            }
            else if (requestTypeId == (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours)
            {
                StartTimeValidationMsg = LeaveAttendanceRequest.StartTime == null ? translationState.Translate("Required_Field") : "";
                EndTimeValidationMsg = LeaveAttendanceRequest.EndTime == null ? translationState.Translate("Required_Field") : "";
                DaysOfAbsenceValidationMsg = LeaveAttendanceRequest.SelectedDays == null ? translationState.Translate("Required_Field") : "";

                if (LeaveAttendanceRequest.StartTime is not null && LeaveAttendanceRequest.EndTime is not null)
                {
                    var startTimeOnly = LeaveAttendanceRequest.StartTime.Value.TimeOfDay;
                    var endTimeOnly = LeaveAttendanceRequest.EndTime.Value.TimeOfDay;
                    if (startTimeOnly >= endTimeOnly)
                    {
                        startTimeRangeValidationMsg = translationState.Translate("Start_time_must_be_earlier_than_end_time");
                        endTimeRangeValidationMsg = translationState.Translate("End_time_must_be_later_than_start_time");
                    }
                    if ((endTimeOnly - startTimeOnly) > TimeSpan.FromHours(4))
                    {
                        //startTimeFourHourDifferenceValidationMsg = translationState.Translate("Must_be_at_least_a_4_hour_difference");
                        //endTimeFourHourDifferenceValidationMsg = translationState.Translate("Must_be_at_least_a_4_hour_difference");
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Must_be_at_least_a_4_hour_difference"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
        }

        private void OnStartTimeChange()
        {
            InValidateStartAndEndTimes();
        }

        private void OnEndTimeChange()
        {
            InValidateStartAndEndTimes();
        }
        #endregion

        #region Set Task Name 

        protected async Task SetTaskName()
        {
            switch (requestTypeId)
            {
                case (int)ServiceRequestTypeEnum.SubmitaLeaveRequest:
                    var leaveType = LeaveTypes.Where(x => x.Id == LeaveAttendanceRequest.LeaveTypeId).FirstOrDefault();
                    LeaveAttendanceRequest.TaskName = $"Review_{leaveType.NameEn}_Request";
                    LeaveAttendanceRequest.TaskName = LeaveAttendanceRequest.TaskName.Replace(" ", "_");
                    LeaveAttendanceRequest.TaskName = translationState.Translate(LeaveAttendanceRequest.TaskName);
                    break;
            }
        }
        #endregion
    }
}

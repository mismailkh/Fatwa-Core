using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using Microsoft.Extensions.Options;
using FATWATIMEINTERVALSERVICES.Helper;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Quartz;

namespace FATWATIMEINTERVALSERVICES.Services
{
    public class AssignToMOJReminderService : IJob
    {
        //<History Author = 'Muhammad Abuzar' Date='2024-01-22' Version="1.0" Branch="master"> Worker Service to Remind Messenger that Request has not been submitted in MOJ</History>

        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AssignToMOJReminderService> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;


        int reAttemptCount = 0;
        bool isFirstRun = true;

        Guid ExecutionId;
        CmsComsReminder? reminderInterval;
        #endregion

        #region Constructor

        public AssignToMOJReminderService(ILogger<AssignToMOJReminderService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;

            using var scope = _serviceScopeFactory.CreateScope();
            _workerServiceRepository = scope.ServiceProvider.GetRequiredService<WorkerServiceRepository>();
            _iTimeIntervals = scope.ServiceProvider.GetRequiredService<ITimeIntervals>();
            _IAuditLog = scope.ServiceProvider.GetRequiredService<IAuditLog>();
            _reminderMethods = scope.ServiceProvider.GetRequiredService<ReminderMethods>();
        }
        #endregion

        #region MOJ reminder
        public async Task Execute(IJobExecutionContext context)
        {
            do
            {
                WSCmsComsReminderErrorLog cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog();
                WSCmsMOJMessangerIntervalVM cmsMOJMessangerIntervalVM = new WSCmsMOJMessangerIntervalVM();
                WSWorkerServiceExecution workerServiceExecution = new WSWorkerServiceExecution();
                int reminderNo = 1;
                int interval = 1;
                User messenger = null;
                try
                {
                    reminderInterval = _iTimeIntervals.GetCmsComsReminderById((int)CmsComsReminderTypeEnums.DefineThePeriodToRegisterACaseAtMOJ).Result;
                    if (!reminderInterval.IsActive)
                        return;
                    var isCorrectTimeToExecute = await _reminderMethods.CheckValidTimeToRunService();
                    if (!isCorrectTimeToExecute.Item1)
                        return;

                    _logger.LogInformation("MOJReminder Worker service running at: {time}", DateTimeOffset.Now);
                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);


                    if (reminderInterval != null)
                    {
                        var reminder = await _workerServiceRepository.GetAllMojReminderByReminder();
                        if (reminder != null)
                        {
                            interval = _reminderMethods.SetIntervalNumber(reminderInterval);
                            while (reminderNo <= interval)
                            {
                                var intervalDuration = _reminderMethods.SetIntervalReminderNo(reminderInterval, reminderNo);
                                Console.WriteLine(DateTime.Now.Add(TimeSpan.FromDays(-intervalDuration)));
                                // var reminderUsers = reminder.Where(x => x.RequestCreatedDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Add(TimeSpan.FromDays(-intervalDuration)).ToString("dd/MM/yyyy")).ToList();
                                var reminderUsers = reminder.Where(x => x.RequestCreatedDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Add(TimeSpan.FromDays(-intervalDuration + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.RequestCreatedDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy")).ToList();
                                var ManagerUsers = reminder.Where(x => x.RequestCreatedDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Add(TimeSpan.FromDays(-(intervalDuration + 1) + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.RequestCreatedDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy")).ToList();

                                if (reminderUsers.Any())
                                {
                                    WSCmsComsReminderProcessLog createdBycmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    WSCmsComsReminderProcessLog mojCmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();

                                    foreach (var user in reminderUsers)
                                    {
                                        NotificationParameter notificationParameter = new NotificationParameter();
                                        notificationParameter.FileNumber = user.FileNumber;
                                        cmsMOJMessangerIntervalVM = user;
                                        messenger = _workerServiceRepository.GetMojBySectorId(user.SectorTypeId);  //messenger

                                        bool CreatedByNotification = await _reminderMethods.SendReminderAsync(user.RequestCreatedbyId, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.AssignToMOJReminder, "view", new CaseFile().GetType().Name, user.FileId.ToString(), notificationParameter);
                                        if (CreatedByNotification)
                                            createdBycmsComsReminderProcessLog.IsNotification = true;

                                        bool MOJnotification = await _reminderMethods.SendReminderAsync(messenger.Id, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.AssignToMOJReminder, "requests", "moj-registration", "", notificationParameter);
                                        //bool MOJnotification = await _reminderMethods.SendReminderAsync(messenger.Id, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.RegisterCase, "WS_NOTIF_Messenger_CASE_NOT_REGISTERED_IN_MOJ", "registered-case", "create", (user.FileId+"/"+user.MOJRequestId.ToString()+"/"+);
                                        if (MOJnotification)
                                            mojCmsComsReminderProcessLog.IsNotification = true;

                                        //if (reminderInterval.IsTask == true)
                                        //{
                                        //    bool taskResult = await _reminderMethods.CreateTaskAsync("WS_TSK_Register_Case_In_MOJ", messenger.Id, user.SectorTypeId, user.FileId, "requests", "moj-registration", "", "WS_TSK_Register_Case_In_MOJ");
                                        //    if (CreatedByNotification)
                                        //        mojCmsComsReminderProcessLog.IsTask = true;
                                        //}
                                        if (createdBycmsComsReminderProcessLog.IsNotification)
                                        {
                                            createdBycmsComsReminderProcessLog = _reminderMethods.SetReminderNo(createdBycmsComsReminderProcessLog, reminderNo);
                                            createdBycmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                                            createdBycmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                            createdBycmsComsReminderProcessLog.ReferenceId = user.MOJRequestId.ToString();
                                            createdBycmsComsReminderProcessLog.ReceiverId = user.RequestCreatedbyId;
                                            createdBycmsComsReminderProcessLog.Description = "CASE_NOT_REGISTERED_IN_MOJ_" + reminderNo;
                                            createdBycmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            await _workerServiceRepository.ReminderProcessLogAsync(createdBycmsComsReminderProcessLog);
                                        }
                                        else
                                        {
                                            cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                                            cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderErrorLog.ReferenceId = user.MOJRequestId.ToString();
                                            cmsComsReminderErrorLog.ReceiverId = user.RequestCreatedbyId;
                                            cmsComsReminderErrorLog.Message = "Failed_To_Send_Registered_Case_In_MOJ_Reminder_To_Request_Creator_" + reminderNo;
                                            cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            await _workerServiceRepository.ReminderErrorLogAsync(cmsComsReminderErrorLog);
                                        }

                                        if (mojCmsComsReminderProcessLog.IsNotification)
                                        {
                                            mojCmsComsReminderProcessLog = _reminderMethods.SetReminderNo(mojCmsComsReminderProcessLog, reminderNo);
                                            mojCmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                                            mojCmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                            mojCmsComsReminderProcessLog.ReferenceId = user.MOJRequestId.ToString();
                                            mojCmsComsReminderProcessLog.ReceiverId = messenger.Id;
                                            mojCmsComsReminderProcessLog.Description = "Reminder_Case_Not_Yet_Registered_In_MOJ_" + reminderNo;
                                            mojCmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(mojCmsComsReminderProcessLog);
                                        }
                                        else
                                        {
                                            cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                                            cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderErrorLog.ReferenceId = user.MOJRequestId.ToString();
                                            cmsComsReminderErrorLog.ReceiverId = messenger.Id;
                                            cmsComsReminderErrorLog.Message = "Failed_To_Send_Registered_Case_In_MOJ_Reminder_To_MOJ_" + reminderNo;
                                            cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            await _workerServiceRepository.ReminderErrorLogAsync(cmsComsReminderErrorLog);
                                        }
                                    }
                                }
                                if (ManagerUsers.Any())
                                {
                                    WSCmsComsReminderProcessLog createdBycmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    WSCmsComsReminderProcessLog mojCmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();

                                    foreach (var user in ManagerUsers)
                                    {
                                        NotificationParameter notificationParameter = new NotificationParameter();
                                        notificationParameter.FileNumber = user.FileNumber;
                                        cmsMOJMessangerIntervalVM = user;
                                        messenger = _workerServiceRepository.GetMojBySectorId(user.SectorTypeId);  //messenger
                                        var Manager = await _workerServiceRepository.GetManagerByUserId(messenger.Id);
                                        notificationParameter.ReviewerName = Manager.ManagerName;

                                        if (!string.IsNullOrEmpty(Manager.ManagerId))
                                        {
                                            mojCmsComsReminderProcessLog.IsNotification = await _reminderMethods.SendReminderAsync(messenger.Id, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.AssignToMOJReminderForManager, "requests", "moj-registration", "", notificationParameter);


                                            if (createdBycmsComsReminderProcessLog.IsNotification)
                                            {
                                                createdBycmsComsReminderProcessLog = _reminderMethods.SetReminderNo(createdBycmsComsReminderProcessLog, reminderNo);
                                                createdBycmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                                                createdBycmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                                createdBycmsComsReminderProcessLog.ReferenceId = user.MOJRequestId.ToString();
                                                createdBycmsComsReminderProcessLog.ReceiverId = Manager.ManagerId;
                                                createdBycmsComsReminderProcessLog.Description = "CASE_NOT_REGISTERED_IN_MOJ_Reminder for Manager" + reminderNo;
                                                createdBycmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                                await _workerServiceRepository.ReminderProcessLogAsync(createdBycmsComsReminderProcessLog);
                                            }
                                            else
                                            {
                                                cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                                                cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderErrorLog.ReferenceId = user.MOJRequestId.ToString();
                                                cmsComsReminderErrorLog.ReceiverId = Manager.ManagerId;
                                                cmsComsReminderErrorLog.Message = "Failed_To_Send_Registered_Case_In_MOJ_Reminder_To_Manager" + reminderNo;
                                                cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                                await _workerServiceRepository.ReminderErrorLogAsync(cmsComsReminderErrorLog);
                                            }
                                        }
                                    }
                                }
                                reminderNo++;
                            }
                            reminderNo--;
                        }
                    }
                    if (reminderNo == interval)
                    {
                        workerServiceExecution.ExecutionDetails = reminderInterval != null ? "Successfully Completed" : "No Active Interval";
                        workerServiceExecution.Id = ExecutionId;
                        workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                        await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    reAttemptCount++;
                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ;
                    cmsComsReminderErrorLog.ReminderId = reminderInterval?.ID;
                    cmsComsReminderErrorLog.ReferenceId = cmsMOJMessangerIntervalVM?.MOJRequestId.ToString();
                    cmsComsReminderErrorLog.ReceiverId = messenger?.Id;
                    cmsComsReminderErrorLog.Message = ("Failed_To_Send_Registered_Case_In_MOJ_Reminder_To_MOJ_" + reminderNo);
                    cmsComsReminderErrorLog.ReminderTypeId = reminderInterval?.CmsComsReminderTypeId;
                    await _workerServiceRepository.ReminderErrorLogAsync(cmsComsReminderErrorLog);

                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Exception;
                    workerServiceExecution.ExecutionDetails = ("Exception occur due to " + ex.Message);
                    workerServiceExecution.ReAttemptCount = reAttemptCount;
                    await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);

                    _IAuditLog.CreateErrorLog(new ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = "Send Reminder To Draft Reviewer",
                        Body = ex.Message,
                        Category = "Unable to Send MOJ Registered Case Reminder",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Unable to Send MOJ Registered Case Reminder",
                        IPDetails = "WorkerService",
                        ApplicationID = (int)PortalEnum.WorkerServices,
                        ModuleID = (int)WorkflowModuleEnum.TimeIntervalWorkerService
                    });
                }
            }
            while (reAttemptCount < 3);
        }
    }
}
#endregion



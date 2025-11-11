using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Interfaces;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using FATWATIMEINTERVALSERVICES.Helper;
using Microsoft.Extensions.Options;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Quartz;
using System.DirectoryServices.AccountManagement;

namespace FATWATIMEINTERVALSERVICES.Services
{
    //<History Author = 'Muhammad Abuzar' Date='2024-01-22' Version="1.0" Branch="master"> Worker Service to Remind User that no Claim Statement or Request for Additional Info Document is Submitted</History>
    public class RequestForAdditionalInfoService : IJob
    {

        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RequestForAdditionalInfoService> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;

        int reAttemptCount = 0;
        bool isFirstRun = true;

        Guid ExecutionId;
        CmsComsReminder reminderInterval;
        #endregion

        #region Constructor
        public RequestForAdditionalInfoService(ILogger<RequestForAdditionalInfoService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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

        #region Reminder for Adding Additonal Information
        public async Task Execute(IJobExecutionContext context)
        {
            do
            {
                WSCmsComsReminderErrorLog cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog();
                WSRequestForAdditionalInfoVM requestForAdditionalInfoVM = new WSRequestForAdditionalInfoVM();
                WSWorkerServiceExecution workerServiceExecution = new WSWorkerServiceExecution();
                int reminderNo = 1;
                int interval = 1;
                try
                {

                    reminderInterval = _iTimeIntervals.GetCmsComsReminderById((int)CmsComsReminderTypeEnums.DefineThePeriodToRequestForAdditionalInformation).Result;
                    if (!reminderInterval.IsActive)
                        return;
                    _logger.LogInformation("Worker service running at: {time}", DateTimeOffset.Now);
                    var isCorrectTimeToExecute = await _reminderMethods.CheckValidTimeToRunService();
                    if (!isCorrectTimeToExecute.Item1)
                        return;
                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRequestForAdditionalInformation;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);

                    if (reminderInterval != null)
                    {
                        interval = _reminderMethods.SetIntervalNumber(reminderInterval);
                        var reminder = await _workerServiceRepository.GetReminderForAdditionalInformation();
                        if (reminder != null)
                        {
                            while (reminderNo <= interval)
                            {
                                var intervalDuration = _reminderMethods.SetIntervalReminderNo(reminderInterval, reminderNo);
                                // var userReminder = reminder.Where(x => x.CaseAssignDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-(intervalDuration + WeekEndsHolidaysNumber))).ToString("dd/MM/yyyy")).ToList();
                                var userReminder = reminder.Where(x => x.CaseAssignDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-(intervalDuration + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.CaseAssignDate.Date, isCorrectTimeToExecute.Item3)))).ToString("dd/MM/yyyy")).ToList();
                                var ManagerReminder = reminder.Where(x => x.CaseAssignDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-((intervalDuration + 1) + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.CaseAssignDate.Date, isCorrectTimeToExecute.Item3)))).ToString("dd/MM/yyyy")).ToList();
                                await Task.Delay(1000);
                                if (userReminder.Any())
                                {
                                    WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    foreach (var user in userReminder)
                                    {
                                        requestForAdditionalInfoVM = user;
                                        NotificationParameter notificationParameter = new NotificationParameter();
                                        notificationParameter.ReferenceNumber = user.ReferenceNumber;
                                        notificationParameter.Entity = user.Entity;
                                        notificationParameter.Type = user.DocumentType;
                                        var LawyerNotification = await _reminderMethods.SendReminderAsync(user.LawyerId, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.LegalNotificationReminder, "view", user.Entity, user.ReferenceId.ToString(), notificationParameter);
                                        if (LawyerNotification == true)
                                            cmsComsReminderProcessLog.IsNotification = true;
                                        //if (reminderInterval.IsTask == true)
                                        //    taskResult = await _reminderMethods.CreateTaskAsync("WS_TSK_Complete_Claim_Statement_Reminder", user.LawyerId, user.SectorTypeId, user.FileId, "view", new CaseFile().GetType().Name, user.FileId.ToString(), "WS_TSK_Complete_Claim_Statement_Reminder");

                                        //if (taskResult == true)
                                        //    cmsComsReminderProcessLog.IsTask = true;

                                        if (LawyerNotification)
                                        {
                                            cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                            cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRequestForAdditionalInformation;
                                            cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                            cmsComsReminderProcessLog.ReceiverId = user.LawyerId;
                                            cmsComsReminderProcessLog.Description = "Reminder_send_for_additional_information_Request_" + reminderNo;
                                            cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                        }
                                        else
                                        {
                                            cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRequestForAdditionalInformation;
                                            cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderErrorLog.ReferenceId = user.ReferenceId.ToString();
                                            cmsComsReminderErrorLog.ReceiverId = user.LawyerId;
                                            cmsComsReminderErrorLog.Message = "Failed_to_send_additional_information_Request_reminder_" + reminderNo;
                                            cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                        }
                                    }
                                }
                                if (ManagerReminder.Any())
                                {
                                    WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    foreach (var user in ManagerReminder)
                                    {
                                        requestForAdditionalInfoVM = user;
                                        NotificationParameter notificationParameter = new NotificationParameter();
                                        notificationParameter.ReferenceNumber = user.ReferenceNumber;
                                        notificationParameter.Entity = user.Entity;
                                        notificationParameter.Type = user.DocumentType;
                                        notificationParameter.SenderName = user.SenderName;
                                        var manager = await _workerServiceRepository.GetManagerByUserId(user.LawyerId);
                                        if (!string.IsNullOrEmpty(manager.ManagerId))
                                        {
                                            var LawyerNotification = await _reminderMethods.SendReminderAsync(manager.ManagerId, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.LegalNotificationReminderForManager, "view", user.Entity, user.ReferenceId.ToString(), notificationParameter);
                                            if (LawyerNotification == true)
                                                cmsComsReminderProcessLog.IsNotification = true;

                                            #region Task
                                            //if (reminderInterval.IsTask == true)
                                            //    taskResult = await _reminderMethods.CreateTaskAsync("WS_TSK_Complete_Claim_Statement_Reminder", user.LawyerId, user.SectorTypeId, user.FileId, "view", new CaseFile().GetType().Name, user.FileId.ToString(), "WS_TSK_Complete_Claim_Statement_Reminder");

                                            //if (taskResult == true) 
                                            //    cmsComsReminderProcessLog.IsTask = true;
                                            #endregion

                                            if (LawyerNotification)
                                            {
                                                cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                                cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRequestForAdditionalInformation;
                                                cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderProcessLog.ReceiverId = manager.ManagerId;
                                                cmsComsReminderProcessLog.Description = "Reminder_send_for_additional_information_Request_To Laywer Manager : Reminder NO " + reminderNo;
                                                cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                                var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                            }
                                            else
                                            {
                                                cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRequestForAdditionalInformation;
                                                cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderErrorLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderErrorLog.ReceiverId = manager.ManagerId;
                                                cmsComsReminderErrorLog.Message = "Reminder_send_for_additional_information_Request_To Laywer Manager : Reminder NO " + reminderNo;
                                                cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
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
                        workerServiceExecution.Id = ExecutionId;
                        workerServiceExecution.ExecutionDetails = reminderInterval != null ? "Successfully Completed" : "No Active Interval";
                        workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                        await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    reAttemptCount++;
                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRequestForAdditionalInformation;
                    cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                    cmsComsReminderErrorLog.ReferenceId = requestForAdditionalInfoVM?.ReferenceId.ToString();
                    cmsComsReminderErrorLog.ReceiverId = requestForAdditionalInfoVM?.LawyerId;
                    cmsComsReminderErrorLog.Message = ("Failed_to_send_additional_information_Request_reminder_" + reminderNo);
                    cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                    await _workerServiceRepository.ReminderErrorLogAsync(cmsComsReminderErrorLog);

                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Exception;
                    workerServiceExecution.ExecutionDetails = ("Exception occur due to " + ex.Message);
                    workerServiceExecution.ReAttemptCount = reAttemptCount;
                    await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);

                    _IAuditLog.CreateErrorLog(new ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = "Reminder Send For Complete Additional Information Request",
                        Body = ex.Message,
                        Category = "Unable to Send Reminder For Additional Information Request",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Unable to Send Reminder No " + reminderNo,
                        IPDetails = "WorkerService",
                        ApplicationID = (int)PortalEnum.WorkerServices,
                        ModuleID = (int)WorkflowModuleEnum.TimeIntervalWorkerService
                    });
                }
            }
            while (reAttemptCount < 3);
        }
        #endregion
    }
}

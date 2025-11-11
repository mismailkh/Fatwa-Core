using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models;
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
    public class ReminderToCompleteClaimStatementDefenseLetter : IJob
    {
        //<History Author = 'Muhammad Abuzar' Date='2024-01-22' Version="1.0" Branch="master"> Worker Service to Remind Lawyer to Complete Claim Statement</History>

        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReminderToCompleteClaimStatementDefenseLetter> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;

        int reAttemptCount = 0;
        Guid ExecutionId;
        CmsComsReminder? reminderInterval;
        #endregion

        #region Constructor
        public ReminderToCompleteClaimStatementDefenseLetter(ILogger<ReminderToCompleteClaimStatementDefenseLetter> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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

        #region Reminder to Complete Claim Statment
        public async Task Execute(IJobExecutionContext context)
        {
            WSCmsComsReminderErrorLog cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog();
            WSReminderToCompleteClaimStatementVM rminderToCompleteClaimStatementVM = new WSReminderToCompleteClaimStatementVM();
            WSWorkerServiceExecution workerServiceExecution = new WSWorkerServiceExecution();
            int reminderNo = 1;
            int interval = 1;
            do
            {
                try
                {

                    reminderInterval = _iTimeIntervals.GetCmsComsReminderById((int)CmsComsReminderTypeEnums.DefineThePeriodToCompleteTheClaimStatement).Result;
                    if (!reminderInterval.IsActive)
                        return;
                    var isCorrectTimeToExecute = await _reminderMethods.CheckValidTimeToRunService();
                    if (!isCorrectTimeToExecute.Item1)
                        return;
                    _logger.LogInformation("Reminder Worker service running at: {time}", DateTimeOffset.Now);
                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToCompleteTheClaimStatement;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);

                    if (reminderInterval != null)
                    {
                        interval = _reminderMethods.SetIntervalNumber(reminderInterval);
                        var reminder = await _workerServiceRepository.GetReminderToCompleteClaimStatement();
                        if (reminder != null)
                        {
                            while (reminderNo <= interval)
                            {
                                var intervalDuration = _reminderMethods.SetIntervalReminderNo(reminderInterval, reminderNo);
                                var userReminder = reminder.Where(x => x.CaseAssignDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-intervalDuration + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.CaseAssignDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy")).ToList();
                                var ManagerReminder = reminder.Where(x => x.CaseAssignDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-(intervalDuration + 1) + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.CaseAssignDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy")).ToList();

                                if (userReminder.Any())
                                {
                                    WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    foreach (var user in userReminder)
                                    {
                                        rminderToCompleteClaimStatementVM = user;
                                        NotificationParameter notificationParameter = new NotificationParameter();
                                        notificationParameter.ReferenceNumber = user.ReferenceNumber;
                                        notificationParameter.Entity = user.Entity;
                                        notificationParameter.Type = user.DocumentType;
                                        var LawyerNotification = await _reminderMethods.SendReminderAsync(user.LawyerId, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.ClaimStatementandDefenseLetterReminder, "view", user.Entity, user.ReferenceId.ToString(), notificationParameter);
                                        if (LawyerNotification == true)
                                            cmsComsReminderProcessLog.IsNotification = true;
                                        if (LawyerNotification)
                                        {
                                            cmsComsReminderProcessLog.IsNotification = true;
                                            cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                            cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToCompleteTheClaimStatement;
                                            cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                            cmsComsReminderProcessLog.ReceiverId = user.LawyerId;
                                            cmsComsReminderProcessLog.Description = "Complete_Claim_Statement_Reminder_" + reminderNo;
                                            cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                        }
                                        else
                                        {
                                            cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToCompleteTheClaimStatement;
                                            cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderErrorLog.ReferenceId = user.ReferenceId.ToString();
                                            cmsComsReminderErrorLog.ReceiverId = user.LawyerId;
                                            cmsComsReminderErrorLog.Message = "Complete_Claim_Statement_Reminder_" + reminderNo;
                                            cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                        }
                                    }
                                }
                                if (ManagerReminder.Any())
                                {
                                    WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    foreach (var user in ManagerReminder)
                                    {
                                        rminderToCompleteClaimStatementVM = user;
                                        NotificationParameter notificationParameter = new NotificationParameter();
                                        notificationParameter.ReferenceNumber = user.ReferenceNumber;
                                        notificationParameter.Entity = user.Entity;
                                        notificationParameter.Type = user.DocumentType;
                                        notificationParameter.SenderName = user.SenderName;
                                        var manager = await _workerServiceRepository.GetManagerByUserId(user.LawyerId);
                                        if (!string.IsNullOrEmpty(manager.ManagerId))
                                        {
                                            var ManagerNotification = await _reminderMethods.SendReminderAsync(manager.ManagerId, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.ClaimStatementandDefenseLetterReminderForManager, "view", user.Entity, user.ReferenceId.ToString(), notificationParameter);
                                            if (ManagerNotification == true)
                                                cmsComsReminderProcessLog.IsNotification = true;
                                            if (ManagerNotification)
                                            {
                                                cmsComsReminderProcessLog.IsNotification = true;
                                                cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                                cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToCompleteTheClaimStatement;
                                                cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderProcessLog.ReceiverId = manager.ManagerId;
                                                cmsComsReminderProcessLog.Description = " Reminder to Manager for not Complete Claim Statement by Lawyer" + reminderNo;
                                                cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                                var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                            }
                                            else
                                            {
                                                cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToCompleteTheClaimStatement;
                                                cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderErrorLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderErrorLog.ReceiverId = user.LawyerId;
                                                cmsComsReminderErrorLog.Message = " Reminder to Manager for not Complete Claim Statement by Lawyer" + reminderNo;
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
                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToCompleteTheClaimStatement;
                    cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                    cmsComsReminderErrorLog.ReferenceId = rminderToCompleteClaimStatementVM?.ReferenceId.ToString();
                    cmsComsReminderErrorLog.ReceiverId = rminderToCompleteClaimStatementVM?.LawyerId;
                    cmsComsReminderErrorLog.Message = ("Failed_to_send_Complete_Claim_Statement_reminder_" + reminderNo);
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
                        Subject = "Reminder Send For Complete Claim Statement",
                        Body = ex.Message,
                        Category = "Unable to Send Reminder To Complete Claim Statement",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Unable to Send Reminder To Complete Claim Statement " + reminderNo,
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



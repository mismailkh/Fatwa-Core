using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWATIMEINTERVALSERVICES.Helper;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Quartz;

namespace FATWATIMEINTERVALSERVICES.Services
{
    //<History Author = 'Muhammad Abuzar' Date='2024-02-12' Version="1.0" Branch="master">Prepare Defense Letter Reminder Service</History>

    public class OpinionLetterReminderService : IJob
    {
        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpinionLetterReminderService> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;


        int reAttemptCount = 0;
        Guid ExecutionId;
        CmsComsReminder? reminderInterval;
        #endregion

        #region Constructor
        public OpinionLetterReminderService(ILogger<OpinionLetterReminderService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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

        #region Opinion Latter reminder
        public async Task Execute(IJobExecutionContext context)
        {
            do
            {
                WSCmsComsReminderErrorLog cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog();
                WSDefenseLetterReminderServiceVM defenseLetterReminderService = new WSDefenseLetterReminderServiceVM();
                WSWorkerServiceExecution workerServiceExecution = new WSWorkerServiceExecution();
                int reminderNo = 1;
                int interval = 1;
                try
                {
                    reminderInterval = _iTimeIntervals.GetCmsComsReminderById((int)CmsComsReminderTypeEnums.DefineThePeriodToPrepareDefenseLetter).Result;
                    if (!reminderInterval.IsActive)
                        return;
                    var isCorrectTimeToExecute = await _reminderMethods.CheckValidTimeToRunService();
                    if (!isCorrectTimeToExecute.Item1)
                        return;
                    _logger.LogInformation("Opinions Letter Reminder Worker service running at: {time}", DateTimeOffset.Now);
                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToPrepareDefenseLetter;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);

                    if (reminderInterval != null)
                    {
                        var reminder = await _workerServiceRepository.GetReminderToPrepareDefenseLetter();
                        if (reminder != null)
                        {
                            interval = _reminderMethods.SetIntervalNumber(reminderInterval);

                            while (reminderNo <= interval)
                            {
                                var intervalDuration = _reminderMethods.SetIntervalReminderNo(reminderInterval, reminderNo);
                                WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                foreach (var user in reminder)
                                {
                                    defenseLetterReminderService = user;
                                    NotificationParameter notificationParameter = new NotificationParameter();
                                    notificationParameter.CaseNumber = user.CaseNumber;
                                    notificationParameter.SenderName = user.SenderName;
                                    //if (user.JudgmentDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-intervalDuration )).ToString("dd/MM/yyyy"))
                                    if (user.JudgmentDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-intervalDuration + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, user.JudgmentDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy"))
                                    {
                                        var LawyerNotification = await _reminderMethods.SendReminderAsync(user.LawyerId, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.OpinionNotesReminder, "view", "case", user.CaseId.ToString(), notificationParameter);
                                        if (LawyerNotification)
                                        {
                                            cmsComsReminderProcessLog.IsNotification = true;
                                            cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                            cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToPrepareDefenseLetter;
                                            cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderProcessLog.ReferenceId = user.CaseId.ToString();
                                            cmsComsReminderProcessLog.ReceiverId = user.LawyerId;
                                            cmsComsReminderProcessLog.Description = "Complete_Opinions_Letter_Reminder_" + reminderNo;
                                            cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                        }
                                        else
                                        {
                                            cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToPrepareDefenseLetter;
                                            cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderErrorLog.ReferenceId = user.CaseId.ToString();
                                            cmsComsReminderErrorLog.ReceiverId = user.LawyerId;
                                            cmsComsReminderErrorLog.Message = "Complete_Opinions_Letter_Reminder_" + reminderNo;
                                            cmsComsReminderErrorLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                        }
                                    }
                                    if (user.JudgmentDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.Add(TimeSpan.FromDays(-(intervalDuration + 1) + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, user.JudgmentDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy"))
                                    {
                                        var manager = await _workerServiceRepository.GetManagerByUserId(user.LawyerId);
                                        if (!string.IsNullOrEmpty(manager.ManagerId))
                                        {
                                            var LawyerNotification = await _reminderMethods.SendReminderAsync(manager.ManagerId, (int)WorkflowModuleEnum.CaseManagement, (int)NotificationEventEnum.OpinionNotesReminderForManager, "view", "case", user.CaseId.ToString(), notificationParameter);
                                            if (LawyerNotification)
                                            {
                                                cmsComsReminderProcessLog.IsNotification = true;
                                                cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                                cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToPrepareDefenseLetter;
                                                cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderProcessLog.ReferenceId = user.CaseId.ToString();
                                                cmsComsReminderProcessLog.ReceiverId = manager.ManagerId;
                                                cmsComsReminderProcessLog.Description = "Reminder to Manager for not Complete Opinion Letter by Lawyer " + reminderNo;
                                                cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                                var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                            }
                                            else
                                            {
                                                cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToPrepareDefenseLetter;
                                                cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderErrorLog.ReferenceId = user.CaseId.ToString();
                                                cmsComsReminderErrorLog.ReceiverId = user.LawyerId;
                                                cmsComsReminderErrorLog.Message = "Reminder to Manager for not Complete Opinion Letter by Lawyer " + reminderNo;
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
                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToPrepareDefenseLetter;
                    cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                    cmsComsReminderErrorLog.ReferenceId = defenseLetterReminderService?.CaseId.ToString();
                    cmsComsReminderErrorLog.ReceiverId = defenseLetterReminderService?.LawyerId;
                    cmsComsReminderErrorLog.Message = ("Failed_to_send_Complete_Opions_Letter_Reminder_" + reminderNo);
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
                        Subject = "Reminder Send For Opinion Letter Prepration",
                        Body = ex.Message,
                        Category = "Unable to Send Reminder To Prepare Opinion Letter",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Unable to Send Reminder To Prepare Opinion Letter " + reminderNo,
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

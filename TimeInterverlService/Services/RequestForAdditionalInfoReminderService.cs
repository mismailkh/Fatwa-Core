using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Interfaces;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_GENERAL.Helper;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Quartz;

namespace FATWATIMEINTERVALSERVICES.Services
{
    //<History Author = 'Muhammad Abuzar' Date='2024-01-22' Version="1.0" Branch="master"> Worker Service to Remind User that no Claim Statement or Request for Additional Info Document is Submitted</History>
    public class RequestForAdditionalInfoReminderService : IJob
    {

        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RequestForAdditionalInfoReminderService> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;

        int reAttemptCount = 0;
        bool isFirstRun = true;

        Guid ExecutionId;
        CmsComsReminder reminderInterval;
        #endregion

        #region Constructor
        public RequestForAdditionalInfoReminderService(ILogger<RequestForAdditionalInfoReminderService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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

        #region Requesting Additional Information Reminder /GE
        public async Task Execute(IJobExecutionContext context)
        {
            do
            {
                WSCmsComsReminderErrorLog cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog();
                WSRequestForAdditionalInfoReminderVM requestForAdditionalInfoVM = new WSRequestForAdditionalInfoReminderVM();
                WSWorkerServiceExecution workerServiceExecution = new WSWorkerServiceExecution();
                int reminderNo = 1;
                int interval = 1;
                try
                {

                    reminderInterval = _iTimeIntervals.GetCmsComsReminderById((int)CmsComsReminderTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation).Result;
                    if (!reminderInterval.IsActive)
                        return;
                    var isCorrectTimeToExecute = await _reminderMethods.CheckValidTimeToRunService();
                    if (!isCorrectTimeToExecute.Item1)
                        return;
                    _logger.LogInformation("Worker service running at: {time}", DateTimeOffset.Now);
                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);


                    if (reminderInterval != null)
                    {
                        interval = _reminderMethods.SetIntervalNumber(reminderInterval);
                        var reminder = await _workerServiceRepository.GetReminderForAdditionalInformationReminder();

                        if (reminder != null)
                        {
                            while (reminderNo <= interval)
                            {
                                var intervalDuration = _reminderMethods.SetIntervalReminderNo(reminderInterval, reminderNo);
                                var userReminder = reminder.Where(x => x.OutboxDate.Date.Add(TimeSpan.FromDays(intervalDuration + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.OutboxDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy") == DateTime.Now.Date.ToString("dd/MM/yyyy")).ToList();
                                var ManagerReminder = reminder.Where(x => x.OutboxDate.Date.Add(TimeSpan.FromDays(intervalDuration + 1 + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.OutboxDate.Date, isCorrectTimeToExecute.Item3))).ToString("dd/MM/yyyy") == DateTime.Now.Date.ToString("dd/MM/yyyy")).ToList();
                                if (userReminder.Any())
                                {
                                    WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    foreach (var user in userReminder)
                                    {
                                        requestForAdditionalInfoVM = user;
                                        NotificationParameter notificationParameter = new NotificationParameter();
                                        notificationParameter.CaseNumber = user.ReferenceNumber;
                                        notificationParameter.GEName = user.GovtEntityName;
                                        var LawyerNotification = await _reminderMethods.SendReminderAsync(user.LawyerId,
                                                                                            (int)WorkflowModuleEnum.CaseManagement,
                                                                                            (int)NotificationEventEnum.LegalNotificationResponseReminder,
                                                                                            "For-More-Information",
                                                                                            user.SubmoduleId != "Case" ? "Request" : "Case", user.ReferenceId.ToString() + "/false/" + user.CommunicationId.ToString() + "/true",
                                                                                            notificationParameter);
                                        if (LawyerNotification == true)
                                            cmsComsReminderProcessLog.IsNotification = true;

                                        //if (reminderInterval.IsTask == true)
                                        //    taskResult = await _reminderMethods.CreateTaskAsync("WS_TSK_Complete_Claim_Statement_Reminder", user.LawyerId, user.SectorTypeId, user.FileId, "view", new CaseFile().GetType().Name, user.FileId.ToString(), "WS_TSK_Complete_Claim_Statement_Reminder");

                                        //if (taskResult == true)
                                        //    cmsComsReminderProcessLog.IsTask = true;

                                        if (LawyerNotification)
                                        {
                                            cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                            cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation;
                                            cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                            cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                            cmsComsReminderProcessLog.ReceiverId = user.LawyerId;
                                            cmsComsReminderProcessLog.Description = "Reminder_send_for_additional_information_Request_" + reminderNo;
                                            cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                            var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                        }
                                        else
                                        {
                                            cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation;
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
                                        notificationParameter.CaseNumber = user.ReferenceNumber;
                                        notificationParameter.GEName = user.GovtEntityName;
                                        notificationParameter.SenderName = user.SenderName;

                                        var manager = await _workerServiceRepository.GetManagerByUserId(user.LawyerId);
                                        if (!string.IsNullOrEmpty(manager.ManagerId))
                                        {
                                            var LawyerNotification = await _reminderMethods.SendReminderAsync(manager.ManagerId,
                                                                                            (int)WorkflowModuleEnum.CaseManagement,
                                                                                            (int)NotificationEventEnum.LegalNotificationResponseReminderForManager,
                                                                                            "For-More-Information",
                                                                                            user.SubmoduleId != "Case" ? "Request" : "Case", user.ReferenceId.ToString() + "/false/" + user.CommunicationId.ToString() + "/true",
                                                                                            notificationParameter);
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
                                                cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation;
                                                cmsComsReminderProcessLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderProcessLog.ReceiverId = manager.ManagerId;
                                                cmsComsReminderProcessLog.Description = "Reminder_send_for_additional_information_Request_to Lawyer Manager" + reminderNo;
                                                cmsComsReminderProcessLog.ReminderTypeId = reminderInterval.CmsComsReminderTypeId;
                                                var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                            }
                                            else
                                            {
                                                cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation;
                                                cmsComsReminderErrorLog.ReminderId = reminderInterval.ID;
                                                cmsComsReminderErrorLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderErrorLog.ReceiverId = manager.ManagerId;
                                                cmsComsReminderErrorLog.Message = "Failed_to_send_additional_information_Request_reminder_To lawyer manager" + reminderNo;
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
                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation;
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

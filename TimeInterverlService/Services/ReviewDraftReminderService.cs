using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using FATWATIMEINTERVALSERVICES.Helper;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Quartz;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWATIMEINTERVALSERVICES.Services
{
    public class ReviewDraftReminderService : IJob
    {
        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReviewDraftReminderService> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;

        int reAttemptCount = 0;
        bool isFirstRun = true;

        Guid ExecutionId;
        #endregion

        #region Constructor
        public ReviewDraftReminderService(ILogger<ReviewDraftReminderService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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

        #region Draft Inreview/approval/rejection reminder to ACTORz
        public async Task Execute(IJobExecutionContext context)
        {
            do
            {

                WSCmsComsReminderErrorLog cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog();
                WSCmsDraftTemplateIntervalVM cmsDraftTemplateIntervalVM = new WSCmsDraftTemplateIntervalVM();
                WSWorkerServiceExecution workerServiceExecution = new WSWorkerServiceExecution();
                int reminderNo = 1;
                int interval = 0;
                try
                {
                    var reminderInterval = _iTimeIntervals.GetCmsComsCommunicationReminder((int)CmsComsReminderTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant).Result;
                    if (!reminderInterval.Any())
                        return;
                    var isCorrectTimeToExecute = await _reminderMethods.CheckValidTimeToRunService();
                    if (!isCorrectTimeToExecute.Item1)
                        return;
                    _logger.LogInformation("Worker service running at: {time}", DateTimeOffset.Now);

                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);

                    if (reminderInterval != null)
                    {
                        foreach (var commReminder in reminderInterval)
                        {
                            var reminder = _workerServiceRepository.GetCmsDraftTemplateReminder((int)commReminder.CommunicationTypeId);
                            interval = _reminderMethods.SetIntervalNumber(commReminder);
                            if (reminder != null)
                            {
                                while (reminderNo <= interval)
                                {
                                    var intervalDuration = _reminderMethods.SetIntervalReminderNo(commReminder, reminderNo);
                                    var userReminder = reminder.Where(x => x.CreatedDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.AddDays(-intervalDuration + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.CreatedDate.Date, isCorrectTimeToExecute.Item3)).ToString("dd/MM/yyyy")).ToList();
                                    var ManagerReminder = reminder.Where(x => x.CreatedDate.Date.ToString("dd/MM/yyyy") == DateTime.Now.Date.AddDays(-(intervalDuration + 1) + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, x.CreatedDate.Date, isCorrectTimeToExecute.Item3)).ToString("dd/MM/yyyy")).ToList();
                                    if (userReminder.Any())
                                    {
                                        WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                        foreach (var user in userReminder)
                                        {
                                            NotificationParameter notificationParameter = new NotificationParameter();
                                            notificationParameter.ReferenceNumber = user.ReferenceNumber;
                                            notificationParameter.Type = user.DocumentType;
                                            notificationParameter.Entity = user.Entity;
                                            notificationParameter.SenderName = user.SenderName;
                                            cmsDraftTemplateIntervalVM = user;
                                            var DraftTemplateNotification = await _reminderMethods.SendReminderAsync(
                                                                                                    user.ReviewerUserId,
                                                                                                    (int)WorkflowModuleEnum.CaseManagement,
                                                                                                    commReminder.CommunicationTypeId == (int)DraftVersionStatusEnum.InReview || commReminder.CommunicationTypeId == (int)DraftVersionStatusEnum.Approve ? (int)NotificationEventEnum.ReviewDraftReminder : (int)NotificationEventEnum.DraftModificationReminder,
                                                                                                    "detail",
                                                                                                    "draftdocument",
                                                                                                    user.DraftedTemplateId.ToString() + "/" + user.VersionId + "/" + user.TaskId,
                                                                                                    notificationParameter);
                                            if (DraftTemplateNotification == true)
                                                cmsComsReminderProcessLog.IsNotification = true;
                                            if (DraftTemplateNotification)
                                            {
                                                cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                                cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant;
                                                /*cmsComsReminderProcessLog.ReminderId = user.Id;*/
                                                cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderProcessLog.ReceiverId = user.ReviewerUserId;
                                                cmsComsReminderProcessLog.Description = "Reminder_send_to_draft_reviewer_" + reminderNo;
                                                //cmsComsReminderProcessLog.ReminderTypeId = user.CmsComsReminderTypeId;
                                                var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                            }
                                            else
                                            {
                                                cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant;
                                                /*cmsComsReminderErrorLog.ReminderId = user.Id;*/
                                                cmsComsReminderErrorLog.ReferenceId = user.ReferenceId.ToString();
                                                cmsComsReminderErrorLog.ReceiverId = user.ReviewerUserId;
                                                cmsComsReminderErrorLog.Message = "Failed_to_send_draft_reviewer_reminder_" + reminderNo;
                                                //cmsComsReminderErrorLog.ReminderTypeId = user.CmsComsReminderTypeId;
                                                await _workerServiceRepository.ReminderErrorLogAsync(cmsComsReminderErrorLog);
                                            }
                                        }
                                    }
                                    if (ManagerReminder.Any())
                                    {
                                        WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                        foreach (var user in ManagerReminder)
                                        {
                                            NotificationParameter notificationParameter = new NotificationParameter();
                                            notificationParameter.ReferenceNumber = user.ReferenceNumber;
                                            notificationParameter.Type = user.DocumentType;
                                            notificationParameter.Entity = user.Entity;
                                            notificationParameter.SenderName = user.SenderName;
                                            notificationParameter.ReviewerName = await _workerServiceRepository.GetReviewerByUserId(user.ReviewerUserId);
                                            cmsDraftTemplateIntervalVM = user;
                                            var manager = await _workerServiceRepository.GetManagerByUserId(user.ReviewerUserId);
                                            if (!string.IsNullOrEmpty(manager.ManagerId))
                                            {
                                                var DraftTemplateNotification = await _reminderMethods.SendReminderAsync(
                                                                                                    manager.ManagerId,
                                                                                                    (int)WorkflowModuleEnum.CaseManagement,
                                                                                                    commReminder.CommunicationTypeId == (int)DraftVersionStatusEnum.InReview || commReminder.CommunicationTypeId == (int)DraftVersionStatusEnum.Approve ? (int)NotificationEventEnum.ReviewDraftReminderForManager : (int)NotificationEventEnum.DraftModificationReminderForManager,
                                                                                                    "detail",
                                                                                                    "draftdocument",
                                                                                                    user.DraftedTemplateId.ToString() + "/" + user.VersionId + "/" + user.TaskId,
                                                                                                    notificationParameter);
                                                if (DraftTemplateNotification == true)
                                                    cmsComsReminderProcessLog.IsNotification = true;
                                                if (DraftTemplateNotification)
                                                {
                                                    cmsComsReminderProcessLog = _reminderMethods.SetReminderNo(cmsComsReminderProcessLog, reminderNo);
                                                    cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant;
                                                    /*cmsComsReminderProcessLog.ReminderId = user.Id;*/
                                                    cmsComsReminderProcessLog.ReferenceId = user.ReferenceId.ToString();
                                                    cmsComsReminderProcessLog.ReceiverId = user.ReviewerUserId;
                                                    cmsComsReminderProcessLog.Description = "Reminder_send_to_draft_reviewer_" + reminderNo;
                                                    //cmsComsReminderProcessLog.ReminderTypeId = user.CmsComsReminderTypeId;
                                                    var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                                }
                                                else
                                                {
                                                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant;
                                                    /*cmsComsReminderErrorLog.ReminderId = user.Id;*/
                                                    cmsComsReminderErrorLog.ReferenceId = user.ReferenceId.ToString();
                                                    cmsComsReminderErrorLog.ReceiverId = user.ReviewerUserId;
                                                    cmsComsReminderErrorLog.Message = "Failed_to_send_draft_reviewer_reminder_" + reminderNo;
                                                    //cmsComsReminderErrorLog.ReminderTypeId = user.CmsComsReminderTypeId;
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
                            workerServiceExecution.Id = ExecutionId;
                            workerServiceExecution.ExecutionDetails = reminderInterval != null ? "Successfully Completed" : "No Active Interval";
                            workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                            await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    reAttemptCount++;
                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant;
                    /*cmsComsReminderErrorLog.ReminderId = cmsDraftTemplateIntervalVM?.Id;*/
                    cmsComsReminderErrorLog.ReferenceId = cmsDraftTemplateIntervalVM?.ReferenceId.ToString();
                    cmsComsReminderErrorLog.ReceiverId = cmsDraftTemplateIntervalVM?.ReviewerUserId;
                    cmsComsReminderErrorLog.Message = ("Failed_to_send_draft_reviewer_reminder_" + reminderNo);
                    //cmsComsReminderErrorLog.ReminderTypeId = cmsDraftTemplateIntervalVM?.CmsComsReminderTypeId;
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
                        Category = "Unable to Send Reminder To Draft Reviewer",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Unable to Send Reminder To Draft Reviewer",
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
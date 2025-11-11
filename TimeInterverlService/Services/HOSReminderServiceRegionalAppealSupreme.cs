using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using Microsoft.Extensions.Options;
using FATWATIMEINTERVALSERVICES.Helper;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using Quartz;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Enums;

namespace FATWATIMEINTERVALSERVICES.Services
{
    public class HOSReminderServiceRegionalAppealSupreme : IJob
    {
        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HOSReminderServiceRegionalAppealSupreme> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;


        int reAttemptCount = 0;
        Guid ExecutionId;
        //CmsComsReminder? reminderInterval;
        #endregion

        #region Constructor
        public HOSReminderServiceRegionalAppealSupreme(ILogger<HOSReminderServiceRegionalAppealSupreme> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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

        #region Reminder for Both HOS & Lawyer of Regional/Appeal/Supreme
        public async Task Execute(IJobExecutionContext context)
        {
            do
            {
                var cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog();
                //var wSCMSCaseFileHOSReminderVM = new WSCMSCaseFileHOSReminderVM();
                var wSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM = new WSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM();
                //List<string> Users = new List<string>();
                var workerServiceExecution = new WSWorkerServiceExecution();
                int reminderNo = 1;
                int interval = 0;
                int sectorType = 0;
                try
                {
                    var reminderInterval = _iTimeIntervals.GetCmsComsCommunicationReminder((int)CmsComsReminderTypeEnums.DefineThePeriodToRegionalORAppealORSupreme).Result;
                    if (!reminderInterval.Any())
                        return;
                    var isCorrectTimeToExecute = await _reminderMethods.CheckValidTimeToRunService();
                    if (!isCorrectTimeToExecute.Item1)
                        return;
                    _logger.LogInformation("Worker service running at: {time}", DateTimeOffset.Now);

                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegionalORAppealORSupreme;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);
                    // CmsComsReminder
                    if (reminderInterval != null)
                    {
                        foreach (var reminderType in reminderInterval)
                        {
                            //var reminder = _workerServiceRepository.GetCmsDraftTemplateReminder(reminderNo);
                            reminderNo = 1;
                            interval = _reminderMethods.SetIntervalNumber(reminderType);
                            while (reminderNo <= interval)
                            {
                                var intervalDuration = _reminderMethods.SetIntervalReminderNo(reminderType, reminderNo);
                                sectorType = (int)reminderType.CommunicationTypeId;
                                var casesTobeClosed = _workerServiceRepository.GetCaseFileHOSReminderRegionalORAppealORSupreme(sectorType);

                                if (casesTobeClosed != null)
                                {
                                    WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                                    foreach (var caseToBeClosed in casesTobeClosed)
                                    {
                                        DateTime closureDate = caseToBeClosed.JudgementDate.AddDays((Double)reminderType.SLAInterval + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, caseToBeClosed.JudgementDate.Date, isCorrectTimeToExecute.Item3));
                                        TimeSpan timeUntilClosure = closureDate - DateTime.Now; //GET the number of remaining DAys
                                        //if (caseToBeClosed.JudgementDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-intervalDuration).ToString("dd/MM/yyyy"))
                                        if (caseToBeClosed.JudgementDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-intervalDuration + _reminderMethods.FetchNumberOfHolidaysAndWeekends(isCorrectTimeToExecute.Item2, caseToBeClosed.JudgementDate.Date, isCorrectTimeToExecute.Item3)).ToString("dd/MM/yyyy"))
                                        {
                                            //var HOSUser = await _workerServiceRepository.GetHOSBySectorId(caseToBeClosed.SectorTypeId);
                                            //Users.Add(caseToBeClosed.LawyerId);
                                            // Users.Add(HOSUser.Id);

                                            var HOSUsers = await _workerServiceRepository.GetHOSAndViceHOSBySectorId("", caseToBeClosed.SectorTypeId, false, true, caseToBeClosed.ChamberNumberId);
                                            wSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM = caseToBeClosed;

                                            NotificationParameter notificationParameter = new NotificationParameter();
                                            notificationParameter.CaseNumber = caseToBeClosed.CaseNumber;
                                            notificationParameter.Duration = timeUntilClosure.Days.ToString();
                                            notificationParameter.SectorTo = caseToBeClosed.SectorTo;

                                            //var taskResult = await _reminderMethods.CreateTaskAsync(("Closing case" + caseToBeClosed.CaseId), hosUser.Id, caseToBeClosed.SectorTypeId, caseToBeClosed.FileId, "requests", "closing-case", "", "");
                                            if (HOSUsers.Any())
                                            {
                                                foreach (var user in HOSUsers)
                                                {
                                                    // Sending notifications to both the HOS and the ViceHOS to take action before the file is closed
                                                    cmsComsReminderProcessLog.IsNotification = await _reminderMethods.SendReminderAsync(
                                                                                    user.Id,
                                                                                    (int)WorkflowModuleEnum.CaseManagement,
                                                                                    caseToBeClosed.SectorTypeId==(int)OperatingSectorTypeEnum.AdministrativeSupremeCases || caseToBeClosed.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases ? (int)NotificationEventEnum.CaseCloseReminder: (int)NotificationEventEnum.CaseCloseReminder,
                                                                                    "view",
                                                                                    "case",
                                                                                    caseToBeClosed.CaseId.ToString(),
                                                                                    notificationParameter);
                                                    cmsComsReminderProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegionalORAppealORSupreme;
                                                    cmsComsReminderProcessLog.ReminderId = reminderType.ID;
                                                    cmsComsReminderProcessLog.ReferenceId = caseToBeClosed.CaseId.ToString();
                                                    cmsComsReminderProcessLog.ReceiverId = user.Id;
                                                    cmsComsReminderProcessLog.Description = "Closing case " + caseToBeClosed.CaseId;
                                                    cmsComsReminderProcessLog.ReminderTypeId = reminderType.CmsComsReminderTypeId;
                                                    var reminderProcessLog = await _workerServiceRepository.ReminderProcessLogAsync(cmsComsReminderProcessLog);
                                                }
                                                HOSUsers = new List<User>();
                                                //Users = new List<string>();
                                            }
                                        }
                                        if (closureDate.Date == DateTime.Now.Date)
                                        {
                                            await _workerServiceRepository.CloseCaseAndTaskStatusDone(caseToBeClosed.CaseId);
                                        }
                                    }
                                }
                                reminderNo++;
                            }
                        }
                        reminderNo--;
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
                    cmsComsReminderErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DefineThePeriodToRegionalORAppealORSupreme;
                    /*cmsComsReminderErrorLog.ReminderId = 1;*/
                    cmsComsReminderErrorLog.ReferenceId = wSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM?.CaseId.ToString();
                    cmsComsReminderErrorLog.ReceiverId = $"HOSandVICSHOS from this caseID :  {wSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM?.CaseId.ToString()}";
                    cmsComsReminderErrorLog.Message = ("Failed to close case " + wSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM.CaseId);
                    cmsComsReminderErrorLog.ReminderTypeId = (int)CmsComsReminderTypeEnums.DefineThePeriodToRegionalORAppealORSupreme;
                    await _workerServiceRepository.ReminderErrorLogAsync(cmsComsReminderErrorLog);
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Exception;
                    workerServiceExecution.ExecutionDetails = ("Exception occur due to " + ex.Message);
                    workerServiceExecution.ReAttemptCount = reAttemptCount;
                    await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);

                    _IAuditLog.CreateErrorLog(new ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = $"HOSReminderServiceAppeal failed for case id {wSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM.CaseId}",
                        Body = ex.Message,
                        Category = "HOSReminderServiceAppeal",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = $"HOSReminderServiceAppeal failed for case id {wSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM.CaseId}",
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



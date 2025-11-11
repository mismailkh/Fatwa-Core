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
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_DOMAIN.Models.Consultation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FATWATIMEINTERVALSERVICES.Services
{
    public class ReminderForPendingTaskDecisionService : IJob
    {
        //<History Author = 'Muhammad Zaeem' Date='2024-11-28' Version="1.0" Branch="master"> Worker Service to Remind Manager For Pending Task Decision</History>

        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReminderForPendingTaskDecisionService> _logger;
        private readonly ITimeIntervals _iTimeIntervals;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;


        int reAttemptCount = 0;
        Guid ExecutionId;
        CmsComsReminder? reminderInterval;
        #endregion

        #region Constructor
        public ReminderForPendingTaskDecisionService(ILogger<ReminderForPendingTaskDecisionService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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

        #region Reminder For Pending Task Decision
        public async Task Execute(IJobExecutionContext context)
        {
            WSReminderForPendingTaskDecisionVM reminderForPendingTaskDecisionVM = new WSReminderForPendingTaskDecisionVM();
            do
            {
                try
                {
                    int NumberOfDays = 2;
                    var reminder = await _workerServiceRepository.GetReminderForPendingTaskDecision(NumberOfDays);
                    if (reminder != null)
                    {
                        if (reminder.Any())
                        {
                            WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog();
                            foreach (var task in reminder)
                            {
                                string entityName = "";
                                string action = "";
                                reminderForPendingTaskDecisionVM = task;
                                NotificationParameter notificationParameter = new NotificationParameter();
                                notificationParameter.ReferenceNumber = task.ReferenceNumber;
                                notificationParameter.CreatedDate = task.CreatedDate;
                                notificationParameter.AssigneeNameEn = task.AssignedToEn; 
                                notificationParameter.AssigneeNameAr = task.AssignedToAr; 
                                notificationParameter.AssignorNameEn = task.AssignedByEn; 
                                notificationParameter.AssignorNameAr = task.AssignedByAr;
                                notificationParameter.SubjectEn = task.SubjectEn;
                                notificationParameter.SubjectAr = task.SubjectAr;
                                if (task.SubModuleId == (int)SubModuleEnum.CaseRequest)
                                {
                                    entityName = new CaseRequest().GetType().Name;
                                    action = "view";
                                }
                                else if (task.SubModuleId == (int)SubModuleEnum.CaseFile)
                                {
                                    entityName = new CaseFile().GetType().Name;
                                    action = "view";
                                }
                                else if (task.SubModuleId == (int)SubModuleEnum.RegisteredCase)
                                {
                                    entityName = "Case";
                                    action = "view";
                                }
                                else if (task.SubModuleId == (int)SubModuleEnum.ConsultationRequest)
                                {
                                    entityName = new ConsultationRequest().GetType().Name;
                                    action = "detail";

                                }
                                else if (task.SubModuleId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    entityName = new ConsultationFile().GetType().Name;
                                    action = "view";
                                }
                                notificationParameter.Entity = entityName;
                                if (!string.IsNullOrEmpty(task.ManagerId))
                                {
                                    task.TaskUrl = task.TaskUrl + "/" + task.TaskId;
                                    var ManagerNotification = await _reminderMethods.SendReminderAsync(task.ManagerId, (int)WorkflowModuleEnum.Task, (int)NotificationEventEnum.PendingTaskReminderForManager, action, entityName, task.ReferenceId.ToString(), notificationParameter ,task.TaskUrl);
                                    if (ManagerNotification)
                                    {
                                        TaskDecisionReminder taskDecisionReminder = new TaskDecisionReminder();
                                        taskDecisionReminder.Id = Guid.NewGuid();
                                        taskDecisionReminder.TaskId = task.TaskId;
                                        taskDecisionReminder.IsReminderSent = true;
                                        taskDecisionReminder.CreatedDate = DateTime.Now;
                                       await _workerServiceRepository.SavePendingTaskReminder(taskDecisionReminder);

                                    }
                                }
                            }
                        }
                    }
                    
                    break;
                }
                catch (Exception ex)
                {
                   
                    throw new Exception(ex.Message);
                }
            }
            while (reAttemptCount < 3);

        }
        #endregion
    }
}



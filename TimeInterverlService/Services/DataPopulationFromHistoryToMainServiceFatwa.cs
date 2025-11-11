using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using FATWATIMEINTERVALSERVICES.Helper;
using Quartz;
using FATWA_DOMAIN.Common;
using FATWATIMEINTERVALSERVICES.RabbitMQ;

namespace FATWATIMEINTERVALSERVICES.Services
{
    public class DataPopulationFromHistoryToMainServiceFatwa : IJob
    {

        #region Variables      
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly ILogger<DataPopulationFromHistoryToMainServiceFatwa> _logger;
        private readonly ReminderMethods _reminderMethods;
        private readonly WorkerServiceRepository _workerServiceRepository;
        private readonly RabbitMQClient _client;

        int reAttemptCount = 0;
        Guid ExecutionId;
        #endregion

        #region Constructor 
        public DataPopulationFromHistoryToMainServiceFatwa(ILogger<DataPopulationFromHistoryToMainServiceFatwa> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, RabbitMQClient client)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _workerServiceRepository = scope.ServiceProvider.GetRequiredService<WorkerServiceRepository>();
            _IAuditLog = scope.ServiceProvider.GetRequiredService<IAuditLog>();
            _reminderMethods = scope.ServiceProvider.GetRequiredService<ReminderMethods>();
            _client = client;
        }
        #endregion

        #region Number Pattren Modification from History to Main table and ALSO to G2G 
        public async Task Execute(IJobExecutionContext context)
        {
            do
            {
                WSCmsComsReminderErrorLog cmsComsDataMigrationErrorLog = new WSCmsComsReminderErrorLog();
                WSWorkerServiceExecution workerServiceExecution = new WSWorkerServiceExecution();
                WSCmsComsReminderProcessLog cmsComsSDataMigrationProcessLog = new WSCmsComsReminderProcessLog();
                try
                {
                    _logger.LogInformation("Worker service running at: {time}", DateTimeOffset.Now);
                    ExecutionId = Guid.NewGuid();
                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.DataMigrationFromHistoryToMain;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);

                    var dataMigration = _workerServiceRepository.MigratePatternsFromHistory().Result;
                    if (dataMigration.Item1 == true)
                    {
                        cmsComsSDataMigrationProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DataMigrationFromHistoryToMain;
                        cmsComsSDataMigrationProcessLog.Description = dataMigration.Item2;
                        var reminderProcessLog = await _workerServiceRepository.ProcessLogAsync(cmsComsSDataMigrationProcessLog);

                        workerServiceExecution.Id = ExecutionId;
                        workerServiceExecution.ExecutionDetails = "Successfully Completed";
                        workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                        await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);
                    }
                    else
                    {
                        cmsComsDataMigrationErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DataMigrationFromHistoryToMain;
                        cmsComsDataMigrationErrorLog.Message = dataMigration.Item2;
                        await _workerServiceRepository.ErrorLogAsync(cmsComsDataMigrationErrorLog);
                    }

                    // Rabbit Call to reflect Pattrn History Data to G2G for latest Number Pattren
                    if (dataMigration.Item1 == true && dataMigration.Item3 is not null)
                    {
                        await _client.SendMessage(dataMigration.Item3, RabbitMQKeys.GovEntityPatternEditKey);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    reAttemptCount++;
                    cmsComsDataMigrationErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.DataMigrationFromHistoryToMain;
                    cmsComsDataMigrationErrorLog.Message = ("Failed_to_Migrate_Data");
                    await _workerServiceRepository.ErrorLogAsync(cmsComsDataMigrationErrorLog);

                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Exception;
                    workerServiceExecution.ExecutionDetails = ("Exception occur due to " + ex.Message);
                    workerServiceExecution.ReAttemptCount = reAttemptCount;
                    await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);

                    _IAuditLog.CreateErrorLog(new ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = "Data Migration from History to Main for Number Pattern",
                        Body = ex.Message,
                        Category = "Unable to Send Data from History to Main table for Number pattern",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Unable to Send Data from History to Main table for Number pattern",
                        IPDetails = "WorkerService",
                        ApplicationID = (int)PortalEnum.WorkerServices,
                        ModuleID = (int)WorkflowModuleEnum.NumberPatternWorkerService
                    });
                }
            }
            while (reAttemptCount < 3);
        }
        #endregion

    }
}

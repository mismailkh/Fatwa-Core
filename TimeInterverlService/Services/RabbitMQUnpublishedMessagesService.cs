using Quartz;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Interfaces;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using Newtonsoft.Json;
using FATWATIMEINTERVALSERVICES.RabbitMQ;

namespace FATWATIMEINTERVALSERVICES.Services
{
    public class RabbitMQUnpublishedMessagesService : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;
        private readonly ILogger<RabbitMQUnpublishedMessagesService> _logger;
        private readonly WorkerServiceRepository _workerServiceRepository;
        private readonly RabbitMQClient _client;


        int reAttemptCount = 0;
        Guid ExecutionId;

        public RabbitMQUnpublishedMessagesService(ILogger<RabbitMQUnpublishedMessagesService> logger, IServiceScopeFactory serviceScopeFactory, RabbitMQClient client)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _workerServiceRepository = scope.ServiceProvider.GetRequiredService<WorkerServiceRepository>();
            _IAuditLog = scope.ServiceProvider.GetRequiredService<IAuditLog>();
            _client = client;
        }


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
                    workerServiceExecution.WorkerServiceId = (int)WorkerServiceTypeEnums.RabbitMQUnpublishedMessage;
                    workerServiceExecution.StartDateTime = DateTime.Now;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    workerServiceExecution.ExecutionDetails = "Successfully Started";
                    await _workerServiceRepository.AddWorkerServiceExecution(workerServiceExecution);

                    var UnpublshedMessages = _workerServiceRepository.RabbitMQMessages().Result;
                    if (UnpublshedMessages.Any())
                    {
                        foreach (var queue in UnpublshedMessages)
                        {
                            //RabbitMQClient rabbit = new RabbitMQClient(_configuration, _IAuditLog, _serviceScopeFactory);
                            var IsMessagePublished = await _client.SendMessage(JsonConvert.DeserializeObject<object>(queue.RQMessages), queue.RoutingKey, true);
                            await _workerServiceRepository.MarkMessageAsPublished(queue.Id, IsMessagePublished);

                        }
                    }

                    var description = UnpublshedMessages.Any() ? "Message Published Successfully" : "No RabbitMQ messages in Queue";
                    cmsComsSDataMigrationProcessLog.WorkerServiceId = (int)WorkerServiceTypeEnums.RabbitMQUnpublishedMessage;
                    cmsComsSDataMigrationProcessLog.Description = description;
                    await _workerServiceRepository.ProcessLogAsync(cmsComsSDataMigrationProcessLog);

                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.ExecutionDetails = "Successfully Completed";
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Successfull;
                    await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);

                    break;
                }
                catch (Exception ex)
                {
                    reAttemptCount++;
                    cmsComsDataMigrationErrorLog.WorkerServiceId = (int)WorkerServiceTypeEnums.RabbitMQUnpublishedMessage;
                    cmsComsDataMigrationErrorLog.Message = ("Failed_to_Publishe RabbiqMQ MEssages");
                    await _workerServiceRepository.ErrorLogAsync(cmsComsDataMigrationErrorLog);

                    workerServiceExecution.Id = ExecutionId;
                    workerServiceExecution.ExecutionStatusId = (int)WorkerServiceExecutionStatusEnums.Exception;
                    workerServiceExecution.ExecutionDetails = ("Exception occur due to " + ex.Message);
                    workerServiceExecution.ReAttemptCount = reAttemptCount;
                    await _workerServiceRepository.UpdateWorkerServiceExecution(workerServiceExecution);

                    _IAuditLog.CreateErrorLog(new FATWA_DOMAIN.Models.ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = "failed to published RabbitMQ unpublished message",
                        Body = ex.Message,
                        Category = "RabbiqMQ messages",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Unable to published RabbiqMQ Queues messages ",
                        IPDetails = "WorkerService",
                        ApplicationID = (int)PortalEnum.WorkerServices,
                        ModuleID = (int)WorkflowModuleEnum.NumberPatternWorkerService
                    });
                }

            }
            while (reAttemptCount < 3);
        }
    }
}

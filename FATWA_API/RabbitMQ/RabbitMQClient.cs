using FATWA_DOMAIN.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using FATWA_DOMAIN.Models;
using static FATWA_GENERAL.Helper.Enum;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.RabbitMQ;
using Newtonsoft.Json;
using FATWA_INFRASTRUCTURE.Database;

namespace FATWA_API.RabbitMQ
{
    public class RabbitMQClient
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private readonly IAuditLog _auditLogs;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const string ExchangeName = "Topic_Exchange";
        //private const string ExchangeName = "RPA_Exchange";
       
        public RabbitMQClient(IConfiguration configuration, IAuditLog auditLogs, IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _auditLogs = auditLogs;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private void CreateConnection()
        {
            try
            {
                _factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQClient:Host"],
                    Port = Int16.Parse(_configuration["RabbitMQClient:Port"]),
                    UserName = _configuration["RabbitMQClient:UserName"],
                    Password = _configuration["RabbitMQClient:Password"],
                };

                //_factory = new ConnectionFactory
                //{
                //    HostName = "115.186.185.190",
                //    Port = 5672,
                //    UserName = "fatwa",
                //    Password = "fatwa",
                //    VirtualHost = "RPAVHost"
                //};

                _connection = _factory.CreateConnection("Fatwa_Producer");
				_model = _connection.CreateModel();

            } catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        public async void SendMessage(object obj, string RoutingKey)
        {
            try
            {
                CreateConnection();
                PublishMessage(obj.Serialize(), RoutingKey);
            }
            catch (Exception ex)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                RMQ_UnpublishMessage rMQ_Unpublish = new RMQ_UnpublishMessage();
                rMQ_Unpublish.Id = Guid.NewGuid();
                rMQ_Unpublish.RoutingKey = RoutingKey;
                rMQ_Unpublish.RQMessages = JsonConvert.SerializeObject(obj);
                rMQ_Unpublish.CreatedBy = "System";
                rMQ_Unpublish.CreatedDate = DateTime.Now;
                await _DbContext.RMQ_UnpublishMessages.AddAsync(rMQ_Unpublish);
                await _DbContext.SaveChangesAsync();
            }
        }

        public void PublishMessage(byte[] message, string routingKey)
        {
            try
            {
                //reply mechanisam
                var replyQueue = _model.QueueDeclare("", exclusive: true, autoDelete: true);

                var consumer = new EventingBasicConsumer(_model);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Reply Recieved: {message}");
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Send Data from G2G Portal",
                        Task = "Reply Recieved RabbitMQ ",
                        Description = "Reply Recieved " + message.ToString(),
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Reply Recieved " + message.ToString(),
                        IPDetails = ea.RoutingKey,
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = String.Empty
                    });
                };

                _model.BasicConsume(queue: replyQueue.QueueName, autoAck: true, consumer: consumer);

                var properties = _model.CreateBasicProperties();
                properties.ReplyTo = replyQueue.QueueName;
                properties.CorrelationId = Guid.NewGuid().ToString();
                _model.BasicPublish(ExchangeName, routingKey, null, message);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}

using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Common.Service;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Email;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_GENERAL.Helper.Request;
using static FATWA_GENERAL.Helper.Response;

namespace TARASOLRPACONSUMER.RABBITMQ
{
    public class RabbitMQRPAConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private readonly IConfiguration _config;

        public string BaseURLFATWA = "";
        public string BaseURLDMS = "";
        public static string RestAPIUrl = "";
        public static string Token = "";

        IdentityRequest identityUser = new IdentityRequest();
        IEmailService emailService;

        public RabbitMQRPAConsumer(IConfiguration config )
        {
            _config = config;
            emailService = new EmailService();
            BaseURLFATWA = _config["api_url"];
            BaseURLDMS = _config["dms_api_url"];
        }

        RestAPICall restAPICall = new RestAPICall();

        private const string ExchangeName = "RPA_Exchange";
        private const string DLXExchangeName = "dlx_exchange";
        // Queues
        private const string TarasolQueue = "TarasolQueue";
        private const string DLXQueue = "DLXQueue";
        // keys
        private const string TarasolPayloadKey = "TarasolPayloadKey";
        private const string TarasolDocKey = "TarasolDocKey";

        // mojImageQueue // mojImageKey


        
        public void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQClient:Host"],
                Port = Int16.Parse(_config["RabbitMQClient:Port"]),
                UserName = _config["RabbitMQClient:UserName"],
                Password = _config["RabbitMQClient:Password"],
                VirtualHost = _config["RabbitMQClient:vhost"]
            };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, type: ExchangeType.Direct);
            _model.ExchangeDeclare(DLXExchangeName, type: ExchangeType.Direct);

            _model.QueueDeclare(TarasolQueue, true, false, false, arguments:new Dictionary<string, object>
            {
                { "x-dead-letter-exchange",DLXExchangeName},
            });
            _model.QueueDeclare(DLXQueue, true, false, false, null);
            _model.QueueBind(TarasolQueue, ExchangeName, TarasolPayloadKey);
            _model.QueueBind(TarasolQueue, ExchangeName, TarasolDocKey);
            _model.QueueBind(DLXQueue, DLXExchangeName, TarasolPayloadKey);
            _model.QueueBind(DLXQueue, DLXExchangeName, TarasolDocKey);
        }

        public void Close()
        {
            _connection.Close();
        }
        public void GetToken()
        {
            identityUser.UserName = _config["identityUser:UserName"];
            identityUser.Password = _config["identityUser:Password"];

            var response = restAPICall.GetToken(BaseURLFATWA, identityUser);
            if (response.Result.IsSuccessStatusCode)
            {
                var res = response.Result;
                Token = res.ResultData.ToString();
            }
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic from G2G RabbitMQ");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    channel.BasicQos(0, 1, false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        Log.Information("Message Receive From RoutingKey" + ea.RoutingKey);
                        switch (ea.RoutingKey)
                        {
                            case TarasolPayloadKey:
                                // code block
                                RestAPIUrl = "/CommunicationTarasolRPA/AddCommunicationData";
                                await ConsumeMessages(channel, ea);
                                break;
                            case TarasolDocKey:
                                // code block
                                RestAPIUrl = "/FileUpload/UploadG2GTarasolCorrespondenceDocumentRabbitMQ";
                                await ConsumeMessages(channel, ea);
                                break;
                            default:
                                // code block
                                var replyMessage = $"fatwa message to this corelation Id: {ea.BasicProperties.CorrelationId}";
                                var body = Encoding.UTF8.GetBytes(replyMessage);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                                channel.BasicPublish("", ea.BasicProperties.ReplyTo, null, body);
                                break;
                        }

                        async Task ConsumeMessages(IModel channel, BasicDeliverEventArgs resp)
                        {
                            GetToken();
                            // Get Email Configuration
                            var emailCongResponse = await restAPICall.GetEmailConfiguration(Token, (int)CommunicationSourceEnum.Tarasul, BaseURLFATWA + "/Common/GetEmailConfiguration");

                            var message = (object)resp.Body.DeSerialize(typeof(object));
                            Console.WriteLine(message);
                            CommunicationTarasolRpaPayload CommunicationTarasolPayload = new CommunicationTarasolRpaPayload
                            {
                                Id = Guid.NewGuid(),
                                Payload = JsonConvert.SerializeObject(message),
                                isSucceeded = false,
                                CreatedBy = "G2G TARASOL RPA",
                                CreatedDate = DateTime.Now
                            };
                            ApiCallResponse response = new ApiCallResponse();
                            if (resp.RoutingKey == TarasolPayloadKey)
                            {
                                var communicationPayload = (CommunicationTarasolRPAPayload)resp.Body.DeSerialize(typeof(CommunicationTarasolRPAPayload));
                                CommunicationTarasolPayload.CorrespondenceId = communicationPayload.CorrespondenceId;
                                CommunicationTarasolPayload.CommunicationPayload = true;
                                response = await restAPICall.postData(Token, message, BaseURLFATWA + RestAPIUrl);
                            }
                            else if (resp.RoutingKey == TarasolDocKey)
                            {
                                var documentsDMS = (CommunicationDocumentPayload)resp.Body.DeSerialize(typeof(CommunicationDocumentPayload));
                                CommunicationTarasolPayload.CorrespondenceId = documentsDMS.CorrespondenceId;
                                CommunicationTarasolPayload.CommunicationDocumentPayload = true;
                                response = await restAPICall.post2Data(Token, documentsDMS, BaseURLDMS + RestAPIUrl);
                            }
                                
                            if (response.IsSuccessStatusCode)
                            {
                                channel.BasicAck(deliveryTag: resp.DeliveryTag, multiple: false);
                                Log.Information("Successfully Process the Message and Acknowledged");
                            }
                            else
                            {
                                BadRequestResponse? badRequest = (BadRequestResponse)response.ResultData ?? new BadRequestResponse();
                                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && (badRequest.Message != null && badRequest.Message.Contains("No connection could be made because the target machine actively refused it")))
                                {
                                    channel.BasicNack(deliveryTag: resp.DeliveryTag, multiple: false, requeue: true);
                                }
                                else
                                {
                                    response = await restAPICall.postData(Token, CommunicationTarasolPayload, BaseURLFATWA + "/CommunicationTarasolRPA/AddFaultyCommunicationData");
                                    if (emailCongResponse.IsSuccessStatusCode)
                                    {
                                        var emailConfiguration = (EmailConfiguration)emailCongResponse.ResultData;
                                        emailConfiguration.EmailBody = message.ToString();
                                        emailService.Send(emailConfiguration);
                                    }
                                    channel.BasicNack(deliveryTag: resp.DeliveryTag, multiple: false, requeue: false);
                                }
                            }
                        }
                    };
                    channel.BasicConsume(queue: TarasolQueue, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}

using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Common.Service;
using FATWA_DOMAIN.Models.Email;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using System.Security.AccessControl;
using System.Text;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_GENERAL.Helper.Request;

namespace FATWACONSUMER.RABBITMQ
{
    public class RabbitMQPullConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private readonly IConfiguration _config;

        IdentityRequest identityUser = new IdentityRequest();

        public RabbitMQPullConsumer(IConfiguration config)
        {
            _config = config;
        }

        RestAPICall restAPICall = new RestAPICall();
        IEmailService emailService;

        private const string ExchangeName = "Topic_Exchange";
        // Queues
        private string[] QueueNames = { "CMSCaseRequest", "G2GCommunication", "G2GCOMSConsultationRequest", "GERepresentativeG2GQueue", "G2GMeetingQueue", "G2GBugReporting" };
        // keys
        private const string CreateRequestKey = "routingkeys.createCaseRequest";
        private const string UpdateRequestKey = "routingkeys.updateCaseRequest";
        private const string WithdrawRequestKey = "routingkeys.withdrawCaseRequest";
        private const string CommunicationKey = "routingkeys.G2GCommunicationKey";
        private const string CreateConsultationRequestKey = "routingkeys.CreateConsultationRequestKey";
        private const string UpdateConsultationRequestKey = "routingkeys.UpdateConsultationRequestKey";
        private const string WithdrawConsultationRequestKey = "routingkeys.WithdrawConsultationRequest";
        private const string G2GRequestStatusKey = "routingkeys.G2GRequestStatusKey";
        private const string GERepresentativeG2GKey = "routingkeys.GERepresentativeG2GKey";
        private const string UpdateMeetingStatusKey = "routingkeys.UpdateMeetingStatus";
        private const string caseJudgmentDecisionKey = "routingkeys.caseJudgmentDecisionKey";
        private const string UpdateGovEntityPatternKey = "routingkeys.UpdateGovEntityPatternKey";
        private const string SendGEAttendee = "routingkeys.UpdateGovEntityPatternKey";
        private const string UpdateMeetingMOMAttendeeDecisionKey = "routingkeys.UpdateMeetingMOMAttendeeDecisionKey";
        private const string RaiseTicketKey = "routingkeys.raiseTicketKey";
        private const string AddFatwaCommentFeedBackKey = "routingkeys.addFatwaCommentFeedBackKey";
        private const string UpdateFatwaCommentFeedBackKey = "routingkeys.updateFatwaCommentFeedBackKey";
        private const string DeleteFatwaCommentFeedBackKey = "routingkeys.deleteFatwaCommentFeedBackKey";
        private const string UpdateFatwaTicketStatus = "routingkeys.updateFatwaTicketStatus";
        private const string UpdateFatwaAllTicketStatus = "routingkeys.updateFatwaAllTicketStatus";


        public string BaseURL = "";
        public static string RestAPIUrl = "";
        //public static string Token = "";


        public void CreateConnection()
        {
          //  BaseURL = _config["Base_Url"];
            _factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQClient:Host"],
                Port = Int16.Parse(_config["RabbitMQClient:Port"]),
                UserName = _config["RabbitMQClient:UserName"],
                Password = _config["RabbitMQClient:Password"]
            };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, type: ExchangeType.Topic);

            _model.QueueDeclare(QueueNames[0], true, false, false, arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });
            _model.QueueDeclare(QueueNames[1], true, false, false, arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });
            _model.QueueDeclare(QueueNames[2], true, false, false, arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });
            _model.QueueDeclare(QueueNames[3], true, false, false, arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });
            _model.QueueDeclare(QueueNames[4], true, false, false, arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });
            _model.QueueDeclare(QueueNames[5], true, false, false, arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });



            _model.QueueBind(QueueNames[0], ExchangeName, CreateRequestKey);
            _model.QueueBind(QueueNames[0], ExchangeName, UpdateRequestKey);
            _model.QueueBind(QueueNames[0], ExchangeName, WithdrawRequestKey);
            _model.QueueBind(QueueNames[1], ExchangeName, CommunicationKey);
            _model.QueueBind(QueueNames[2], ExchangeName, CreateConsultationRequestKey);
            _model.QueueBind(QueueNames[2], ExchangeName, UpdateConsultationRequestKey);
            _model.QueueBind(QueueNames[2], ExchangeName, WithdrawConsultationRequestKey);
            _model.QueueBind(QueueNames[0], ExchangeName, G2GRequestStatusKey);
            _model.QueueBind(QueueNames[3], ExchangeName, GERepresentativeG2GKey);
            _model.QueueBind(QueueNames[4], ExchangeName, UpdateMeetingStatusKey);
            _model.QueueBind(QueueNames[0], ExchangeName, caseJudgmentDecisionKey);
            _model.QueueBind(QueueNames[0], ExchangeName, SendGEAttendee);
            _model.QueueBind(QueueNames[4], ExchangeName, UpdateMeetingMOMAttendeeDecisionKey);
            _model.QueueBind(QueueNames[5], ExchangeName, RaiseTicketKey);
            _model.QueueBind(QueueNames[5], ExchangeName, AddFatwaCommentFeedBackKey);
            _model.QueueBind(QueueNames[5], ExchangeName, DeleteFatwaCommentFeedBackKey);
            _model.QueueBind(QueueNames[5], ExchangeName, UpdateFatwaCommentFeedBackKey);
            _model.QueueBind(QueueNames[5], ExchangeName, UpdateFatwaTicketStatus);
            _model.QueueBind(QueueNames[5], ExchangeName, UpdateFatwaAllTicketStatus);

        }

        public void Close()
        {
            _connection.Close();
        }
        public void GetToken()
        {
            BaseURL = _config["Base_Url"];

            //identityUser.UserName = _config["identityUser:UserName"];
            //identityUser.Password = _config["identityUser:Password"];

            //var response = restAPICall.GetToken(BaseURL, identityUser);
            //if (response.Result.IsSuccessStatusCode)
            //{
            //    var res = response.Result;
            //    Token = res.ResultData.ToString();
            //}
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection("Fatwa_Consumer"))
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic from Fatwa RabbitMQPull");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    channel.BasicQos(0, 1, false);

                    while (true)
                    {
                        foreach (string QueueName in QueueNames)
                        {
                            BasicGetResult resp = channel.BasicGet(QueueName, false);
                            if (resp != null)
                            {
                                Log.Information("Message Receive From Queue" + QueueName);
                                var message = (object)resp.Body.DeSerialize(typeof(object));
                                Console.WriteLine(message);
                                switch (resp.RoutingKey)
                                {
                                    case CreateRequestKey:
                                        // code block
                                        RestAPIUrl = "/CMSCaseRequest/CreateCaseRequest";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case UpdateRequestKey:
                                        // code block
                                        RestAPIUrl = "/CMSCaseRequest/UpdateCaseRequest";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case WithdrawRequestKey:
                                        // code block
                                        RestAPIUrl = "/CMSCaseRequest/WithdrawCaseRequest";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case CommunicationKey:
                                        // code block
                                        RestAPIUrl = "/Communication/SendCommunication";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case CreateConsultationRequestKey:
                                        // code block
                                        RestAPIUrl = "/COMSConsultation/CreateConsultationRequest";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case UpdateConsultationRequestKey:
                                        // code block
                                        RestAPIUrl = "/COMSConsultation/UpdateConsultationRequest";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case WithdrawConsultationRequestKey:
                                        // code block
                                        RestAPIUrl = "/COMSConsultation/CreateConsultationWithDrawRequest";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case G2GRequestStatusKey:
                                        // code block
                                        RestAPIUrl = "/Common/UpdateEntityStatus";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case GERepresentativeG2GKey:
                                        // code block
                                        RestAPIUrl = "/CmsShared/CreateGeRepresentative";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case UpdateMeetingStatusKey:
                                        // code block
                                        RestAPIUrl = "/Meeting/UpdateMeetingStatus";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case caseJudgmentDecisionKey:
                                        // code block
                                        RestAPIUrl = "/CmsRegisteredCase/AddJudgementDecision";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case SendGEAttendee:
                                        // code block
                                        RestAPIUrl = "/Meeting/GetGEAttendeeDetails";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case UpdateMeetingMOMAttendeeDecisionKey:
                                        // code block
                                        RestAPIUrl = "/Meeting/UpdateMeetingMOMAttendeeDecision";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case RaiseTicketKey:
                                        // code block
                                        RestAPIUrl = "/BugReporting/CreateBugTicket";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case AddFatwaCommentFeedBackKey:
                                        // code block
                                        RestAPIUrl = "/BugReporting/AddCommentFeedBack";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case UpdateFatwaCommentFeedBackKey:
                                        // code block
                                        RestAPIUrl = "/BugReporting/UpdateCommentFeedBack";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case DeleteFatwaCommentFeedBackKey:
                                        // code block
                                        RestAPIUrl = "/BugReporting/DeleteBugTicketComment";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case UpdateFatwaTicketStatus:
                                        // code block
                                        RestAPIUrl = "/BugReporting/UpdateTicketStatus";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    case UpdateFatwaAllTicketStatus:
                                        // code block
                                        RestAPIUrl = "/BugReporting/UpdateAllSelectedTicketStatus";
                                        ConsumeMessages(channel, resp);
                                        break;
                                    default:
                                        break;

                                }
                            }

                        }
                    }
                }
            }
        }
        public async Task ConsumeMessages(IModel channel, BasicGetResult resp)
        {
           var message = (object)resp.Body.DeSerialize(typeof(object));
            var response = await restAPICall.post1Data(_config["FatwaApiKey"], message,  BaseURL+RestAPIUrl);
            if (response.IsSuccessStatusCode)
            {
                var replyMessage = $"Success: Data is successfully sent to Fatwa with respect to this corelation Id: {resp.BasicProperties.CorrelationId}";
                var body = Encoding.UTF8.GetBytes(replyMessage);
                channel.BasicAck(deliveryTag: resp.DeliveryTag, multiple: false);
                channel.BasicPublish("", resp.BasicProperties.ReplyTo, null, body);
                Log.Information("Successfully Process the Message and Acknowledged");
            }
            else
            {
                //channel.BasicNack(deliveryTag: resp.DeliveryTag, multiple: false, requeue: true);
                channel.BasicAck(deliveryTag: resp.DeliveryTag, multiple: false);
                Log.Warning("Not Successfully Process the Message and Unacknowledged");
                Log.Information("API URL" + RestAPIUrl);
                Log.Debug("Message" + message);
                
            }

        }
    }
}

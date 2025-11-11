
namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    public class SendNotificationVM
    { 
        public Notification Notification { get; set; }
        public int EventId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public NotificationParameter NotificationParameter { get; set; }
    }
}

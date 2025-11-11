
namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    public class CheckNotificationVM
    {
        public List<Guid> notificationIds { get; set; }
        public int notificationStatus { get; set; }
        public DateTime? NotificationReadDate { get; set; }
    }
}

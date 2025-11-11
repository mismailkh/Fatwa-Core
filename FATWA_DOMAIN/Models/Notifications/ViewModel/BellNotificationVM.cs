using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    public class BellNotificationVM
    {
        public Guid NotificationId { get; set; } 
        public string? NotificationMessageEn { get; set; }
        public string? NotificationMessageAr { get; set; }
        public string? EventNameEn { get; set; }
        public string? EventNameAr { get; set; }
        public string? Url { get; set; } 
        public DateTime CreationDate { get; set; } 
        [NotMapped] 
        public string? BellNotificationMessageEn { get; set; }
        [NotMapped]
        public string? BellNotificationMessageAr { get; set; }

        [NotMapped]
        public string? ReceiverId { get; set; }
        [NotMapped]
        public string? DeletedBy { get; set; }
    }
}

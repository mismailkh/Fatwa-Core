using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications
{
    [Table("NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP")]
    public class NotificationEventPlaceholders
    {
        [Key]
        public int PlaceHolderId { get; set; }
        public string PlaceHolderName { get; set; }
        public int? EventId { get; set; }
        public NotificationEvent Event { get; set; }
    }
}

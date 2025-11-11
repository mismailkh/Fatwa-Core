using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications
{
    [Table ("NOTIF_NOTIFICATION_CHANNEL_LKP")]
    public class NotificationChannel
    {
        [Key]
        public int ChannelId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public ICollection<NotificationTemplate> NotificationTemplates { get; set; }

    }
}

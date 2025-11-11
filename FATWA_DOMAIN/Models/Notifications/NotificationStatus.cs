using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications
{
    [Table("NOTIF_NOTIFICATION_STATUS_LKP")]
    public class NotificationStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}

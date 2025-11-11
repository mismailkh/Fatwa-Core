using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications
{
    [Table("NOTIF_NOTIFICATION_TEMPLATE")]
    public class NotificationTemplate : TransactionalBaseModel
    {
        [Key]
        public Guid TemplateId { get; set; }
        public int EventId { get; set; }
        public int ChannelId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? SubjectEn { get; set; }
        public string? SubjectAr { get; set; }
        public string BodyEn { get; set; }
        public string BodyAr { get; set; }
        public string? Footer { get; set; }
        public bool isActive { get; set; }
        public NotificationEvent? Event { get; set; }
        public NotificationChannel? Channel { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}

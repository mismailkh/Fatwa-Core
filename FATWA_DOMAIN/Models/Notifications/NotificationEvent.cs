using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications
{
    [Table("NOTIF_NOTIFICATION_EVENT")]
    public class NotificationEvent : TransactionalBaseModel
    {
        [Key]
        public int EventId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public int ReceiverTypeId { get; set; }
        public Guid? ReceiverTypeRefId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<NotificationEventPlaceholders>? NotificationEventPlaceholders { get; set; }
        public ICollection<NotificationTemplate>? NotificationTemplates { get; set; }
    }
}

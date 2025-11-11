using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications
{
    [Table("NOTIF_NOTIFICATION")]
    public partial class Notification : TransactionalBaseModel
    {
        [Key]
        public Guid NotificationId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int ModuleId { get; set; }
        public string NotificationURL{ get; set; }
        public string NotificationMessageEn { get; set; }
        public string NotificationMessageAr { get; set; }
        public int NotificationStatusId { get; set; }
        public Guid? NotificationTemplateId { get; set; }
        public NotificationTemplate NotificationTemplate { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        [NotMapped]
        public int EventId { get; set; }
        [NotMapped]
        public string Action { get; set; }
        [NotMapped]
        public string EntityName { get; set; }
        [NotMapped]
        public string EntityId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; }
    }
}

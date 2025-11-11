using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.Notifications
{
    public partial class NotificationVM : GridMetadata
    {
        [Key]
        public Guid NotificationId { get; set; }
        public string? SubjectAr { get; set; }
        public string? SubjectEn { get; set; }
        public string NotificationMessageEn { get; set; }
        public string NotificationMessageAr { get; set; }
        public string? NotificationLink { get; set; }
        public string? EventNameAr { get; set; }
        public string? EventNameEn { get; set; }
        public string? ReceiverId { get;set; } 
        public string? SenderId { get; set; }
        public DateTime? DueDate { get; set; }
        public string? ModuleNameAr { get; set; }
        public string? ModuleNameEn { get; set; }
        public int? NotificationStatus { get; set; }
        public DateTime? ReadDate { get; set; }


        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
       
    }
}

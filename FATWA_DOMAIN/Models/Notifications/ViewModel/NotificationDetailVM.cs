//NotifNoticationdetail view
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Notifications
{
    public partial class NotificationDetailVM
    {
        [Key]
        public Guid NotificationId { get; set; }
        public string EventNameEn { get; set; }
        public string EventNameAr { get; set; }
        public string NotificationMessageEn { get; set; }
        public string NotificationMessageAr { get; set; }
        public string Url { get; set; }
        public bool LinkIsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedByAr { get; set; }
        public string? CreatedByEn { get; set; }
        public string? SubjectAr { get; set; }
        public string? SubjectEn { get; set; }
        public string? BodyEn { get; set; }
        public string? BodyAr { get; set; }
        public string? Title_Ar { get; set; }
        public string? Title_En { get; set; }
        public string? ModuleName_Ar { get; set; }  
        public string? ModuleName_En { get; set; }  

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    public class NotificationEventListVM
    {
        public int EventId { get; set; }   
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? ReceiverTypeName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Browser { get; set; }
        public bool Email { get; set; }
        public bool Mobile { get; set; }
        public bool IsActive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    public class NotificationTemplateListVM
    {
        public Guid? TemplateId { get; set; }   
        public string? NameEn { get; set; }   
        public string? NameAr { get; set; }  
        public string? BodyAr { get; set; }   
        public string? BodyEn { get; set; }    
        public string? ChannelNameEn { get; set; }   
        public string? ChannelNameAr { get; set; }  
        public string? EventNameEn { get; set; }   
        public string? EventNameAr { get; set; }   
        public int? EventId { get; set; }   
        public DateTime? CreatedDate { get; set; }
        [NotMapped]
        public string? DeletedBy { get; set; }
        public bool isActive { get; set; }


    }
}

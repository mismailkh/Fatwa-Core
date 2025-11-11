using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs
{
    public class TimeIntervalDetailVM
    {
        [Key]
        public int ID { get; set; }
        public int FirstReminderDuration { get; set; }
        public int? SecondReminderDuration { get; set; }
        public int? ThirdReminderDuration { get; set; }
        public bool IsNotification { get; set; }
        public bool IsTask { get; set; }
        public string CommunicationTypeNameEn { get; set; }
        public string CommunicationTypeNameAr { get; set; }
        public string CreatedBy { get; set; }
        //public string Title { get; set; }
        public bool? IsFirstReminder { get; set; }
        public bool? IsSecondReminder { get; set; }
        public bool? IsThirdReminder { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

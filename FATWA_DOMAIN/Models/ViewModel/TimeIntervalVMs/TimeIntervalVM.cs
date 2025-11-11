using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs
{
    public class TimeIntervalVM
    {
        public int ID { get; set; }
        public int ReminderId { get; set; }
        public int? FirstReminderDuration { get; set; }
        public int? SecondReminderDuration { get; set; }
        public int? ThirdReminderDuration { get; set; }
        public int? SLAInterval { get; set; }
		public bool IsNotification { get; set; }
		public DateTime? ExecutionTime { get; set; }
        public bool? IsTask { get; set; }
        public int? CommunicationTypeId { get; set; }
        public string? CommunicationTypeAr { get; set; }
        public string? CommunicationTypeEn { get; set; }
        public string IntervalNameEn { get; set; }
        public string IntervalNameAr { get; set; }
        public string? CreatedByEn { get; set; }
        public string? CreatedByAr { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}

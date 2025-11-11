using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs
{
    public class TimeIntervalHistoryVM
    {
        [Key]
        public Guid Id { get; set; }
        public int ReminderId { get; set; }
        public int? FirstReminderDuration { get; set; }
        public int? SecondReminderDuration { get; set; }
        public int? ThirdReminderDuration { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public int? SLAInterval { get; set; }

        public int? CommunicationTypeId { get; set; }
        public string? CommunicationTypeAr { get; set; }
        public string? CommunicationTypeEn { get; set; }
        public string IntervalNameEn { get; set; }
        public string IntervalNameAr { get; set; }
        public string? CreatedByEn { get; set; }
        public string? CreatedByAr { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string StatusEn { get; set; }
        public string StatusAr { get; set; }
    }
}

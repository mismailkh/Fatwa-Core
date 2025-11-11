using FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("EXCEPTIONS")]
    public class AMSExeceptions
    {
        [Key]
        public int ExceptionLogId { get; set; }
        public int? ItemId { get; set; }
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionTraceback { get; set; }
        public DateTime? RecordedDatetime { get; set; }
        public bool? Viewed { get; set; }
        public bool? ViewedBy { get; set; }
        public DateTime? ViewedOn { get; set; }
      
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("SESSIONS")]
        public class AMSSession
        {
            [Key]
            public int SessionId { get; set; }
            public int ProcessId { get; set; }
            public int ResourceId { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }
            public int StatusId { get; set; }
            public string? Remarks { get; set; }
        }
}

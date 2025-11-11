using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("PROCESS")]
    public class AMSProcesses
    {
        [Key]
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public DateTime? LastRunDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LaunchDate { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public string? ProcessCode { get; set; }

    }
}

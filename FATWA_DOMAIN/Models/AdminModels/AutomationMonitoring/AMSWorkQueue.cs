using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("WORK_QUEUE")]
    public class AMSWorkQueue
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MaxAttempts { get; set; }
        public string? DefaultFilter { get; set; }
        [Required]
        public int ProcessId { get; set; }

    }
}

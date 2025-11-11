using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("WORK_QUEUE_LOG")]
    public class AMSWorkQueueLog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public DateTime EventTime { get; set; }
        [Required]
        public string Description { get; set; }
        public string? LogType { get; set; }
    }
}

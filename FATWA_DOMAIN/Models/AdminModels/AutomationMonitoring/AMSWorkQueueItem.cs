using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("WORK_QUEUE_ITEM")]
    public class AMSWorkQueueItem
    {
        [Key]
        public int Id { get; set; }
        public int? QueueId { get; set; }
        public int? StatusId { get; set; }
        public int? Attempts { get; set; }
        public int? Priority { get; set; }
        public string? Data { get; set; }
        public string? ItemName { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public string? CreatedBy { get; set; } 
        public string? UpdatedBy { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DateStarted { get; set; }
        public TimeSpan? CompletedDuration { get; set; }
        public string? Tag { get; set; }
        public string? ExceptionComment { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? ResourceId { get; set; }
        public bool IsFatwaManual { get; set; }
 
    }
}

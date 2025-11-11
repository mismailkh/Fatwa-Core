using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("RESOURCES")]
    public class AMSResources
    {
        [Key]
        public int Id { get; set; }
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
        public int StateId { get; set; } = 1; // Default state is 1 (Idle)
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public int? ProcessId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("SESSION_LOGS")]
    public class AMSSessionLog
    {
        [Key]
        public int Id { get; set; }

        public int? SessionId { get; set; }

        public int? ItemId { get; set; }

        public DateTime? EventTime { get; set; }

        public string? Description { get; set; }
    }
}

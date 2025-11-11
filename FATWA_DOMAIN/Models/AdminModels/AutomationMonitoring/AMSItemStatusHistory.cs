using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring
{
    [Table("ITEM_STATUS_HISTORY")]
    public class AMSItemStatusHistory
    {
        [Key]
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? FromStatusId { get; set; }
        public int? ToStatusId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

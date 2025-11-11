using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_SLA")]
    public partial class SLA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowSLAId { get; set; }
        public int WorkflowActivityId { get; set; }
        public int Duration { get; set; }
        public SlaAction ActionId { get; set; }
        [NotMapped]
        public int SequenceNumber { get; set; }
        [NotMapped]
        public bool IsExpanded { get; set; }
        [NotMapped]
        public string? ActivityName { get; set; }
        [NotMapped]
        public List<Parameter>? Parameters { get; set; } = new List<Parameter>();
    }
}

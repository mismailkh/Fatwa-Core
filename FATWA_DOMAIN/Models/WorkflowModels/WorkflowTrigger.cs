using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_TRIGGER")]
    public partial class WorkflowTrigger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowTriggerId { get; set; }
        public int ModuleTriggerId { get; set; }
        public int? WorkflowId { get; set; }
        [NotMapped]
        public bool HasConditions { get; set; }
        [NotMapped]
        public bool HasOptions { get; set; }
        [NotMapped]
        public IList<WorkflowCondition> WorkflowConditions { get; set; } = new List<WorkflowCondition>();
        [NotMapped]
        public IList<WorkflowOption> WorkflowOptions { get; set; } = new List<WorkflowOption>();
        [NotMapped]
        public IList<WorkflowTriggerSectorOptions> WorkflowTriggerSectorOptions { get; set; } = new List<WorkflowTriggerSectorOptions>();
        [NotMapped]
        public IList<WorkflowTriggerCondition> workflowTriggerConditions { get; set; } = new List<WorkflowTriggerCondition>();

    }
}

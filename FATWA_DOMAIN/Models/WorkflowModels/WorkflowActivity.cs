using FATWA_DOMAIN.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_ACTIVITY")]
    public partial class WorkflowActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowActivityId { get; set; }
        public int ActivityId { get; set; }
        public int? WorkflowId { get; set; }
        public int SequenceNumber { get; set; }
        public WorkflowBranch BranchId { get; set; }
        public bool IsTask { get; set; }
        public bool IsNotification { get; set; }

        [NotMapped] 
        public Guid UniqueIdentity { get; set; } = new Guid();
        [NotMapped]
        public bool HasSLA { get; set; }
        [NotMapped]
        public string? ActivityName { get; set; }
        [NotMapped]
        public List<Parameter>? Parameters { get; set; } = new List<Parameter>();
        [NotMapped]
        public IList<SLA>? SLAs { get; set; } = new List<SLA>();
        [NotMapped]
        public IList<WorkflowCondition>? WorkflowConditions { get; set; } = new List<WorkflowCondition>();
        [NotMapped]
        public ModuleActivity? ModuleActivity { get; set; }
         [NotMapped]
        public bool isColVisible { get; set; }
        [NotMapped]
        public IList<WorkflowOption>? WorkflowOptions { get; set; } = new List<WorkflowOption>();

    }
}

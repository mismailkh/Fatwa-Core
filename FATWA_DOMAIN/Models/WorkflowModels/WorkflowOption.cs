using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_OPTIONS")]
    public class WorkflowOption : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowOptionId { get; set; }
        public int ModuleOptionId { get; set; }
        public int WorkflowActivityId { get; set; }
        public WorkflowControl TrueCaseFlowControlId { get; set; }
        public int TrueCaseActivityNo { get; set; }
        [NotMapped]
        public Guid UniqueIdentity { get; set; } = new Guid();

        [NotMapped]
        public int SequenceNumber { get; set; }
        [NotMapped]
        public string? OptionName { get; set; }
        [NotMapped]
        public string? TrueCaseActivityName { get; set; }
        [NotMapped]
        public string? FalseCaseActivityName { get; set; }
        [NotMapped]
        public bool IsActivityOption { get; set; }
        [NotMapped]
        public string? MKey { get; set; }
        [NotMapped]
        public IList<WorkflowConditionOptions>? workflowConditionOptions { get; set; } = new List<WorkflowConditionOptions>();
        [NotMapped]
        public Guid? ConditionGuid { get; set; }
    }
}

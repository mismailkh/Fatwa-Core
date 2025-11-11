using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_CONDITION_OPTIONS")]
    public class WorkflowConditionOptions : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowOptionId { get; set; }
        public int ModuleOptionId { get; set; }
        public int WorkflowConditionId { get; set; }
        public WorkflowControl TrueCaseFlowControlId { get; set; }
        public int TrueCaseActivityNo { get; set; }
        [NotMapped]
        public bool HasOptions { get; set; }
        [NotMapped]
        public Guid UniqueIdentity { get; set; } = new Guid();
        [NotMapped]
        public int SequenceNumber { get; set; }
        [NotMapped]
        public bool IsOptionCondition { get; set; }
        [NotMapped]
        public string? TrueCaseActivityName { get; set; }
        [NotMapped]
        public string? OptionName { get; set; }
        [NotMapped]
        public Guid? ConditionGuid { get; set; }
    }
}

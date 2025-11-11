using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_ACTIVITY_CONDITION")]
    public class WorkflowCondition 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowConditionId { get; set; }
        public int ModuleConditionId { get; set; }
        public int WorkflowActivityId { get; set; }
        public WorkflowControl TrueCaseFlowControlId { get; set; }
        public WorkflowControl FalseCaseFlowControlId { get; set; }
        public int TrueCaseActivityNo { get; set; }
        public int FalseCaseActivityNo { get; set; }
        public bool IsLawyerTask { get; set; }
        public bool IsOption { get; set; }
        [NotMapped]
        public bool IsCollapse { get; set; }

        [NotMapped] 
        public Guid UniqueIdentity { get; set; } = new Guid();
        
        [NotMapped]
        public int SequenceNumber { get; set; }
        [NotMapped]
        public string? ConditionName { get; set; }
        [NotMapped]
        public string? TrueCaseActivityName { get; set; }
        [NotMapped]
        public string? FalseCaseActivityName { get; set; }
        [NotMapped]
        public bool IsActivityCondition { get; set; }
        [NotMapped]
        public string? MKey { get; set; }
        [NotMapped]
        public IList<WorkflowConditionOptions>? workflowConditionOptions { get; set; } = new List<WorkflowConditionOptions>();
        [NotMapped]
        public Guid? ConditionGuid { get; set; } = Guid.NewGuid();
        [NotMapped]
        public IList<WorkflowConditionsOptionsListVM>? WorkflowConditionOptionLists { get; set; } = new List<WorkflowConditionsOptionsListVM>();
    }
}

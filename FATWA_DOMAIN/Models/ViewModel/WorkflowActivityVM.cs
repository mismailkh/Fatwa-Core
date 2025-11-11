using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class WorkflowActivityVM
    {
        [Key]
        public int? WorkflowActivityId { get; set; }
        public int? ActivityId { get; set; }
        public int? BranchId { get; set; }
        public int? WorkflowId { get; set; }
        public int? CategoryId { get; set; }
        public int? SequenceNumber { get; set; }
        public string? Name { get; set; }
        public string? Class { get; set; }
        public string? Method { get; set; }
        public string? AKey { get; set; }
        public bool? IsEndofFlow { get; set; }
    }

    public class WorkflowActivityParametersVM
    {
        public int? WorkflowActivityId { get; set; }
        public int? ParameterId { get; set; }
        public string? Value { get; set; }
        public string? Name { get; set; }
        public string? PKey { get; set; }
    }

    public class WorkflowTriggerConditionsVM
    {
        [Key]
        public int? TriggerConditionId { get; set; }
        public int? ConditionId { get; set; }
        public int? WorkflowTriggerId { get; set; }
        public WorkflowControl? TrueCaseFlowControlId { get; set; }
        public WorkflowControl? FalseCaseFlowControlId { get; set; }
        public int? TrueCaseActivityNo { get; set; }
        public int? FalseCaseActivityNo { get; set; }
        public string? MKey { get; set; }
        public string? ValueToCompare { get; set; }
    }
    public class WorkflowConditionsVM
    {
        [Key]
        public int? WorkflowConditionId { get; set; }
        public int? ModuleConditionId { get; set; }
        public int? WorkflowActivityId { get; set; }
        public WorkflowControl? TrueCaseFlowControlId { get; set; }
        public WorkflowControl? FalseCaseFlowControlId { get; set; }
        public int? TrueCaseActivityNo { get; set; }
        public int? FalseCaseActivityNo { get; set; }
        public string? MKey { get; set; }
        public string? ValueToCompare { get; set; }
        public bool IsLawyerTask { get; set; }
        public bool IsOption { get; set; }
    }
    public class WorkflowOptionsVM
    {
        [Key]
        public int? WorkflowOptionId { get; set; }
        public int? ModuleOptionId { get; set; }
        public int? WorkflowActivityId { get; set; }
        public WorkflowControl? TrueCaseFlowControlId { get; set; }
        public int? TrueCaseActivityNo { get; set; }
    }
    public class WorkflowConditionsOptionVM
    {
        [Key]
        public int? WorkflowOptionId { get; set; }
        public int? WorkflowConditionId { get; set; }
        public int? ModuleOptionId { get; set; }
        public WorkflowControl? TrueCaseFlowControlId { get; set; }
        public int? TrueCaseActivityNo { get; set; }
        public string? ConditionOption { get;set; }
        public int? WorkflowId { get; set; }


    }
   public class WorkflowActivityOptionVM
    {
        [Key]
        public int? WorkflowOptionId { get; set; }
        public int? WorkflowActivityId { get; set; }
        public int? ModuleOptionId { get; set; }
        public WorkflowControl? TrueCaseFlowControlId { get; set; }
        public int? TrueCaseActivityNo { get; set; }
        public string? ActivityOptions { get;set; }
        public int? WorkflowId { get; set; }


    }
    public class WorkflowConditionsOptionsListVM
    {
        [Key]
        public int? WorkflowOptionId { get; set; }
        public int? ModuleOptionId { get; set; }
        public int? WorkflowConditionId { get; set; }
        public WorkflowControl? TrueCaseFlowControlId { get; set; }
        public int? TrueCaseActivityNo { get; set; }
        public string? OptionName { get; set; }
        [NotMapped]
        public Guid? ConditionGuid { get; set; } = new Guid();
        [NotMapped]
        public string? TrueCaseActivityName { get;set; }
        [NotMapped]
        public int SequenceNumber { get; set; }
    }
    public class WorkflowTriggerConditionOptionsVM
    {
        [Key]
        public int? WorkflowOptionId { get; set; }
        public int? ModuleOptionId { get; set; }
        public int? TriggerConditionId { get; set; }
        public WorkflowControl? TrueCaseFlowControlId { get; set; }
        public int? TrueCaseActivityNo { get; set; }
        public string? ConditionOption { get; set; }
        public int? WorkflowId { get; set; }
    }
}

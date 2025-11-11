using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_TRIGGER_CONDITION")]
    public class WorkflowTriggerCondition : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TriggerConditionId { get; set; }
        public int ConditionId { get; set; }
        public int WorkflowTriggerId { get; set; }
        public WorkflowControl TrueCaseFlowControlId { get; set; }
        public WorkflowControl FalseCaseFlowControlId { get; set; }
        public int TrueCaseActivityNo { get; set; }
        public int FalseCaseActivityNo { get; set; }
        public bool IsOption { get;set; }
        [NotMapped]
        public IList<WorkflowTriggerConditionOption>? workflowTriggerConditionOptions { get; set; } = new List<WorkflowTriggerConditionOption>();
    }
}

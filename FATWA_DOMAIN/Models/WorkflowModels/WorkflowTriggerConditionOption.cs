using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_TRIGGER_CONDITION_OPTIONS")]
    public class WorkflowTriggerConditionOption :TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowOptionId { get; set; }
        public int ModuleOptionId { get; set; }
        public int TriggerConditionId { get; set; }
        public WorkflowControl TrueCaseFlowControlId { get; set; }
        public int TrueCaseActivityNo { get; set; }
    }
}

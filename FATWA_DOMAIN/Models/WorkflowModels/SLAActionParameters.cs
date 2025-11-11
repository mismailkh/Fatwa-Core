using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_SLA_ACTION_PARAMETERS")]
    public partial class SLAActionParameters
    {
        public int WorkflowSLAId { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }
    }
}

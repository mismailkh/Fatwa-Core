using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_ACTIVITY_PARAMETERS")]
    public partial class WorkflowActivityParameters
    {
        public int WorkflowActivityId { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }
    }
}

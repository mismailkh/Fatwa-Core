using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_INSTANCE")]
    public partial class WorkflowInstance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstanceId { get; set; }
        public Guid ReferenceId { get; set; }
        public int WorkflowId { get; set; }
        public int StatusId { get; set; }
        public int WorkflowActivityId { get; set; }
        public DateTime SlaStartDate { get; set; }
        public DateTime SlaEndDate { get; set; }
        public bool IsSlaExecuted { get; set; }
        public bool ApplySla { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_STATUS_PR_LKP")]
    public partial class WorkflowStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string? Name_En { get; set; }
        public string? Name_Ar { get; set; }
    }
}

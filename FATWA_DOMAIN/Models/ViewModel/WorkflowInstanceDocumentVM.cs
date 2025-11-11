using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class WorkflowInstanceDocumentVM : GridMetadata
    {
        [Key]
        public int? InstanceId { get; set; }
        public Guid? ReferenceId { get; set; }
        public int? WorkflowId { get; set; } 
        public string? WorkflowName { get; set; }
        public int? WorkflowActivityId { get; set; }
        public string? ActivityName { get; set; }
        public string? Status { get; set; }
        public string? Title { get; set; }
    }
}

using FATWA_DOMAIN.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW")]
    public partial class Workflow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int StatusId { get; set; }
        public string? Version { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int SubModuleId { get; set; }

        [NotMapped]
        public ModuleTriggerVM? ModuleTriggerVM { get; set; }

        [NotMapped]
        public WorkflowTrigger WorkflowTrigger { get; set; } = new WorkflowTrigger();

        [NotMapped]
        public List<WorkflowActivity>? WorkflowActivities { get; set; } = new List<WorkflowActivity>();

        [NotMapped]
        public IEnumerable<int>? AttachmentTypeId { get; set; }
        [NotMapped]
        public IList<AttachmentTypeListVM>? AttachmentTypesList { get; set; } = new List<AttachmentTypeListVM>(); 
    }
}

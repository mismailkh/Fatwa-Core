using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_TRIGGER_PR_LKP")]
    public partial class ModuleTrigger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleTriggerId { get; set; }
        public string Name { get; set; }
        public bool IsConditional { get; set; }
        public bool IsOptional { get; set; }
    }
}

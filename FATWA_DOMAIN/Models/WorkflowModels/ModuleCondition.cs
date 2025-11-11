using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_CONDITION_PR_LKP")]
    public partial class ModuleCondition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleConditionId { get; set; }
        public string Name { get; set; }
        public string MKey { get; set; }
        public string ValueToCompare { get; set; }
        public bool IsTriggerSpecific { get; set; }
    }
}

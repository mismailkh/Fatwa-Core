using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_OPTIONS_PR_LKP")]
    public partial class ModuleConditionOptions
    {
        [Key]
        public int ModuleOptionId { get; set; }
        public string Name { get; set; }
        public bool IsTriggerSpecific { get; set; }
    }
}

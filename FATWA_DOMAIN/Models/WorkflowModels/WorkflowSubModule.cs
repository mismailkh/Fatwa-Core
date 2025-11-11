using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_SUBMODULE_PR_LKP")]
    public partial class WorkflowSubModule
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public int ModuleId { get; set; }
        public bool IsActive { get; set; }
    }
}

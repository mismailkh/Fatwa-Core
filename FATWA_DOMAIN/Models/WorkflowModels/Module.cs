using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("MODULE")]
    public partial class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
        public string ModuleNameAr { get; set; }
        public string ModuleNameEn { get; set; }
    }
}

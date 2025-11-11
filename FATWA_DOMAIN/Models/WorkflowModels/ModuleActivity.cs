using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_ACTIVITY_PR_LKP")]
    public partial class ModuleActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string AKey { get; set; }
        public bool IsEndofFlow { get; set; }
    }
}

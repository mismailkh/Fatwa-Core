using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_PARAMETER_PR_LKP")]
    public partial class Parameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string PKey { get; set; }
        public bool Mandatory { get; set; }
        public bool IsAutoPopulated { get; set; }
        [NotMapped]
        public string? Value { get; set; }
        [NotMapped]
        public string? Class { get; set; }
    }
}

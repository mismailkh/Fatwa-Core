using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_TEMPLATE_PARAMETER")]
    public class CmsTemplateParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string PKey { get; set; }
        public bool Mandatory { get; set; }
        public bool IsAutoPopulated { get; set; }
        public int ModuleId { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string? Value { get; set; }
        [NotMapped]
        public string? Class { get; set; }
    }
}

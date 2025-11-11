using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_TEMPLATE_SETTING")]
    public class LegalTemplateSetting
    {
        [Key]
        public int TemplateSettingId { get; set; }
        public string Template_Heading { get; set; }
        public string Template_Value { get; set; }
        public string? Template_Heading_Ar { get; set; }
        public string? Template_Value_Ar { get; set; }
    }
}

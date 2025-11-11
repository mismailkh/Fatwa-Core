using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_TEMPLATE_SECTION")]
    public class ConsultationTemplateSection
    {
        [Key]
        public int TemplateSectionId { get; set; }
        public string Name { get; set; }
        public string? Content_En { get; set; }
        public string? Content_Ar { get; set; }
        public string? Name_Ar { get; set; }
        public int SectionHeadId { get; set; }
        public string? SectionName_En { get; set; }
        public string? SectionName_Ar { get; set; }
    }
}

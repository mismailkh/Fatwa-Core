using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_TEMPLATE_TEMPLATE_SECTION")]
    public class ConsultationTemplateTemplateSection
    {
        [Key]
        public Guid Id { get; set; }
        public int TemplateId { get; set; }
        public int TemplateSectionId { get; set; }
    }
}

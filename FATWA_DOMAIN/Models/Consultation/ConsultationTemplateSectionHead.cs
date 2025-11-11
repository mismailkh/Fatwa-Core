using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_TEMPLATE_SECTION_HEAD")]
    public class ConsultationTemplateSectionHead
    {
        [Key]
        public int SectionHeadId { get; set; }
        public string Name_En { get; set; }
        public string? Name_Ar { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_LEGISLATION_LEGAL_TEMPLATE")]
    public class LegalLegislationLegalTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid LegislationId { get; set; }
        public Guid TemplateId { get; set; }
        public int TemplateSettingId { get; set; }
    }
}

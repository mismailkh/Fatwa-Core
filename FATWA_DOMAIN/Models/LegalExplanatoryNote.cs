using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_EXPLANATORY_NOTE", Schema = "dbo")]
    public partial class LegalExplanatoryNote
    {
        [Key]
        public Guid ExplanatoryNoteId { get; set; }
        public Guid LegislationId { get; set; }
        public string? ExplanatoryNote_Body { get; set; }
    }
}

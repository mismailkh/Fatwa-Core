using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_CLAUSE", Schema = "dbo")]
    public partial class LegalClause
    {
        [Key]
        public Guid ClauseId { get; set; }
        public Guid LegislationId { get; set; }
        public Guid? SectionId { get; set; }
        public string Clause_Name { get; set; }
        public int? Clause_Status { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string Clause_Content { get; set; }
        public DateTime? ClauseOrder { get; set; }
        [NotMapped]
        public string? ShowSectionTitle { get; set; } = string.Empty;
        [NotMapped]
        public string? Clause_Status_Name_En { get; set; } = string.Empty;
        [NotMapped]
        public string? Clause_Status_Name_Ar { get; set; } = string.Empty;
    }
}

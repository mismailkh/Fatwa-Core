using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_SECTION", Schema = "dbo")]
    public partial class LegalSection
    {
        [Key]
        public Guid SectionId { get; set; }
        public Guid LegislationId { get; set; }
        public Guid? Section_Parent_Id { get; set; } = Guid.Empty;
        public int Section_Number { get; set; }
        public int? ParentId { get; set; }
        public string? SectionTitle { get; set; }
        public string? SectionParentTitle { get; set; } = string.Empty;
        public bool HasChildren { get; set; }

        [NotMapped]
        public List<LegalArticle>? LegalArticlesUnderSection { get; set; } = new List<LegalArticle>();
        [NotMapped]
        public List<LegalClause>? LegalClauseUnderSection { get; set; } = new List<LegalClause>();
    }
}

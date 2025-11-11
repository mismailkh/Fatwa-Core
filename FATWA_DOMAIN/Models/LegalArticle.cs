using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_ARTICLE", Schema = "dbo")]
    public partial class LegalArticle
    {
        [Key]
        public Guid ArticleId { get; set; }
        public Guid LegislationId { get; set; }
        public Guid? SectionId { get; set; }
        public string Article_Name { get; set; }
        public string Article_Title { get; set; }
        public DateTime? Start_Date { get; set; }
        public int? Article_Status { get; set; }
        public DateTime? End_Date { get; set; }
        public string Article_Text { get; set; }
        public string? Article_Explanatory_Note { get; set; }
        public Guid? NextArticleId { get; set; }
        public DateTime? ArticleOrder { get; set; }

        [NotMapped]
        public string? ShowSectionTitle { get; set; } = string.Empty;
        [NotMapped]
        public int? Article_Source { get; set; } = 0;
        [NotMapped]
        public Guid? ExistingArticleId { get; set; } = Guid.Empty;
        [NotMapped]
        public string? Article_Status_Name_En { get; set; } = string.Empty;
        [NotMapped]
        public string? Article_Status_Name_Ar { get; set; } = string.Empty;
        [NotMapped]
        public string? ArticleEffectNoteHistory { get; set; } = string.Empty;
    }
    public partial class ArticleNumberForEffect
    {
        public int ArticleNumber { get; set; }
        public Guid ArticleId { get; set; }
    }
}

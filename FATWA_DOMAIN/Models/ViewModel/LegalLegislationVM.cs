using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegalLegislationVM
    {
        [Key]
        public Guid LegislationId { get; set; }
        public string? LegislationTitleEn { get; set; }
        public int? Legislation_Type { get; set; }
        public string? Legislation_Type_Name_En { get; set; }
        public string? Legislation_Type_Name_Ar { get; set; }
        public int? Legislation_Status { get; set; }
        public DateTime? LegislationIssueDate { get; set; }
        public string? Legislation_Number { get; set; }
        public string? LegislationStatusNameEn { get; set; }
        public string? LegislationStatusNameAr { get; set; }
        public int? Legislation_Year { get; set; }
        public DateTime? AddedDate { get; set; }
        [NotMapped]
        public string? CanceledBy { get; set; } = string.Empty;

        [NotMapped]
        public ICollection<LegalArticle>? RelatedArticles { get; set; } = new List<LegalArticle>();
        [NotMapped]
        public DateTime? CanceledDate { get; set; }
    }
    public class LegalLegislationSourceDocSearchVM
    {
      
        public string? EditionNumber { get; set; }
        public string? EditionType { get; set; }
        public string? DocumentTitle { get; set; }
        public DateTime? PublicationFrom { get; set; }
        public DateTime? PublicationTo { get; set; }
    }
}

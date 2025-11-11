using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegalLegislationDetailVM
    {
        [Key]
        public Guid LegislationId { get; set; }
        public string? LegislationtypeAr { get; set; }
        public string? LegislationtypeEn { get; set; }
        public string? Introduction { get; set; }
        public string? Legislation_Number { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? IssueDate_Hijri { get; set; }
        public string? LegislationTitle { get; set; }
        public string? StatusNameEn { get; set;  }
        public string? StatusNameAr { get; set;  }
        public DateTime? StartDate { get; set; }
        public DateTime? CanceledDate { get; set; }
    }
}

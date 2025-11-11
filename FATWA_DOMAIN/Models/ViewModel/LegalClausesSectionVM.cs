using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegalClausesSectionVM
    {
        [Key]

        public Guid? ClauseId { get; set; }
        public Guid? LegislationId { get; set; }
        public string? Clause_Name { get; set; }
        public string? Clause_Content { get; set; }
        public string? SectionTitle { get; set; }
        public string? ClauseStatusAr { get; set; }
        public string? ClauseStatusEn { get; set; }
        public DateTime? Start_Date { get; set; }
    }
}

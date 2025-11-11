using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegalArticalSectionVM
    {
        [Key]

        public Guid? ArticleId { get; set; }
        public Guid? LegislationId { get; set; }
        public string? Article_Name { get; set; }
        public string? Article_Text { get; set; }
        public string? Article_Title { get; set; }
        public string? SectionTitle { get; set; }
        public string? ArticalStatusAr { get; set; }
        public string? ArticalStatusEn { get; set; }
        public DateTime? Start_Date { get; set; }
    }
}

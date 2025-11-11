using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrincipleDetailVM 
    {
        [Key]
        public Guid PrincipleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? PageNumber { get; set; }
        public string? PrincipleContent { get; set; }
        public int? PrincipleNumber { get; set; }
        public string? CategoryName { get; set; }
        public string CreatedByEn { get; set; }
        public string CreatedByAr { get; set; }
    }   
}

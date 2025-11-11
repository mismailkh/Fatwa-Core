using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrincipleCategoriesVM
    {
        [Key]
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int PrincipleContentCount { get; set; }
        [NotMapped]
        public bool Expanded { get; set; }
        [NotMapped]
        public bool HasSubChildren { get; set; }
        [NotMapped]
        public List<LLSLegalPrincipleContent> PrincipleContent { get; set; } = new List<LLSLegalPrincipleContent>();
    }
}

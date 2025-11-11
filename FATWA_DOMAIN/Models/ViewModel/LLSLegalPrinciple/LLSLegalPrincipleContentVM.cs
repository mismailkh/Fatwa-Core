using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrincipleContentVM
    {
        [Key]
        public Guid PrincipleContentId { get; set; }
        public Guid PrincipleId { get; set; }
        public string PrincipleContent { get; set; }
        public DateTime StartDate { get; set; }
        public int SourceDocumentId { get; set; }
    }
}

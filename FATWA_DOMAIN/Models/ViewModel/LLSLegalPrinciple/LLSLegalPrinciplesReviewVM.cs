using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrinciplesReviewVM : GridMetadata
    {
        [Key]
        public Guid PrincipleId { get; set; }
        public int PrincipleNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FlowStatusId { get; set; }
        public string FlowStatusEn { get; set; }
        public string FlowStatusAr { get; set; }
        public int? NumberOfPrincipleContents { get; set;}
        public string? TypeEn { get; set;}
        public string? TypeAr { get; set;}

    }
    public class AdvanceSearchLegalPrinciplesVM
    {
        public int? Principle_Number { get; set; }
        public int? Principle_Type { get; set; }    
        public string? Principle_Flow_Status_Ar { get; set; }
        public string? Principle_Flow_Status_En { get; set; }
        public string? Principle_Type_Ar { get; set; }
        public string? Principle_Type_En { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}

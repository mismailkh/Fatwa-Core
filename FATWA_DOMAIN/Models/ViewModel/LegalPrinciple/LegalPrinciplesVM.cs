using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.LegalPrinciple
{
    public class LegalPrinciplesDmsVM
    {
        [Key]
        public Guid PrincipleId { get; set; }
        public int Principle_Type { get; set; }
        public int Principle_Number { get; set; }
        public string? Introduction { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? IssueDate_Hijri { get; set; }
        public string? PrincipleTitle { get; set; }
        public int? Principle_Year { get; set; }
        public int? Principle_Status { get; set; }
        public int Principle_Flow_Status { get; set; }
        public DateTime? StartDate { get; set; }
        public string? AddedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Principle_Type_Ar { get; set; }
        public string? Principle_Type_En { get; set; }
        public string? Principle_Status_Ar { get; set; }
        public string? Principle_Status_En { get; set; }
        public string? Principle_Flow_Status_Ar { get; set; }
        public string? Principle_Flow_Status_En { get; set; }
        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}


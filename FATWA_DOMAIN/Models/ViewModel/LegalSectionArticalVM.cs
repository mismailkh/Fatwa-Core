using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegalSectionArticalVM
    {
        [Key]
        public Guid LegislationId { get; set; }
        public int Legislation_Type { get; set; }
        public int Legislation_Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime IssueDate_Hijri { get; set; }
        public string? Legislation_Title_En { get; set; }
        public string? Legislation_Title_Ar { get; set; }
        public int Legislation_Status { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CanceledBy { get; set; }
        public DateTime? CanceledDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid SectionId { get; set; }
        public Guid? Section_Parent_Id { get; set; }
        public int Section_Number { get; set; }
        public string? Section_Title_En { get; set; }
        public string? Section_Title_Ar { get; set; }

        [NotMapped]
        public ICollection<LegalArticle>? legalArticles { get; set; }
    }
}

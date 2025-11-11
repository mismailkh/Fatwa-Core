using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegislationVM
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
        [NotMapped]
        public ICollection<LegalArticle>? legalArticles { get; set; }
        [NotMapped]
        public ICollection<LegalLegislationSignature>? legalLegislationSignatures { get; set; }
        [NotMapped]
        public ICollection<LegalSection>? legalSection { get; set; }
     
    }
}

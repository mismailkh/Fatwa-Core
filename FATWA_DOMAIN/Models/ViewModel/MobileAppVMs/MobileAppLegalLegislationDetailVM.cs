using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppLegalLegislationDetailVM
    {
        [Key]
        public Guid LegislationId { get; set; }
        public string? LegislationtypeEn { get; set;}
        public string? LegislationtypeAr { get; set;}
        public string? Introduction { get; set;}
        public string? LegislationNumber { get; set;}
        public DateTime? IssueDate { get; set;}
        public DateTime? IussueHijriDate { get; set;}
        public string? Subject { get; set;}
        public string? StatusNameEn { get; set;}
        public string? StatusNameAr { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        //Publication Source Detail
        public string? LegalPublicationSourceNameEn { get; set; }
        public string? LegalPublicationSourceNameAr { get; set; }
        public int? IssueNumber { get; set; }
        public DateTime? PublicationHijriDate { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int? PageStart { get; set; }
        public int? PageEnd { get; set; }
        [NotMapped]
        public List<MobileAppUploadDocumentsVM> Attachments { get; set; } = new List<MobileAppUploadDocumentsVM>();
    }
}

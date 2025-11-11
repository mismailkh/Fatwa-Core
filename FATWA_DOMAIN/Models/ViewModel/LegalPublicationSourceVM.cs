using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegalPublicationSourceVM
    {
        [Key]
        public int SourceId { get; set; }
        public Guid? LegislationId { get; set; }
        public int? Issue_Number { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime? PublicationDate_Hijri { get; set; }
        public int? Page_Start { get; set; }
        public int? Page_End { get; set; }
        public string? LegalPublicationSourceNameAr { get; set; }
        public string? LegalPublicationSourceNameEn { get; set; }
    }
}

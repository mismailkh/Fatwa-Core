using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_PUBLICATION_SOURCE", Schema = "dbo")]
    public partial class LegalPublicationSource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SourceId { get; set; }
        public int PublicationNameId { get; set; }
        public Guid LegislationId { get; set; }
        public int Issue_Number { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime? PublicationDate_Hijri { get; set; }
        public int Page_Start { get; set; }
        public int Page_End { get; set; }
    }
}

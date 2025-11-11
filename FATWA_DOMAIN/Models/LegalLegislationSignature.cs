using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_LEGISLATION_SIGNATURE", Schema = "dbo")]
    public partial class LegalLegislationSignature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SignatureId { get; set; }
        public Guid LegislationId { get; set; }
        public string Full_Name { get; set; }
        public string Job_Title { get; set; }
    }
}

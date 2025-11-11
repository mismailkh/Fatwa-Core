using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_LEGISLATION_STATUS", Schema = "dbo")]
    public partial class LegalLegislationStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}

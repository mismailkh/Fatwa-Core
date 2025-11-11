using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_LEGISLATION_REFERENCE", Schema = "dbo")]
    public partial class LegalLegislationReference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReferenceId { get; set; }
        public Guid Reference_Parent_Id { get; set; }
        public Guid Legislation_Link_Id { get; set; }
        public string Legislation_Link { get; set; }
        [NotMapped]
        public bool CheckNewLegislation { get; set; }

        [NotMapped]
        public ICollection<LegalLegislation>? ReferenceAssociate { get; set; } = new List<LegalLegislation>();
    }
}

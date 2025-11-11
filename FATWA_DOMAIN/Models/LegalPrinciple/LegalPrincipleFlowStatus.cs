using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.LegalPrinciple
{
    [Table("LEGAL_PRINCIPLE_FLOW_STATUS", Schema ="dbo")]
    public partial class LegalPrincipleFlowStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_PARTY_TYPE_G2G_LKP")]
    public class ConsultationPartyType
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}

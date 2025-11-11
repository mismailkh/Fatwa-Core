using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


//< History Author = 'Muhammad Zaeem' Date = '2023-1-2' Version = "1.0" Branch = "master" >Consultation INTERNATIONAL ARBITRATION</History>

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_INTERNATIONAL_ARBITRATION")]
    public partial class ConsultationInternationalArbitration
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}

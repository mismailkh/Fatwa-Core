using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_PRE_COURT_TYPE_G2G_LKP")]
    public partial class PreCourtType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}

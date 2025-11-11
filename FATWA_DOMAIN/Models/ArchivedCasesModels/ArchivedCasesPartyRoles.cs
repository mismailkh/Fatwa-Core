using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ArchivedCasesModels
{
    [Table("ARC_PARTY_ROLE_LKP")]
    public class ArchivedCasesPartyRoles : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}

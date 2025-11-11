using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_BANK_GOVERNMENT_ENTITY_G2G_LKP")]
    public partial class CmsBankGovernmentEntity : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BankId { get; set; }
        public int GovtEntityId { get; set; }
        public string IBAN { get; set; }
        [NotMapped]
        public string BankNameEn { get; set; }
        [NotMapped]
        public string BankNameAr { get; set; }
    }
}
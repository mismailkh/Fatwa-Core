using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Consultation
{
    public class ComsInternationalArbitrationTypeVM : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsActive { get; set; }
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; }

    }
}

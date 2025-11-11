using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{

    [Table("EP_COUNTRY")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public ICollection<Governorate> Governorates { get; set; }

    }
}

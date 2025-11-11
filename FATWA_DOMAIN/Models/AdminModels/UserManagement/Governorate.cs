using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_GOVERNORATE")]
    public class Governorate
    {
        [Key]
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string? Name_En { get; set; }
        public string ?Name_Ar { get; set; }
        public Country ?Country { get; set; }
        public ICollection<City>? Cities { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_ADDRESS")]
    public class UserAdress
    {
        [Key]
        public Guid AddressId { get; set; }
        public string? UserId { get; set; }
        public string? Address { get; set; }
        public int? CityId { get; set; }
        public City? City { get; set; }
        public UserPersonalInformation? UserPersonalInformation { get; set; }

    }

    [Table("CITY_LKP")]
    public class UserCity
    {
        [Key]
        public int CityId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
    [Table("COUNTRY_LKP")]
    public class UserCountry
    {
        [Key]
        public int CountryId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string StudentId { get; set; }

    }


}

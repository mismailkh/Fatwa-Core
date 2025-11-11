using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_CITY")]
	public class City
	{
		[Key]
		public int CityId { get; set; }
        public int? GovernorateId { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public Governorate? Governorate { get; set; }
        public ICollection<UserAdress>? Adresses { get; set; }
        public ICollection<Company>? Companies { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    
    [Table("EP_COMPANY")]
	public class Company
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CompanyId { get; set; }
		public string? NameEn { get; set; }
		public string? NameAr { get; set; }
		public int? CityId { get; set; }
		public string? CompanyAddress { get; set; }
		public string? CompanyPhoneNumber { get; set; }
		public string? CompanyAlternatePhoneNumber { get; set; }
		public string? CompanyEmail { get; set; }
        public City City { get; set; }
       
	}

}

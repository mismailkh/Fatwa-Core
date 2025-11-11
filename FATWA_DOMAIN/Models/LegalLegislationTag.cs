using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_LEGISLATION_TAG", Schema = "dbo")]
    public class LegalLegislationTag
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int TagId { get; set; }
		public string TagName { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string? DeletedBy { get; set; }
		public DateTime? DeletedDate { get; set; }
		public bool IsDeleted { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel
{
	public class LegalLegislationCommentVM
	{
		[Key]
		public Guid CommentId { get; set; }
		public Guid? DocumentId { get; set; }
		public string? Comment { get; set; }
		public string? Status { get; set; }
		public string? StatusNameEn { get; set; }
		public string? StatusNameAr { get; set; }
		public string? Reason { get; set; }

		public string? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? DeletedBy { get; set; }
		public DateTime? DeletedDate { get; set; }
	}
}

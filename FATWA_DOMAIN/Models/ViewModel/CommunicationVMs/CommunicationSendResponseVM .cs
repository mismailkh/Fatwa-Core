using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public partial class CommunicationSendResponseVM
	{
        [Key]
        public Guid CommunicationId { get; set; }
        public string? CommunicationResponseTitle { get; set; }
        public string? Description { get; set; }
        public string? InboxNumber { get; set; }
        public DateTime? InboxDate { get; set; }
        public string? OutboxNumber { get; set; }
        public DateTime? OutboxDate { get; set; }
        public DateTime? RequestDate { get; set; }
		public DateTime? ResponseDate { get; set; }
		public int? ResponseTypeId { get; set; } 
		public string? Reason { get; set; }
        public bool? IsUrgent { get; set; }
        public string? Other { get; set; }
        public string? CorrespondenceTypeEn { get; set; }
        public string? CorrespondenceTypeAr { get; set; }
        public string? Activity_En { get; set; }
        public string? Activity_Ar { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime? ReferenceDate { get; set; }
    }
}

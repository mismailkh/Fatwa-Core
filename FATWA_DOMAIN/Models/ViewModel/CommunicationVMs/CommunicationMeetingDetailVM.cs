using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public partial class CommunicationMeetingDetailVM
    {
        [Key]
        public Guid CommunicationId { get; set; }
        public Guid? MeetingId { get; set; }
        public string? CommunicationResponseTitle { get; set; }
        public string? Description { get; set; }
        public string? InboxNumber { get; set; } 
        public DateTime? InboxDate { get; set; } 
        public string? OutboxNumber { get; set; }
        public DateTime? OutboxDate { get; set; }
        public string? CorrespondenceTypeEn { get; set; }
        public string? CorrespondenceTypeAr { get; set; }
        public string? Activity_En { get; set; }
        public string? Activity_Ar { get; set; }
    }
}

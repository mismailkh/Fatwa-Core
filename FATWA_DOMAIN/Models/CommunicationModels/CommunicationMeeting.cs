using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_MEETING")]
    public partial class CommunicationMeeting
    {
        [Key]
        public Guid CommunicationMeetingId { get; set; }
        public string? Subject { get; set; } 
        public string? Description { get; set; } 
        public string? Agenda { get; set; } 
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? MeetingLink { get; set; }
        public bool IsOnline { get; set; }
        public bool RequirePassword { get; set; }
        public string? MeetingPassword { get; set; }
        public string? Note { get; set; }
		public bool IsReplyForMeetingRequest { get; set; }

		[NotMapped]
        public string? MeetingStatusEn { get; set; }
        [NotMapped]
        public string? MeetingStatusAr { get; set; }
        #region Foreign Keys

        public Guid CommunicationId { get; set; }
        public int StatusId { get; set; }

        #endregion

    }
}

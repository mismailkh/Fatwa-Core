using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
	public class AttendeeDecisionVM
	{
		public Guid? MeetingId { get; set; }
		public string? Subject { get; set; }
		public string? Location { get; set; }
		public string? Description { get; set; }
		public string? Agenda { get; set; }
		public string? Comment { get; set; }
		public string? ReferenceNumber { get; set; }
		public DateTime? Date { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public string? MeetingLink { get; set; }
		public string? MeetingTypeAr { get; set; }
		public string? MeetingTypeEn { get; set; }
		public int? MeetingStatusId { get; set; }
		public int? AttendeeStatusId { get; set; }
		public bool? IsApproved { get; set; }
		public string? MeetingStatusAr { get; set; }
		public string? MeetingStatusEn { get; set; }
		public string? ModifiedBy { get; set; }
		// For Communication
		//public string? ReceivedBy { get; set; }
		public Guid? ReferenceGuid { get; set; }
		public int? SubModulId { get; set; }
		[NotMapped]
		public string? UserId { get; set; } 
		[NotMapped]
		public int? SectorTypeId { get; set; }
		public Guid? MeetingMomId { get; set; }
		public string? MOMTitle { get; set; }
		public string? MOMDescription { get; set; }
		public int? MOMStatusId { get; set; }
		public string? MOMStatusNameEn { get; set; }
		public string? MOMStatusNameAr { get; set; }
		public string? MOMContent { get; set; }
		[NotMapped]
		public string? MOMAttendeeRejectReason { get; set; }
        [NotMapped]
        public string? LoggedInUser { get; set; }
        public string? InitiatorId { get; set; }
		public string? Note { get; set; }
		public Guid? CommunicationId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}

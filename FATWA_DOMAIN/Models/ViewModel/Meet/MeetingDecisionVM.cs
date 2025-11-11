using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
	public class MeetingDecisionVM
	{
		public Guid MeetingId { get; set; }
		public string Subject { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string Agenda { get; set; }
		public string Comment { get; set; }
		public string ReferenceNumber { get; set; }
		public DateTime Date { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string MeetingLink { get; set; }
		public string MeetingTypeAr { get; set; }
		public string MeetingTypeEn { get; set; }
		public int MeetingStatusId { get; set; }
		public int MeetingTypeId { get; set; }
		public bool IsApproved { get; set; }
		public string MeetingStatusAr { get; set; }
		public string MeetingStatusEn { get; set; }
		public string ModifiedBy { get; set; }
		// For Communication
		public string? ReceivedBy { get; set; }
		public Guid ReferenceGuid { get; set; }
		public int SubModulId { get; set; }
		[NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public string SentBy { get; set; }
        [NotMapped]
        public int? GovtEntityId { get; set; }
		[NotMapped]
		public int? MeetingAttendeeStaus { get; set; }
		[NotMapped]
		public string? AttendeeUserId { get; set; }
		public Guid? ApprovalReqBy { get; set; }
		public string? CreatorId { get; set; }
		public string? createdBy { get; set; }
        [NotMapped]
        public bool? IsCreatorHOS { get; set; }
        [NotMapped]
        public bool? HOSUser { get; set; }
        [NotMapped]
        public bool? HosApprovalRequire { get; set; }
        [NotMapped]
        public Guid? LoggedInUser { get; set; }

        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}

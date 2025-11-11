using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.MeetModels
{
    [Table("MEET_MEETING")]
    public partial class Meeting : TransactionalBaseModel
	{
        [Key]
        public Guid MeetingId { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Agenda { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string MeetingLink { get; set; }
        public bool RequirePassword { get; set; }
        public string MeetingPassword { get; set; }
        public string Comment { get; set; }
        public bool IsOnline { get; set; }
        public bool IsApproved { get; set; }
        public int SubModulId { get; set; }
        public string? Note { get; set; }
        public bool IsReplyForMeetingRequest { get; set; }
        public Guid? CommunicationId { get; set; }

        public bool? IsSendToHOS { get; set; } = false;
        #region Foreign Keys

        public int MeetingTypeId { get; set; }
        public int MeetingStatusId { get; set; }
		public Guid? ReferenceGuid { get; set; }
        public Guid? ApprovalReqBy { get; set; }
        #endregion

        // for communication
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public string SentBy { get; set; }
        [NotMapped]
        public string ReceivedBy { get; set; }
        [NotMapped]
        public int? GovtEntityId { get; set; }
        [NotMapped]
        public string? MeetingStatusEn { get; set; }
        [NotMapped]
        public string? MeetingStatusAr { get; set; }
        [NotMapped]
        public bool? IsHeld { get; set; } = false;
    }
}

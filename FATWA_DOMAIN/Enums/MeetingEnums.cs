using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Enums
{
    public partial class MeetingEnums
    {
        public enum MeetingStatusEnum
        {
            [Display(Name = "Requested By GE")]
            RequestedByGE = 1,
            [Display(Name = "Scheduled")]
            Scheduled = 2,
            [Display(Name = "Approved By HOS")]
            ApprovedByHOS = 4,
            [Display(Name = "RejectedByHOS")]
            RejectedByHOS = 8,
            [Display(Name = "Pending For Review By GE")]
            PendingForReviewByGE = 16,
            [Display(Name = "Approved By GE")]
            ApprovedByGE = 32,
            [Display(Name = "Rejected By GE")]
            RejectedByGE = 64,
            [Display(Name = "Held")]
            Held = 128,
            [Display(Name = "Complete")]
            Complete = 256,
            [Display(Name = "Save As Draft")]
            SaveAsDraft = 512,
            [Display(Name = "On Hold")]
            OnHold = 1024,
            [Display(Name = "Approved")]
            Approved = 2048,
            [Display(Name = "Approved By Vice Hos")]
            ApprovedByViceHos = 4096,
            [Display(Name = "Rejected By Vice Hos")]
            RejectedByViceHos = 8192,
            [Display(Name = "Requestted By Organizer")]
            RequestedByOrganizer = 16384,
            [Display(Name = "Rejected")]
            Rejected = 32768
        }

        public enum MeetingAttendeeTypeEnum
        {
            [Display(Name = "Legislation Attendee")]
            LegislationAttendee = 1,
            [Display(Name = "GE Attendee")]
            GeAttendee = 2
        }
        public enum MeetingTypeEnum
        {
            [Display(Name = "Internal")]
            Internal = 1,
            [Display(Name = "External")]
            External = 2
        }
        public enum MeetingAttendeeStatusEnum
        {
            [Display(Name = "New")]
            New = 1,
            [Display(Name = "Pending")]
            Pending = 2,
            [Display(Name = "Accept")]
            Accept = 4,
            [Display(Name = "Reject")]
            Reject = 8,
            [Display(Name = "NotResponded")]
            NotResponded = 16,
        }
    }
}

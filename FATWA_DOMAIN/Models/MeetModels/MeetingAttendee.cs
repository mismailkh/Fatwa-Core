using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.MeetModels
{
    [Table("MEET_MEETING_ATTENDEE")] 
    public partial class MeetingAttendee
    {
        [Key]
        public Guid MeetingAttendeeId { get; set; }
        public string? RepresentativeNameAr { get; set; }
        public string? RepresentativeNameEn { get; set; }
        public Guid? RepresentativeId { get; set; }
        public string? AttendeeUserId { get; set; }
        public bool IsPresent { get; set; }

        #region Foreign Keys

        public int? GovernmentEntityId { get; set; }
        public int? DepartmentId { get; set; }
        public Guid MeetingId { get; set; }
        public int MeetingAttendeeTypeId { get; set; }
        public int? AttendeeStatusId { get; set; } = 1;  

        #endregion

        #region Common 

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }

        #endregion
    }
}

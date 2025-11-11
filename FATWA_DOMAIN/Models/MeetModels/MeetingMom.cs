using FATWA_DOMAIN.Models.ViewModel.Meet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.MeetModels
{
    [Table("MEET_MEETING_MOM")]
    public partial class MeetingMom
    {
        [Key]
        public Guid MeetingMomId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        #region Foreign Keys

        public Guid MeetingId { get; set; }

        #endregion

        #region
        public int? MOMStatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        #endregion

        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public bool isSaveAsDraft { get; set; }
        public string? Content { get; set; }
        [NotMapped]
        public List<MeetingAttendeeVM>? MeetingAttendees { get; set; } = new List<MeetingAttendeeVM>();
        [NotMapped]
        public byte[]? FileData { get; set; }
        [NotMapped]
        public string? Project { get; set; }
    }
}

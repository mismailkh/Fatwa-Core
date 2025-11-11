using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
    public partial class MeetingAttendeeVM
    {
        public int SerialNo { get; set; }
        public string? RepresentativeNameEn { get; set; }
        public string? RepresentativeNameAr { get; set; }
        public Guid? RepresentativeId { get; set; }
        public int GovernmentEntityId { get; set; }
        public string GovernmentEntityNameEn { get; set; }
        public string GovernmentEntityNameAr { get; set; }

        public int? DepartmentId { get; set; }
        public string DepartmentNameEn { get; set; }
        public string DepartmentNameAr { get; set; }
        public int? AttendeeStatusId { get; set; }
        public string? AttendeeStatusEn { get; set; }
        public string? AttendeeStatusAr { get; set; }
        public Guid MeetingId { get; set; }
        public Guid MeetingAttendeeId { get; set; }
        public string? AttendeeUserId { get; set; }
        [NotMapped]
        public bool IsSaved { get; set; }
        public int? SubModulId { get; set; }
        public int? SectorTypeId { get; set; }
        public bool? IsPresent { get; set; }
        [NotMapped]
        public bool? IsGEUser { get; set; } = false;
    }
}

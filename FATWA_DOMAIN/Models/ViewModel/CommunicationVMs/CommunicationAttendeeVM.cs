
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public partial class CommunicationAttendeeVM
	{
        [Key]
        public Guid CommunicationAttendeeId { get; set; }
        public int SerialNo { get; set; }
		public string? RepresentativeNameEn { get; set; }
		public string? RepresentativeNameAr { get; set; }
		public Guid? RepresentativeId { get; set; }
		public int GovernmentEntityId { get; set; }
        public string GovernmentEntityNameEn { get; set; }
        public string GovernmentEntityNameAr { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentNameEn { get; set; }
        public string DepartmentNameAr { get; set; }

        public Guid CommunicationId { get; set; }

        public string? AttendeeUserId { get; set; }
        //public int? AttendeeStatusId { get; set; }
        public bool? IsPresent { get; set; }

    }
}

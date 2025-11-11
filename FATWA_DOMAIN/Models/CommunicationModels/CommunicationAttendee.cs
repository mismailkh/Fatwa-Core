using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.CaseManagment;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_ATTENDEES")]
    public partial class CommunicationAttendee
    {
        [Key]
        public Guid CommunicationAttendeeId { get; set; }
        public string? RepresentativeNameAr { get; set; }
        public string? RepresentativeNameEn { get; set; }
        public Guid? RepresentativeId { get; set; }
         
        #region Foreign Keys

        public int? GovernmentEntityId { get; set; } 
        public int? DepartmentId { get; set; }
        public Guid CommunicationId { get; set; }

        #endregion

        #region Common

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        public string? AttendeeUserId { get; set; }
        public bool? IsPresent { get; set; }
        //public int? AttendeeStatusId { get; set; }
    }
}

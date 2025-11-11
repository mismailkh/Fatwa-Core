using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
    public partial class MomAttendeeDecisionVM
    {
        public Guid MomAttendeeDecisionId { get; set; }
        public Guid MeetingMomId { get; set; }
        public Guid MeetingId { get; set; }
        public Guid MeetingAttendeeId { get; set; }
        public int AttendeeStatusId { get; set; }
        public string Comment { get; set; }
        [NotMapped]
        public int? SerialNo { get; set; }
        public string? AttendeeStatusEn { get; set; }
        public string? AttendeeStatusAr { get; set; }
        public string? RepresentativeNameEn { get; set; }
        public string? RepresentativeNameAr { get; set; }
        public Guid? RepresentativeId { get; set; }


    }
}

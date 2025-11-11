using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.MeetModels
{
    [Table("MEET_MEETING_MOM_ATTENDEE_DECISION")]
    public partial class MomAttendeeDecision
    {
        [Key]
        public Guid MomAttendeeDecisionId { get; set; }
        public Guid MeetingMomId { get; set; }
        public Guid MeetingId { get; set; }
        public Guid MeetingAttendeeId { get; set; }
        public int AttendeeStatusId { get; set; }
        public string? Comment { get; set; }
       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.MeetModels
{
	[Table("MEET_MEETING_ATTENDEE_STATUS")]
	public partial class MeetingAttendeeStatus
	{
		[Key]
		public int Id { get; set; }
		public string? NameEn { get; set; }
		public string? NameAr { get; set; }
	}
}

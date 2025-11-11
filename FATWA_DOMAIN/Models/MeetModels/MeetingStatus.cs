using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.MeetModels
{
	[Table("MEET_MEETING_STATUS")] 
	public partial class MeetingStatus
	{
		[Key]
		public int MeetingStatusId { get; set; }
		public string NameEn { get; set; }
		public string NameAr { get; set; } 
	}
}

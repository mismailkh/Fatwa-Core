using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.MeetModels
{
	[Table("MEET_MEETING_TYPE")] 
	public partial class MeetingType
	{
		[Key]
		public int MeetingTypeId { get; set; }
		public string NameEn { get; set; }
		public string NameAr { get; set; }

	}
}

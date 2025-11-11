using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel.Meet;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public partial class CommunicationVM
	{ 
		public Communication Communication { get; set; }
		public CommunicationMeeting CommunicationMeeting { get; set; } 
		public List<MeetingAttendeeVM> MeetingAttendee { get; set; }
        public List<FatwaAttendeeVM>? FatwaAttendee { get; set; } = new List<FatwaAttendeeVM>();
    }
}

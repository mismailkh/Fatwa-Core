using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
    public class SaveMomVM
    {
        public Meeting Meeting { get; set; } = new Meeting();   
        public List<MeetingAttendeeVM> GeAttendee { get; set; } = new List<MeetingAttendeeVM>();
        public List<FatwaAttendeeVM>? LegislationAttendee { get; set; } = new List<FatwaAttendeeVM>();
        public MeetingMom? MeetingMom { get; set; } = new MeetingMom();
        public List<MomAttendeeDecision>? MomAttendeeDecisionDetails { get; set; } = new List<MomAttendeeDecision>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }
}

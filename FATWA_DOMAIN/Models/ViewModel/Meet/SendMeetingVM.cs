using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Microsoft.AspNetCore.Components;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
	public partial class SaveMeetingVM
	{
		public Meeting Meeting { get; set; }
		public List<MeetingAttendeeVM> GeAttendee { get; set; } 
		public List<Guid> DeletedGeAttendeeIds { get; set; }
		[NotMapped]
		public List<MeetingAttendeeVM>? GEAttandeeSelected { get; set; } = new List<MeetingAttendeeVM>();

        public List<FatwaAttendeeVM>? LegislationAttendee { get; set; } = new List<FatwaAttendeeVM>();
		public List<Guid> DeletedLegislationAttendeeIds { get; set; }
		[NotMapped]
		public List<UserVM>? LegislationAttandeeSelected { get; set; } = new List<UserVM>();

		public MeetingMom? MeetingMom { get; set; }
       
		public Guid CommunicationId { get; set; }
        [NotMapped]
        public ObservableCollection<TempAttachementVM> AdditionalTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();
        public bool isSaveAsDraft { get; set; }
        [NotMapped]
        public bool? iSSaveAndSendToAttendees { get; set; } = false;
        [NotMapped]
        public Guid? TaskId { get; set; } = Guid.Empty;

        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public bool? IsCreateMeeting { get; set; } = false; 
        [NotMapped]
        public bool? OnlyViceHosApproval { get; set; } = false; 
        [NotMapped]
        public string? LoggedInUser { get; set; }  
    }

    public partial class SendMeetingVM
    {
        public Meeting Meeting { get; set; }
        public List<MeetingAttendee> MeetingAttendees { get; set; }
        public MeetingMom? MeetingMom { get; set; }
    }
}

using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public partial class SendCommunicationVM
	{
		public Communication Communication { get; set; }
		public CommunicationMeeting? CommunicationMeeting { get; set; }
		public List<CommunicationAttendeeVM>? CommunicationAttendee { get; set; }
        public List<UploadedDocument>? CommunicationAttachments { get; set; } = new List<UploadedDocument>();
        public CommunicationResponse? CommunicationResponse { get; set; }
        public CommunicationTargetLink? CommunicationTargetLink { get; set; }
		public List<LinkTarget>? LinkTarget { get; set; }
        public List<Guid>? DeletedGeAttendeeIds { get; set; }

        //public List<FatwaAttendeeVM>? LegislationAttendee { get; set; } = new List<FatwaAttendeeVM>();
        //public List<Guid>? DeletedLegislationAttendeeIds { get; set; }
        [NotMapped]
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        //[NotMapped]
        //public ObservableCollection<TempAttachementVM> AdditionalTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public IList<TempAttachementVM> SelectedDocuments = new List<TempAttachementVM>();
        [NotMapped]
        public string RecieverId { get; set; }  
    }
    public partial class CaseRequestCommunicationVM
    {
        public Communication? Communication { get; set; }
        public CommunicationTargetLink? CommunicationTargetLink { get; set; }
        public List<LinkTarget>? LinkTarget { get; set; }
        public CaseRequest? CaseRequest { get; set; }
        public ConsultationRequest? ConsultationRequests { get; set; }
        public CaseFile? CaseFile { get; set; }
        public ConsultationFile? ConsultationFile { get; set; }
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();

    }
    //public partial class ConsultationRequestCommunicationVM
    //{
    //    public Communication? Communication { get; set; }
    //    public CommunicationTargetLink? CommunicationTargetLink { get; set; }
    //    public List<LinkTarget>? LinkTarget { get; set; }
    //    public ConsultationRequest ConsultationRequest { get; set; }

    //}
    public partial class MeetingCommunicationVM
    {
        public Communication? Communication { get; set; }
        public CommunicationTargetLink? CommunicationTargetLink { get; set; }
        public List<LinkTarget>? LinkTarget { get; set; }
        public Meeting Meeting { get; set; }
        public List<MeetingAttendee> MeetingAttendees { get; set; }
        public CommunicationMeeting? CommunicationMeetings { get; set; }

    }
    public partial class WithdrawRequestCommunicationVM
    {
        public Communication? Communication { get; set; }
        public CommunicationTargetLink? CommunicationTargetLink { get; set; }
        public List<LinkTarget>? LinkTarget { get; set; }
        public CmsWithdrawRequest? WithdrawRequest { get; set; }
        public ComsWithdrawRequest ComsWithdrawRequest { get; set; }

        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}

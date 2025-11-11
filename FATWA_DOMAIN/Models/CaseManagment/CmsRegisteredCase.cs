using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-20' Version = "1.0" Branch = "master" > Registered Case Model</History>
    [Table("CMS_REGISTERED_CASE")]
    public partial class CmsRegisteredCase : TransactionalBaseModel
    {
        [Key]
        public Guid CaseId { get; set; }
        public Guid FileId { get; set; }
        public string CANNumber { get; set; }
        public string? OldCANNumber { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? CaseDate { get; set; }
        public int CourtId { get; set; }
        public int ChamberId { get; set; }
        public int ChamberNumberId { get; set; }
        public int? StatusId { get; set; }
        public int? GovtEntityId { get; set; }
        public bool IsConfidential { get; set; }
        public string? CaseRequirements { get; set; }
        public bool HasRequirement { get; set; }
        public decimal? CaseAmount { get; set; }
        public Double? RequirementPercentage { get; set; }
        public bool? IsPrimary { get; set; } = false;
        public bool? IsDissolved { get; set; } = false;
        public int? SectorTypeId { get; set; }
        public bool? IsSubCase { get; set; } = false;
		public int? RequestTypeId { get; set; }
        public string FloorNumber { get; set; } = string.Empty;
		public string RoomNumber { get; set; } = string.Empty;
        public string AnnouncementNumber { get; set; } = string.Empty;
        [NotMapped]
        public DateTime HearingDate { get; set; }
        [NotMapped]
        public TimeSpan HearingTime { get; set; }
        [NotMapped]
		public DateTime Time { get; set; }
		[NotMapped]
        public Guid ParentCaseId { get; set; }
        [NotMapped]
        public Guid? MojRegistrationRequestId { get; set; }

        [NotMapped]
        public IList<Court> selectedcourts { get; set; } = new List<Court>();

        [NotMapped]
        public IList<Chamber> selectedchambers { get; set; } = new List<Chamber>();
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public string? Remarks { get; set; }

		[NotMapped]
		public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
        
        [NotMapped]
        public string? LawyerId { get; set; }// Fetching the userID who Added the Judgment in Case

        // For Fatwa To G2G Communication
        [NotMapped]
        public IList<CasePartyLink> CasePartyLink { get; set; } = new List<CasePartyLink>();
        [NotMapped]
        public CmsRegisteredCaseStatusHistory RegisteredCaseHistory { get; set; }
        [NotMapped]
        public Hearing Hearing { get; set; }
        [NotMapped]
        public string ClaimStatementCreatedBy { get; set; }
        [NotMapped]
        public int AttachmentTypeId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public List<CanAndCaseNumber> RegisteredCasesCanNumber { get; set; } = new List<CanAndCaseNumber>();
    }
}
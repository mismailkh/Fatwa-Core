using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Hassan Abbas' Date = '2022-09-28' Version = "1.0" Branch = "master" >Created Case Request Model</History>

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_CASE_REQUEST")]
    public class CaseRequest : TransactionalBaseModel
    {
        [Key]
        public Guid RequestId { get; set; }
        public Guid ParentRequestId { get; set; }
        public string RequestNumber { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public decimal? ClaimAmount { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string? Remarks { get; set; }
        public string? CaseRequirements { get; set; }
        public string? Subject { get; set; }
        public bool IsConfidential { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsLinked { get; set; }
        public int? GovtEntityId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RequestTypeId { get; set; }
        public int? TransferStatusId { get; set; }
        public int? SectorTypeId { get; set; }
        public int? SubTypeId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? CourtTypeId { get; set; }
        public string? ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool IsViewed { get; set; }
        public int? PreCourtTypeId { get; set; }
        public bool Pledge { get; set; }
		public string? RequestNumberFormat { get; set; }
		public Guid? RequestPatternId { get; set; }
        public string PatternSequenceResult { get; set; }
        public string? AssignedBy { get; set; }

        [NotMapped]
        public List<CasePartyLinkVM> CasePartyLinks { get; set; } = new List<CasePartyLinkVM>();
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        //[NotMapped]
        //public TempAttachementVM AuthorityLetter { get; set; }
        [NotMapped]
        public ObservableCollection<TempAttachementVM> AdditionalTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
		// For G2G to Fatwa Communication
		[NotMapped]
		public int? EventId { get; set; }
        [NotMapped]
        public IList<CasePartyLink> CaseParties { get; set; } = new List<CasePartyLink>();
        
        [NotMapped]
        public ObservableCollection<UploadedDocument> UploadedDocuments { get; set; } = new ObservableCollection<UploadedDocument>();

        [NotMapped]
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public NumberPatternResult cMSCOMSInboxOutbox { get; set; } = new NumberPatternResult();
        [NotMapped]
        public bool IsEdit { get; set; } = false;
    }
}

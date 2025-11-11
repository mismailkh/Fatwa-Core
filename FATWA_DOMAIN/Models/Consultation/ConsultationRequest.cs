
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Muhammad Zaeem' Date = '2023-1-2' Version = "1.0" Branch = "master" >Consultation request model</History>

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_REQUEST")]
    public class ConsultationRequest : TransactionalBaseModel
    {
        [Key]
        public Guid ConsultationRequestId { get; set; }
        public string RequestNumber { get; set; }
        public int RequestTypeId { get; set; }
        public int? RequestSubTypeId { get; set; } //ContractId 
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string? Subject { get; set; } 
        public string? Description { get; set; }
        public int? CompetentAuthorityId { get; set; } 
        public string? ComplainantName { get; set; } 
        public string? ComplaintAgainst { get; set; } 
        public string? ComplainantDecisionNumber { get; set; } 
        public int? LegislationFileTypeId { get; set; } 
        public string? CompetentAuthorityOpinionWithNote { get; set; } 
        public string? Introduction { get; set; } 
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string? RequestTitle { get; set; }
        public string? GEOpinion { get; set; }
        public int PriorityId { get; set; }
        public int RequestStatusId { get; set; }
        public bool Pledge { get; set; }
        public int? GovtEntityId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Remarks { get; set; }
        public bool ContractAmount75000KD { get; set; }
        public bool IsConfidential { get; set; }
        public string? OfficialLetterOutboxNumber { get; set; } 
        public string? FatwaInboxNumber { get; set; } 
        public int? TemplateId { get; set; } = 0; 
        public int? TransferStatusId { get; set; }
        public string? ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? SectorTypeId { get; set; }
        public int? InternationalArbitrationTypeId { get; set; }
        public string? HighPriorityReason { get; set; }
        public DateTime? CSCSubmissionDate { get; set; }
        public string? TemplateContent { get; set; }
        public string RequestNumberFormat { get; set; }

        public string PatternSequenceResult { get; set; }
        public string? AssignedBy { get; set; }
        //public string? CivilId { get; set; }
        [NotMapped]
        public IList<ConsultationParty>? ConsultationPartys { get; set; } = new List<ConsultationParty>();
        [NotMapped]
        public ObservableCollection<TempAttachementVM> AdditionalTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public IList<ConsultationArticle>? ConsultationArticles { get; set; } = new List<ConsultationArticle>();
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public IList<ConsultationSection>? ConsultationSections { get; set; } = new List<ConsultationSection>();

        // For G2G to Fatwa Communication
        [NotMapped]
        public int? EventId { get; set; }
        [NotMapped]
        public ObservableCollection<UploadedDocument> UploadedDocuments { get; set; } = new ObservableCollection<UploadedDocument>();
        [NotMapped]
        public byte[]? FileData { get; set; }
        [NotMapped]
        public int? AttachmentTypeId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public int? PreviousAttachmentTypeId { get; set; }
        [NotMapped]
        public string? CivilId { get; set; }
        [NotMapped]
        public ConsultationFile? ConsultationFile { get; set; } = new ConsultationFile();
        [NotMapped]
        public bool IsEdit { get; set; } = false;

    }
}

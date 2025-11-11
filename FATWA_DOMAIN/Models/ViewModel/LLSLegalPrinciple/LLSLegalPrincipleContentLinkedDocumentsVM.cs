using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrincipleContentLinkedDocumentsVM
    {
        public int? UploadedDocumentId { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? StoragePath { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? FileTitle { get; set; }
        public string? Description { get; set; }
        public int? PageNumber { get; set; }
		public int? OriginalSourceDocId { get; set; }
		public int? CopySourceDocId { get; set; }
		public bool IsMaskedJudgment { get; set; }
	}

    public class LLSLegalPrincipleAppealSupremenContentLinkedDocVM : LLSLegalPrincipleContentLinkedDocumentsVM
    {
        public string? JudgementTypeEn { get; set; }
        public string? JudgementTypeAr { get; set; }
        public DateTime? JudgementDate { get; set; }
        public string? CourtEn { get; set; }
        public string? CourtAr { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public int CourtTypeId { get; set; }
        [Key]
        public int? ReferenceId { get; set; }

	}
    public class LLSLegalPrincipleLegalAdviceContentLinkedDocVM : LLSLegalPrincipleContentLinkedDocumentsVM
    {
        public DateTime? CreatedDate { get; set; }
        public string? FinalDocFileName { get; set; }
        public string? FileNumber { get; set; }
        [Key]
        public int? ReferenceId { get; set; }
	}   
    public class LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM : LLSLegalPrincipleContentLinkedDocumentsVM
    {
        public DateTime? CreatedDate { get; set; }
        public string? EditionNumber { get; set; }
        public string? EditionType { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? PublicationDateHijri { get; set; }
        public string? DocumentTitle { get; set; }
        [Key]
        public int? ReferenceId { get; set; }
	}
    public class LLSLegalPrincipleOthersContentLinkedDocVM : LLSLegalPrincipleContentLinkedDocumentsVM
    {
        public DateTime? DocumentDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? OtherAttachmentType { get; set; }
        [Key]
        public int? ReferenceId { get; set; }
	}

    public class LLSLegalPrincipleLinkedDocVM
    {
        public List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM> AppealSupremenLinkedDocuments { get; set; } = new List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
        public List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM> LegalAdviceLinkedDocuments { get; set; } = new List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM>();
        public List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM> KuwaitAlYoumLinkedDocuments { get; set; } = new List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM>();
        public List<LLSLegalPrincipleOthersContentLinkedDocVM> OthersLinkedDocuments { get; set; } = new List<LLSLegalPrincipleOthersContentLinkedDocVM>();
    }
}

using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrincipleDocumentVM : GridMetadata
    {
        [Key]
        public int? UploadedDocumentId { get; set; }
        public string? JudgementTypeEn { get; set; }
        public string? JudgementTypeAr { get; set; }
        public DateTime? JudgementDate { get; set; }
        public int? NumberOfPrinciple { get; set; }
        public string? CourtEn { get; set; }
        public string? CourtAr { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? StoragePath { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? FileTitle { get; set; }  
        public string? Description { get; set; }
        public int? IsJudgement { get; set; }
        public int CourtTypeId { get; set; }
        [NotMapped]
        public byte[]? MaskedFileData { get; set; }
        [NotMapped]
        public bool IsMaskedJudgment { get; set; }
        [NotMapped]
        public string? UploadFrom { get; set; } = string.Empty;
        [NotMapped]
        public int? OriginalUploadedJudgmentDocumentId { get; set; } = 0;
        [NotMapped]
        public int? CopySourceDocId { get; set; }
    }
    public class LLSLegalPrincipalDocumentSearchVM : GridPagination
    {
        public int? JudgementTypeId { get; set; }
        public int? ChamberId { get; set; }
        public int? CourtId { get; set; }
        public int? ChamberNumberId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CaseNumber { get; set; }
        public string? CANNumber { get; set; }
        public int? FileType { get; set; }
        public string? EditionNumber { get; set; }
        public string? EditionType { get; set; }
        public string? DocumentTitle { get; set; }
        public string? PublicationDateHijri { get; set; }
        public int? CourtTypeId { get; set; }

    }

    public class LLSLegalPrinciplesRelationVM
    {
        [Key]
        public Guid PrincipleContentId { get; set; }
        public DateTime? StartDate { get; set; }
        public string? PrincipleContent { get; set; }
        public int? PageNumber { get; set; }
        public int? SourceDocumentId { get; set; }
        public string? FlowStatusName_En { get; set; }
        public string? FlowStatusName_Ar { get; set; }
        [NotMapped]
        public bool IsChecked { get; set; } = false;
        [NotMapped]
        public int FromPage { get; set; } = 0;
    }

    public class LLSLegalPrinciplContentLinkedDocumentVM
    {
        public int? UploadedDocumentId { get; set; }
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
        public int? AttachmentTypeId { get; set; }
        public string? StoragePath { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? FileTitle { get; set; }
        public string? Description { get; set; }
        public int CourtTypeId { get; set; }
        public int? PageNumber { get; set; }
        public int? ReferenceId { get; set; }
        [NotMapped]
		public byte[]? MaskedFileData { get; set; }
		[NotMapped]
		public bool IsMaskedJudgment { get; set; }
		[NotMapped]
		public string? UploadFrom { get; set; } = string.Empty;
		[NotMapped]
		public int? OriginalUploadedJudgmentDocumentId { get; set; } = 0;
	}
    public class LLSLegalPrinciplesContentVM
    {
        [Key]
        public Guid PrincipleContentId { get; set; }
        public Guid PrincipleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? PrincipleContent { get; set; }
        public int? PageNumber { get; set; }
        public int? ReferenceId { get; set; }
    }
    public class LLSLegalPrinciplLegalAdviceDocumentVM : GridMetadata
    {
        [Key]
        public int? UploadedDocumentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? StoragePath { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public string? FinalDocFileName { get; set; }
        public string? FileNumber { get; set; }
        public int NoOfPrinciples { get; set; }
        [NotMapped]
        public int? CopySourceDocId { get; set; }
        [NotMapped]
        public bool IsMaskedJudgment { get; set; }
    }   
    public class LLSLegalPrincipleKuwaitAlYoumDocuments : GridMetadata
    {
        [Key]
        public int UploadedDocumentId { get; set; }
        public string EditionNumber { get; set; }
        public string EditionType { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string PublicationDateHijri { get; set; }
        public string FileTitle { get; set; }
        public string DocumentTitle { get; set; }
        public string StoragePath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DocType { get; set; }
        public int NoOfPrinciples { get; set; }
        [NotMapped]
        public int AttachmentTypeId { get; set; } = (int)AttachmentTypeEnum.KuwaitAlYawm;   
        [NotMapped]
        public string? Description { get; set; }
        [NotMapped]
        public int? CopySourceDocId { get; set; }
        [NotMapped]
        public bool IsMaskedJudgment { get; set; }
    }

    public class LLSLegalPrinciplOtherDocumentVM : GridMetadata
    {
        [Key]
        public int? UploadedDocumentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DocumentDate { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? StoragePath { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public string? OtherAttachmentType { get; set; }
        public int NoOfPrinciples { get; set; }
        [NotMapped]
        public int? CopySourceDocId { get; set; }
        [NotMapped]
        public bool IsMaskedJudgment { get; set; }
    }
}

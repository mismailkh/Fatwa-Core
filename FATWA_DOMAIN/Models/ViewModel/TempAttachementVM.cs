using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class TempAttachementVM
    {
        [Key]
        public int? AttachementId { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? FileName { get; set; }
        public string? FileNameWithoutTimeStamp { get; set; }
        public string? StoragePath { get; set; }
        public string? SignedDocStoragePath { get; set; }
        public int StatusId { get; set; }
        public string? StatusAr { get; set; }
        public string? StatusEn { get; set; }
        public Guid? Guid { get; set; }
        public Guid? CommunicationGuid { get; set; }
        public Guid? PreCommunicationId { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string? DocType { get; set; }
        public DateTime? DocDateTime { get; set; } = DateTime.Today;
        public string? Type_Ar { get; set; }
        public string? Type_En { get; set; }
        public bool IsDigitallySign { get; set; }
        public string? OtherAttachmentType { get; set; }
        public string? Description { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? DocumentDate { get; set; } = DateTime.Today;
        [NotMapped]
        public string? ReferenceId { get; set; }
        [NotMapped]
        public int? UploadedDocumentId { get; set; }
        [NotMapped]
        public int? LiteratureId { get; set; }
        [NotMapped]
        public int? ModuleId { get; set; }
        [NotMapped]
        public bool? IsMandatory { get; set; }
        [NotMapped]
        public bool? IsOfficialLetter { get; set; }
        [NotMapped]
        public string? ReferenceGuid { get; set; }
        
        public string? FileTitle { get; set; }
       
        public string? FileNumber { get; set; }
        public bool? IsMOJRegistered { get; set; }
        public Guid? VersionId { get; set; }
        public int ChildCount { get; set; } = 0;
        public string? DocumentSourceEn { get; set; }
        public string? DocumentSourceAr { get; set; }
        [NotMapped]
        public byte[]? MaskedFileData { get; set; }
        [NotMapped]
        public bool IsMaskedAttachment { get; set; }
        [NotMapped]
        public string? UploadFrom { get; set; } = string.Empty;
        [NotMapped]
        public string? AttachmentTypeEn { get; set; } = string.Empty;
        [NotMapped]
        public string? AttachmentTypeAr { get; set; } = string.Empty;
        [NotMapped]
        public bool allowedDesignation { get; set; }
        [NotMapped]
        public bool isAlreadySigned { get; set; }
    }

    public class LegalPrincipleTempAttachmentVM
    {
        [Key]
        public int? UploadedDocumentId { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? AttachmentTypeEn { get; set; }
        public string? AttachmentTypeAr { get; set; }
        public string? FileName { get; set; }
        public string? AttachmentNo { get; set; }
        public string? StoragePath { get; set; }
        public string? DocType { get; set; }
        public string? CaseConsultationFileNumber { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}

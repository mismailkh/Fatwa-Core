using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("TEMP_ATTACHEMENTS")]
    //<History Author = 'Hassan Abbas' Date='2022-04-08' Version="1.0" Branch="master"> Add Temp Attachement Entry</History>
    public partial class TempAttachement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachementId { get; set; }
        public int AttachmentTypeId { get; set; }
        public string FileName { get; set; }
        public string FileNameWithoutTimeStamp { get; set; }
        public string StoragePath { get; set; }
        public string? SignedDocStoragePath { get; set; }
        public int StatusId { get; set; }
        public Guid Guid { get; set; }
        public Guid? CommunicationGuid { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public string DocType { get; set; }
        public string? OtherAttachmentType { get; set; }
        public string? Description { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string? FileTitle { get; set; }
        public string? FileNumber { get; set; }
        public bool? IsMaskedAttachment { get; set; }
        [NotMapped]
        public DateTime DocDateTime { get; set; }
        [NotMapped]
        public string ReferenceId { get; set; }
        [NotMapped]
        public int UploadedDocumentId { get; set; }
        [NotMapped]
        public string? AttachmentTypeEn { get; set; } = string.Empty;
        [NotMapped]
        public string? AttachmentTypeAr { get; set; } = string.Empty;
        [NotMapped]
        public string? ReferenceGuid { get; set; }

    }
}

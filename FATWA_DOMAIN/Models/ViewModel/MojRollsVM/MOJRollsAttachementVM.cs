using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.MojRollsVM
{
    public class MOJRollsAttachementVM
    {
        public int UploadedDocumentId
        {
            get;
            set;
        }
        public int LiteratureId
        {
            get;
            set;
        }

        public int AttachmentTypeId
        {
            get;
            set;
        }

        public Guid ReferenceGuid { get; set; }
        public Guid? CommunicationGuid { get; set; }

        public DateTime DocumentDate
        {
            get;
            set;
        }

        public string? Description
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string StoragePath
        {
            get;
            set;
        }
        public string DocType { get; set; }

        public bool? IsActive
        {
            get;
            set;
        }
        public string? OtherAttachmentType { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }

        public string CreatedBy
        {
            get;
            set;
        }
        public string? FileTitle { get; set; }
        public string? FileNumber { get; set; }

        public DateTime CreatedDateTime
        {
            get;
            set;
        }

        public string? CreatedAt
        {
            get;
            set;
        }

        public string? ModifiedBy
        {
            get;
            set;
        }

        public DateTime? ModifiedDateTime
        {
            get;
            set;
        }

        public string? ModifiedAt
        {
            get;
            set;
        }

        public string? DeletedBy
        {
            get;
            set;
        }

        public DateTime? DeletedDateTime
        {
            get;
            set;
        }

        public bool IsDeleted
        {
            get;
            set;
        }
        public bool IsMOJRegistered { get; set; }
        public Guid? VersionId { get; set; }
        public bool IsMaskedAttachment { get; set; }
    }
}

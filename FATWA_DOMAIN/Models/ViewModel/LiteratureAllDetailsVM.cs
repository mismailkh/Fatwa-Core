using FATWA_DOMAIN.Models.ViewModel.Lms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LiteratureAllDetailsVM
    {
        [Key]
        public int? LiteratureId { get; set; }
        public string? LitratureName { get; set; }
        //public string? LitratureNameAr { get; set; }
        public string? LitratureTypeNameAr { get; set; }
        public string? LitratureTypeNameEn { get; set; }
        public string? LitratureClassificaitonNameEn { get; set; }
        public string? LitratureClassificationNameAr { get; set; }
        public DateTime? LitratureBookPurchaseDate { get; set; }
        public string? Characters82 { get; set; }
        public string? Characters { get; set; }
        public string? Publisher { get; set; }
        public int? BookNumber { get; set; }
        public string? ISBN { get; set; }
        public bool? IsSeries { get; set; }
        public int? NumberOfPages { get; set; }
        public int? SeriesNumber { get; set; }
        public bool? IsViewable { get; set; } = null;
        public bool? IsBorrowable { get; set; }
        public bool? AllowToPublish { get; set; }
        public int? NumberOfBorrowedBooks { get; set; }
        public string? IndexNumber { get; set; }
        public string? DivisionNumber { get; set; }
        public string? LitratureIndexNameEn { get; set; }
        public string? LitratureIndexNameAr { get; set; }
        public string? SubjectEn { get; set; }
        public string? SubjectAr { get; set; }
        public string? Subject { get; set; }
        public int? CopyCount { get; set; }
        public string? EditionNumber { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeweyBookNumber { get; set; }
        public string? AuthorName_En { get; set; }
        public string? AuthorName_Ar { get; set; }
		public DateTime? EditionYear { get; set; } 

		[NotMapped]
        public List<LmsLiteratureBarcode>? literatureBarCodeList { get; set; } = new List<LmsLiteratureBarcode>();
        [NotMapped]
        public List<MobileAppUploadDocumentsVM> Attachments { get; set; } = new List<MobileAppUploadDocumentsVM>();
        [NotMapped]
        public List<LiteratureDetailLiteratureTagVM> LiteratureTags { get; set; } = new List<LiteratureDetailLiteratureTagVM>();
    }
    public class MobileAppUploadDocumentsVM
    {
        [Key]
        public int UploadedDocumentId { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public string AttachmentType_En { get; set; }
        public string AttachmentType_Ar { get; set; }
        public int AttachmentTypeId { get; set; }
    }
}

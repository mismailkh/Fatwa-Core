using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Umer Zaman' Date = '2022-03-18' Version = "1.0" Branch = "master" > Add Literature Type, Classification & Index properties</History>

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_DETAILS")]
    public partial class LmsLiterature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LiteratureId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string? Subject_En { get; set; }
        public string? Subject_Ar { get; set; }
        public string? ISBN { get; set; }
        public string Characters { get; set; }
        public string Characters82 { get; set; }
        public string Publisher { get; set; }
        public int CopyCount { get; set; }
        public int NumberOfPages { get; set; }
        public bool IsSeries { get; set; }
        public int SeriesNumber { get; set; }
        public string EditionNumber { get; set; }
        public bool IsViewable { get; set; }
        public bool IsDraft { get; set; }
        public bool IsBorrowable { get; set; }
        public bool AllowtoPublish { get; set; }
        public int NumberOfBorrowedBooks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int? TypeId { get; set; }
        public LmsLiteratureType? LmsLiteratureType { get; set; }
        public int ClassificationId { get; set; }
        public LmsLiteratureClassification? LmsLiteratureClassification { get; set; }

        public int IndexId { get; set; }
        public int DivisionAisleId { get; set; } = 0;
        public string DeweyBookNumber { get; set; }
        public LmsLiteratureIndex? LmsLiteratureIndex { get; set; }
        public ICollection<LmsLiteratureBorrowDetail>? LmsLiteratureBorrowDetails { get; set; }
        public ICollection<LmsLiteraturePurchase>? LmsLiteraturePurchases { get; set; }
        public ICollection<UploadedDocument>? LmsLiteratureUploadedDocuments { get; set; }
        public Guid LiteratureUniqueId { get; set; }
        public string SeriesSequenceNumber { get; set; }
        public DateTime? EditionYear { get; set; }  

		[NotMapped]
        public List<LmsLiteratureAuthor>? LmsLiteratureAuthors { get; set; } = new List<LmsLiteratureAuthor>();
        //public ICollection<LmsLiteratureBarcode>? LmsLiteratureBarcodes { get; set; }
        [NotMapped]
        public ICollection<IFormFile>? LiteratureFiles { get; set; }
        [NotMapped]
        public List<IBrowserFile>? LiteratureAttachements { get; set; }

        //Author Details
        [NotMapped]
        public int Author_Id { get; set; }

        [NotMapped]
        public string Author_FullName_En { get; set; }

        [NotMapped]
        public string Author_FullName_Ar { get; set; }

        [NotMapped]
        public string Author_FirstName_En { get; set; }

        [NotMapped]
        public string Author_FirstName_Ar { get; set; }

        [NotMapped]
        public string Author_SecondName_En { get; set; }

        [NotMapped]
        public string Author_SecondName_Ar { get; set; }

        [NotMapped]
        public string Author_ThirdName_En { get; set; }

        [NotMapped]
        public string Author_ThirdName_Ar { get; set; }

        [NotMapped]
        public string Author_Address_En { get; set; }

        [NotMapped]
        public string Author_Address_Ar { get; set; }

        //Purchase Details

        [NotMapped]
        public DateTime? Purchase_Date { get; set; }

        [NotMapped]
        public string? Purchase_Location { get; set; } = string.Empty;

        [NotMapped]
        public decimal? Purchase_Price { get; set; } = 0;

        //Barcode Details

        [NotMapped]
        public string? BarCodeNumber { get; set; }
        [NotMapped]
        public List<LmsLiteratureBarcode> LiteratureBarcodes { get; set; } = new List<LmsLiteratureBarcode>();
        [NotMapped]
        public IList<LmsLiteratureBarcode> DeletedLiteratureBarcodes { get; set; } = new List<LmsLiteratureBarcode>();
        [NotMapped]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; }
        [NotMapped]
        public IList<LiteratureDetailLiteratureTagVM> LiteratureTags { get; set; } = new List<LiteratureDetailLiteratureTagVM>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public string? RoleId { get; set; }

        [NotMapped]
        public int? PreviousCopyCount { get; set; }
        [NotMapped]
        public int? PreviousSeriesNumber { get; set; }
        [NotMapped]
        public string? PreviousDeweyBookNumber { get; set; }
        [NotMapped]
        public LmsLiteratureIndex? PreviousLmsLiteratureIndex { get; set; }
        [NotMapped]
        public List<LmsLiteratureBarcode>? PreviousLmsLiteratureBarcode { get; set; }
        [NotMapped]
        public List<int> LiteratureIdList { get; set; } = new List<int>();
    }
}

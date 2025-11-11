using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LiteratureDetailVM : GridMetadata
    {
        [Key]
        public int LiteratureId { get; set; }
        public string? ISBN { get; set; }
        public string? Subject { get; set; }
        [NotMapped]
        public int? ClassificationId { get; set; }
        public string? LiteratureName { get; set; }
        public string? IndexNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? DeletedBy { get; set; }
        [NotMapped]
        public string? Name_En { get; set; }
        [NotMapped]
        public string? Name_Ar { get; set; }
        public string? BookStatus { get; set; } = null;
        public string? BookStatus_Ar { get; set; } = null;
        public string? DeweyBookNumber { get; set; }
        [NotMapped]
        public string? DivisionNumber { get; set; }
        [NotMapped]
        public string? AisleNumber { get; set; }
        [NotMapped]
        public string? Name { get; set; }
       
        public int? CopyCount { get; set; }
        [NotMapped]
        public int? NumberOfBorrowedBooks { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
    public class LiteratureListMobileAppVM
    {
        [Key]
        public int LiteratureId { get; set; }
        public string? ISBN { get; set; }
        public string? LiteratureName { get; set; }
        public string? IndexNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Subject { get; set; }
        public string? AuthorName_En { get; set; }
        public string? AuthorName_Ar { get; set; }
        public string? BookStatus { get; set; } = null;
        public string? BookStatus_Ar { get; set; } = null;
        public string? DeweyBookNumber { get; set; }
        public int? CopyCount { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? ThumbnailDocumentId { get; set; }
    }

    public class LiteratureAdvancedSearchVM : GridPagination
    {
        public enum AdvancedSearchDropDownEnum
        {
            [Display(Name = "Book Name")]
            Book_Name = 1,
            [Display(Name = "Book Index")]
            Book_Index = 2,
            [Display(Name = "Barcode")]
            Barcode = 3,
            [Display(Name = "Sticker")]
            Sticker = 4,
            [Display(Name = "Author Name")]
			Author_Name = 5,
		}

        public int LiteratureId { get; }
        public int IndexId { get; }
        public int ClassificationId { get; set; } = 0;
        public int GenericsIntergerKeyword { get; set; } = 0;

        public AdvancedSearchDropDownEnum EnumSearchValue { get; set; } = 0;
        public string? KeywordsType { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? FromDate { get; set; } = null;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ToDate { get; set; } = null;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? PurchaseDateKeyword { get; set; } = null;
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1950, 1, 1);

    }

    public class LiteratureDetailsForBorrowRequestVM
    {
        [Key]
        public int LiteratureId { get; set; }
        public string? LiteratureName { get; set; }
        public string? EditionNumber { get; set; }
        public string? AuthorNameEn { get; set; }
        public string? AuthorNameAr { get; set; }
        public string? BarcodeNumber { get; set; }
        public int? BarcodeId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class AdvancedSearchVM
    {
        public enum AdvancedSearchDropDownEnum
        {
            [Display(Name = "Book Name")]
            Book_Name = 1,
            [Display(Name = "Author Name")]
            Author_Name = 2,
            [Display(Name = "Index Name")]
            Index_Name = 3,
            [Display(Name = "Index Number")]
            Index_Number = 4,
            [Display(Name = "Division Number")]
            Division_Number = 5,
            [Display(Name = "Aisle Number")]
            Aisle_Number = 6,
            [Display(Name = "Barcode")]
            Barcode = 7,
            [Display(Name = "Character")]
            Character = 8,
            [Display(Name = "Price")]
            Price = 9,
            [Display(Name = "Purchase Date")]
            Purchase_Date = 10
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
    public class ErrorLogAdvanceSearchVM : GridPagination
    {
        public Guid ErrorLogId { get; set; }
        public string? Subject { get; set; } = null;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? ComputerName { get; set; } = null;
        public string? UserName { get; set; } = null;
    }
    public class ProcessLogAdvanceSearchVM : GridPagination
    {
        public Guid ProcessLogId { get; set; }
        public string? Task { get; set; } = null;
        public string? Process { get; set; } = null;
        public string? ComputerName { get; set; } = null;
        public string? UserName { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class LmsStockTakingBooksReportListVm
    {
        [Key]
        public int BarcodeId { get; set; }
        public string? BookName { get; set; }
        public string? AuthorNameEn { get; set; }
        public string? AuthorNameAr { get; set; }
        public string? IndexNameEn { get; set; }
        public string? IndexNameAr { get; set; }
        public string? RFIDValue { get; set; }
        public string? BarCodeNumber { get; set; }
        public int? CopiesBorrowed { get; set; }
        public int? CopiesNotBorrowed { get; set; }
        public int? Excess { get; set; }
        public int? Shortage { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public bool IsRfIdNotMatch { get; set; } = false;
    }
}

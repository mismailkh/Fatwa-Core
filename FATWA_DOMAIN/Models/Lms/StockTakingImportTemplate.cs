using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Lms
{
    public class StockTakingImportTemplate
    {
        public string RFIdValue { get; set; }
        public string BarcodeNumber { get; set; }
        public string TotalCopiesCounted { get; set; }
        public string? BookName { get; set; }
        public string? AuthorNameEn { get; set; }
        public string? AuthorNameAr { get; set; }
        public string? IndexNameEn { get; set; }
        public string? IndexNameAr { get; set; }
        public bool IsExist { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Lms
{
    public class ImportStockTakingResponse
    {
        public bool IsStatusCode { get; set; }
        public List<StockTakingImportTemplate> stockTakingImports { get; set; } = new List<StockTakingImportTemplate>();
    }
}

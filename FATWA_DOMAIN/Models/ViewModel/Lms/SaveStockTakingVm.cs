using FATWA_DOMAIN.Models.Lms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class SaveStockTakingVm
    {
        public LmsStockTaking stockTaking { get; set; } = new LmsStockTaking();
        public List<LmsStockTakingBooksReportListVm> lmsStockTakingBooksReportListVms { get; set; } = new List<LmsStockTakingBooksReportListVm>();
        public bool IsEdit { get; set; } = false;
        public IEnumerable<string> StockTakingPerformerIds { get; set; } = new List<string>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Lms
{
    [Table("LMS_LITERATURE_STOCKTAKING_REPORT")]
    public class LmsStockTakingReport
    {
        public Guid Id { get; set; }
        public int BarcodeId { get; set; }
        public Guid StockTakingId { get; set; }
        public bool IsBorrowed { get; set; }
        public int? Excess { get; set; }
        public int? Shortage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class StockTakingPerformerVm
    {
        public string? UserId { get; set; }
        public string? FullNameEn { get; set; }
        public string? FullNameAr { get; set; }
        public string? AddedByEn { get; set; }
        public string? AddedByAr { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

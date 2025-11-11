using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppJudgementDetailVM
    {
        public DateTime? HearingDate { get; set; }
        public DateTime? JudgementDate { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? AmountCollected { get; set; }
        public string? CategoryEn { get; set; }
        public string? CategoryAr { get; set; }
        public string? SerialNumber { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? ExecutionFileLevelEn { get; set; }
        public string? ExecutionFileLevelAr { get; set; }
        public bool? IsFinal { get; set; } = false;
        public string? Remarks { get; set; }
    }
}

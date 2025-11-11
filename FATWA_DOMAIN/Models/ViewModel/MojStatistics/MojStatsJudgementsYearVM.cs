using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsJudgementsYearVM
    {
        public string? JudgementYear { get; set; }
        public int? TotalCaseAutomatedNumber { get; set; }
        public string? TotalAmount { get; set; }
    }
}

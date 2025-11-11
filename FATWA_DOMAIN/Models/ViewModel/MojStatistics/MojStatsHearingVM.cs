using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsHearingVM
    {
        public int HearingId { get; set; }
        public string CaseNumber { get; set; }
        public string HearingDate { get; set; }
        public string CourtName { get; set; }
        public string ChamberType { get; set; }
        public int ChamberNumber { get; set; }
        public string CourtDecisionCode { get; set; }
        public string CourtDecision { get; set; }
    }
}

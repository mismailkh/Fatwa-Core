using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsAnnouncementVM
    {
        public int? AnnouncementId { get; set; }
        public string? CaseNumber { get; set; }
        public string? PartyName { get; set; }
        public string? AnnouncementNumber { get; set; }
        public string? HearingDate { get; set; }
        public string? AnnouncementMakingDate { get; set; }
        public string? AnnouncementGoOutDate { get; set; }
        public string? ActualAnnouncementDate { get; set; }
        public string? AnnouncementResult { get; set; }
        public string? Reason { get; set; }
        public string? DistributionStatus { get; set; }
        public string? LastUpdateDate { get; set; }
    }
}

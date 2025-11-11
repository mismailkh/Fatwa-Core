using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsAdvanceSearchStatisticesVM
    {
        public string? FileNumber { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? ModifiedFrom { get; set; }
        public DateTime? ModifiedTo { get; set; }
        public string? EntityId { get; set; }
        public int? id { get; set; }
        public string? EntityName { get; set; }

    }
}


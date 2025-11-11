using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public  class MojStatsRegisteredDasboardVM
    {
        public int EntityId { get; set; }
        public string?Name_Ar { get; set; }
        public string? Name_En { get; set; }
        public int? TotalInFavour { get; set; }
        public int? TotalAgainst { get; set; }
        public int? TotalCase { get; set;}
        public string EntityName { get; set;}
        public DateTime? InFavourFromDate { get; set; }
        public DateTime? InFavourToDate { get; set; }
        public DateTime? AgainstFromDate { get; set; }
        public DateTime? AgainstToDate { get; set; }

    }
}

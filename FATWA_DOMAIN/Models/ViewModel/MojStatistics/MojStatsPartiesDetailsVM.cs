using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsPartiesDetailsVM
    {
        public string PartyType { get; set; }
        public string CaseNumber { get; set; }
        public string PartyName { get; set; }
        public int? PartyId { get; set; }
    }
}

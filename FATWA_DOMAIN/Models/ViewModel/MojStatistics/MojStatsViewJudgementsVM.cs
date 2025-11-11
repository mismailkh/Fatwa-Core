using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsViewJudgementsVM
    {
        public string? CaseNumber { get; set; }
        public string? CourtName { get; set; }
        public string? JudgementDate { get; set; }
        public string? JudgementStatement { get; set; }
        public string? JudgementType { get; set; }
        public string? JudgementCategory { get; set; }
        
        [NotMapped]
        public IList<MojStatsViewJudgementsVM> JudgementsDetails { get; set; } = new List<MojStatsViewJudgementsVM>();


    }
   
}

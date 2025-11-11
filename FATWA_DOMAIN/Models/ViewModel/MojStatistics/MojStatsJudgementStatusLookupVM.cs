using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsJudgementStatusLookupVM
    {
        public int Id { get; set; }
        public string? Value_En { get; set; }
        public string? Value_Ar { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsCaseStudyVM
    {
        public string? CaseAutomatedNumber { get; set; }
        public string? EntityName { get; set;}
        public DateTime? CaseDate { get; set; }
        public string? JudgementType { get; set;}
        public int? CanId { get; set;}
        public DateTime? JudgementDate { get; set;}
        public string? CaseStudyStatus { get; set;}
    }

    public class MojStatsDasboardVM
    {
        public int EntityId { get; set; }
        public string? Name_Ar { get; set; }
        public string? Name_En { get; set; }
        public int? TotalInFavourCase { get; set; }
        public int? TotalAgainstCase { get; set; }
        public int? TotalCase { get; set; }
        //public Decimal? TotalAmount { get; set;}
        public string? TotalFavourAmount { get; set; }
        public string? TotalAgainstAmount { get; set; }
        public string EntityName { get; set; }
        public DateTime? InFavourFromDate { get; set; }
        public DateTime? InFavourToDate { get; set; }
        public DateTime? AgainstFromDate { get; set; }
        public DateTime? AgainstToDate { get; set; }
    }
}

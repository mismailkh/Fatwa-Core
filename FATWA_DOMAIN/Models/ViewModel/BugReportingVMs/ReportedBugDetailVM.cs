using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class ReportedBugDetailVM
    {
        public Guid Id { get; set; }
        public string? PrimaryBugId { get; set; }
        public string? ApplicationEn { get; set; }
        public string? ApplicationAr { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? StatusId { get;set; }
        public string? IssueTypeEn { get; set; }
        public string? IssueTypeAr { get; set; }
        public string? ModuleEn { get; set; }
        public string? ModuleAr { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? Subject { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Description { get; set; }
        public string? ScreenReference { get; set; }
    }
}

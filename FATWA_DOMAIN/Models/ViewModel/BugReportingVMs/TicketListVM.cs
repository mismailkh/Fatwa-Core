using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class TicketListVM : GridMetadata
    {
        public Guid Id { get; set; }
        public string? TicketId { get; set; }
        public string? Subject { get; set; }
        public Guid? BugId { get; set; }
        public string? ApplicationEn { get; set; }
        public string? ApplicationAr { get; set; }
        public string? ModuleEn { get; set; }
        public string? ModuleAr { get; set; }
        public string? SeverityEn { get; set; }
        public string? SeverityAr { get; set; }
        public string? PriorityEn { get; set; }
        public string? PriorityAr { get; set; }
        public string? IssueTypeEn { get; set; }
        public string? IssueTypeAr { get; set; }
        public int? StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ReportedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public string? AssignTo { get; set; }
        public string? ResolvedBy { get; set; }
        public Guid? GroupId { get; set; }
    }
}

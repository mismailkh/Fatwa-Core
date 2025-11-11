using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class AdvanceSearchTicketListVM : GridPagination
    {
        public string? TicketId { get; set; }
        public int? ApplicationId { get; set; }
        public int? StatusId { get; set; }
        public int? ModuleId { get; set; }
        public int? PriorityId { get; set; }
        public int? SeverityId { get; set; }
        public int? IssueTypeId { get; set; }
        public string? AssignTo { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ReportedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public string? Subject { get; set; }
        public string? CreatedBy { get; set; }
        public string? UserId { get; set; }
    }
}

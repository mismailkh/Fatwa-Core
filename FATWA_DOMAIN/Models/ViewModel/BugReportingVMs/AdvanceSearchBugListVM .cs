using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class AdvanceSearchBugListVM : GridPagination
    {
        public string? PrimaryBugId { get; set; }
        public int? ApplicationId { get; set; }
        public int? StatusId { get; set; }
        public int? IssueTypeId { get; set; }
        public string? ModifiedBy { get; set; }
        public string? Subject { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public int? ModuleId { get; set; }

    }
}

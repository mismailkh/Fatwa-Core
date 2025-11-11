using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class ReportedBugListVM : GridMetadata
    {
        public Guid Id { get; set; }
        public string? PrimaryBugId { get; set; }
        public string? ApplicationEn { get; set; }
        public string? ApplicationAr { get; set; }
        public int? ApplicationId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? StatusId { get; set; }
        public int? TypeId { get; set; }
        public int? ModuleId { get; set; }
        public string? ModuleEn { get; set; }
        public string? ModuleAr { get; set; }
        public string? IssueTypeEn { get; set; }
        public string? IssueTypeAr { get; set; }
        public string? ScreenReference {  get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Description { get; set; }
        public bool IsTicket { get; set; }
        public string? Subject { get; set; }
        [NotMapped]
        public string? DisplayDescription { get; set; }
    }
}

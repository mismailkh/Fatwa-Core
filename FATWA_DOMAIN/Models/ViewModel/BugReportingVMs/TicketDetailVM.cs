namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class TicketDetailVM
    {
        public Guid Id { get; set; }
        public string? TicketId { get; set; }
        public Guid? BugId { get; set; }
        public string? ApplicationEn { get; set; }
        public string? ApplicationAr { get; set; }
        public string? ModuleEn { get; set; }
        public string? ModuleAr { get; set; }
        public int? SeverityId { get; set; }
        public string? SeverityEn { get; set; }
        public string? SeverityAr { get; set; }
        public int? PriorityId { get; set; }

        public string? PriorityEn { get; set; }
        public string? PriorityAr { get; set; }
        public string? IssueTypeEn { get; set; }
        public string? IssueTypeAr { get; set; }
        public int? StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? AssignTo { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ReportedBy { get; set; }
        public string? ReporterNameEn { get; set; }
        public string? ReporterNameAr { get; set; }
        public string? ResolvedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Description { get; set; }
        public string? BugNumber { get; set; }
        public string? Subject { get; set; }
        public string? CreatedBy { get; set; }
        public string? AssignedUserEn { get; set; }
        public string? AssignedUserAr { get; set; }
        public string? GroupNameEn { get; set; }
        public string? GroupNameAr { get; set; }
        public Guid? GroupId { get; set; }
        public int? PortalId { get; set; }
        public string? PortalEn { get; set; }
        public string? PortalAr { get; set; }


    }
}

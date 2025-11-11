using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    public class NotificationParameter : TransactionalBaseModel
    {
        public NotificationParameter() { CreatedBy = "System Generated"; CreatedDate = DateTime.Now; }
        public string? Entity { get; set; }
        public string? FileNumber { get; set; }
        public string? CaseNumber { get; set; }
        public string? PrimaryCaseNumber { get; set; }
        public string? CANNumber { get; set; }
        public string? RequestNumber { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverName { get; set; }
        public string? DocumentName { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? SectorFrom { get; set; }
        public string? SectorTo { get; set; }
        public string? RequestType { get; set; }
        public string? LegislationNumber { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Name { get; set; }
        public string? PrincipleNumber { get; set; }
        public string? Duration { get; set; }
        public string? CorrespodenceNumber { get; set; }
        public string? GEName { get; set; }
        public int DocumentNumber { get; set; }
        public string? DraftNumber { get; set; }
        public string? DraftName { get; set; }
        public string? ServiceRequestNumber { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? PermissionDate { get; set; }
        public string? ReviewerName { get; set; }
        public string? AssigneeNameEn { get; set; }
        public string? AssigneeNameAr { get; set; }
        public string? AssignorNameEn { get; set; }
        public string? AssignorNameAr { get; set; }
        public string? SubjectEn { get; set; }
        public string? SubjectAr { get; set; }
        public string? EmployeeName { get; set; }
    }
}

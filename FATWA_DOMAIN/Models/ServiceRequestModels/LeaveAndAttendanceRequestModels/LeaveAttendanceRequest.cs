using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels
{
    public class LeaveAttendanceRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ServiceRequestId { get; set; }
        public int? LeaveTypeId { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int? TotalDuration { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? TotalTimeDuration { get; set; }
        public int? LeaveReasonTypeId { get; set; }
        public string? OtherLeaveReasonType { get; set; }
        public int? ExemptionTypeId { get; set; }
        public int? ExemptionTimeTypeId { get; set; }
        public int? PermissionTypeId { get; set; }
        public string? Reason { get; set; }
        public string? Remarks { get; set; }
        public string? Description { get; set; }
        public string? DelegatedUserId { get; set; }
        public DateTime? HeldDate { get; set; }
        public DateTime? SuggestedAppointmentDate { get; set; }
        public DateTime? PermissionDate { get; set; }

        public int? ReduceHoursReasonId { get; set; }
        public bool IsInvolvePaying { get; set; }
        public bool IsDecisionFormUploaded { get; set; }
        public bool IsReSubmitDecisionForm { get; set; }

        [NotMapped]
        public IEnumerable<int>? SelectedDays { get; set; }
        [NotMapped]
        public ServiceRequest? ServiceRequest { get; set; } = null!;
        [NotMapped]
        public string? ReceiverId { get; set; }
        [NotMapped]
        public Guid? UserId { get; set; }
        [NotMapped]
        public bool IsSubmit { get; set; }
        [NotMapped]
        public bool IsReSubmit { get; set; }
        [NotMapped]
        public string? AssignTaskUserId { get; set; }
        [NotMapped]
        public string? RequestTypeId { get; set; }
        [NotMapped]
        public string? TaskActionName { get; set; }
        [NotMapped]
        public string? TaskUrl { get; set; }
        [NotMapped]
        public string? TaskName { get; set; }
        [NotMapped]
        public string? ServiceRequestNumber { get; set; }
        [NotMapped]
        public string? ServiceRequestTitle { get; set; }
        [NotMapped]
        public int? EntityId { get; set; } // for notification Enum
        [NotMapped]
        public string? NotificationTitle { get; set; }

    }
}

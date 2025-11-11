using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels
{
    public class LeaveAttendanceRequestDetailVM
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string ServiceRequestNumber { get; set; }
        public int ServiceRequestTypeId { get; set; }
        public int ServiceRequestStatusId { get; set; }

        public int? LeaveTypeId { get; set; }
        public string? LeaveTypeEn { get; set; }
        public string? LeaveTypeAr { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int? TotalDuration { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? TotalTimeDuration { get; set; }
        public string? Reason { get; set; }
        public string? Remarks { get; set; }
        public string? DelegatedUserId { get; set; }
        public string? DelegatedUserNameEn { get; set; }
        public string? DelegatedUserNameAr { get; set; }
        public DateTime? HeldDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? RequestCreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? PermissionNameEn { get; set; }
        public string? PermissionNameAr { get; set; }
        public DateTime? PermissionDate { get; set; }
        public string? Description { get; set; }
        public DateTime? SuggestedAppointmentDate { get; set; }
        public int? PermissionTypeId { get; set; }
        public int? ReduceHoursReasonId { get; set; }
        public string? selectedDaysNameAr { get; set; }
        public string? selectedDaysNameEn { get; set; }
        public string? SelectedDaysId { get; set; }
        public string? ReduceHoursReasonEn { get; set; }
        public string? ReduceHoursReasonAr { get; set; }
        public string? ExemptionNameEn { get; set; }
        public string? ExemptionNameAr { get; set; }
        public string? ExemptionTimeNameEn { get; set; }
        public string? ExemptionTimeNameAr { get; set; }
        public bool IsInvolvePaying { get; set; }
        public bool IsDecisionFormUploaded { get; set; }
        public bool IsReSubmitDecisionForm { get; set; }
        public int? ExemptionTypeId { get; set; }
        public int? ExemptionTimeTypeId { get; set; }
    }
}

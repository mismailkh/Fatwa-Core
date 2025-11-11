namespace FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels
{
    public class LeaveType
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int LeaveBalance { get; set; }
        public bool IsActive { get; set; }
    }
    public class LeaveTypeAdvanceSearchVM
    {
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public int? LeaveBalance { get; set; }
    }
}

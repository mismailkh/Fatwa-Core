namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class DeactivateEmployeesVM
    {
        public List<EmployeeVM>? EmployeesList { get; set; }
        public int? DeactivationReason { get; set; }
        public string? StatusReason { get; set; }
        public DateTime? StatusDate { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        public string? UserId { get; set; }
        public int? EmployeeTypeId { get; set; }
    }

    public class DeactivateEmployeesResponse
    {
        public string FailedDeactivations { get; set; }
    }
}

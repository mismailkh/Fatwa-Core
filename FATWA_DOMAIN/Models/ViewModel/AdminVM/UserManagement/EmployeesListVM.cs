using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class EmployeesListVM
    {
        [Key]
        public string UserId { get; set; }
        public string? EmployeeId { get; set; }
        public int EmployeeTypeId { get; set; }
        public int? DesignationId { get; set; }
        public int? SectorTypeId { get; set; }
        public string? RoleId { get; set; }
        public string? EmployeeNameEn { get; set; }
        public string? EmployeeNameAr { get; set; }
        public string? RoleNameEn { get; set; }
        public string? RoleNameAr { get; set; }
        public string? SectorTypeNameEn { get; set; }
        public string? SectorTypeNameAr { get; set; }
        public string? DesignationEn { get; set; }
        public string? DesignationAr { get; set; }
        public bool IsDefaultCorrespondenceReceiver { get; set; }
        public bool IsUserHasAnyTask { get; set; }
        public bool AllowDigitalSign { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class EmployeesListDropdownVM
    {
        [Key]
        public string UserId { get; set; }
        public string EmployeeNameEn { get; set; }
        public string EmployeeNameAr { get; set; }
    }

}

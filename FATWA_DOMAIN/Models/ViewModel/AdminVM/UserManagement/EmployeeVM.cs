using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class EmployeeVM : GridMetadata
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string? AdUserName { get; set; }
        public string EmployeeId { get; set; }
        public string? EmployeeNameEn { get; set; }
        public string? EmployeeNameAr { get; set; }
        public string? CivilId { get; set; }
        public string? DepartmentNameEn { get; set; }
        public string? PassportNumber { get; set; }
        public string? DepartmentNameAr { get; set; }
        public int? DesignationId { get; set; }
        public string? DesignationEn { get; set; }
        public string? DesignationAr { get; set; }
        public int EmployeeTypeId { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? CompanyNameEn { get; set; }
        public string? CompanyNameAr { get; set; }
        public string? SectorTypeNameEn { get; set; }
        public string? SectorTypeNameAr { get; set; }
        public int? SectorTypeId { get; set; }
        public string? RoleId { get; set; }
        public string? CreatedByName_En { get; set; }
        public string? CreatedByName_Ar { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedByName_En { get; set; }
        public string? ModifiedByName_Ar { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? RoleName_En { get; set; }
        public string? RoleName_Ar { get; set; }
        public bool IsUserHasAnyTask { get; set; }
        public bool IsDefaultCorrespondenceReceiver { get; set; }
        public bool AllowDigitalSign { get; set; }
    }

    public class EmployeeVMForDropDown
    {
        public string UserId { get; set; }
        public string? EmployeeNameEn { get; set; }
        public string? EmployeeNameAr { get; set; }
        [NotMapped]
        public Dictionary<string, bool> UserIds { get; set; }
    }
}

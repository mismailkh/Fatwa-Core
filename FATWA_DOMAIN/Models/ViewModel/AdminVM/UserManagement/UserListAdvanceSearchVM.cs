using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class UserListAdvanceSearchVM : GridPagination
    {

        public string? Name { get; set; }
        public string? CivilId { get; set; }
        public string? PassportNumber { get; set; }
        public int? CompanyId { get; set; }
        public int? SectorId { get; set; }
        public int? DesignationId { get; set; }
        public int? EmployeeStatusId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string? RoleId { get; set; }
        public string? UserId { get; set; }
    }
}

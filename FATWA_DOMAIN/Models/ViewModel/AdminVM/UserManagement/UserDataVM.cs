using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class UserDataVM
    {
        public string? UserId { get; set; }
        public bool? IsActive { get; set; }
        public int? SectorTypeId { get; set; }
        public int? SectorParentId { get; set; }
        public string? FirstName_En { get; set; }
        public string? LastName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? LastName_Ar { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }        
        public string? RoleId { get; set; }
        public int? TotalTickets { get; set; }
        public int? PendingTickets { get; set; }
        public int? ClosedTickets { get; set; }
    }    
    public class ManagersListVM
    {
        public string? UserId { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        public string? FullNameWithRole_En { get; set; }
        public string? FullNameWithRole_Ar { get; set; }
    }
    public class SectorUsersVM : GridMetadata
    {
        public string? UserId { get; set; }
        public string? DelegatedUserId { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        public int? SectorTypeId { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName_En { get; set; }
        public string? RoleName_Ar { get; set; }
        public string? DepartmentName_En { get; set; }
        public string? DepartmentName_Ar { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int? EmployeeStatusId { get; set; }
        public int? DesignationId { get; set; }
        public string? EmployeeStatusEn { get; set; }
        public string? EmployeeStatusAr { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class EmployeeDelegationVM
    {
        [Key]
        public string? UserId { get; set; }
        public string? DelegatedUserId { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsAbsent { get; set; }
        public string? LoggedInUsername { get; set; }
        public int? EmployeeTypeId { get; set; }
    }
    public class EmployeeDelegationHistoryVM
    {
        [Key]
        public string? UserId { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        public string? DelegatorName_En { get; set; }
        public string? DelegatorName_Ar { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

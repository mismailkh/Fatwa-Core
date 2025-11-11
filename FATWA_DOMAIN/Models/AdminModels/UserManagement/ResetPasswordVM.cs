using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    public class ResetPasswordVM
    {
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? CreatedBy { get; set; }
        public int? EmployeeType { get; set; }
        public string? AdUserName { get; set; }
    }
}


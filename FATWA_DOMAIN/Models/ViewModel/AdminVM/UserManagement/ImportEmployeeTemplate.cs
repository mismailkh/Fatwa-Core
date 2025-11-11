using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class ImportEmployeeTemplate
    {
        public string? FirstName_En { get; set; }
        public string? SecondName_En { get; set; }
        public string? LastName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? SecondName_Ar { get; set; }
        public string? LastName_Ar { get; set; }
        public string LKP_Nationality { get; set; }
        public string LKP_Gender { get; set; }
        public string LKP_Designation { get; set; }
        public string LKP_Grade { get; set; }
        public string LKP_Department { get; set; }
        public string LKP_SectorType { get; set; }
        public string? Email { get; set; }
        public string AD_UserName { get; set; }
        public string LKP_Role { get; set; }
        //public string? PhoneNumber { get; set; }
        //public DateTime DateOfBirth { get; set; }


        //public string? CivilId { get; set; }
        //public string? PassportNumber { get; set; }
        public string EmployeeId { get; set; }

        //public int FingerPrintId { get; set; }
        //public DateTime DateOfJoining { get; set; }
        public string LKP_EmployeeType { get; set; }

        public string LKP_Company { get; set; }
        //public string LKP_WorkingTime { get; set; }
        //public string LKP_GroupType { get; set; }
        //public string LKP_Group { get; set; }
        public string CreatedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs
{
    public partial class ContactDetailVM
    {
        public Guid ContactId { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? JobRoleEn { get; set; }
        public string? JobRoleAr { get; set; }
        public string? ContactTypeEn { get; set; }
        public string? ContactTypeAr { get; set; }
        public string? DepartmentEn { get; set; }
        public string? DepartmentAr { get; set; }
        public string? Email { get; set; }
        public string? CivilId { get; set; }
        public string? Notes { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DOB { get; set; }
    }
}

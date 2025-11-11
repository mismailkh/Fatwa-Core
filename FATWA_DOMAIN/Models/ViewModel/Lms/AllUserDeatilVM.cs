using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class AllUserDetailVM
    {
        [Key]
        public string Id { get; set; }

        //User Details
        public string FullNameEnglish { get; set; }
        public string FullNameArabic { get; set; }
        public string PhoneNumber { get; set; }
        public int EligibleCount { get; set; }

        // Departments                              
        public int DepartmentId { get; set; }
        public string DepartmentArabic { get; set; }
        public string DepartmentEnglish { get; set; }

        // User Types                              
        public string UserTypeArabic { get; set; }
        public string UserTypeEnglish { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public int? SectorTypeId { get; set; } = 0;

        public string? CivilId { get; set; }


    }
}

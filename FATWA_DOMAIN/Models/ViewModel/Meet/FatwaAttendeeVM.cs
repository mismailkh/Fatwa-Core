using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
    public class FatwaAttendeeVM
    {
        [Key]
        public string Id { get; set; }

        //User Details
        public string? FirstNameEnglish { get; set; }
        public string? FirstNameArabic { get; set; }
        //public string? PhoneNumber { get; set; }
        public int? EligibleCount { get; set; }

        // Departments                              
        public int? DepartmentId { get; set; }
        public string? DepartmentArabic { get; set; }
        public string? DepartmentEnglish { get; set; }

        // User Types                              
        public string? UserTypeArabic { get; set; }
        public string? UserTypeEnglish { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? SectorTypeId { get; set; } = 0;



        [NotMapped]
        public int? SerialNo { get; set; }

        public int? AttendeeStatusId { get; set; }

        public string? AttendeeStatusEn { get; set; }

        public string? AttendeeStatusAr { get; set; }
        public bool? IsPresent { get; set; }
    }

}



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class BookAuthorVM
    {
        [Key]
        public int AuthorId { get; set; } 
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        public string? FirstName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? SecondName_En { get; set; }
        public string? SecondName_Ar { get; set; }
        public string? ThirdName_En { get; set; }
        public string? ThirdName_Ar { get; set; }
        public string? Address_En { get; set; }
        public string? Address_Ar { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }

    }
}
 
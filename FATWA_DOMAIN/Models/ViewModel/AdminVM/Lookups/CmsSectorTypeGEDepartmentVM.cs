using FATWA_DOMAIN.Enums.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class CmsSectorTypeGEDepartmentVM
    {
        [Key]
        public int Id { get; set; }  
        public string GovernmentEntityEn { get; set; }
        public string GovernmentEntityAr { get; set; }
        public string GEDepartmentEN { get; set; }
        public string GEDepartmentAr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } 
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; } 
        public FatwaSectorTypeEnum CmsFatwaSectorType { get; set; } 

    }
}

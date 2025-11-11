using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class ChamberDetailVM
    {
        [Key]
        public int Id { get; set; } 
        public string? Number { get; set; }
        public string? Name_En { get; set; }
        public string? Name_Ar { get; set; }
        public string? ChamberCode { get; set; }
        public string? Address { get; set; }
        public string? CourtNameEn { get; set; }
       public string? CourtNameAr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Description { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }
        public string? SectorTypeEn { get; set; }
        public string? SectorTypeAr { get; set; }


    }
}
 
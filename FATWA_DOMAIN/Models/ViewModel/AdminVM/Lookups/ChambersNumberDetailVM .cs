using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class ChambersNumberDetailVM
    {
        [Key]
        public int Id { get; set; }
        public string? Number { get; set; }
       //public int ChamberId { get; set; }
        public string? Code { get; set; }
        public string? ChamberNamesEn { get; set; }
        public string? ChamberNamesAr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }
        public string? ShiftNameEn { get; set; }
        public string? ShiftNameAr { get; set; }

    }
}
 
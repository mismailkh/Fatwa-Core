using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class ChamberNumberHearingDetailVM
    {
        public int Id { get; set; }
        public string ChamberNumber { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? CourtTypeEn { get; set; }
        public string? CourtTypeAr { get; set; }
        public string? HearingDaysEn { get; set; }
        public string? HearingDaysAr { get; set; }
        //public string ChamberIds { get; set; }
        //public string CourtIds { get; set; }
        public int? HearingDaysId { get; set; }
        public int? ChamberNumberId { get; set; }
        public int? ChamberId { get; set; }
        public int? CourtId { get; set; }
        public int HearingsRollDays { get; set; }
		public int JudgmentsRollDays { get; set; }

	}
}

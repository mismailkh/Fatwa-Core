
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.MojRollsVM
{
    public class MOJRollsChamberCourtChamberNumberVM
    {
        [Key]
        public int? ChamberNumberId { get; set; }
        public int? ChamberId { get; set; }
        public int? CourtId { get; set; }
        public string ChamberName_En { get; set; }

        public string ChamberName_Ar { get; set; }

        public string ChamberCode { get; set; }

        public string CourtName_En { get; set; }

        public string CourtName_Ar { get; set; }

        public string CourtCode { get; set; }

        public string ChamberNumber { get; set; }

        public string ChamberNumberCode { get; set; }
        public string? LawyerId { get; set; }
        public string? LawyerFullNameEn { get; set; }
        public string? LawyerFullNameAr { get; set; }

    }


    }


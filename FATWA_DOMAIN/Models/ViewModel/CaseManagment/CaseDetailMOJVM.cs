using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CaseDetailMOJVM
    {
        [Key]
        public Guid CaseId { get; set; } 
        public string CANNumber { get; set; } 
        public string CaseNumber { get; set; }
        public DateTime? CaseDate { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }  
        public string? SectorNameEn { get; set; }
        public string? SectorNameAr { get; set; }
        public string? RequestTypeEn { get; set; }
        public string? RequestTypeAr { get; set; }
        public string? FloorNumber { get; set; } = string.Empty;
        public string? RoomNumber { get; set; } = string.Empty;
        public string? AnnouncementNumber { get; set; } = string.Empty;
        public DateTime? HearingDate { get; set; }
        public TimeSpan? HearingTime { get; set; }

    }
}

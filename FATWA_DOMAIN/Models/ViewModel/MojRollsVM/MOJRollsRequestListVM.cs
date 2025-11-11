using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojRollsVM
{
    public class MOJRollsRequestListVM
    {
        [Key]
        public int Id { get; set; }
        public int? ChamberNumberId { get; set; }
        public string? RollDescription { get; set; }
        public string? StatusName { get; set; }
        public int? StatusId { get; set; }
        public string? StatusNameAr { get; set; }
        public DateTime? RequestedDateTime { get; set; }
        public DateTime? SessionDate { get; set; }
        public string? FilePath { get; set; }
        public string? CreatedBy { get; set; }
        public int? DocumentId { get; set; }
        public int? ChamberId { get; set; }
        public int? CourtId { get; set; }
        public string? ChamberName_En { get; set; }
        public string? ChamberName_Ar { get; set; }
        public string? ChamberCode { get; set; }
        public string? CourtName_En { get; set; }
        public string? CourtName_Ar { get; set; }
        public string? CourtCode { get; set; }
        public string? ChamberNumber { get; set; }
        public string? ChamberNumberCode { get; set; }
        public bool IsAssigned { get; set; }
        public string? LawyerId { get; set; }
        public string? LawyerFullNameEn { get; set; }
        public string? LawyerFullNameAr { get; set; }

    }
}

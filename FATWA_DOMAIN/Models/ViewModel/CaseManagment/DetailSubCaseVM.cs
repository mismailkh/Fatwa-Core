using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class DetailSubCaseVM
    {
        [Key]
        public Guid? CaseId { get; set; }
        public Guid? FileId { get; set; }
        public string? CaseNumber { get; set; }
        public DateTime CaseDate { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
     //  public string? CaseAmount { get; set; }
        public string? CaseRequirements { get; set; }
     //  public string? CaseRequirmen { get; set; }
     // public string? Requirment { get; set; }
    }
}

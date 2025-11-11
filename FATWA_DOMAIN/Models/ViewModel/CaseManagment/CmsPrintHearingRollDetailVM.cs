using FATWA_DOMAIN.Models.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2024-04-05' Version = "1.0" Branch = "master">View Model for Printing Hearing Roll Detail</History>
    public class CmsPrintHearingRollDetailVM
    {
        [Key]
        public Guid HearingId { get; set; }
        public Guid CaseId { get; set; }
        public string CANNumber { get; set; }
        public string CaseNumber { get; set; }
        public string? HearingDescription { get; set; }
        public string? PortfolioStoragePath { get; set; }
        public string? PrimaryLawyerNameEn { get; set; }
        public string? PrimaryLawyerNameAr { get; set; }
        public string? PrimaryLawyerId { get; set; }
        [NotMapped]
        public OutcomeHearing OutcomeHearing { get; set; } = new OutcomeHearing { Id = Guid.NewGuid()};
    }

    public class CmsHearingRollDetailSearchVM
    {
        public DateTime HearingDate { get; set; }
        public int ChamberNumberId { get; set; }
        public int HearingRollId { get; set; }
    }
}

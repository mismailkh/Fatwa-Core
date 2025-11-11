using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppHearingDetailVM
    {
        [DisplayName("Case_Number")]
        public string CaseNumber { get; set; }
        [DisplayName("Hearing_Date")]
        public DateTime HearingDate { get; set; }
        [DisplayName("Lawyer")]
        public string LawyerName { get; set; }
        [DisplayName("Hearing_Time")]
        public TimeSpan HearingTime { get; set; }
        [DisplayName("Hearing_Description")]
        public string? Description { get; set; }
        [DisplayName("Status")]
        public string? Status { get; set; }
        [DisplayName("Reason")]
        public string? PostponeReason { get; set; }
    }
}

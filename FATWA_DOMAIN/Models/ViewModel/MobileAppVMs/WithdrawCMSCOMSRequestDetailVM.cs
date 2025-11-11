using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class WithdrawCMSCOMSRequestDetailVM
    {
        [DisplayName("Request_Number")]
        public string RequestNumber { get; set; }
        [DisplayName("Reason")]
        public string? Reason { get; set; }
        [DisplayName("Inbox_Number")]
        public string? InboxNumber { get; set; }
        [DisplayName("Inbox_Date")]
        public DateTime? InboxDate { get; set; }
        [DisplayName("Reference_Number")]
        public string? ReferenceNumber { get; set; }
        [DisplayName("Reference_Date")]
        public DateTime? ReferenceDate { get; set; }
        [DisplayName("Correspondence_Type")]
        public string? CorrespondenceType { get; set; }
        [DisplayName("Status")]
        public string? StatusName { get; set; }
        [DisplayName("Rejection_Reason")]
        public string? RejectionReason { get; set; }
        [DisplayName("Activity_Name")]
        public string? ActivityName { get; set; }
        [DisplayName("Remarks")]
        public string? Remarks { get; set; }
        [DisplayName("Created_Date")]
        public DateTime CreatedDate { get; set; }
    }
}

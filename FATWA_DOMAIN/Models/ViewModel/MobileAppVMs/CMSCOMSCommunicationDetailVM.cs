using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class CMSCOMSCommunicationDetailVM
    {
        [DisplayName("Reference_No")]
        public string? ReferenceNumber { get; set; }
        [DisplayName("Communication_Response_Title")]
        public string? CommunicationResponseTitle { get; set; }
        [DisplayName("Description")]
        public string? Description { get; set; }
        [DisplayName("Date")]
        public DateTime? RequestOrFileDate { get; set; }
        [DisplayName("Request_Date")]
        public DateTime? RequestDate { get; set; }
        [DisplayName("Legal_Notification_Type")]
        public string? ResponseType { get; set; }
        [DisplayName("Correspondence_Type")]
        public string? CorrespondenceType { get; set; }
        [DisplayName("Activity")]
        public string? ActivityName { get; set; }
        [DisplayName("Response_Date")]
        public DateTime? ResponseDate { get; set; }
        [DisplayName("Reason")]
        public string? Reason { get; set; }
        [DisplayName("Other")]
        public string? Other { get; set; }
        [DisplayName("Due_Date")]
        public DateTime? DueDate { get; set; }
        [DisplayName("Priority")]
        public string? PriorityName { get; set; }
        [DisplayName("Frequency_Name")]
        public string? FrequencyName { get; set; }
        [DisplayName("Additiona_GE_Users")]
        public string? AdditionalGEUser { get; set; }
        [DisplayName("Outbox_Number")]
        public string? OutboxNumber { get; set; }
        [DisplayName("Outbox_Date")]
        public DateTime? OutboxDate { get; set; }
        [DisplayName("Inbox_Number")]
        public string? InboxNumber { get; set; }
        [DisplayName("Inbox_Date")]
        public DateTime? InboxDate { get; set; }
        [DisplayName("Reference_Number")]
        public string? G2GReferenceNumber { get; set; }
        [DisplayName("Reference_Date")]
        public DateTime? G2GReferenceDate { get; set; }
        [DisplayName("Government_Entity_Name")]
        public string? PartyEntityName { get; set; }
    }
}

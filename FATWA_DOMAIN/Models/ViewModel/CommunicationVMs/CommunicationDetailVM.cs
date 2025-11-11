using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public class CommunicationDetailVM
    {
        
        public int? ResponseTypeId { get; set; }
        public Guid? ReferenceId { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? CommunicationResponseTitle { get; set; }
        public string? Description { get; set; }
        public DateTime? RequestOrFileDate { get; set; } 
        public DateTime? RequestDate { get; set; } 
        public string? ResponseTypeAr { get; set; }
        public string? ResponseTypeEn { get; set; }
        public string? CorrespondenceTypeEn { get; set; }
        public string? CorrespondenceTypeAr { get; set; }
        public int? CorrespondenceTypeId { get; set; }
        public string? Activity_En { get; set; }
        public string? Activity_Ar { get; set; }
        public DateTime? ResponseDate { get; set; } 
        public string? Reason { get; set; }
        public string? Other { get; set; }
        public DateTime? DueDate { get; set; } 
        public string? PriorityNameEN { get; set; }
        public string? PriorityNameAr { get; set; }
        public string? FrequencyNameEn { get; set; }
        public string? FrequencyNameAr { get; set; }
        public string? AdditionalGEUserEn { get; set; }
        public string? AdditionalGEUserAr { get; set; }
        public bool? IsUrgent { get; set; }
        public string? OutboxNumber { get; set; }
        public DateTime? OutboxDate { get; set; } 
        public string? InboxNumber { get; set; }
        public DateTime? InboxDate { get; set; } 
        public string? G2GReferenceNumber { get; set; }
        public DateTime? G2GReferenceDate { get; set; }
        public string? PartyEntityEn { get; set; }
        public string? PartyEntityAr { get; set; }
        public int? GovtEntityId { get; set; }
        public int? DepartmentId { get; set; }
        public bool? IsReplied { get; set; }

		[NotMapped]
        public string? StopExeRejectionReason { get; set; }
    }
}

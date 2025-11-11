using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public partial class WithdrawRequestDetailVM
    {
        [Key]
        public Guid CommunicationId { get; set; }
        public Guid ReferenceGuid { get; set; }
        public string RequestNumber { get; set; }
        public int CommunicationTypeId { get; set; }
        public int CorrespondenceTypeId { get; set; }
        public string? FatwaReferenceNo { get; set; }
        public DateTime? FatwaReferenceDate { get; set; }
        public string? GeReferenceNo { get; set; }
        public DateTime? GeReferenceDate { get; set; }
        public string? Reason { get; set; }
        public string? RejectionReason { get; set; }
        public string? CorrespondenceTypeEn { get; set; }
        public string? CorrespondenceTypeAr { get; set; }
        public string? Activity_En { get; set; }
        public string? Activity_Ar { get; set; }
        public string? CreatedBy { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? WithDrawStatusId { get; set; }
        public string? ModifiedBy { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [NotMapped]
        public string TaskUserId { get; set; }
        public Guid WithdrawRequestId { get; set; }
    }
}

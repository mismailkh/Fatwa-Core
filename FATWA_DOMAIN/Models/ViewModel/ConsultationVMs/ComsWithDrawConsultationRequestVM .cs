using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ComsWithDrawConsultationRequestVM
    {
        public Guid WithdrawId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public string? RequestNumber { get; set; }
        public string? Subject { get; set; }
        public int? RequestTypeId { get; set; }
        public string? RequestType_Name_En { get; set; }
        public string? RequestType_Name_Ar { get; set; }
        public int? SectorTypeId { get; set; }
        public string? Status_Name_En { get; set; }
        public string? Status_Name_Ar { get; set; }
        public string? SectorType_Name_Ar { get; set; }
        public string? SectorType_Name_En { get; set; }
        public string? OfficialLetterOutboxNumber { get; set; }
        public string? FatwaInboxNumber { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? WithDrawStatusId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? WithdrawRequestedBy { get; set; }
        public string? WithdrawApprovedBy { get; set; }
        public string? WithdrawRejectedBy { get; set; }
        [NotMapped]
        public string? Reason { get; set; }
        //public int CommunicationTypeId { get; set; }

    }
}

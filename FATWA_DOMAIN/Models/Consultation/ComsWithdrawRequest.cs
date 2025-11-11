using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_WITHDRAW_REQUEST")]
    public class ComsWithdrawRequest
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public string? Reason { get; set; }
        public int? RequestStatusId { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime? RequestedDate { get; set; } = DateTime.Now;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; } = DateTime.Now;
        public string? RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public string? FatwaReason { get; set; }
        [NotMapped]
        public UploadedDocument? UploadedDocument { get; set; }
        public int? RequestPreviousStateStatusId { get; set; } = 0;
        public int? FilePreviousStateStatusId { get; set; } = 0;
        [NotMapped]
        public int GovtEntityId { get; set; }
        [NotMapped]
        public int DepartmentId { get; set; }
        [NotMapped]
        public int SectorTypeId { get; set; }
        public string RequestNumber { get; set; }
        public string? RejectionReason { get; set; }
    }
}

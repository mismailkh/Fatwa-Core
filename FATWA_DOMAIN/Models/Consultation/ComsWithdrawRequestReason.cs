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
    [Table("COMS_WITHDRAW_REQUEST_REASON")]
    public class ComsWithdrawRequestReason : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid WithdrawRequestId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string? Reason { get; set; }
        public int? RequestStatusId { get; set; }
        [NotMapped]
        public UploadedDocument? UploadedDocument { get; set; }
    }
}

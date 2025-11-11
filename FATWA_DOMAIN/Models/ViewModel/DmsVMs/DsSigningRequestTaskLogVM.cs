using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class DsSigningRequestTaskLogVM
    {
        [Key]
        public Guid SigningTaskId { get; set; }
        public int DocumentId { get; set; }
        public string? Remarks { get; set; }
        public string? RejectionReason { get; set; }
        public string? SenderNameEn { get; set; }
        public string? SenderNameAr { get; set; }
        public string? TaskStatusEn { get; set; }
        public string? TaskStatusAr { get; set; }
        public int StatusId { get; set; }
        public string? ReceiverNameEn { get; set; }
        public string? ReceiverNameAr { get; set; }
        public string ReceiverId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate{ get; set; }
        public string SigningMethodEn { get; set; }
        public string SigningMethodAr { get; set; }
    }

    public class DsSigningResponseVM
    {
        public string RequestStatus { get; set; }
        public int SigningMethodId { get; set; }
    }
}

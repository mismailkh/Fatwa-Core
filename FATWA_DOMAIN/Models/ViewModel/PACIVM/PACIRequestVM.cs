
using FATWA_DOMAIN.Models.ViewModel.PACIVM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.PACIVM

{
    public class PACIRequestVM
    {
        [Key]
        public Guid RequestId { get; set; }
        public string? CaseNumber { get; set; }

        public string? Year { get; set; }

        public string? AddressType { get; set; }

        public string? RequestDocPath { get; set; }

        public string? ResponseDocPath { get; set; }

        public int? EmailSentStatusId { get; set; }

        public DateTime? ResponseDate { get; set; }

        public int? RequestStatusId { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsCases { get; set; }
        public bool? IsDeleted { get; set; }

        public int? ChannelId { get; set; }
        public int? RefrenceNumber { get; set; }
  
        [NotMapped] 
        public List<PACIRequestDataVM>? PACIRequestsData { get; set; } = new List<PACIRequestDataVM>();

    }

}

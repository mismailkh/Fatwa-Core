using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.DigitalSignature
{
    [Table("DS_SIGNING_REQUEST_LOG")]
    public class DSPRequestLog : TransactionalBaseModel
    {
        [Key]
        public int LogId { get; set; }
        public string ExternalId { get; set; }
        public string RequestId { get; set; }
        public string CivilId { get; set; }
        public int DocumentId { get; set; }
        public bool Status { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string RequestStatus { get; set; }
        public int SigningMethodId { get; set; }
    }
}

using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.DigitalSignature
{
    [Table("DSP_Athentication_Requests")]
    public class DSPAuthenticationRequestLog : TransactionalBaseModel
    {
        [Key]
        public int AuthenticationLogId { get; set; }
        public string RequestId { get; set; }
        public string ErrorCode { get; set; }
        public string RequestPayload { get; set; }
        public string ResponsePayload { get; set; }
    }
}

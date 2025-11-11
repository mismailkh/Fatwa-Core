using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.DigitalSignature
{
    public class AuthenticateRequestVM
    {
        public string AuthenticationReasonAr { get; set; }
        public string AuthenticationReasonEn { get; set; }
        public string UserId { get; set; }
        public string RefId { get; set; }
        public bool RequestUserDetails { get; set; }
        public string ServiceDescriptionAR { get; set; }
        public string ServiceDescriptionEN { get; set; }
        public string CreatedBy { get; set; }
    }
      //"additionalData": "string",
      //"challenge": "string",
      //"spCallbackURL": "string",
}

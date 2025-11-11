using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("tToken")]
    public class TokenModel
    {
        [Key]
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public Boolean Used { get; set; }
        public bool Invalidated { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public string JwtToken { get; set; }
    }
}

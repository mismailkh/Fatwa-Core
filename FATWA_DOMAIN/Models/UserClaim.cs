using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("tUserClaim")]
    public class UserClaim
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("FK_User")]
        [Required]
        public Guid UserId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
    }
}

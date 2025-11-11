using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("tUserRole")]

    public class UserRole
    {
        [ForeignKey("FK_Role")]
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("FK_User")]
        [Required]
        public Guid UserId { get; set; }

    }
}

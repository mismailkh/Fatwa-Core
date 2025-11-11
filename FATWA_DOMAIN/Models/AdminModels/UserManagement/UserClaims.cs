using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_USER_CLAIMS")]
    public class UserClaims:TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
        public long? ModuleId { get; set; }
        public User User { get; set; }
    }
}

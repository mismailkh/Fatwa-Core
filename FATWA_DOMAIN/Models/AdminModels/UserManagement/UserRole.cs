using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_USER_ROLES")]
    public class UserRole : TransactionalBaseModel
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}

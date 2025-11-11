using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_USER_GROUP")]
    public class UserGroup:TransactionalBaseModel
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

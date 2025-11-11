using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_GROUP_CLAIMS")]
    public class GroupClaims
    {
        [Key]
        public int Id { get; set; }
        public Guid GroupId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
    }

}

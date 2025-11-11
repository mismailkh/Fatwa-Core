using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_GROUP_ACCESS_TYPE")]
    public class GroupTypeWebSystem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupAccessTypeId { get; set; }
        public int GroupTypeId { get; set; }
        public GroupType GroupType { get; set; }
        public int WebSystemId { get; set; }
        public WebSystem WebSystem { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}

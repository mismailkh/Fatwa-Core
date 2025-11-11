using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_GROUP_TYPE")]
    public class GroupType : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<GroupAccessType> GroupTypes { get; set; }
        public ICollection<Group> Group { get; set; }

    }
}

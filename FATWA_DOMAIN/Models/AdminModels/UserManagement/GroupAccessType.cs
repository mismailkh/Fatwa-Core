using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_GROUP_ACCESS_TYPE")]

    public class GroupAccessType 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupAccessTypeId { get; set; }
        public int GroupTypeId { get; set; }
        public GroupType GroupType { get; set; }
        public int WebSystemId { get; set; }
        public WebSystem WebSystem { get; set; }
        
    }


    public class GroupAccessTypeVM : TransactionalBaseModel
    {
        public int GroupTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> SelectedIdz { get; set; }
    }

    public class GroupTypeWebSystemVM
    {
        public int GroupTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebSystemsEn { get; set; }
        public string WebSystemsAr{ get; set; }
        public List<int> WebSystemsIds { get; set; }

        public string CustomTextEn
        {
            get { return $"{Name} ({WebSystemsEn})"; }
        }

        public string CustomTextAr
        {
            get { return $"{Name} ({WebSystemsEn})"; }
        }
    }

    





}

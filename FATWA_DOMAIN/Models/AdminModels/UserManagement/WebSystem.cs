using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("WEB_SYSTEM")]
    public class WebSystem : TransactionalBaseModel
    {
        [Key]

        public int WebSystemId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public ICollection<GroupAccessType> GroupTypes { get; set; }

    }
}

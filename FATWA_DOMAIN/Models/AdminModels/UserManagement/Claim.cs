using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> created new table for Claims which can be used for assigning to both Roles and Users</History>
    [Table("UMS_CLAIM")]
    public class ClaimUms
    {
        [Key]
        public int Id { get; set; }
        public string Title_En { get; set; }
        public string Title_Ar { get; set; }
        public string Module { get; set; }
        public string SubModule { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public bool IsDeleted { get; set; }
        public int ModuleId { get; set; }
    }
}

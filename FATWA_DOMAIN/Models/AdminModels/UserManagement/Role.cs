using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Role model</History>
    [Table("UMS_ROLE")]
    public class Role : TransactionalBaseModel
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [StringLength(256)]
        public string? Name { get; set; }
        [StringLength(256)]
        public string? NameAr { get; set; }
        [StringLength(256)]
        public string? NormalizedName { get; set; }
        public string? Description_En { get; set; }
        public string? Description_Ar { get; set; }
        public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public ICollection<UserRole> UserRoles { get; set; }
    }
}

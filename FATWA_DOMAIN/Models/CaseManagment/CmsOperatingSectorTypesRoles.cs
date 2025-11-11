using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE")]
    public class CmsOperatingSectorTypesRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SectorId { get; set; }
        public string RoleId { get; set; }
        [NotMapped]
        public IEnumerable<string> RoleIds { get; set; }

    }
}

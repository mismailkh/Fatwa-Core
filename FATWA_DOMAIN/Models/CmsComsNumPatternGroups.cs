using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models
{
    [Table("CMS_COMS_PATTERN_GROUP")]
    //<History Author = 'Nabeel ur Rehman' Date='2023-08-24' Version="1.0" Branch="master">Add Case and consultation number and file Number</History>
    public partial class CmsComsNumPatternGroups
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid NumPattrenId { get; set; }

    }
}

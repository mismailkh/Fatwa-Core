using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models
{
    [Table("LITERATURE_DEWEY_NUMBER_PATTERN_TYPE")]

    //<History Author = 'Ihsaan Abbas' Date='2024-05-06' Version="1.0" Branch="master">LITERATURE DEWEY NUMBER PATTERN TYPE</History>
    public partial class LiteratureDeweyNumberPatternType    
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PatternNameEn { get; set; }
        public string PatternNameAr { get; set; }

    }
}

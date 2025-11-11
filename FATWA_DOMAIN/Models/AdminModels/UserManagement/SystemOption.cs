using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-08-11' Version="1.0" Branch="master"> Create model for system configuration</History>
    [Table("SYSTEM_OPTION", Schema = "dbo")]
    public class SystemOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }
        public string Option_Name_En { get; set; }
        public string Option_Name_Ar { get; set; }
    }
}

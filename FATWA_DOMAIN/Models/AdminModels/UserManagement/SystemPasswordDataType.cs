using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-08-13' Version="1.0" Branch="master"> Create model for system configuration</History>
    [Table("SYSTEM_PASSWORD_DATA_TYPE", Schema = "dbo")]
    public class SystemPasswordDataType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DataTypeId { get; set; }
        public string DataType_Name_En { get; set; }
        public string DataType_Name_Ar { get; set; }
    }
}

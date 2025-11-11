using FATWA_DOMAIN.Models.Dms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-09-10' Version="1.0" Branch="master"> Create model for system setting</History>
    [Table("SYSTEM_SETTING", Schema = "dbo")]
    public class SystemSetting
    {
        [Key]
        public Guid SettingId { get; set; }
        public int Grid_Pagination { get; set; }
        public int Book_Copy_Count { get; set; }
        public int Eligible_Count { get; set; }
        public int Borrow_Period { get; set; }
        public int Extension_Period { get; set; }
        public int File_Minimum_Size { get; set; } 
        public int File_Maximum_Size { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? FileTypes { get; set; }
        public List<DsSigningMethods> SigningMethods { get; set; } = new List<DsSigningMethods>();
        [NotMapped]
        public IList<string>? SelectedTypesIdList { get; set; } = new List<string>();
        [NotMapped]
        public IList<DmsFileTypes>? DmsFileTypesSelectedList { get; set; } = new List<DmsFileTypes>();
    }
}

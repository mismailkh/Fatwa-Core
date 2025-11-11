using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_WEEKDAYS_SETTINGS_LKP")]
    public class WeekdaysSetting
    {
        [Key]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsRestDay { get; set; }
    }
}

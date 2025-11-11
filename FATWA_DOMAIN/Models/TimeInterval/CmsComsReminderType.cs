using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TimeInterval
{
    [Table("CMS_COMS_REMINDER_Type")]
    //<History Author = 'Nabeel ur Rehman' Date='2023-08-24' Version="1.0" Branch="master">Add Case and consultation number and file Number</History>
    public partial class CmsComsReminderType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string IntervalNameEn { get; set; }
        public string IntervalNameAr { get; set; }

    }
}

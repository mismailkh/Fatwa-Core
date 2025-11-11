using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs
{
    public class PublicHolidaysVM : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; }
        public int? HolidayEnumValue { get; set; } = 0;
        public bool IsActive { get; set; }
        public string CreatedByEn { get; set; }
        public string CreatedByAr { get; set; }
    }
}

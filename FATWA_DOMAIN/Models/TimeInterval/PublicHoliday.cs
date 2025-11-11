using FATWA_DOMAIN.Models.BaseModels;
using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FATWA_DOMAIN.Models.TimeInterval
{
    [Table("WS_PUBLIC_HOLIDAYS")]

    public class PublicHoliday : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Today.AddDays(1);
        public DateTime? ToDate { get; set; }
        public string Description { get; set; }
        public int? HolidayEnumValue { get; set; } = 0;
        public bool IsActive { get; set; }
        [NotMapped]
        public string CreatedByEn { get; set; }
        [NotMapped]
        public string CreatedByAr { get; set; }

    }
}

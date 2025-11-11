using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Lms
{
    [Table("LMS_STOCKTAKING_EVENT_LKP")]
    public class LmsStockTakingEvent : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; } 
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}

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
    [Table("LMS_STOCKTAKING_PERFORMER")]

    public class LmsStockTakingPerformer : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get;set; }
        public Guid StockTakingId { get;set; }
        public string UserId { get;set; }
    }
}

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
    [Table("LMS_LITERATURE_STOCKTAKING")]
    public  class LmsStockTaking : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StockTakingDate { get; set; }
        public int StatusId { get;set; }
        public int TotalBooks { get; set; }
        public string? Note { get; set; }
        public string ReportNumber {get; set;}
        public int? ShortNumber { get; set; }
        [NotMapped]
        public string? UploadFrom { get; set; }
        [NotMapped]
        public string? Project { get; set; }
        [NotMapped]
        public byte[]? FileData { get; set; }
    }
}

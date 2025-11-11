using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public  class LmsStockTakingDetailVM:TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public DateTime StockTakingDate { get; set; }
        public int StatusId { get; set; }
        public int TotalBooks { get; set; }
        public string? Note { get; set; }
        public string StatusEn { get; set; }
        public string StatusAr { get; set; }
        public string MemeberNameEn { get; set; }
        public string MemeberNameAr { get; set; }
        public string? ReportNumber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class LmsStockTakingHistoryVm
    {
        [Key]
        public Guid Id { get; set; }
        public string? EventEn { get; set; }
        public string? EventAr { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
        public DateTime CreatedDate { get;set; }

    }
}

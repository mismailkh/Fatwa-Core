using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class RMSChamberVM
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? CourtId { get; set; }
    }
    public class MobileAppChamberVM
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public int? CourtId { get; set; }
    }
}

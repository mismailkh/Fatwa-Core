using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM
{
    public class TimeTrackingFilesVM
    {
        public Guid? FileId { get; set; }
        public string? FileNumber { get; set; }
        public string? GovernmentEntityNameEn { get; set; }
        public string? GovernmentEntityNameAr { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? LastActionEn { get; set; }
        public string? LastActionAr { get; set; }
        public int? StatusId { get; set; }
    }
}

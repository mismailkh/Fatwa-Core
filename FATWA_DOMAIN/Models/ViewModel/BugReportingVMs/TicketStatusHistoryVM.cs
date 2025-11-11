using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class TicketStatusHistoryVM
    {
        public Guid HistoryId { get; set; }
        public Guid? ReferenceId { get; set; }
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserNameEn { get; set; }
        public string UserNameAr { get; set; }
        public string Value_En { get; set; }
        public string Value_Ar { get; set; }
        public string? EventEn { get; set; }
        public string? EventAr { get; set; }
    }
}

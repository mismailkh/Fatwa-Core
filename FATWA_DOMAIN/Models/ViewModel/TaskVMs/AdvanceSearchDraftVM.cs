using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
    public class AdvanceSearchDraftVM
    {
        public int? DraftNumber { get; set; }
        public string? Name { get; set; }
        public string? FileNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class MojDocumentAdvanceSearchVM
    {
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public int? AttachmentTypeId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}

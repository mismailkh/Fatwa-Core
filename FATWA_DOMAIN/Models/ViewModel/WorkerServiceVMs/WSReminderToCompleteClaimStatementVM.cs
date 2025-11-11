using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    //use only for worker service
    public class WSReminderToCompleteClaimStatementVM
    {
        public DateTime CaseAssignDate { get; set; }
        public string LawyerId { get; set; }
        public string SenderName { get; set; }
        public int SectorTypeId { get; set; }
        public string? ReferenceNumber { get; set; }
        public Guid? ReferenceId { get; set; }
        public string Entity { get; set; }
        public string? DocumentType { get; set; }

    }
}

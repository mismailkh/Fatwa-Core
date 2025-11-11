using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    public class WSRequestForAdditionalInfoVM
    {
        public DateTime CaseAssignDate { get; set; }
        public string LawyerId { get; set; }
        public string SenderName { get; set; }
        public int SectorTypeId { get; set; }
        public string? ReferenceNumber { get; set; }
        public Guid? ReferenceId { get; set; }
        public string Entity { get; set; }
        public string DocumentType { get; set; }
    }

    public class WSRequestForAdditionalInfoReminderVM
    {
        public DateTime CaseAssignDate { get; set; }
        public string LawyerId { get; set; }
        public string SenderName { get; set; }
        public int SectorTypeId { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? ReferenceName { get; set; }
        public Guid? ReferenceId { get; set; }
        public Guid? CommunicationId { get; set; }
        public string SubmoduleId { get; set; }
        public DateTime OutboxDate { get; set; }
        public string? GovtEntityName { get; set; }

    }
}

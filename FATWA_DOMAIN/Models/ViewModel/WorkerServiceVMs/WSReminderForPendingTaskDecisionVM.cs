using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    public class WSReminderForPendingTaskDecisionVM
    {
        public Guid TaskId { get; set; }
        public int? TaskNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModuleId { get; set; }
        public int? SectorId { get; set; }
        public string? ManagerId { get; set; }
        public int? SubModuleId { get; set; }
        public Guid? ReferenceId { get; set; }
        public string AssignedByEn { get; set; }
        public string AssignedByAr { get; set; }
        public string AssignedToEn { get; set; }
        public string AssignedToAr { get; set; }
        public string? ReferenceNumber { get; set; }
        public string SubjectEn { get; set; }
        public string SubjectAr { get; set; }
        public string TaskUrl { get; set; }
    }
}

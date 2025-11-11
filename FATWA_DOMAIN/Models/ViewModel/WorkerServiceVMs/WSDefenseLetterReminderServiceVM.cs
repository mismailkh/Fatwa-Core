using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    //Use only for Defense Letter Reminder Service
    public class WSDefenseLetterReminderServiceVM
    {
        public DateTime JudgmentDate { get; set; }
        public string LawyerId { get; set; }
        public string SenderName { get; set; }
        public int SectorTypeId { get; set; }
        public string? FileNumber { get; set; }
        public string? CaseNumber { get; set; }
        public string? FileName { get; set; }
        public Guid? FileId { get; set; }
        public Guid? CaseId { get; set; }
    }
}

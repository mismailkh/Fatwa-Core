using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    public partial class WSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM
    {
        [Key]
        public Guid FileId { get; set; }
        public string LawyerId { get; set; }
        public string FileNumber { get; set; }
        public string CaseNumber { get; set; }
        public Guid CaseId { get; set; }
        public int CourtId { get; set; }
        public int StatusId { get; set; }
        public int SectorTypeId { get; set; }
        public int ChamberNumberId { get; set; }
        public DateTime JudgementDate { get; set; }
        public string? SectorTo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    //use only for worker service
    public class WSReminderToCompleteDraftModificationVMS
    {
        public DateTime? supervisorRejectDate { get; set; }
        public string? LawyerId { get; set; } 
        public string? FileNumber { get; set; }
        public string? FileName { get; set; }
        public Guid? FileId { get; set; }
        public int SectorTypeId { get; set; }

        //public string? LawyerName { get; set; }
        //public bool? IsNotification { get; set; }
        //public bool? IsTask { get; set; }
        //public int Id { get; set; }
        //public int CmsComsReminderTypeId { get; set; }
        //public string? IntervalNameEn { get; set; }
        //public string? IntervalNameAr { get; set; }
    }
}

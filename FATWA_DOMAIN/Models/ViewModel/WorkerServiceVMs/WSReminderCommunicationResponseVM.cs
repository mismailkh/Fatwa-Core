using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    public class WSReminderCommunicationResponseVM
    {
        public int? Id { get; set; }
        public Guid? CommunicationId { get; set; } 
        public string? CommunicationTypeNameEN { get; set; }
        public string? CommunicationTypeNameAr { get; set; }
        public int? CmsComsReminderTypeId { get; set; }
        public string? IntervalNameEn { get; set; }
        public string? IntervalNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedById { get; set; }
        public bool? IsTask { get; set; }
        public bool? IsNotification { get; set; }
        public int SectorTypeId { get; set; }
        public Guid ReferenceId { get; set; }
   
    }
}

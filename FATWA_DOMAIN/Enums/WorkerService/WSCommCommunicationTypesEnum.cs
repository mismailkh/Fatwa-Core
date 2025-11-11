using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Enums.WorkerService
{
    public class WSCommCommunicationTypesEnum
    { 
        public enum WSCommCommunicationTypeEnum
        {
            [Display(Name = "Request For More Information")]
            RequestForMoreInformation = 8,
            [Display(Name = "Meeting Scheduled")]
            MeetingScheduled = 64,
            [Display(Name = "Request For More Information Reminder")]
            RequestForMoreInformationReminder = 8192,
            [Display(Name = "Save And Close File")]
            SaveAndCloseFile = 16384,
            [Display(Name = "Case Registered")]
            CaseRegistered = 32768,
            [Display(Name = "Judgement")]
            Judgement = 131072,
            [Display(Name = "General Update")]
            GeneralUpdate = 262144,
            [Display(Name = "Incoming Report")]
            IncomingReport = 1048576,
            [Display(Name = "Case File Execution")]
            CaseFileExecution = 65536,
            [Display(Name = "Reject Save And Close File")]
            RejectSaveAndCloseFile = 1073741824,



             
        }

    }
}

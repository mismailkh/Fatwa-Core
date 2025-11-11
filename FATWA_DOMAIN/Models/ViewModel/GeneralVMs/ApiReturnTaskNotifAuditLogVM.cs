using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.GeneralVMs
{
    public class ApiReturnTaskNotifAuditLogVM
    {
        public bool result { get; set; }
        [NotMapped]
        public List<SaveTaskVM> addedTaskList { get; set; } = new List<SaveTaskVM>();
        [NotMapped]
        public List<Notification> sendNotifications { get; set; } = new List<Notification>();
        [NotMapped]
        public ProcessLog processLog { get; set; } = new ProcessLog();
        [NotMapped]
        public ErrorLog errorLog { get; set; } = new ErrorLog();
    }
}

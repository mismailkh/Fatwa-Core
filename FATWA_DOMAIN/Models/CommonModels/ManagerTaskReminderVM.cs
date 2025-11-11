using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CommonModels
{
    public class ManagerTaskReminderVM
    {
        public Guid TaskId { get; set; }
        public bool IsReminderSent { get; set; }
        public string ManagerId { get; set; } 
    }
}

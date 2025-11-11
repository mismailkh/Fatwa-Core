using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkerService
{
    [Table("TASK_DECISION_REMINDER")]
    public class TaskDecisionReminder
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public bool IsReminderSent { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}

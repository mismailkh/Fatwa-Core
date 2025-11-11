using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
    public class TaskDashboardVM
    {
        public int? Approved { get; set; }
        public int? Rejected { get; set; }
        public int? Inprogress { get; set; }
        public int? Completed { get; set; }
        public int? Pending { get; set; }
        [NotMapped]
        public string ToDoItem { get; set; }
        [NotMapped]
        public string User { get; set; }
    }
}

using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TaskModels
{
    [Table("TSK_TASK_ACTION")]
    public class TaskAction : TransactionalBaseModel
    {
        [Key]
        public Guid ActionId { get; set; }
        public string ActionName { get; set; }  
        public DateTime? DueDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public Guid TaskId { get; set; } 
    }
}

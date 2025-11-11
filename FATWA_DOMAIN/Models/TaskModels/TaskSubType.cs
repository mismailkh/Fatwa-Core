using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TaskModels
{
    [Table("TSK_TASK_SUB_Type")]
    public class TaskSubType
    {
        [Key]
        public int SubTypeId { get; set; }
        public string NameEn { get; set; }  
        public string NameAr { get; set; }  

    }
}

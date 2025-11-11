using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkerService
{
    [Table("WS_EXECUTION_STATUS_LKP")]

    public class WSExecutionStatus
    {
        [Key]
        public int Id { get; set; }
        public string? ExecutionStatusEn { get; set; }
        public string? ExecutionStatusAr { get; set; }
        
    }
}

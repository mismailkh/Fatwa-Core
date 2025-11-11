using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkerService
{
    [Table("WS_WORKERSERVICE_EXECUTION")]
    public class WSWorkerServiceExecution
    {
        [Key]
        public Guid? Id { get; set; }
        public int? WorkerServiceId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? ExecutionStatusId { get; set; }
        public int ReAttemptCount { get; set; }
        public string? ExecutionDetails { get; set; }
    }
}

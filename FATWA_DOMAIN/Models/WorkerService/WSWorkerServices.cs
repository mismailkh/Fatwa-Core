using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkerService
{
    [Table("WS_WORKERSERVICES_LKP")]
    public class WSWorkerServices
    {
        public int Id { get; set; }
        public string WorkerServiceEn { get; set; }
        public string WorkerServiceAr { get; set; }
    }
}

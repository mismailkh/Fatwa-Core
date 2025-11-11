using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsExecutionProcedureVM
    {
        public int ExecutionProceduresId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? ProcedureType { get; set; }
        public string? ProcedureDate { get; set; }
        public string? FileStatus { get; set; }
        public string? Balance { get; set; }
    }
}

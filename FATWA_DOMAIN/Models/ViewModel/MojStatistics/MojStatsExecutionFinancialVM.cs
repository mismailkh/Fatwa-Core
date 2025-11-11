using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsExecutionFinancialVM
    {
        public int ExecutionFinancialAfterId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? ProcedureType { get; set; }
        public string? ProcedureDate { get; set; }
        public string? PayerName { get; set; }
        public string? ReceiverName { get; set; }
        public string? Amount { get; set; }
      
    }
}

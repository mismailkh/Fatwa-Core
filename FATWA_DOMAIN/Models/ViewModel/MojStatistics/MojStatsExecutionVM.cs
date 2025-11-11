using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsExecutionVM
    {
        public int ExecutionId { get; set; }
        public string? ExecutionType { get; set; }
        public string? FileAttachedDate { get; set; }
        public string? ExecutionFileNumber { get; set; }
        public string? FileStatus { get; set; }
        public string? FileOpeningDate { get; set; }
        public string? FileBalance { get; set; }
        public string? Notes { get; set; }
        public string? AttachedFileSection { get; set; }
        public string? ExecuterNumber { get; set; }
        public string? AttachedFileNumber { get; set; }
    }
}

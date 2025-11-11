using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsFilesDetailsVM
    {
        public int FilesId { get; set; }
        public string CaseNumber { get; set; }
        public string DocumentTypeName { get; set; }
        public string? DocumentDate { get; set; }
        public string NumberofPages { get; set; }
        public string FileName { get; set; }
        public string CaseAutomatedNumber { get; set; }
    }
}

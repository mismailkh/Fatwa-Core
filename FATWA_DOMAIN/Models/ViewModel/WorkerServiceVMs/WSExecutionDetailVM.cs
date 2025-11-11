using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    public class WSExecutionDetailVM : GridMetadata
    {
        [Key]
        public Guid Id { get; set; }
        public int WorkerServiceId { get; set; }
        public string WorkerServiceAr { get; set; }
        public string WorkerServiceEn { get; set; }
        public string ExecutionStatusAr { get; set; }
        public string ExecutionStatusEn { get; set; }
        public string? ExecutionDetails { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? ReattemptCount { get; set; }
    }
	public class WSExecutionAdvanceSearchVM : GridPagination
	{
		//public string? SearchKeywords { get; set; } = null;
		public DateTime? FromDate { get; set; } = null;
		public DateTime? ToDate { get; set; } = null;
		public int StatusId { get; set; } = 0;
		public int WorkerServiceId { get; set; } = 0;
	}
}

using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class ProcessLogVM : GridMetadata
    {
        [Key]
        public Guid ProcessLogId { get; set; }
        public Guid? ProcessLogTypeId { get; set; }
        public int? ProcessLogEventId { get; set; }
        public string? Process { get; set; }
        public string? Task { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Description { get; set; }
        public string? Computer { get; set; }

        public string? Message { get; set; }
        public string? UserName { get; set; }
        public string? TerminalID { get; set; }
        public string? IPDetails { get; set; }
        public string? ChannelName { get; set; }
        public int? ApplicationID { get; set; }
        public int? ModuleID { get; set; }
        //public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        //public DateTime Min = new DateTime(1950, 1, 1);
    }
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LOG_PROCESSLOG")]
    public class ProcessLog
    {
        [Key]
        public Guid ProcessLogId { get; set; }

        public Guid? ProcessLogTypeId { get; set; }

        public int ProcessLogEventId { get; set; }

        public string Process { get; set; }

        public string Task { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Description { get; set; }

        public string? Computer { get; set; }

        public string? Message { get; set; }

        public string? UserName { get; set; }

        public string? TerminalID { get; set; }

        public string? IPDetails { get; set; }

        public string? ChannelName { get; set; }

        public int? ApplicationID { get; set; }

        public int? ModuleID { get; set; }
        [NotMapped]
        public string? Token { get; set; }
    }
}

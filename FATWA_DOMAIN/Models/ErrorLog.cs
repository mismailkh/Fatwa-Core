using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LOG_ERRORLOG")]
    public class ErrorLog
    {
        [Key]
        public Guid ErrorLogId { get; set; }

        public Guid? ErrorLogTypeId { get; set; }
        public int ErrorLogEventId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime LogDate { get; set; }

        public string
            
            ? Category { get; set; }

        public string? Source { get; set; }

        public string? Type { get; set; }

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

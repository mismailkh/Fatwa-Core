using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class ErrorLogVM
    {


        [Key]
        public Guid ErrorLogId { get; set; }
        public string Subject { get; set; }
        public string Source { get; set; }
        public string Body { get; set; }
        public DateTime? LogDate { get; set; }
        public string? Computer { get; set; }
        public string? UserName { get; set; }
        public string? IPDetails { get; set; }
        public int TotalCount { get; set; } = 0;
       
    }

}




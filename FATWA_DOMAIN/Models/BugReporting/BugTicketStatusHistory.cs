using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.BugReporting
{
    [Table("BUG_TICKET_STATUS_HISTORY")]
    public class BugTicketStatusHistory
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid? ReferenceId { get; set; }
        public int? StatusId { get; set; }
        public int? EventId { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}

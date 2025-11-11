using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.BugReporting
{
    [Table("BUG_TICKET_ASSIGNMENT_HISTORY")]
    public class TicketAssignmentHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HistoryId { get; set; }
        public string? UserId { get; set; }
        public Guid? GroupId { get; set; }
        public int? AssignmentType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

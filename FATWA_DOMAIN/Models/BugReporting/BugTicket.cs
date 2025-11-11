using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FATWA_DOMAIN.Models.BugReporting
{
    [Table("BUG_TICKET")]
    public class BugTicket : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? TicketId { get; set; } 
        public string? Subject { get; set; }
        public Guid? BugId { get; set; }
        public int? ApplicationId { get; set; }
        public int? ModuleId { get; set; }
        public int? IssueTypeId { get; set; }
        public string? Description { get; set; }
        public int? PriorityId { get; set; }
        public int? SeverityId { get; set; }
        public int? StatusId { get; set; }
        public string? AssignTo { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string? ReportedBy { get; set; }
        public string? ResolvedBy { get; set; }
        public int? ShortNumber { get; set; }
        public Guid? GroupId { get; set; }
        public int? PortalId { get; set; }
        [NotMapped]
        public string? BugNumber { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();

    }
}

using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class TicketAssignmentVM
    {
        public Guid BugId { get; set; }
        public Guid? TicketId { get; set; }
        public int? IssueTypeId { get; set; }
        public int? ApplicationId { get; set; }
        public int? ModuleId { get; set; }
        public int? PriorityId { get; set; }
        public int? SeverityId { get; set; }
        public int AssignmentTypeId { get; set; } = 0;
        public string? Remarks { get; set; }
        public string? UserId { get; set; }
        public Guid? GroupId { get; set; }
        public string UserName { get;set; }
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();


    }
}

using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class DecisionStatusVM
    {
        public Guid ReferenceId { get; set; }
        public Guid? CommentId { get; set; } = Guid.NewGuid();
        public int? StatusId { get; set; }
        public string? Reason { get; set; } = "";
        public string? UserName { get; set; }
        public string? EntityCreator { get; set; }
        public string? AssignedUser { get; set; }
        public Guid? GroupId { get; set; }
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public bool FromFatwa { get; set; } = false;
        public IList<TicketListVM> selectedTicketList { get; set; } = new List<TicketListVM>();


    }
}

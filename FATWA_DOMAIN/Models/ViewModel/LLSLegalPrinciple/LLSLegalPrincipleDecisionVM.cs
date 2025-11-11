using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrincipleDecisionVM
    {
        public Guid PrincipleId { get; set; }
        public int PrincipleNumber { get; set; }
        public string? Principle_Comment { get; set; }
        public int? FlowStatusId { get; set; }
        public string CreatedBy { get; set; }
        public string ReceiverEmail { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }
}

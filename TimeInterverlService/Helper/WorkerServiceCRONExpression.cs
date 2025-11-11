using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWATIMEINTERVALSERVICES.Helper
{
    public class WorkerServiceCRONExpression
    {
        public string MOJReminderJob { get; set; }
        public string DataPopulationJob { get; set; }
        public string OpinionLetterReminderJob { get; set; }
        public string HOSReminderServiceRegionalAppealSupremeJob { get; set; }
        public string CompleteClaimStatement { get; set; }
        public string RequestForAdditionalInfoReminderServiceJob { get; set; }
        public string RequestForAdditionalInfoServiceJob { get; set; }
        public string ReviewDraftReminderServiceJob { get; set; }
        public string RabbitMQUnpublishedMessageJob { get; set; }
        public string TaskDecisionPendingReminderServiceJob { get; set; }
    }
}

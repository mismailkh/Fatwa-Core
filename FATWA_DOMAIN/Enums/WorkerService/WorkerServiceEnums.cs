using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Enums.WorkerService
{
    public class WorkerServiceEnums
    {
        public enum WorkerServiceTypeEnums
        {
            DefineThePeriodToRegionalORAppealORSupreme = 1,
            DefineThePeriodToRequestForAdditionalInformation = 2,
            DefineThePeriodForReminderRequestForAdditionInformation = 3,
            DefineThePeriodToRegisterACaseAtMOJ = 4,
            DefineThePeriodToCompleteTheClaimStatement = 5,
            DefineThePeriodToReplyOnTheOperationConsultant = 6,
            DefineThePeriodToPrepareDefenseLetter = 7,
            DataMigrationFromHistoryToMain = 8,
            RabbitMQUnpublishedMessage = 9,
            DefineThePeriodForPendingTaskDecisionReminder = 10,
        }
        public enum WorkerServiceExecutionStatusEnums
        {
            Successfull = 1,
            Failed = 2,
            Exception = 3,
            Reattempt = 4
        }
    }
}

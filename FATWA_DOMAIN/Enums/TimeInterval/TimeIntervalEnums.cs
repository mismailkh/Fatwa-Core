using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Enums.TimeInterval
{
    public class TimeIntervalEnums
    {
        public enum CmsComsReminderTypeEnums
        {
            DefineThePeriodToRegionalORAppealORSupreme = 1,
            DefineThePeriodToRequestForAdditionalInformation = 2,
            DefineThePeriodForReminderRequestForAdditionInformation = 3,
            DefineThePeriodToRegisterACaseAtMOJ = 4,
            DefineThePeriodToCompleteTheClaimStatement = 5,
            DefineThePeriodToReplyOnTheOperationConsultant = 6,
            DefineThePeriodToPrepareDefenseLetter = 7,
        }
        public enum CmsComsReminderHistoryStatusEnums
        {
            [Display(Name = "Added")]
            Added = 1,
            [Display(Name = "Updated")]
            Updated = 2,
        }
    }
}

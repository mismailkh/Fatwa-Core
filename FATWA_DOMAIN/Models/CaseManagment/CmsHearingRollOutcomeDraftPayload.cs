using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
	//< History Author = 'Hassan Abbas' Date = '2024-04-14' Version = "1.0" Branch = "master" >For Logging Hearing Roll Outcome Draft Payload</History>
	[Table("CMS_HEARING_ROLL_OUTCOME_DRAFT_PAYLOAD")]
    public class CmsHearingRollOutcomeDraftPayload : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int HearingRollId { get; set; }
        public string Payload { get; set; }
    }
}

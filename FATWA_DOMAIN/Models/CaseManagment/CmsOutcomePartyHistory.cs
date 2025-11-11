using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;


namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_OUTCOME_PARTY_HISTORY")]
    public  class CmsOutcomePartyHistory : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OutcomeId { get; set; }
        public Guid CasePartyLinkId { get; set; }
        public int ActionId { get; set; }
    }
}

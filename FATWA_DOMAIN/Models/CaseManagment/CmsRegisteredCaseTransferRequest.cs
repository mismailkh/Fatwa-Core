using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_RESGISTERED_CASE_TRANSFER_REQUEST")]
    public class CmsRegisteredCaseTransferRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OutcomeId { get; set; }
        public int ChamberFromId { get; set; }
        public int ChamberToId { get; set; }
        public int ChamberNumberFromId { get; set; }
        public int ChamberNumberToId { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    [Table("WF_WORKFLOW_TRIGGER_SECTOR_TRANSFER_OPTIONS")]
    public class WorkflowTriggerSectorTransferOptions : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TriggerOptionId { get; set; }
        public int SectorToId { get; set; }
    }
}

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
    [Table("WF_WORKFLOW_TRIGGER_SECTOR_OPTIONS")]
    public class WorkflowTriggerSectorOptions: TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TriggerOptionId { get; set; }
        public int TriggerId { get; set; }
        public int SectorFromId { get; set; }
        [NotMapped]
        public OperatingSectorType SectorFrom { get; set; } = new OperatingSectorType();
        [NotMapped]
        public IEnumerable<OperatingSectorType> SectorToIds { get; set; } = new List<OperatingSectorType>();
    }
}

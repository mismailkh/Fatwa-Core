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
    [Table("CMS_TRANSFER_HISTORY")]
    public class CmsTransferHistory : TransactionalBaseModel
    {
        
        [Key]
        public Guid TransferHistoryId { get; set; }
        public Guid ReferenceId { get; set; }
        public int StatusId { get; set; }
        public int SectorFrom { get; set; }
        public int SectorTo { get; set; }
        public string? Reason { get; set; }
        public int SubModuleId { get; set; }
        public Guid? ApprovalTrackingId { get; set; }

    }
}

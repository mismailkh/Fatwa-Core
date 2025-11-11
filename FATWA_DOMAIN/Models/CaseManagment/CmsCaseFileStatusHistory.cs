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
    //< History Author = 'Hassan Abbas' Date = '2022-10-21' Version = "1.0" Branch = "master" >Created Case File Status History Model</History>
    [Table("CMS_CASE_FILE_STATUS_HISTORY")]
    public class CmsCaseFileStatusHistory : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid FileId { get; set; }
        public int EventId { get; set; }
        public string? Remarks { get; set; }
        public int StatusId { get; set; }
        public int SectorTo { get; set; }

    }
}

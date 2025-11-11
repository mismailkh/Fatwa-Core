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
    //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Merge Cases Request</History>
    [Table("CMS_MERGE_REQUEST_SECONDARIES")]
    public class MergeRequestSecondaries
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MergeRequestId { get; set; }
        public Guid SecondaryId { get; set; }
    }
}

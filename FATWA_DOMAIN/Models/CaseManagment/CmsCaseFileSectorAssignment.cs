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
    //< History Author = 'Hassan Abbas' Date = '2022-12-22' Version = "1.0" Branch = "master" >Model for Assignment of Sectors to a Case File</History>
    [Table("CMS_CASE_FILE_SECTOR_ASSIGNMENT")]
    public class CmsCaseFileSectorAssignment : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public int SectorTypeId { get; set; }
        public bool IsReadOnly { get; set; }
    }
}

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

    //<History Author = 'Hassan Abbas' Date='2024-06-06' Version="1.0" Branch="master"> Save Government Entity and Departments Synchronization Log</History>
    [Table("CMS_GOVERNMENT_ENTITY_AND_DEPARTMENTS_SYNC_LOG")]
    public partial class GovermentEntityAndDepartmentSyncLog : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }
    }
}

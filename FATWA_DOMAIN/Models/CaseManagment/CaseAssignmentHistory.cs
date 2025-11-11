using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >
    //      -> Case File Assignment History
    //</History>
    [Table("CMS_CASE_ASSIGNMENT_HISTORY")]
    public partial class CaseAssignmentHistory : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid ReferenceId { get; set; }
        public string AssigneeId { get; set; }
        public string Remarks { get; set; }
    }
}

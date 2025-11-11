using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2023-03-04' Version="1.0" Branch="master">Moj Execution Request Assignees</History>
    [Table("CMS_MOJ_EXECUTION_REQUEST_ASSIGNEE")]
    public class MojExecutionRequestAssignee : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public string? UserId { get; set; }
    }
}

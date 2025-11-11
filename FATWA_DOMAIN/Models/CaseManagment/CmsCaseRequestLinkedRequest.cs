using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Case Request and Linked Requests</History>
    [Table("CMS_CASE_REQUEST_LINKED_REQUEST")]
    public class CmsCaseRequestLinkedRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PrimaryRequestId { get; set; }
        public Guid LinkedRequestId { get; set; }
        public string Reason { get; set; }
    }
}

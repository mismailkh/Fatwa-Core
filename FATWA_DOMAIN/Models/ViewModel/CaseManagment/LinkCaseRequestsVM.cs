using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Case Request and Linked Requests</History>
    public class LinkCaseRequestsVM : TransactionalBaseModel
    {
        public Guid PrimaryRequestId { get; set; }
        public string Reason { get; set; }
        public IList<CmsCaseRequestVM> LinkedRequests { get; set; } = new List<CmsCaseRequestVM>();
    }
}

using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class RequestMoreInfoVM : TransactionalBaseModel
    {
        //public CommunicationResponse? communicationResponse { get; set; }
        //public Communication? communication { get; set; }
        public int? RequestNumber { get; set; }
        public int? ResponseId { get; set; }
        public string? Reason { get; set; }
        public string? Other { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }

    }
}

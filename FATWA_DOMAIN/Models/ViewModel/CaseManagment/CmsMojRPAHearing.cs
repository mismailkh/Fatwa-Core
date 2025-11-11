using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public  class CmsMojRPAHearing
    {
        public int DocumentId { get; set; }
        public DateTime HearingDate { get; set; }
      public List<CanAndCaseNumber> Cases { get; set; } = new List<CanAndCaseNumber>();
    }
    public  class CanAndCaseNumber
    {
        public string CANNumber { get; set; }
        public string CaseNumber { get; set; }
        //[NotMapped]
        //public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter(); for later whenc notification module is complete
    }
         
}

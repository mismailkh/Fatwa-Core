using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public  class CmsMojRPAOutComeHearing
    {
        public int DocumentId { get; set; }
        public DateTime HearingDate { get; set; }
        public DateTime? NextHearingDate { get; set; }

      public List<CanAndCaseNumber> Cases { get; set; } = new List<CanAndCaseNumber>();
    }
   
         
}

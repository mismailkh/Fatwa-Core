using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojRollsVM
{
    public  class MOJRollsRequestDetailVM
    {
        public int? Id { get; set; }
        public DateTime? SessionDate { get; set; }
        public int? RollId_LookUp { get; set; }
        public string? RollId_LookUp_Value { get; set; }
        public int? CourtType_LookUp { get; set; }
        public string? CourtType_LookUp_Value { get; set; }
        public int? ChamberType_LookUp { get; set; }
        public string? ChamberType_LookUp_Value { get; set; }
        public int? ChamberTypeCode_LookUp { get; set; }
        public string? ChamberTypeCode_LookUp_Value { get; set; }
        public DateTime? RequestedDateTime { get; set; }
        public string? RequestedBy { get; set; }
        public string? FilePath { get; set; }
        public int? RequestStatus_LookUp { get; set; }
        public string? RequestStatus_LookUp_Value { get; set; }
        public string? ExceptionDetails { get; set; }
      
    }
}

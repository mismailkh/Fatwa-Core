using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class AdvanceSearchCmsDraftedDocumentVM
    {
       
        public string? DraftNumber { get; set; }
        public int? Document_Type { get; set; }
        public string? Type_En { get; set; }
        public string? Type_Ar { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? Start_From { get; set; }
        public DateTime? End_To { get; set; }
    }
}

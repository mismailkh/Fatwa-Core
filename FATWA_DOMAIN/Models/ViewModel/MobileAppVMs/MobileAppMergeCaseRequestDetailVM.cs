using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppMergeCaseRequestDetailVM
    {
        [DisplayName("Primary_CAN_Number")]
        public string? PrimaryCANNumber { get; set; }
        [DisplayName("Primary_Case_Number")]
        public string? PrimaryCaseNumber { get; set; }
        [DisplayName("Reason")]
        public string? Reason { get; set; }
        [DisplayName("Is_Merge_Type_Case")]
        public bool? IsMergeTypeCase { get; set; }
        [DisplayName("Created_Date")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Merged_CANs")]
        [NotMapped]
        public string MergedCANs { get; set; }
    }
}

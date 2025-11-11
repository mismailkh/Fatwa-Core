using FATWA_DOMAIN.Enums.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class LiteratureDeweyNumberPatternVM
    {  
        public Guid Id { get; set; }
        public int PatternTypId { get; set; }
        public string SeriesNumber { get; set; }
        public string DigitSequenceNumber { get; set; }
        public string SequenceResult { get; set; }
        public string SequenceFormatResult { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }
        public literatureDeweyNumberPatternEnum literatureDeweyNumberPatternType { get; set; }
        public int CheracterSeriesOrder { get; set; }
        public int DigitSequnceOrder { get; set; }
        public string SeriesSequenceNumber { get; set; }
        public string SeperatorPattern { get; set; }

    }
}
 
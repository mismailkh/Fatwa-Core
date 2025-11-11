using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models
{
    [Table("LITERATURE_DEWEY_NUMBER_PATTERN")]
    //<History Author = 'Ihsaan Abbas' Date='2024-05-06' Version="1.0" Branch="master">LITERATURE DEWEY NUMBER PATTERN</History>
    public partial class LiteratureDeweyNumberPattern : TransactionalBaseModel
    {
        [Key] 
        public Guid Id { get; set; } 
        public int PatternTypId { get; set; } 
        public string SeriesNumber { get; set; } 
        public string DigitSequenceNumber { get; set; } 
        public string? SequenceResult { get; set; } 
        public string? SequenceFormatResult { get; set; }
        public bool IsActive { get; set; }
        public int CheracterSeriesOrder { get; set; }
        public int DigitSequnceOrder { get; set; }
        public string SeriesSequenceNumber { get; set; }
        public string SeperatorPattern { get; set; }

    }

}


 
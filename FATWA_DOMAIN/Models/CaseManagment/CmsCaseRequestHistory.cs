using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Muhammad Zaeem' Date = '2022-10-21' Version = "1.0" Branch = "master" >Created Case Request History Model</History>

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_CASE_REQUEST_STATUS_HISTORY")]
    public class CmsCaseRequestHistory : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid RequestId { get; set; }
        public int EventId { get; set; }
        public string? Remarks { get; set; }
        public int StatusId { get; set; }
       
    }
}

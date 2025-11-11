using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//< History Author = 'Ijaz Ahmad' Date = '2022-11-29' Version = "1.0" Branch = "master" >Created Registered Case History Model</History>

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_REGISTERED_CASE_STATUS_HISTORY")]
    public class CmsRegisteredCaseStatusHistory
    {
        [Key]
        public Guid HistoryId { get; set; }
        public int EventId { get; set; }
        public string? Remarks { get; set; }
        public int StatusId { get; set; }
        public Guid CaseId { get; set; }
        public Guid FileId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}

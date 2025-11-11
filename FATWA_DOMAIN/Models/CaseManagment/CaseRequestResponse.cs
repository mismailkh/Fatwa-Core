using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Umer Zaman' Date = '2022-08-18' Version = "1.0" Branch = "master" >Create model class to handle case request response</History>

namespace FATWA_DOMAIN.Models.AdminModels.CaseManagment
{
    [Table("CMS_REQUEST_RESPONSE")]
    public partial class CaseRequestResponse
    {
        [Key]
        public Guid ResponseId { get; set; }
        public Guid RequestId { get; set; }
        public int ResponseTypeId { get; set; }
        public string? Others_Conditional { get; set; }
        public int ResponseReasonId { get; set; }
        public string? Comment { get; set; }
        public string? Justification { get; set; }
        public DateTime? Due_Date { get; set; }
        public string? Reminder { get; set; }
        public bool Is_Urgent { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}

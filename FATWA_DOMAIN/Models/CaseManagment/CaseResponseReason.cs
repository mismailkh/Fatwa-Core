using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Umer Zaman' Date = '2022-08-18' Version = "1.0" Branch = "master" >Create model class to handle case response type</History>

namespace FATWA_DOMAIN.Models.AdminModels.CaseManagment
{
    [Table("CMS_RESPONSE_REASON")]
    public partial class CaseResponseReason
    {
        [Key]
        public int ResponseReasonId { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public string? Other_Reason { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}

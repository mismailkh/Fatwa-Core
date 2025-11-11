using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels
{
    [Table("SR_FINAL_APPROVAL_ACTIVITIES")]
    public class ServiceRequestFinalApprovalActivities : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FinalApprovalId { get; set; }
        public int ApprovalSequenceNo { get; set; }
        public string RoleId { get; set; }
        public int SectorTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int VersionId { get; set; }
        public bool IsActive { get; set; }
    }
}

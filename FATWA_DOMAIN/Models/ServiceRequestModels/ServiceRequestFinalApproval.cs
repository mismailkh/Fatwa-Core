using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels
{
    [Table("SR_FINAL_APPROVAL")]
    public class ServiceRequestFinalApproval : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ServiceRequestTypeId { get; set; }
        public int NoOfApprovals { get; set; }
        public bool IsActive { get; set; }
    }
}
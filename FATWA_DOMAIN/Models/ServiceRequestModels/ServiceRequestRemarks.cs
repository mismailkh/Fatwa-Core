using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels
{
    public class ServiceRequestRemarks : TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string RemarkContent { get; set; } = null!;
        public int TypeId { get; set; }
        [NotMapped]
        public Guid ServiceRequestId { get; set; }
        [NotMapped]
        public int? ServiceRequestStatus { get; set; }
    }
}

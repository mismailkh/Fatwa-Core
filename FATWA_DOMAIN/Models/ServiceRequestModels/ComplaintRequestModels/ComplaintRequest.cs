using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels.ComplaintRequestModels
{
    public class ComplaintRequest : TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? TypeId { get; set; }
        public string OtherType { get; set; }
        public int? PriorityId { get; set; }
        public DateTime? IssueStartDate { get; set; }

        [NotMapped]
        public ServiceRequest ServiceRequest { get; set; } = null!;
        [NotMapped]
        public string ReceiverId { get; set; }
        [NotMapped]
        public Guid? UserId { get; set; }
        [NotMapped]
        public bool IsSubmit { get; set; }
    }
}

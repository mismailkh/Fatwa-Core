using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.InventoryManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels
{
    public class ServiceRequest : TransactionalBaseModel
    {
        public Guid ServiceRequestId { get; set; }
        public int ServiceRequestTypeId { get; set; }
        public int ServiceRequestStatusId { get; set; }
        public string ServiceRequestNumber { get; set; }
        public Guid StoreId { get; set; }
        public string? Description { get; set; }
        public string? SpecialInstruction { get; set; }
        [NotMapped]
        public List<InvServiceRequestItem> RequestItems { get; set; } = new List<InvServiceRequestItem>();
        [NotMapped]
        public Guid userId { get; set; }
        [NotMapped]
        public Guid ReceiverId { get; set; }
        [NotMapped]
        public Guid ItemId { get; set; }
        [NotMapped]
        public int RequestQuantity { get; set; }
    }
}

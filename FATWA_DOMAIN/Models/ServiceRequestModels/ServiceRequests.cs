using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ServiceRequestModels
{
    public class ServiceRequests : TransactionalBaseModel
    {
        public Guid ServiceRequestId { get; set; }
        public int ServiceRequestTypeId { get; set; }
        public int ServiceRequestStatusId { get; set; }
        public string ServiceRequestNumber { get; set; }
        public string Description { get; set; }
        public string SpecialInstruction { get; set; }
        public Guid StoreId { get; set; } = default;
    }
}

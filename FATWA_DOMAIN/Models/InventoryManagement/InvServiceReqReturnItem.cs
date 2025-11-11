using FATWA_DOMAIN.Models.ServiceRequestModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.InventoryManagement
{
    [Table("INV_SERVICE_REQUEST_RETURN_ITEM")]
    public class InvServiceReqReturnItem
    {
        [Key]
        public Guid ServiceRequestItemId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid ItemId { get; set; }
        //public int TotalIssuedQunatity { get; set; }
        public int ReturnQuantity { get; set; }
        public string ReturnReason { get; set; }
        public int ServiceReqReturnItemStatusId { get; set; }
        [NotMapped]
        public ServiceRequest? ServiceRequest { get; set; } = new ServiceRequest();
        //[NotMapped]
        //public UserTask? UserTask { get; set; } = new UserTask();

        [NotMapped]
        public ServiceReqTaskNotificationDto? InvReturnReqDto { get; set; } = new ServiceReqTaskNotificationDto();
    }

    /// <summary>
    /// ServiceReqTaskNotificationDto is used to transfer task and notification data required for service requests
    /// </summary>
    public class ServiceReqTaskNotificationDto
    {
        public string? TaskName { get; set; }
        public string? Url { get; set; }
        public string? AssignToId { get; set; }
        public string? AssignById { get; set; }
        public int? RequestStatusId { get; set; }
        public string? CreatedBy { get; set; }
        public Guid? ServiceReqId { get; set; }
        public string? ServiceReqNumber { get; set; }
        public string? ActionName { get; set; }

    }
}

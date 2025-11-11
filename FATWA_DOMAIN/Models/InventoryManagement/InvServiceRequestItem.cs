using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.InventoryManagement
{
    //<History Author = 'Ihsaan Abbas' Date = '2023-05-03' Version = "1.0" Branch = "master">Created Base Model for Inventory service request</History>

    [Table("INV_SERVICE_REQUEST_ITEM")]
    public class InvServiceRequestItem
    {
        [Key]
        public Guid ServiceRequestItemId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public int ServiceRequestItemStatusId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public int? PendingQuantity { get; set; }
        public int? ApprovedQuantity { get; set; }
        public int? IssuedQuantity { get; set; }

    }
}

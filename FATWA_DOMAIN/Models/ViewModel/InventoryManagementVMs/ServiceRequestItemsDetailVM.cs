using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    public class ServiceRequestItemsDetailVM
    {

        [Key]
        public Guid ServiceRequestItemId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string? ItemCategoryEn { get; set; }
        public string? ItemCategoryAr { get; set; }
        public Guid? ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public int? Quantity { get; set; }
        public int? PendingQuantity { get; set; }
        public int? IssuedQuantity { get; set; }
        public int? ApprovedQuantity { get; set; }
        public int ItemStatusId { get; set; }
        public string? RequestItemStatusEn { get; set; }
        public string? RequestItemStatusAr { get; set; }
    }
}

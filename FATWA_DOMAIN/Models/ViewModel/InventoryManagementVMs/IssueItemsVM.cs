using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    public class IssueItemsVM
    {
        [Key]
        public Guid IssueitemId { get; set; }
        public Guid ReferenceId { get; set; }
        public Guid ReferenceItemId { get; set; }
        public Guid StoreId { get; set; }
        public int IssuedQuantity { get; set; }
        public string ItemNameEn { get; set; }
        public string ItemNameAr { get; set; }
        public string StoreIssued { get; set; } = string.Empty;
        public string IssuedByEn { get; set; } = string.Empty;
        public string IssuedByAr { get; set; } = string.Empty;
        public string IssuedById { get; set; } = string.Empty;
        public int ItemStatusId { get; set; }
        public DateTime IssuedDate { get; set; }
        [NotMapped]
        public string userName { get; set; }
    }
    // Vm for Return Item
    public class ReturnItemVM
    {
        public string ServiceRequestId { get; set; }
        public string ItemId { get; set; }
        public string ItemNameEn { get; set; }
        public string ItemNameAr { get; set; }
        public string ItemCode { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCategoryNameEn { get; set; }
        public string ItemCategoryNameAr { get; set; }
        public int ReturnQuantity { get; set; }
        public string ReturnReason { get; set; }
        public Guid StoreId { get; set; }

    }
}

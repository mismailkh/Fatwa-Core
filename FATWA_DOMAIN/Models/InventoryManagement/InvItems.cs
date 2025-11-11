using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.InventoryManagement
{
    [Table("INV_ITEMS")]
    public class InvItems : TransactionalBaseModel
    {
        [Key]
        public Guid ItemId { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCode { get; set; }
        public string ItemNameEn { get; set; }
        public string ItemNameAr { get; set; }
        public string? Description { get; set; }
        public bool IsViewable { get; set; }
        public int? QuantityPerUnit { get; set; }
        public int? Unit { get; set; }
        public int TotalQuantity { get; set; }
        public Guid? VendorId { get; set; }
    }
}

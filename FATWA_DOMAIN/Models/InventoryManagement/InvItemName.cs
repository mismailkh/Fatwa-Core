using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.InventoryManagement
{
    [Table("INV_ITEM_NAME")]
     public class InvItemName : TransactionalBaseModel
    {
        [Key]
        public Guid ItemNameId { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCode { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        //new varriable
        public string? Description { get; set; }
        public bool IsViewable { get; set; }
        public int? QuantityPerUnit { get; set; }
        public int? Unit { get; set; }
        public int Quantity { get; set; }
        public Guid? VendorId { get; set; }

    }
}

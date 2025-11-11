using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.InventoryManagement
{
    [Table("INV_ITEM_CATEGORY_LKP")]
    public class InvItemCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemCategoryId { get; set; }
        public int SectorTypeId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}

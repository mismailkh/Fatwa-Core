namespace FATWA_DOMAIN.Models.InventoryManagement
{
    public class InvItemsVM : InvItems
    {
        public int ItemCategoryId { get; set; }
        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }
    }
}

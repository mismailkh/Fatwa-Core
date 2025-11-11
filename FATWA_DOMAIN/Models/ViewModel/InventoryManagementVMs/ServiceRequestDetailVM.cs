namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    public class ServiceRequestDetailVM
    {
        public Guid RequestId { get; set; }
        public string ItemCode { get; set; }
        public int? Quantity { get; set; }
        public string? ItemStatusEn { get; set; }
        public string? ItemStatusAr { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public string? ItemCategoryEn { get; set; }
        public string? ItemCategoryAr { get; set; }
        public string? Description { get; set; }
        public string? SpecialInstruction { get; set; }

    }
}

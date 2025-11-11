namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    public class RequestDetailVM
    {

        public Guid RequestId { get; set; }
        public Guid? RequestItemId { get; set; }
        public int? RequestNumber { get; set; }
        public int? ItemCategoryId { get; set; }
        public string? ItemCategoryEn { get; set; }
        public string? ItemCategoryAr { get; set; }
         public Guid? ItemNameId { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public string? ItemCode { get; set; }
        public int? Quantity { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? Requestor { get; set; }
        public int? PendingQuantity { get; set; }
        public int? ApproveQuantity { get; set; }
        public string? RequestStatusEn { get; set; }
        public string? RequestStatusAr { get; set; }
        public int? ItemStatusId { get; set; }
        public string? ItemStatusEn { get; set; }
        public string? ItemStatusAr { get; set; }
        public string? RequestoreNameEn { get; set; }
        public string? RequestoreNameAr { get; set; }
        public int? DepartmentId { get; set; }
        public string? DeptNameEn { get; set; }
        public string? DeptNameAr { get; set; }
    }
}

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    public class ItemRequestAdvanceSearchVM
    {
        public int? ItemCategoryId { get; set; }
        public int? ItemNameId { get; set; }
        public int? ItemCodeId { get; set; }
        public int? RequestStatusId { get; set; }
        public DateTime? RequestedDateFrom { get; set; }
        public DateTime? RequestedDateTo { get; set; }
		public Guid? StoreId { get; set; }
		public string? UserName { get; set; }

	}
}

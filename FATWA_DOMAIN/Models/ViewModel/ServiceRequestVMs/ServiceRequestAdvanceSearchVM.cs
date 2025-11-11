namespace FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs
{
	public class ServiceRequestAdvanceSearchVM
	{
		public int? ItemCategoryId { get; set; }
		public int? ItemStatusId { get; set; }
		public int? ItemNameId { get; set; }
		public int? ItemCodeId { get; set; }
		public int? RequestStatusId { get; set; }
		public DateTime? RequestedDateFrom { get; set; }
		public DateTime? RequestedDateTo { get; set; }
		public Guid? StoreId { get; set; } = default(Guid);
		public string? UserName { get; set; }
		public Guid? UserId { get; set; } = default(Guid);
		public bool IsComplaintRequest { get; set; }
		public bool IsInventory { get; set; }
	}
}

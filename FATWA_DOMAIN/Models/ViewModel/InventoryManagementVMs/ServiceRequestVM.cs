using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
	public class ServiceRequestVM
    {
		[Key]
		public Guid ServiceRequestId { get; set; }
		public Guid StoreId { get; set; }
		public string? CustodianId { get; set; }
		public Guid? StoreKeeperId { get; set; }
		public string? ServiceRequestNumber { get; set; }
        public string? RequestorId { get; set; }
        public string? RequestorNameEn { get; set; }
		public string? RequestorNameAr { get; set; }
        public string? SectorFromEn { get; set; }
        public string? SectorFromAr { get; set; }
        public string? SectorToEn { get; set; }
        public string? SectorToAr { get; set; }
        public DateTime? RequestCreatedDate { get; set; }
		public int ServiceRequestStatusId { get; set; }
		public string? RequestStatusEn { get; set; }
		public string? RequestStatusAr { get; set; }
		public string? Description { get; set; }
		public string? SpecialInstruction { get; set; }
		public int? RequestTypeId { get; set; }
		public string? RequestTypeEn { get; set; }
		public string? RequestTypeAr { get; set; }
        public string? ReturnItemReason { get; set; }

    }
}

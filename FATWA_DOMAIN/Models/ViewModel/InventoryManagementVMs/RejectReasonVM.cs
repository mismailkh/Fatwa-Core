using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    public class RejectReasonVM
    {
        [Key]
        public Guid Id { get; set; }
        public string? ServiceRequestNumber { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public string? Reason { get; set; }
        public string RejectedByEn { get; set; }
        public string RejectedByAr { get; set; }
        public DateTime? RejectDateTime { get; set; }
    }
}

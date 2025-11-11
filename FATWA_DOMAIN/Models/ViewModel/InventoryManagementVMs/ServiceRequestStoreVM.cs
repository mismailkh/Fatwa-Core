
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    [NotMapped]
    public class ServiceRequestStoreVM
    {
        [Key]
        public Guid StoreId { get; set; }
        public Guid StoreInchargeId { get; set; }
    }
}

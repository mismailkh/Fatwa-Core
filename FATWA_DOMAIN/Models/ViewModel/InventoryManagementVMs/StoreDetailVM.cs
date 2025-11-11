using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
    public class StoreDetailVM
    {
        public string? StoreName { get; set; }
        public string? StoreLocation { get; set; }
        public int? Quantity { get; set; }

        
    }
}

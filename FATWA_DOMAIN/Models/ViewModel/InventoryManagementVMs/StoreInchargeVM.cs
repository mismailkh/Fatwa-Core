using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs
{
	public class StoreInchargeVM
	{
        [Key]
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }
    }
}

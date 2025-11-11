using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class SaveDeviceTokenVM
    {
        [Required(ErrorMessage = "Device Token Must Be Required")]
        public string DeviceToken { get; set; }
        [Required(ErrorMessage = "User ID Must Be Required")]
        public string UserId { get; set; }
        public int ChannelId { get; set; }
        public string VersionCode { get; set; }
        public string CultureValue { get; set; }
    }
}

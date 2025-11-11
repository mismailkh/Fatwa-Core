using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    [Table("UMS_USER_DEVICE_TOKEN")]
    public class UmsUserDeviceToken: TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string DeviceToken { get; set; }
        public string UserId { get; set; }
        public int ChannelId { get; set; }
    }
}

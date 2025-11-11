using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_USER_ACTIVITY")]
    public class UserActivity
    {
        [Key]
        public Guid ActivityId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string IPAddress { get; set; }
        public string ComputerName { get; set; }
        public bool? IsLoggedIn { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public bool? IsLoggedOut { get; set; }
        public DateTime? LogoutDateTime { get; set; }
        public string? ExceptionMessage { get; set; }
    }
}

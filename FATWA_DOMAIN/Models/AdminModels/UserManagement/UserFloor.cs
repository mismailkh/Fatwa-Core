using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_USER_FLOOR_LKP")]
    public class UserFloor
    {
        [Key]
        public int Id { get; set; } 
        public string NameEn { get; set; }    
        public string NameAr { get; set; }    
    }
}

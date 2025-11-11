using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class UserBasicDetailVM
    {
        [Key]
        public string UserId {  get; set; } 
        public string UserNameEn {  get; set; } 
        public string UserNameAr {  get; set; } 


    }
}

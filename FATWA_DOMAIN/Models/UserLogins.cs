using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models
{
    public class UserLogins
    {
        [Required]
        public string LoginProvider { get; set; }

        [Required]
        public string ProviderKey { get; set; }

        [ForeignKey("FK_User")]
        [Required]
        public Guid UserId { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.PACIVM
{
    public class PACIRequestDataGridVM
    {
        [Key]
        public Guid Id { get; set; }
        public string? CivilID { get; set; }
        public string? Names { get; set; }
    }
}

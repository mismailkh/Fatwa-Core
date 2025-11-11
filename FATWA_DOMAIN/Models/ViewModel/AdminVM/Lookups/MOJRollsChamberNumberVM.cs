using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class MOJRollsChamberNumberVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChamberId { get; set; }
    }
    public class MobileAppChamberNumberVM
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ChamberId { get; set; }
    }
}

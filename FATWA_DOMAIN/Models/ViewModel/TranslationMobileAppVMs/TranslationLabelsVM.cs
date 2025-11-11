using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TranslationMobileAppVMs
{
    public class TranslationLabelsVM
    {
       public Dictionary<string, string> TranslationLabels { get; set; }
       public bool ForceAppUpdate { get; set; }
    }
}

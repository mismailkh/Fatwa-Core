using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-29' Version = "1.0" Branch = "master" > VM for transfering Draft related entity data</History>
    public class DraftEntityDataVM
    {
        public int? DraftEntityType { get; set; }
        public string? Payload { get; set; }
    }
}

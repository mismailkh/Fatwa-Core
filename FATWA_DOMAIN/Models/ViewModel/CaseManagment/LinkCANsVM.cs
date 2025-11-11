using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{

    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link CANs</History>
    public class LinkCANsVM : TransactionalBaseModel
    {
        public string PrimaryCAN { get; set; }
        public string Reason { get; set; }
        public IList<string> LinkedCANs { get; set; } = new List<string>();
    }
}

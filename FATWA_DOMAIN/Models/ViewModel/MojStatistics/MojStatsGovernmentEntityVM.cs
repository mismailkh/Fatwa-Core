using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    //<History Author = 'Ijaz Ahmad' Date='2022-04-22' Version="1.0" Branch="master"> Add government entity</History>
    public partial class MojStatsGovernmentEntityVM
    {
        public int Id { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
    }
}

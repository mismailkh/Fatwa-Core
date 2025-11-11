using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ArchivedCases
{
    public class AdvanceSearchArchivedCaseListVM
    {
        [Key]
        public Guid Id { get; set; }
        public string CaseAutomatedNumber { get; set; }
        public string CaseNumber { get; set; }
        public int? ChamberTypeId { get; set; }
        public int? ChamberNumberId { get; set; }
    }
}

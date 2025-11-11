using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ArchivedCases
{
    public class ArchivedCaseListVM : GridMetadata
    {
        [Key]
        public Guid Id { get; set; }
        public string CaseNumber { get; set; }
        public string CaseAutomatedNumber { get; set; }
        public string CourtNameEn { get; set; }
        public string CourtNameAr { get; set; }
        public string ChamberNameEn { get; set; }
        public string ChamberNameAr { get; set; }
        public string Number { get; set; }
        public int? ChamberTypeId { get; set; }
        public int? ChamberNumberId { get; set; }
    }

    public class ArchivedCaseAdvanceSearchVM : GridPagination
    {
        public string CaseNumber { get; set; }
        public string CaseAutomatedNumber { get; set; }
        public int ChamberTypeId { get; set; }
        public int ChamberNumberId { get; set; }
        public int CourtTypeId { get; set; }
        public string PlaintiffName { get; set; }
        public string DefendantName { get; set; }

    }
}

using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" > Case File Assignee History VM </History>
    public class CmsCaseAssigneesHistoryVM : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid ReferenceId { get; set; }
        public string? AssigneeId { get; set; }
        public string? Remarks { get; set; }
        public string? AssigneeNameEn { get; set; }
        public string? AssigneeNameAr { get; set; }
        public string? AssignorNameEn { get; set; }
        public string? AssignorNameAr { get; set; }
        public bool? IsPrimary { get; set; }
    }
}

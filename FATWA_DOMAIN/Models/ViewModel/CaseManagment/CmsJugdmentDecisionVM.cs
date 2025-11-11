using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsJugdmentDecisionVM : TransactionalBaseModel
    {

        [Key]
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public int? DecisionTypeId { get; set; }
        public string? DecisionTypeEn { get; set; }
        public string? DecisionTypeAr { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public bool? isExecutionLost { get; set; }
        public int StatusId { get; set; }
        public int TotalCount { get; set; }
    }
}

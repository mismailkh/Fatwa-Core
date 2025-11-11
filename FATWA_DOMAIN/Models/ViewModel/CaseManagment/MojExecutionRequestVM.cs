using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2023-03-28' Version="1.0" Branch="master"> Moj Execution Request View Model</History>
    public class MojExecutionRequestVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CaseId { get; set; }
        public Guid? FileId { get; set; }
        public int? StatusId { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public string? FileNumber { get; set; }
        public string? Remarks { get; set; }
        public DateTime? AssignedDate { get; set; }
        public int TotalCount { get; set; }

	}
}

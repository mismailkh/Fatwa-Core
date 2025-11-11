using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> RegisteredCaseFileVM </History>
    public class RegisteredCaseFileVM : TransactionalBaseModel
    {
        [Key]
        //public Guid CaseId { get; set; }
        public Guid FileId { get; set; }
        public Guid RequestId { get; set; }
        public int StatusId { get; set; }
        public string FileNumber { get; set; }
        public string? Subject { get; set; }
        public string? FileName { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? RequestNumber { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? GovernmentEntityNameEn { get; set; }
        public string? GovernmentEntityNameAr { get; set; }
        public string? PriorityNameEn { get; set; }
        public string? PriorityNameAr { get; set; }
        public string? OperatingSectorNameEn { get; set; }
        public string? OperatingSectorNameAr { get; set; }
        public bool? IsAssignedBack { get; set; }
        public string? LastActionEn { get; set; }
        public string? LastActionAr { get; set; }
        public string? CaseFileNumberFormat { get; set; }
        // public string? CANNumber { get; set; }
        // public string? CourtNameEn { get; set; }
        // public string? CourtNameAr { get; set; }
        // public string? ChamberNumber { get; set; }
        // public string? ChamberNameEn { get; set; }
        // public string? ChamberNameAr { get; set; }
        ////public string? Assigne { get; set; }
        // public DateTime? CaseDate { get; set; }
        [NotMapped]
        public IList<CmsRegisteredCaseFileDetailVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseFileDetailVM>();
        public int TotalCount { get; set; } = 0;
    }
}

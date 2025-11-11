using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment
{
    public class CmsCaseFileVM : TransactionalBaseModel 
    {
        [Key]
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
        public string? RequestTypeNameEn { get; set; }
        public string? RequestTypeNameAr { get; set; }
        public bool? IsAssignedBack { get; set; }
        public string? LastActionEn { get; set; }
        public string? LastActionAr { get; set; }
        public string? CaseFileNumberFormat { get; set; }
        public int TotalCount { get; set; } = 0;
    }
    public class CmsCaseFileDmsVM : TransactionalBaseModel
    {
        [Key]
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
        public string? RequestTypeNameEn { get; set; }
        public string? RequestTypeNameAr { get; set; }
    }
}

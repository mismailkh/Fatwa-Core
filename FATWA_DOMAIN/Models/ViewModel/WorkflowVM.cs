using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class WorkflowVM
    {
        [Key]
        public int? WorkflowId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? StatusId { get; set; }
        public string? Status_En { get; set; }
        public string? Status_Ar { get; set; }
        public int? WorkflowTriggerId { get; set; }
        public int? ModuleTriggerId { get; set; }
        public string? TriggerName { get; set; }
        public int? ModuleId { get; set; }
        public string? ModuleNameAr { get; set; }
        public string? ModuleNameEn { get; set; }
        public string? SubModuleNameAr { get; set; }
        public string? SubModuleNameEn { get; set; }
        public string? Version { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class WorkflowListVM : GridMetadata
    {
        [Key]
        public int? WorkflowId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? StatusId { get; set; }
        public string? Status_En { get; set; }
        public string? Status_Ar { get; set; }
        public int? WorkflowTriggerId { get; set; }
        public int? ModuleTriggerId { get; set; }
        public string? TriggerName { get; set; }
        public int? ModuleId { get; set; }
        public string? ModuleNameAr { get; set; }
        public string? ModuleNameEn { get; set; }
        public string? SubModuleNameAr { get; set; }
        public string? SubModuleNameEn { get; set; }
        public string? Version { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DocumentTypeNameEn { get; set; }
        public string? DocumentTypeNameAr { get; set; }
    }
    public class WorkflowAdvanceSearchVM : GridPagination
    {
        public int? ModuleId { get; set; }
        public string? SearchKeywords { get; set; } = null;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public int? StatusId { get; set; } = null;
    }
}

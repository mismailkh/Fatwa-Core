using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
    public partial class TaskVM : GridMetadata
    {
        public string? TaskNo { get; set; }
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ModuleNameEn { get; set; }
        public string ModuleNameAr { get; set; }
        public int TaskStatusId { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameAr { get; set; }
        public string AssignedByEn { get; set; }
        public string AssignedByAr { get; set; }
        public string ModifiedBy { get; set; }
        public string Url { get; set; }
        public string? ReferenceNumber { get; set; }
        public int SectorId { get; set; }
        public bool IsSystemGenerated { get; set; }
        public int? EntityId { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public string Subject { get; set; }
        public int? RequestGovtEntity { get; set; }
        public string? RequestGovtEntityNameEn { get; set; }
        public string? RequestGovtEntityNameAr { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? SubModuleId { get; set; }
        public Guid? ReferenceId { get; set; }
        public DateTime? HearingDate { get; set;}
        public bool IsImportant { get; set;}
        public bool IsCaseUpdated { get; set;}
        public string? CANNumber { get; set;}
    }


    public partial class TaskListMobileAppVM
    {
        public string? TaskNo { get; set; }
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ModuleNameEn { get; set; }
        public string ModuleNameAr { get; set; }
        public int TaskStatusId { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameAr { get; set; }
        public string AssignedByEn { get; set; }
        public string AssignedByAr { get; set; }
        public string ModifiedBy { get; set; }
        public string Url { get; set; }
        public string? ReferenceNumber { get; set; }
        public int SectorId { get; set; }
        public bool IsSystemGenerated { get; set; }
        public int? EntityId { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public string Subject { get; set; }
        public int? RequestGovtEntity { get; set; }
        public string? RequestGovtEntityNameEn { get; set; }
        public string? RequestGovtEntityNameAr { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? SubModuleId { get; set; }
        public Guid? ReferenceId { get; set; }
        public DateTime? HearingDate { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCaseUpdated { get; set; }
        public string? CANNumber { get; set; }
    }

    public class TaskCountVM
    {
        [Key]
        public int Count { get; set; }
    }
}

using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{

    //<History Author = 'Nabeel ur Rehman' Date='2023-08-24' Version="1.0" Branch="master"> Add document entity</History>
    public partial class WSCmsDraftTemplateIntervalVM
    {
        [Key]
        public Guid VersionId { get; set; }
        public Guid? DraftedTemplateId { get; set; }
        public string? ReviewerUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public int? SectorTypeId { get; set; }
        public string ReferenceNumber { get; set; }
        public string? ReferenceName { get; set; }
        public Guid ReferenceId { get; set; }
        public Guid? TaskId { get; set; }
        public string? DocumentType { get; set; }
        public string? Entity { get; set; }
        public string? SenderName { get; set; }
    }
}

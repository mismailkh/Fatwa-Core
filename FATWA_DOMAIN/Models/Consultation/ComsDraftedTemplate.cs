using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.Consultation
{
    //< History Author = 'Muhammad Zaeem' Date = '2023-2-1' Version = "1.0" Branch = "master" > Drafted Consultation Draft Model</History>
    [Table("COMS_DRAFTED_TEMPLATE")]
    public class ComsDraftedTemplate : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DraftNumber { get; set; }
        public double VersionNumber { get; set; }
        public Guid ReferenceId { get; set; }
        public int TemplateId { get; set; }
        public string ReviewerUserId { get; set; }
        public string ReviewerRoleId { get; set; }
        public string Description { get; set; }
        public int AttachmentTypeId { get; set; }
        public int StatusId { get; set; }
        public int SectorTypeId { get; set; }
        public int? DraftEntityType { get; set; }
        public string? Payload { get; set; }
        public string? Content { get; set; }
        [NotMapped]
        public IList<CaseTemplateSectionsVM> TemplateSections { get; set; } = new List<CaseTemplateSectionsVM>();
        [NotMapped]
        public int? WorkflowActivityId { get; set; }
        [NotMapped]
        public WorkflowInstanceStatusEnum? WorkflowInstanceStatusId { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public string? UploadFrom { get; set; }
        [NotMapped]
        public string? Project { get; set; }
        [NotMapped]
        public byte[]? FileData { get; set; }
        [NotMapped]
        public string? Reason { get; set; }
        [NotMapped]
        public UpdateEntityStatusVM UpdateEntity { get; set; }
        [NotMapped]
        public bool IsEdit { get; set; }

    }
}

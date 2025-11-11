using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsDraftedDocumentDetailVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? DraftNumber { get; set; }
        public decimal VersionNumber { get; set; }
        public Guid ReferenceId { get; set; }
        public int? TemplateId { get; set; }
        public int? ModuleId { get; set; }
        public string? ReviewerUserId { get; set; }
        public string? ReviewerRoleId { get; set; }
		//[NotMapped] 
		public string? TypeEn { get; set; }
		//[NotMapped] 
		public string? TypeAr { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? Description { get; set; }
        public string? TemplateNameEn { get; set; }
        public string? TemplateNameAr { get; set; }
        public string? Content { get; set; }
        public int? AttachmentTypeId { get; set; }
        public int? StatusId { get; set; } 
        public Guid? RequestId { get; set; }
        public Guid? FileId { get; set; }
        public string? FileName { get; set; }
        public string? Subject { get; set; }
        public Guid? CaseId { get; set; }
        public string? CaseNumber { get; set; }
        public Guid? ConsultationRequestId { get; set; }
        public Guid? ConsultationFiletId { get; set; }

        [NotMapped]
        public IList<CaseTemplateSectionsVM> TemplateSections { get; set; } = new List<CaseTemplateSectionsVM>();
        [NotMapped]
        public int? WorkflowActivityId { get; set; }
        [NotMapped]
        public WorkflowInstanceStatusEnum? WorkflowInstanceStatusId { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public Guid? VersionId { get; set; }
    }

    public class CmsDraftedDocumentParentEntityDetailVM
    {
        [Key]
        public Guid Id { get; set; }
        public int SubmoduleId { get; set; }
        public dynamic Payload { get; set; }
    }
}
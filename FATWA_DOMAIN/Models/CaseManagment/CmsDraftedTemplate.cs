using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
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

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-20' Version = "1.0" Branch = "master" > Drafted Case Draft Model</History>
    [Table("CMS_DRAFTED_TEMPLATE")]
    public class CmsDraftedTemplate: TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DraftNumber { get; set; }
        public Guid ReferenceId { get; set; }
        public int TemplateId { get; set; }
        public string Description { get; set; }
        public int AttachmentTypeId { get; set; }
        public int ModuleId { get; set; }
        public int SectorTypeId { get; set; }
        public int? DraftEntityType { get; set; }
        public string? Payload { get; set; }
        [NotMapped]
        public IList<CaseTemplateSectionsVM> TemplateSections { get; set; } = new List<CaseTemplateSectionsVM>();
        [NotMapped]
        public CmsDraftedTemplateVersions DraftedTemplateVersion { get; set; }
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
        public bool IsSubmit { get; set; } = false;
        [NotMapped]
        public static int LatestDraftNumber { get; set; }
        [NotMapped]
        public bool IsLawyerTask { get; set; }
        [NotMapped]
        public string? Opinion { get; set; }
        [NotMapped]
        public string? LawyerId { get; set; }
        [NotMapped]
        public bool? IsG2GSend { get; set; }
        [NotMapped]
        public bool IsDraftToDocumentConversion { get; set; }
        [NotMapped]
        public bool IsDraftSigned { get; set; }
        [NotMapped]
        public int subModuleId { get; set; }
        [NotMapped]
        public string userName { get; set; }
        [NotMapped]
        public bool? IsEndofFlow { get; set; }
        [NotMapped]
        public Guid? CommunicationId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public Guid? UserId { get; set; }
        [NotMapped]
        public bool? IsMultipleViceHos { get; set; }
    }
}

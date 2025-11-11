using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_DRAFTED_TEMPLATE_VERSIONS")]
    public class CmsDraftedTemplateVersions : TransactionalBaseModel
    {
        [Key]
        public Guid VersionId { get; set; }
        public Guid DraftedTemplateId { get; set; }
        public decimal VersionNumber { get; set; }
        public string ReviewerUserId { get; set; }
        public string ReviewerRoleId { get; set; }
        public int StatusId { get; set; }
        public string? Content { get; set; }
        [NotMapped]
        public Guid OldVersionId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public int DraftActionId { get; set; }
        [NotMapped]
        public Guid? FileId { get; set; }
        [NotMapped]
        public int OldStatusId { get; set; }
    }
}

using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using FATWA_DOMAIN.Models.Notifications.ViewModel;

namespace FATWA_DOMAIN.Models.CaseManagment
{

    //< History Author = 'Hassan Abbas' Date = '2022-12-22' Version = "1.0" Branch = "master" >Tracking Approvals for Case Management -> Transfer, Assignment, Copy etc</History>
    [Table("CMS_APPROVAL_TRACKING")]
    public class CmsApprovalTracking : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int SectorTo { get; set; }
        public int SectorFrom { get; set; }
        public int StatusId { get; set; }
        public int ProcessTypeId { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public int TransferCaseType { get; set; }
        [NotMapped]
        public int TransferStatusId { get; set; }
        [NotMapped]
        public int? WorkflowActivityId { get; set; }
        [NotMapped]
        public WorkflowInstanceStatusEnum? WorkflowInstanceStatusId { get; set; }
        [NotMapped]
        public dynamic Item { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public string ReviewerUserId { get; set; }
        [NotMapped]
        public string ReviewerRoleId { get; set; }
        [NotMapped]
        public string AssignedBy { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public int? ComsSectorTypeId { get; set; }
        [NotMapped]
        public int? TriggerId { get; set; }
        [NotMapped]
        public bool IsEndofFlow { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public bool IsConfidential { get; set; }
        [NotMapped]
        public int SubModuleId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }
}

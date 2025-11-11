using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.LLSLegalPrinciple
{
    //<History Author = 'Umer Zaman' Date = '2024-04-16' Version = "1.0" Branch = "master">Create new legal principle model to manage new requirements</History>

    [Table("LLS_LEGAL_PRINCIPLE")]
    public class LLSLegalPrincipleSystem : TransactionalBaseModel
    {
        [Key]
        public Guid PrincipleId { get; set; }
		public int PrincipleNumber { get; set; }
        public int FlowStatus { get; set; }
        public int OriginalSourceDocumentId { get; set; }
		public string? UserId { get; set; }
		public string? RoleId { get; set; }
		public string? Principle_Comment { get; set; } = string.Empty;
		[NotMapped]
        public List<LLSLegalPrincipleContent> lLSLegalPrinciplesContentList { get; set; } = new List<LLSLegalPrincipleContent>();
        [NotMapped]
		public List<LLSLegalPrincipleContentCategory> lLSLegalPrincipleCategoryList { get; set; } = new List<LLSLegalPrincipleContentCategory>();
        [NotMapped]
		public List<LLSLegalPrincipleContentSourceDocumentReference> linkContents { get; set; } = new List<LLSLegalPrincipleContentSourceDocumentReference>();
		[NotMapped]
		public int? WorkflowActivityId { get; set; }
		[NotMapped]
		public WorkflowInstanceStatusEnum? WorkflowInstanceStatusId { get; set; }
		[NotMapped]
		public string? Token { get; set; }
		[NotMapped]
		public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
		[NotMapped]
        public List<int> SourceDocumentDeletedReferenceId { get; set; } = new List<int>();
		[NotMapped]
		public string SenderEmail { get; set; }
    }
}

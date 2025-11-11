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

namespace FATWA_DOMAIN.Models.Dms
{
	//< History Author = 'Hassan Abbas' Date = '2023-06-20' Version = "1.0" Branch = "master" > Added Document Model</History>
	[Table("DMS_ADDED_DOCUMENT")]
	public class DmsAddedDocument : TransactionalBaseModel
	{
		[Key]
		public Guid Id { get; set; }
		public int ModuleId { get; set; }
		public int AttachmentTypeId { get; set; }
		public int ClassificationId { get; set; }
		public int DocumentNumber { get; set; }
		public string DocumentName { get; set; }
		public string? Description { get; set; }
		public bool IsConfidential { get; set; }
        [NotMapped]
        public string? Payload { get; set; }
        [NotMapped]
        public int SectorTypeId { get; set; }
        [NotMapped]
		public DmsAddedDocumentVersion DocumentVersion { get; set; }
        [NotMapped]
        public int? WorkflowActivityId { get; set; }
        [NotMapped]
        public WorkflowInstanceStatusEnum? WorkflowInstanceStatusId { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public bool IsLawyerTask { get; set; }
        [NotMapped]
        public bool IsSubmit { get; set; } = false;
        [NotMapped]
        public bool IsEndofFlow { get; set; } = false;
        [NotMapped]
        public string? UserLoginState { get; set; }
        [NotMapped]
        public int SubModuleId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}

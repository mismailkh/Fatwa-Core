using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Dms
{
	//< History Author = 'Hassan Abbas' Date = '2023-06-20' Version = "1.0" Branch = "master" > Added Document Version Model</History>
	[Table("DMS_ADDED_DOCUMENT_VERSION")]
	public class DmsAddedDocumentVersion : TransactionalBaseModel
	{
		[Key]
		public Guid Id { get; set; }
		public Guid AddedDocumentId { get; set; }
		public decimal VersionNo { get; set; }
		public int StatusId { get; set; }
		public string? FileName { get; set; }
		public string? StoragePath { get; set; }
		public string? DocType { get; set; }
		public string? Content { get; set; }
		public string? ReviewerUserId { get; set; }
		public string? ReviewerRoleId { get; set; }
		[NotMapped]
		public string Reason { get; set; }
		[NotMapped]
		public bool IsPreviousVersionApproved { get; set; }
		[NotMapped]
		public Guid PreviousVersionId { get; set; }

    }
}

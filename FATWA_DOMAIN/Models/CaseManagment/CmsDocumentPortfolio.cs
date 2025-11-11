using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2023-03-20' Version = "1.0" Branch = "master" > Add Document Portfolio</History>
    [Table("CMS_DOCUMENT_PORTFOLIO")]
    public partial class CmsDocumentPortfolio : TransactionalBaseModel
	{
		[Key]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid HearingId { get; set; }
        public int AttachmentTypeId { get; set; }
        public string StoragePath { get; set; }
        [NotMapped]
        public Guid ReferenceId { get; set; }
        [NotMapped]
		public IList<TempAttachementVM> SelectedDocuments { get; set; } = new List<TempAttachementVM>();
		[NotMapped]
		public string? UploadFrom { get; set; }
		[NotMapped]
		public string? Project { get; set; }
		[NotMapped]
		public byte[]? FileData { get; set; }
		[NotMapped]
		public string? DocumentPath { get; set; }
	}
}

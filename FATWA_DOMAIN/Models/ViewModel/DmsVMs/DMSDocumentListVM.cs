using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class DMSDocumentListVM : GridMetadata
    {
        public int? UploadedDocumentId { get; set; }
        public Guid? AddedDocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? FileName { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? AttachmentTypeEn { get; set; }
        public string? AttachmentTypeAr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDocunentFavourite { get; set; }
        public bool IsDocumentAddedStatus { get; set; }
        public bool? IsConfidential { get; set; }
        public decimal? VersionNumber { get; set; }
        public Guid? VersionId { get; set; }
        public int? StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? StoragePath { get; set; }
        public string? Content { get; set; }
        public string? SharedByEn { get; set; }
        public string? SharedByAr { get; set; }

        [NotMapped]
        public string UserId { get; set; }
    }


	public class DmsAddedDocumentReasonVM
	{
		[Key]
		public Guid Id { get; set; }
		public string? Reason { get; set; }
		public string? UserNameEn { get; set; }
		public string? UserNameAr { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}

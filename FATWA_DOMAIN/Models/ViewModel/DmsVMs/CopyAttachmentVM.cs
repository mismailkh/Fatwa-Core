using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class CopyAttachmentVM
    {
        public Guid SourceId { get; set; }
        public Guid DestinationId { get; set; }
        public string CreatedBy { get; set; } 
    }

    public class CopySelectedAttachmentsVM
	{
		public List<TempAttachementVM> SelectedDocuments { get; set; }
		public Guid DestinationId { get; set; }
		public string CreatedBy { get; set; }
        [NotMapped]
        public string? Token { get; set; }
    }

    public class MoveAttachmentAddedDocumentVM
	{
		public Guid ReferenceId { get; set; }
		public Guid AddedDocumentVersionId { get; set; }
	}


    public class CopyLegalPrincipleSourceAttachmentsVM
    {
        public List<int> SelectedDocumentIds { get; set; }
        public List<int> KayselectedDocumentsIds { get; set; }
        public Guid DestinationId { get; set; }
        public string CreatedBy { get; set; }
    }
    public class CopyLegalLegislationSourceAttachmentsVM
    {
        public List<int> SelectedDocumentIds { get; set; }
        public List<int> KayselectedDocumentsIds { get; set; }
        public Guid DestinationId { get; set; }
        public string CreatedBy { get; set; }
    }
    public class LinkDocumentsVM
    {
        public Guid SourceDocumentVersionId { get; set;}
        public List<Guid> DestinationIds { get; set; } = new List<Guid>();
        public List<int> LiteratureIds { get; set; } = new List<int>();
        public bool IsLiterature { get; set; }
        public string CreatedBy { get; set; }
        public string UploadFrom { get; set; }
        [NotMapped]
        public IList<LLSLegalPrinciplesRelationVM> PrincipleContentsDetails { get; set; } = new List<LLSLegalPrinciplesRelationVM>();
        [NotMapped]
        public int ModuleId { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class FileUploadVM
    {
        public List<Guid> RequestIds { get; set; }
        public string CreatedBy { get; set; }
        public string FilePath { get; set; }
        public List<int>? DeletedAttachementIds { get; set; }
        public int LiteratureId { get; set; }
        public bool isCommunication { get; set; }
        [NotMapped]
        public bool IsRequestForMeetingSaveAsDraft { get; set; } = false;
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public List<int>? LiteratureIds { get; set; } 


    }
}

using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class DMSKayPublicationDocumentListVM : GridMetadata
    {
        public int Id { get; set; }
        public string EditionNumber { get; set; }
        public string EditionType { get; set; }
        public bool IsFullEdition { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string PublicationDateHijri { get; set; }
        public string FileTitle { get; set; }
        public string DocumentTitle { get; set; }
        public string StoragePath { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
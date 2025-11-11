namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class MojDocumentVM
    {
        public Guid Id { get; set; }
        public string CANNumber { get; set; }
        public string CaseNumber { get; set; }
        public string? AttachmentTypeEn { get; set; }
        public string? AttachmentTypeAr { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string? StoragePath { get; set; }
        public string? Name_En { get; set; }
        public string? Name_Ar { get; set; }
        public string FileName { get; set; }
    }
}

using DSPExternalSigningService;

namespace DMS_API.Model
{
    public class LocalSigningRequest
    {
        public string UserId { get; set; }
        public string DocumentId { get; set; }
        public byte[] SignedDocumentBytes { get; set; }
        public string SessionToken { get; set; }
        public SignatureDetails SignatureDetails { get; set; }
    }
}

namespace DS_SOAPAPI.BusinessLogic
{
    public class LocalSigningRequest
    {
        public string UserId { get; set; }
        public string DocumentId { get; set; }
        public byte[] SignedDocumentBytes { get; set; }
        public string SessionToken { get; set; }
        public SignatureDetails SignatureDetails { get; set; }
    }

    public class SignatureDetails
    {
        public DateTime SignTime { get; set; }
        public string SignautreLevel { get; set; }
        public string SubjectName { get; set; }
        public string IssuerName { get; set; }
        public byte[] SigningCertificate { get; set; }
    }
}

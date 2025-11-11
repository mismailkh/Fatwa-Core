namespace FATWA_DOMAIN.Models.DigitalSignature
{
    public class SignatureVerificationResponse
    {
        public int CertificationLevel { get; set; }
        public string CertificationLevelDetails { get; set; }
        public string OverallMessage { get; set; }
        public int OverallResult { get; set; }
        public Signature[] Signatures { get; set; }= new Signature[0];
    }

    public class Signature
    {
        public string Header { get; set; }
        public bool IsLTVEnabled { get; set; }
        public string Message { get; set; }
        public int Result { get; set; }
        public bool RevisionIsValid { get; set; }
        public string RevisionValidityMessage { get; set; }
        public SignatureDetails SignatureDetails { get; set; }
        public string Email { get; set; }
        public string Field { get; set; }
        public string HashAlgorithm { get; set; }
        public string IssuerName { get; set; }
        public string Level { get; set; }
        public int Revision { get; set; }
        public DateTime SignTime { get; set; }
        public string SignTimeMessage { get; set; }
        public string SignatureAlgorithm { get; set; }
        public string SigningCertificate { get; set; }
        public string SubjectName { get; set; }
        public string SubjectNameCN { get; set; }
        public bool SignerCertificateIsValid { get; set; }
        public string SignerCertificateValidityMessage { get; set; }
        public int SubResult { get; set; }
        public string SubResultDetails { get; set; }
        public TimestampDetails TimestampDetails { get; set; }
        public string? VerificatoinMethod { get; set; }
    }

    public class SignatureDetails
    {
        public ChainDetails[] ChainDetails { get; set; }
    }

    public class ChainDetails
    {
        public string Certificate { get; set; }
        public string Message { get; set; }
        public int Result { get; set; }
        public int SubResult { get; set; }
        public string SubResultDetails { get; set; }
        public string Subject { get; set; }
        public string? VerificatoinMethod { get; set; }
    }

    public class TimestampDetails
    {
        public bool IsValidTimeStamp { get; set; }
        public string IsValidTimeStampMessage { get; set; }
        public string TimeStampAuthority { get; set; }
        public string? TimeStampAuthorityDetils { get; set; }
        public DateTime TimeStampDate { get; set; }
    }

}

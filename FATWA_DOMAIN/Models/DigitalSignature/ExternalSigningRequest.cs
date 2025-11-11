
namespace FATWA_DOMAIN.Models.DigitalSignature
{
    public class ExternalSigningRequest
    {
        public string SignatureProfileName { get; set; }
        public string UserId { get; set; }
        public string CivilId { get; set; }
        public int DocumentId{ get; set; }
        public string DataTitle { get; set; }
        public string DataDescription { get; set; }
        public int PageNumber { get; set; }
        public string SelectedReason { get; set; }
        public string fileName { get; set; }
        public string CreatedBy { get; set; }
    }
}

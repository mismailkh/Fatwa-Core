using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public class CommunicationTarasolRPAPayload
    {
        [Required]
        public Guid Guid { get; set; }
        [Required]
        public string SenderSiteName { get; set; }  
        [Required]
        public string RecieverSiteName { get; set; }  
        [Required]
        public string SenderDepartmentSiteName { get; set; }  
        [Required]
        public string RecieverDepartmentSiteName { get; set; }  
        [Required]
        public string Subject { get; set; }  
        public string? Comments { get; set; }  
        [Required]
        public string LetterNumber { get; set; }  
        [Required]
        public string SendingDate { get; set; }  
        [Required]
        public string RecievingDate { get; set; }
        [Required]
        public string CorrespondenceId { get; set; }

    }

    public class CommunicationDocumentPayload
    {
        [Required]
        public string CorrespondenceId { get; set; }
        public string FileName { get; set; }
        public Guid CommunicationGuid { get; set; }
        public byte[] DocByteArray { get; set; }

    }
    public class TarasolRPAPayloadWithDocuments
    {
        public List<CommunicationTarasolRPAPayloadOutput> CommunicationPayload { get; set; } = new List<CommunicationTarasolRPAPayloadOutput>();
        public List<CommunicationDocumentPayloadOutput> DocumentPayload { get; set; } = new List<CommunicationDocumentPayloadOutput>();
    }

    public class CommunicationTarasolRPAPayloadOutput
    {
        public Guid Guid { get; set; }
        public string? SenderSiteName { get; set; }
        public string? RecieverSiteName { get; set; }
        public string? SenderDepartmentSiteName { get; set; }
        public string? RecieverDepartmentSiteName { get; set; }
        public string? Subject { get; set; }
        public string? Comments { get; set; }
        public string? LetterNumber { get; set; }
        public string? SendingDate { get; set; }
        public string? RecievingDate { get; set; }
        public string? RecieveFromDeadQueue { get; set; }
    }

    public class CommunicationDocumentPayloadOutput
    {
        public string? FileName { get; set; }
        public Guid? CommunicationGuid { get; set; }
        public string? RecieveFromDeadQueue { get; set; }
    }

    public class CommunicationTarasolMarkedCorrespondencesVM
    {
        [Key]
        public Guid CommunicationId { get; set; }
        public string? SenderGEandBr { get; set; } 
        public string? Subject { get; set; }
        public string? BookNo { get; set; }
        public DateTime? SendDateTime { get; set; }
        public DateTime? ReceiveDateTime { get; set; }
        public string? DepartmentName { get; set; }
        public bool? ReturnCorrespondence { get; set; }
        public bool? Archive { get; set; }
        public string? ReturnReason { get; set; }
        public string CorrespondenceId { get; set; }
    }
}

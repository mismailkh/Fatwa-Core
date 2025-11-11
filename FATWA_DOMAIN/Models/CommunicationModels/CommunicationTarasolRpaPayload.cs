using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.CommunicationModels
{//< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >For Logging RPA Payload</History>
    [Table("COMM_Communication_Tarasol_Rpa_Payload")]
    public class CommunicationTarasolRpaPayload : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Payload { get; set; }
        public string CorrespondenceId { get; set; }
        public bool isSucceeded { get; set; }
        public bool CommunicationPayload { get; set; }
        public bool CommunicationDocumentPayload { get; set; }
    }
}



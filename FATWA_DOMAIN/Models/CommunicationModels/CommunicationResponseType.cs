using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_RESPONSE_TYPE")]

    public partial class CommunicationResponseType
    {
        [Key]
        public int CommunicationResponseTypeId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}

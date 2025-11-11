using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Ammaar Naveed' Date = '2024-12-12' Version = "1.0" Branch = "master" >Created Payload Model for FATWA Archving System</History>

namespace FATWA_DOMAIN.Models.ArchivedCasesModels
{
    [Table("ARC_CASE_DATA_PAYLOAD")]
    public class ArchivedCaseDataPayload : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? Payload { get; set; }
    }
}

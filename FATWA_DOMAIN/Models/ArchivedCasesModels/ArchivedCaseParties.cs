using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Ammaar Naveed' Date = '2024-12-10' Version = "1.0" Branch = "master" >Created Parties Model for FATWA Archving System</History>

namespace FATWA_DOMAIN.Models.ArchivedCasesModels
{
    [Table("ARC_PARTIES")]
    public class ArchivedCaseParties : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string PartyName { get; set; }
        public Guid? CANId { get; set; }
        public string? MOJPartyId { get; set; }
        public int PartyRoleId { get; set; }
    }
}

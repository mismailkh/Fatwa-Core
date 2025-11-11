using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Ammaar Naveed' Date = '2024-12-10' Version = "1.0" Branch = "master" >Created Cases Model for FATWA Archving System</History>

namespace FATWA_DOMAIN.Models.ArchivedCasesModels
{
    [Table("ARC_CASES")]
    public class ArchivedCasesModel : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CANId { get; set; }
        public string CaseNumber { get; set; }
        public DateTime CaseDate { get; set; }
        public int CourtTypeId { get; set; }
        public int ChamberTypeId { get; set; }
        public int ChamberNumberId { get; set; }
    }
}

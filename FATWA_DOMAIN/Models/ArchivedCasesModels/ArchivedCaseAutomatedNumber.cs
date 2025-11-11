using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Ammaar Naveed' Date = '2024-12-10' Version = "1.0" Branch = "master" >Created CAN Model for FATWA Archving System</History>

namespace FATWA_DOMAIN.Models.ArchivedCasesModels
{
    [Table("ARC_CASE_AUTOMATED_NUMBER")]
    public class ArchivedCaseAutomatedNumber : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string CaseAutomatedNumber { get; set; }
        public DateTime? MigrationDateTime { get; set; }
    }
}

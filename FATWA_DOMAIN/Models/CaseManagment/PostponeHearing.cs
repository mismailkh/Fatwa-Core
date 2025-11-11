using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master">Postpone Hearing for a Case</History>
    [Table("CMS_POSTPONE_HEARING")]
    public class PostponeHearing : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HearingId { get; set; }
        public string Reason { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }

    }
}

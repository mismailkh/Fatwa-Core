using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_CHAMBER_NUMBER_HEARING")]
    public partial class ChamberNumberHearing : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int HearingDayId { get; set; }
        public int CourtId { get; set; }
        public int ChamberId { get; set; }
		public int ChamberNumberId { get; set; }
        public int HearingsRollDays { get; set; } = 3;
        public int JudgmentsRollDays { get; set; } = 3;


		[NotMapped]
        public IEnumerable<int> SelectedChamberNumbers { get; set; } = new List<int>();
        [NotMapped]
        public IEnumerable<int> SelectedHearingDayIds { get; set; } = new List<int>();
        [NotMapped]
        public IEnumerable<int>? SelectedChambers{ get; set; } = new List<int>();
        [NotMapped]
        public IEnumerable<int>? SelectedCourts { get; set; } = new List<int>();


	}
}

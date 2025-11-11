using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Muhammad Zaeem' Date = '2022-11-28' Version = "1.0" Branch = "master" >Created Case Request Model</History>

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT")]
    public class CmsAssignLawyerToCourt : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string LawyerId { get; set; }
        public int ChamberNumberId { get; set; }
        public int CourtId { get; set; }
        public int ChamberId { get; set; }

        [NotMapped]
        public IEnumerable<int> SelectedChamberNumbers { get; set; } = new List<int>();
        [NotMapped]
        public IEnumerable<string> SelectedUsers { get; set; } = new List<string>();
        [NotMapped]
        public IEnumerable<int> SelectedCourts { get; set; } = new List<int>();
        [NotMapped]
        public IEnumerable<int> SelectedChamber { get; set; } = new List<int>();
    }
}
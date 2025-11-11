using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_TRAININGS_ATTENTED")]
    public class UserTrainingAttended
    {
        [Key]
        public Guid TrainingId { get; set; }
        public string? UserId { get; set; }
        public string? TrainingName { get; set; }
        public string? TrainingCenterName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TrainingLocation { get; set; }
        [DisplayName("Percentage/Grade")]
        public string? Percentage_Grade { get; set; }
        public string? Comments { get; set; }
        public UserPersonalInformation UserPersonalInformation { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_EDUCATIONAL_LEVEL")]
    public class UserEducationalInformation
    {
        [Key]
        public Guid EducationId { get; set; }
        public string? UserId { get; set; }
        public string? DegreeName { get; set; } //No longer in use (updated on 22/01/2024 by Ammaar Naveed)
        public DateTime? GraduationYear { get; set; }
        [DisplayName("Percentage/Grade")]
        public string? Percentage_Grade { get; set; }
        public string? Comments { get; set; }
        public string? MajoringName { get; set; }
        public string? UniversityName { get; set; }
        public string? UniversityCountry { get; set; }
        public string? UniversityCity { get; set; }
        public string? UniversityAddress { get; set; }
        public UserPersonalInformation UserPersonalInformation { get; set; }

    }
}

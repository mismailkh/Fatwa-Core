using Microsoft.AspNetCore.Builder;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;


namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_WORK_EXPERIENCE")]
    public class UserWorkExperience
    {
        [Key]
        public Guid ExperienceId { get; set; }
        public string? UserId { get; set; }
        [DisplayName("CompanyName/GEName")]
        public string? CompanyName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? JobTitle { get; set; }
        public string? JobExperience { get; set; }
        [NotMapped]
        public string? Experience
        {
            get
            {
                if (StartDate == null || EndDate == null)
                {
                    return string.Empty;
                }
                else
                {
                    TimeSpan differenc = (TimeSpan)(EndDate - StartDate);
                    return differenc.Humanize(precision: 2, maxUnit: Humanizer.Localisation.TimeUnit.Year);
                }
            }
        }
        public UserPersonalInformation UserPersonalInformation { get; set; }
    }
}

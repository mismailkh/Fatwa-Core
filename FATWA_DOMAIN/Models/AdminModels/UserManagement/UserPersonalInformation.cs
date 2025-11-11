using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_PERSONAL_INFORMATION")]
    public class UserPersonalInformation
    {
        [Key]
        public string UserId { get; set; }
        public string? FirstName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? SecondName_En { get; set; }
        public string? SecondName_Ar { get; set; }
        public string? LastName_En { get; set; }
        public string? LastName_Ar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int NationalityId { get; set; }
        public int? GenderId { get; set; }
        public string? CivilId { get; set; }
        public string? PassportNumber { get; set; }
        public Nationality? Nationality { get; set; }
        public Gender? Gender { get; set; }
        public User? User { get; set; }
        public ICollection<UserAdress>? UserAdresses { get; set; }
        public ICollection<UserContactInformation>? UserContacts { get; set; }
        public ICollection<UserEducationalInformation>? UserEducationalInformation { get; set; }
        public ICollection<UserWorkExperience>? UserWorkExperiences { get; set; }
        public ICollection<UserTrainingAttended>? UserTrainingAttendeds { get; set; }
    }
    public class UserPersonalInformationVM
    {
        public string UserId { get; set; }
        public string? FirstName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? SecondName_En { get; set; }
        public string? SecondName_Ar { get; set; }
        public string? LastName_En { get; set; }
        public string? LastName_Ar { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AlternatePhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int NationalityId { get; set; }
        public int GenderId { get; set; }
        public string? CivilId { get; set; }
        public string? PassportNumber { get; set; }
        [NotMapped]
        public int EligibleCount { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }

}

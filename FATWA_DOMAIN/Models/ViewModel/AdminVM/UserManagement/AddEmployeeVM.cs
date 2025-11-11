using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class AddEmployeeVM : TransactionalBaseModel
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public bool IsEmailModified { get; set; } = false;
        public string? UserName { get; set; } = string.Empty;
        public int GroupTypeId { get; set; }
        public int? GradeTypeId { get; set; }
        public Guid GroupId { get; set; }
        public string RoleId { get; set; }
        public string? ActiveDirectoryUserName { get; set; }
        public Group? Group { get; set; }
        public Role? Role { get; set; }
        public List<UserAdress>? UserAdresses { get; set; }
        public UserPersonalInformation userPersonalInformation { get; set; }
        public UserEmploymentInformation? UserEmploymentInformation { get; set; }
        public List<UserEducationalInformation>? UserEducationalInformation { get; set; }
        public List<UserWorkExperience>? UserWorkExperiences { get; set; }
        public List<UserTrainingAttended>? userTrainingAttendeds { get; set; }
        public List<UserContactInformation>? UserContactInformationList { get; set; }
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        public bool IsUserHasAnyTask { get; set; }
        public string CreatedByName_En { get; set; }
        public string CreatedByName_Ar { get; set; }
        public string ModifiedByName_En { get; set; }
        public string ModifiedByName_Ar { get; set; }
        public string? DelegatedEmployeeName_En { get; set; }
        public string? DelegatedEmployeeName_Ar { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameters { get; set; } = new NotificationParameter();
    }
    public class EmployeeSuccessVM
    {
        public string EmployeeId { get; set; }
        public string userId { get; set; }
        public string? UserName { get; set; }
        public int EmployeeTypeId { get; set; }
    }
    public class UserRoleAssignmentVM
    {
        public List<string>? SelectedUsersIdsList { get; set; } = new List<string>();
        public List<string>? ExistingRoleIds { get; set; } = new List<string>();
        public string? SelectedUserId { get; set; }
        public string? SelectedRoleId { get; set; }
        public string CreatedBy { get; set; }
    }
}

using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.WorkflowModels;
using System.Security.Claims;

namespace FATWA_DOMAIN.Interfaces
{
    public interface IAccount
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> SingleSignOn(string SamAccountName);
        Task<AuthenticationResult> LoginAsync(string email, string password, string culture);
        Task<string> ResetUserPasswordAsync(ResetPasswordVM resetPasswordVM);

        ClaimsPrincipal GetClaimsFromTokenAsync(string token);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);

        Task<List<UserVM>> UserListBySearchTerm(string? searchTerm);
        Task<List<string>> GetUsersByRoleId(string? RoleId);
        Task<List<Group>> UserGroupListBySearchTerm(string? searchTerm);
        //<History Author = 'Nadia Gull' Date='2022-11-3' Version="1.0" Branch="master"> returns the list of User Borrow Literatures</History>
        Task<List<UserBorrowLiteratureVM>> UserBorrowLiteratures(string? userId);
        Task<UserPersonalInformationVM> UserDetailByUserId(string userId);
        Task<UserEmploymentInformation> GetUserEmploymentInfoByUserId(string userId);
        Task<Group> UserGroupListByUserGroupId(Guid userGroupId);

        Task<string> GetSecurityStampByEmail(string emailId);
        Task<bool> CheckEmailExists(string email);
        User GetUserByUserEmail(string email);
        Task<List<UserRole>> GetUserRolesByUserName(string userName);
        Task<List<TranslationSucessResponse>> GetAllTranslations();
        Task<List<UserPasswordHistory>> GetEmployeePasswordHistory(string userId);
        Task RecordLoginExceptions(string loginError, string innerException);
        Task<List<UserActivity>> GetEmployeeActivities(string userId);
        Task<dynamic> SaveDeviceRegisteredFCMToken(SaveDeviceTokenVM saveDeviceTokenVM);
        Task<List<UmsUserDeviceToken>> GetDeviceRegisteredFCMToken(string userId);
        Task<dynamic> DeleteRegisteredDeviceToken(string token, string userId, int channelId, string cultureValue);
        #region Dropdownlist Functions
        Task<List<Module>> GetModules();
        Task<List<UserTaskStatus>> GetTasktSatuses();
        Task<string> UserIdByUserEmail(string email);
        Task<string> UserSectorTypeIdByUserEmail(string email);
        Task<int> GetSectorIdByEmail(string email);
        Task<List<UserVM>> GetUsersList();
        Task<List<FatwaAttendeeVM>> GetAttendeeUser();
        Task<List<ClaimVM>> GetAllClaims(string userId);
        Task<List<FatwaAttendeeVM>> GetCommitteeMembersByReferenceId(Guid ReferenceId);

        #endregion
        //Task<List<FatwaAttendeeVM>> GetCommitteeMembersByReferenceId(Guid ReferenceId);
        Task<List<UserVM>> GetLegalCulturalCenterUsersList(string? searchTerm);
        Task<List<UserVM>> GetLibraryUsersList(string? searchTerm);


    }
}

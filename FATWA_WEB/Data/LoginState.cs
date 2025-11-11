using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel;
using System.Runtime.InteropServices;

namespace FATWA_WEB.Data
{
    //<History Author = 'Hassan Abbas' Date='2022-02-28' Version="1.0" Branch="master"> Manage User's Login state, Tokens and Access Claims</History>
    //<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master"> Added IsStateChecked on Service update to Scoped</History>
    public class LoginState
    {
        public bool IsLoggedIn { get; set; }
        public bool IsSSOAthenticated { get; set; }
        public bool IsStateChecked { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public UserDetailVM UserDetail { get; set; }
        public List<UserRole> UserRoles { get; set; }

        public string RefreshToken { get; set; }
        public string ProfilePicUrl { get; set; }
        public int PageSize { get; set; } = 10;
        public int ModuleId { get; set; }

        public IEnumerable<ClaimSucessResponse> ClaimList { get; set; }

        public event Action OnChange;

        //<History Author = 'Hassan Abbas' Date='2022-02-28' Version="1.0" Branch="master"> Set Login, cLaims, tokens on login</History>
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="2.0" Branch="master"> Added Username for general purposes i.e. createdBy,modifiedBy etc and Optional form parameter for invoking/waiving NotifyStateChanged Event for specific forms</History>
        public void SetLoginAndClaims(string username, UserDetailVM userDetail, bool login, bool stateCheck, IEnumerable<ClaimSucessResponse> claims, string token, string refreshToken, [Optional] string form, string profilePicUrl)
        {
            Username = username;
            IsLoggedIn = login;
            IsStateChecked = stateCheck;
            ClaimList = claims;
            Token = token;
            UserDetail = userDetail;
            RefreshToken = refreshToken;
            ProfilePicUrl = profilePicUrl;
            if (form != "LoginForm")
            {
                NotifyStateChanged();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-02-28' Version="1.0" Branch="master"> Set Login, cLaims, tokens on logout</History>
        public void SetLogout(bool login)
        {
            Username = String.Empty;
            IsLoggedIn = login;
            IsStateChecked = false;
            ClaimList = null;
            Token = string.Empty;
            RefreshToken = string.Empty;
            ProfilePicUrl = string.Empty;
            NotifyStateChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-02-28' Version="1.0" Branch="master"> Invoke State change</History>
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

    }
}

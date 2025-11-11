using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace FATWA_DOMAIN.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string? ProfilePicUrl { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public IdentityUser User { get; set; }
        public UserDetailVM? UserDetail { get; set; }
        public IEnumerable<ClaimSucessResponse> ClaimsResultList { get; set; }
        public IEnumerable<TranslationSucessResponse> TranslationsList { get; set; }
    }
    public class ClaimSucessResponse
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class TranslationSucessResponse
    {
        public string TranslationKey { get; set; }
        public string Value_Ar { get; set; }
        public string Value_En { get; set; }
    }

}

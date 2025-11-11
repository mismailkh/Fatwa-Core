using System.ComponentModel.DataAnnotations;

namespace FATWA_GENERAL.Helper
{
    public class Request
    {
        public class IdentityRequest
        {
            //<History Author = 'Hassan Abbas' Date='2022-03-04' Version="1.0" Branch="master">Error messages translated to arabic</History>
            [Required(ErrorMessage = "يجب إدخال اسم المستخدم")]
			//[EmailAddress(ErrorMessage = "البريد الإلكتروني المدخل غير صحيح")]
			public string UserName { get; set; }

			//<History Author = 'Hassan Abbas' Date='2022-03-04' Version="1.0" Branch="master">Error messages translated to arabic</History>
			[Required(ErrorMessage = "يجب إدخال كلمة المرور")]
            [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تحتوي على 6 أحرف على الأقل")]
            public string Password { get; set; }
            public int ChannelId { get; set; }
            public string VersionCode { get; set; }
            public string CultureValue { get; set; }
        }
        public class RefreshTokenRequest
        {

            public string Token { get; set; }
            public string RefreshToken { get; set; }
        }

    }
}

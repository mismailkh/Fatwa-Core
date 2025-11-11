

//<History Author = 'Ammaar Naveed' Date='09-10-2024' Version="1.0" Branch="master"> Handled messages and created GetMessage function to handle for both web and mobile</History>
//<History Author = 'Ammaar Naveed' Date='12-09-2024' Version="1.0" Branch="master"> Handling static messages for login page accross Fatwa Web, DMS Web, & Fatwa Admin</History>
namespace FATWA_DOMAIN.Common
{
    public static class LoginPageMessages
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Messages = new()
    {
        { "EN", new Dictionary<string, string>
            {
                { "UserDoesNotExist", "User does not exist" },
                { "UserEmailOrPasswordIncorrect", "Please enter the correct username and password" },
                { "UserIsDeactivated", "User account is deactivated" },
                { "UserIsInactiveOrDeleted", "User is either Inactive or Deleted" },
                { "UseUsernameInsteadOfEmail", "Use username instead of email" }
            }
        },
        { "AR", new Dictionary<string, string>
            {
                { "UserDoesNotExist", "المستخدم غير موجود" },
                { "UserEmailOrPasswordIncorrect", "يرجى إدخال إسم المستخدم وكلمة السر الصحيحة" },
                { "UserIsDeactivated", "تم تعطيل حساب المستخدم" },
                { "UserIsInactiveOrDeleted", "المستخدم إما غير نشط أو محذوف" },
                { "UseUsernameInsteadOfEmail", "اسم المستخدم بدلا من البريد الإلكتروني" }
            }
        }
    };

        public static string GetMessage(string key, string culture)
        {           

            // Default to "AR" (Arabic) if culture is null or empty
            var normalizedCulture = string.IsNullOrEmpty(culture) ? "AR" : culture.ToUpper();

            return Messages.TryGetValue(normalizedCulture, out var localizedMessages) && localizedMessages.ContainsKey(key)
                ? localizedMessages[key]
                : Messages["AR"].ContainsKey(key) ? Messages["AR"][key] : "Error Occured";
        }
    }

}

using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
    // History Author = "Attique ur Rehman" Date = "10-jan-24" Version = "1.0" Branch = "master" > Reset Password Funtionality</History>-->
namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class ResetPassword : ComponentBase
    {
        [Parameter]
        public bool IsTemporaryPassword { get; set; }
        [Parameter]
        public string UserOldPassword { get; set; }
        User user = new User();
        bool busy;
        bool password = true;
        bool confirmpassword = true;
        string specialCharacter = "!@#$%^&*<>/()[]{}-_=|~`";
        public string REGEX_PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        //public string PasswordRequiredMessage = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "Required Valid Password ( يجب ادخال كلمة مرور صحيحة )" : " يجب ادخال كلمة مرور صحيحة  ( Required Valid Password )";
        public string PasswordRequiredMessage = "Require_Valid_Password";
        public string PasswordValidationMessage = string.Empty;
     
        public async Task CancelChanges()
        {
            dialogService.Close();

        }
        private void ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                PasswordValidationMessage = string.Empty;
            }
            if (!IsTemporaryPassword)
            {
                if (password.Length >= 8 && !string.IsNullOrEmpty(password))
                {
                    if (!password.Any(char.IsUpper)
                        || !password.Any(char.IsLower)
                        || !password.Any(char.IsDigit)
                        || !password.Any(c => !char.IsLetterOrDigit(c)))
                    {
                        PasswordValidationMessage = PasswordRequiredMessage;
                    }
                    else
                    {
                        PasswordValidationMessage = string.Empty;
                    }
                }
            }
        }
        public async Task SubmitChnages()
        {

            if (user.Password == user.ConfirmPassword && string.IsNullOrEmpty(PasswordValidationMessage))
            {
                if (user.Password == UserOldPassword)
                {
                    PasswordValidationMessage = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "New password should be different from the previous one" : "يجب أن تكون كلمة المرور الجديدة مختلفة عن السابقة";
                    return;
                }
                else
                {
                    busy = true;
                    await Task.Delay(1500);
                    busy = false;
                    dialogService.Close(user);
                }
            }
        }
        private void TogglePassword(MouseEventArgs args)
        {
            password = !password;
        }
        private void ToggleConfirmPassword(MouseEventArgs args)
        {
            confirmpassword = !confirmpassword;
        }

    }
}

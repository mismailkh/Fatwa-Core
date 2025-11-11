using Microsoft.AspNetCore.Identity;

namespace FATWA_GENERAL.Helper
{
    //<History Author = 'Hassan Abbas' Date='2022-03-04' Version="1.0" Branch="master"> Idenitity Valdiation Messages are localized in respective languages here</History>
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format("البريد الإلكتروني الخاص بك موجود بالفعل", userName)
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = string.Format("البريد الإلكتروني الخاص بك موجود بالفعل", email)
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "الرجاء التأكد من عنوان البريد الإلكتروني"
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = "الرجاء التأكد من عنوان البريد الإلكتروني"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "كلمة المرور يجب أن تحتوي رقم واحد على الأقل"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "كلمة المرور يجب أن تحتوي حرف صغير واحد"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "كلمة المرور يجب أن تحتوي رمز خاص واحد"
            };
        }

        //public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(PasswordRequiresUniqueChars),
        //        Description = string.Format(LocalizedIdentityErrorMessages.PasswordRequiresUniqueChars, uniqueChars)
        //    };
        //}

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "كلمة المرور يجب أن تحتوي حرف كبير واحد"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format("كلمة المرور يجب أن تحتوي على 6 أحرف على الأقل", length)
            };
        }

        //public override IdentityError DuplicateRoleName(string role)
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(DuplicateRoleName),
        //        Description = string.Format(LocalizedIdentityErrorMessages.DuplicateRoleName, role)
        //    };
        //}

        //public override IdentityError InvalidRoleName(string role)
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(InvalidRoleName),
        //        Description = string.Format(LocalizedIdentityErrorMessages.InvalidRoleName, role)
        //    };
        //}

        //public override IdentityError InvalidToken()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(InvalidToken),
        //        Description = LocalizedIdentityErrorMessages.InvalidToken
        //    };
        //}


        //public override IdentityError LoginAlreadyAssociated()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(LoginAlreadyAssociated),
        //        Description = LocalizedIdentityErrorMessages.LoginAlreadyAssociated
        //    };
        //}

        //public override IdentityError PasswordMismatch()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(PasswordMismatch),
        //        Description = LocalizedIdentityErrorMessages.PasswordMismatch
        //    };
        //}


        //public override IdentityError UserAlreadyHasPassword()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(UserAlreadyHasPassword),
        //        Description = LocalizedIdentityErrorMessages.UserAlreadyHasPassword
        //    };
        //}

        //public override IdentityError UserAlreadyInRole(string role)
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(UserAlreadyInRole),
        //        Description = string.Format(LocalizedIdentityErrorMessages.UserAlreadyInRole, role)
        //    };
        //}

        //public override IdentityError UserNotInRole(string role)
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(UserNotInRole),
        //        Description = string.Format(LocalizedIdentityErrorMessages.UserNotInRole, role)
        //    };
        //}

        //public override IdentityError UserLockoutNotEnabled()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(UserLockoutNotEnabled),
        //        Description = LocalizedIdentityErrorMessages.UserLockoutNotEnabled
        //    };
        //}

        //public override IdentityError RecoveryCodeRedemptionFailed()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(RecoveryCodeRedemptionFailed),
        //        Description = LocalizedIdentityErrorMessages.RecoveryCodeRedemptionFailed
        //    };
        //}

        //public override IdentityError ConcurrencyFailure()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(ConcurrencyFailure),
        //        Description = LocalizedIdentityErrorMessages.ConcurrencyFailure
        //    };
        //}

        //public override IdentityError DefaultError()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(DefaultError),
        //        Description = LocalizedIdentityErrorMessages.DefaultIdentityError
        //    };
        //}
    }
}

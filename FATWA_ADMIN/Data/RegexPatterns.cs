namespace FATWA_ADMIN.Data
{
    public static class RegexPatterns
    {
        /// <summary>
        ///  Allow English and Arabic along with these Special Characters: (, ), [, ], /, \, ", ', :, & and comma
        /// </summary>
        public const string EnglishPattern = @"^(?!\s*$)[A-Za-z0-9\s(),[\]/\\\""':&]+$";
        public const string ArabicPattern = @"^(?!\s*$)[\u0621-\u064A0-9\u0660-\u0669\u06F0-\u06F9\s(),[\]/\\\""':&]+$";
        public const string EnglishArabicPattern = @"^(?!\s*$)[A-Za-z\u0621-\u064A0-9\u0660-\u0669\u06F0-\u06F9\s(),[\]/\\\""':&]+$"; 
        public const string TrimmedEnglishPattern = @"^(?!\s*$)[A-Za-z0-9\s(),[\]/\\\""':&]+$";

        // The below pattern allow spcial characters, English, Arabic, along with only two digits
        public const string SpecialPattern = @"^(?=.{1,10}$)(?!.*\s{2})[A-Za-z\u0621-\u064A(),[\]/\\\""':&]*(\d{0,3})[A-Za-z\u0621-\u064A(),[\]/\\\""':& ]*$";
        public const string NoLeadingWhiteSpacesPattern = @"^\S(.*\S)?$";
        public const string NoSpacesPattern = @"^(?:(?!^\s+$).*(?:\r?\n|$))*$";
        //For text area Generic Regex pattern on remarks and Description.
        public const string NoSpacesTextAreaPattern = @"^(?:(?!^\s+$).*(?:\r?\n|$))*$";
        public const string NoSpecialCharactersPattern = "^[A-Za-z0-9 ]+$";
        public const string specificEmptyContentRichTextEditorPattern = @"^(?:<p>\s*(?:&nbsp;|<br\s*/?>|<\s*&nbsp;\s*>|\s*)*\s*</p>)+$";


    }
}

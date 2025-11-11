namespace FATWA_WEB.Data
{
    public static class RegexPatterns
    {
        /// <summary>
        ///  Allow English and Arabic along with these Special Characters: (, ), [, ], /, \, ", ', :, & and comma
        /// </summary>
        public const string EnglishPattern = @"^(?!.*\s{2})[A-Za-z0-9\s(),[\]/\\\""':&]+(?!.*\s{2})$";
        public const string ArabicPattern = @"^(?!.*\s{2})[\u0621-\u064A0-9\u0660-\u0669\u06F0-\u06F9\s(),[\]/\\\""':&]+(?!.*\s{2})$";
        public const string EnglishArabicPattern = @"^(?!.*\s{2})[A-Za-z\u0621-\u064A0-9\u0660-\u0669\u06F0-\u06F9\s(),[\]/\\\""':&]+(?!.*\s{2})$";
		// The below pattern allow spcial characters, English, Arabic, along with only two digits
		public const string SpecialPattern = @"^(?=.{1,10}$)(?!.*\s{2})[A-Za-z\u0621-\u064A(),[\]/\\\""':&]*(\d{0,3})[A-Za-z\u0621-\u064A(),[\]/\\\""':& ]*$";
        public const string NoLeadingWhiteSpacesPattern = @"^\S(.*\S)?$";
        public const string NoLeadingSpacesPattern = @"^(?:(?!^\s+$).*(?:\r?\n|$))*$";
        public const string OnlyDigitAllowPattern = @"^[0-9]+$";
        //For text area Generic Regex pattern on remarks and Description.
        public const string NoLeadingSpacesTextAreaPattern = @"^(?:(?!^\s+$).*(?:\r?\n|$))*$";
        public const string specificEmptyContentRichTextEditorPattern = @"^(?:<p>\s*(?:&nbsp;|<br\s*/?>|<\s*&nbsp;\s*>|\s*)*\s*</p>)+$";

    }
}

using FATWA_DOMAIN.Models;

namespace FATWA_ADMIN.Data
{
    //<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master"> Added Application's translation state class which will be a singleton service</History>
    public class TranslationState
    {
        public string PageSummaryFormat { get; set; }
        public IEnumerable<TranslationSucessResponse> TranslationList { get; set; }

        //<History Author = 'Hassan Abbas' Date='2024-01-04' Version="1.0" Branch="master"> Optimized the function to remove extra code</History>
        public string Translate(string stringToTranslate)
        {
            try
            {
                if (!string.IsNullOrEmpty(stringToTranslate))
                {
                    var translation = TranslationList?.Where(x => x.TranslationKey.ToLower() == stringToTranslate.ToLower()).FirstOrDefault();
                    if (translation != null)
                    {
                        stringToTranslate = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? translation.Value_En : translation.Value_Ar;
                    }
                }
                return stringToTranslate;
            }
            catch (Exception)
            {
                return stringToTranslate;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-01-04' Version="1.0" Branch="master"> Check to translate the supervisorsAndManagersGrid if not null</History>
        public void TranslateGridFilterLabels(dynamic grid)
        {
            if (grid != null)
            {
                grid.ClearFilterText = Translate("Clear");
                grid.ApplyFilterText = Translate("Filter");
                grid.EqualsText = Translate("Equals");
                grid.NotEqualsText = Translate("Not_equals");
                grid.LessThanText = Translate("Less_than");
                grid.LessThanOrEqualsText = Translate("Less_than_or_equals");
                grid.GreaterThanText = Translate("Greater_than");
                grid.GreaterThanOrEqualsText = Translate("Greater_than_or_equals");
                grid.EndsWithText = Translate("Ends_with");
                grid.ContainsText = Translate("Contains");
                grid.DoesNotContainText = Translate("Does_not_contain");
                grid.StartsWithText = Translate("Starts_with");
                grid.IsNullText = Translate("Is_null");
                grid.IsNotNullText = Translate("Is_not_null");
                grid.IsNotNullText = Translate("Is_not_null");
                grid.AndOperatorText = Translate("And");
                grid.OrOperatorText = Translate("Or");
                grid.IsEmptyText = Translate("Is_Empty");
                grid.EmptyText = Translate("Empty");
                grid.FilterText = Translate("Filter");
                PageSummaryFormat = $"{Translate("Page")} {"{0}"} {Translate("of")} {"{1}"} ({"{2}"} {Translate("items")})";
            }
        }
    }
}

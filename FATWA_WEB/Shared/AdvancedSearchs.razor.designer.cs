using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace FATWA_WEB.Shared
{
    public partial class AdvancedSearchsComponent : ComponentBase
    {
       protected IEnumerable<LmsLiteratureClassification> lmsLiteratureClassificationsGrid { get; set; }

        protected AdvancedSearchVM advancedSearchVM;


        public class AdvancedSearchEnumTypes
        {
            public AdvancedSearchVM.AdvancedSearchDropDownEnum advancedSearchEnumValue { get; set; }
            public string advancedSearchEnumName { get; set; }
        }

        protected bool Keywords = false;
       
        protected List<object> AdvancedSearchOptions { get; set; } = new List<object>();

        public AdvancedSearchsComponent()
        {
            advancedSearchVM = new AdvancedSearchVM();
        }

        [Inject]
        protected AdvancedSearchsService advancedSearchsService { get; set; }
        [Inject]
        protected LmsLiteratureClassificationService lmsLiteratureClassificationService { get; set; }




        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        protected RadzenDataGrid<LmsLiteratureIndex> grid0;


        IEnumerable<LmsLiteratureIndex> _getLmsLiteratureIndexResult;
        protected IEnumerable<LmsLiteratureIndex> getLmsLiteratureIndexResult
        {
            get
            {
                return _getLmsLiteratureIndexResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureIndexResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getLmsLiteratureIndexResult", NewValue = value, OldValue = _getLmsLiteratureIndexResult };
                    _getLmsLiteratureIndexResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Load(); 
        }
         
        protected async Task Load()
        {
            try
            {
                var response = await lmsLiteratureClassificationService.GetLiteratureClassifications();
                if (response.IsSuccessStatusCode)
                {
                    lmsLiteratureClassificationsGrid = (List<LmsLiteratureClassification>)response.ResultData;
                }
               
                foreach (AdvancedSearchVM.AdvancedSearchDropDownEnum item in Enum.GetValues(typeof(AdvancedSearchVM.AdvancedSearchDropDownEnum)))
                {
                    AdvancedSearchOptions.Add(new AdvancedSearchEnumTypes {advancedSearchEnumName = item.ToString(), advancedSearchEnumValue = item});
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected void OnChange(object value, string name)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
        }

        //protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
        //{
        //    if (args?.Value == "csv")
        //    {
        //        await lmsLiteratureIndexService.ExportLmsLiteratureIndexsToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Select = "IndexId,Name,Name_Ar, IndexNumber, IndexCreationDate" }, $"Lms Literature Index");

        //    }

        //    if (args == null || args.Value == "xlsx")
        //    {
        //        await lmsLiteratureIndexService.ExportLmsLiteratureIndexsToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Select = "IndexId,Name,Name_Ar, IndexNumber, IndexCreationDate" }, $"Lms Literature Index");

        //    }
        //}  

        //protected void EnableKeywordsTextBox(object value, string name)
        //{
        //    if (value != null)
        //    {
        //        Keywords = true;
        //    }
        //}

        protected Task<IEnumerable<LmsLiterature>> Submit(AdvancedSearchVM args)
        {
            var classificationId = args.ClassificationId;
            var enumValue = args.EnumSearchValue;
            var keywordsText = args.KeywordsType;
            var from = args.FromDate;
            var to = args.ToDate;

            if (classificationId == 0 && enumValue == 0 && keywordsText == null
                && !from.HasValue && !to.HasValue)
            {
                return null;
            }
            else if(enumValue != 0)
            {
                switch (enumValue)
                {
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Name:
                        {
                            // your code
                            // for plus operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Author_Name:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Index_Name:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Index_Number:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Division_Number:
                        {
                            // your code
                            // for plus operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Aisle_Number:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Barcode:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Character:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Price:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    case AdvancedSearchVM.AdvancedSearchDropDownEnum.Purchase_Date:
                        {
                            // your code
                            // for MULTIPLY operator
                            break;
                        }
                    default: break;
                }
            }

            
            return null;
        }
    }
}

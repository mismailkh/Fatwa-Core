//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;
//using Radzen;
//using Radzen.Blazor;
//using FATWA_WEB.Services;
//using FATWA_DOMAIN.Models;
//using FATWA_WEB.Data;
//using FATWA_DOMAIN.Models.ViewModel;
//using FATWA_WEB.Pages.Lms;
//using static FATWA_DOMAIN.Models.ViewModel.AdvancedSearchVM;
//using Microsoft.Extensions.Azure;
//using static FATWA_GENERAL.Helper.Response;
//using Blazored.LocalStorage;

//namespace FATWA_WEB.Pages
//{
//    public partial class ListLiteraturesComponent : ComponentBase
//    {
//        [Parameter(CaptureUnmatchedValues = true)]
//        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
//        protected bool AdvancedSearchResultGrid;
//        public bool isVisible { get; set; }
//        public void Reload()
//        {
//            InvokeAsync(StateHasChanged);
//        }
//        public ListLiteraturesComponent()
//        {
//            advancedSearchVM = new AdvancedSearchVM();
//            args = new AdvancedSearchVM();
//            AdvancedSearchResultGrid = false;
//        }
//        public void OnPropertyChanged(FATWA_WEB.Services.PropertyChangedEventArgs args)
//        {
//        }
//        protected AdvancedSearchVM advancedSearchVM;
//        protected AdvancedSearchVM args;

//        #region Service Injections
//         
//          

//         
//          
//         
//          
//         
//          
//         
//        protected LmsLiteratureService FatwaAPI { get; set; }
//         
//          
//         
//          
//         
//         
//         
//         

//        #endregion

//        protected RadzenDataGrid<LiteratureDetailVM> grid;

//        string _search;
//        protected string search
//        {
//            get
//            {
//                return _search;
//            }
//            set
//            {
//                if (!object.Equals(_search, value))
//                {
//                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
//                    _search = value;
//                    OnPropertyChanged(args);
//                    Reload();
//                }
//            }
//        }

//        public bool allowRowSelectOnRowClick = true;
//        public IEnumerable<LiteratureDetailVM> literatures = new List<LiteratureDetailVM>();
//        protected int count;
//        public IList<LiteratureDetailVM> selectedLiteratures;
         
//        IEnumerable<LiteratureDetailVM> _getLmsLiteraturesResult;
//        protected IEnumerable<LiteratureDetailVM> getLmsLiteraturesResult
//        {
//            get
//            {
//                return _getLmsLiteraturesResult;
//            }
//            set
//            {
//                if (!object.Equals(_getLmsLiteraturesResult, value))
//                {
//                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "getLmsLiteraturesResult", NewValue = value, OldValue = _getLmsLiteraturesResult };
//                    _getLmsLiteraturesResult = value;
//                    OnPropertyChanged(args);
//                    Reload();
//                }
//            }
//        }

//        protected override async Task OnInitializedAsync()
//        {
//            spinnerService.Show();

//            await Load();
//            translationState.TranslateGridFilterLabels(grid);

//            #region AdvanceSearch

//            var lmsLiteratureClassificationsResult = await lmsLiteratureClassificationService.GetLmsLiteratureClassifications();
//            if (lmsLiteratureClassificationsResult.Count() != 0)
//            {
//                lmsLiteratureClassificationsGrid = lmsLiteratureClassificationsResult;
//            }
//            foreach (AdvancedSearchVM.AdvancedSearchDropDownEnum item in Enum.GetValues(typeof(AdvancedSearchVM.AdvancedSearchDropDownEnum)))
//            {
//                AdvancedSearchOptions.Add(new AdvancedSearchEnumTypes { advancedSearchEnumName = translationState.Translate(item.ToString()), advancedSearchEnumValue = item });
//            }
//            #endregion
//            spinnerService.Hide();
//        }
       
//        protected async Task Load()
//        {
            
//            if (string.IsNullOrEmpty(search))
//            {
//                search = "";
//            }
//            else 
//                search = search.ToLower(); 

//            var result = await FatwaAPI.GetLmsLiteratures(new Query()
//            {
//                Filter = $@"i => (i.LiteratureName != null && i.LiteratureName.ToLower().Contains(@0)) || (i.LiteratureType != null && i.LiteratureType.ToLower().Contains(@1)) || (i.AuthorName != null && i.AuthorName.ToLower().Contains(@2)) || (i.ISBN != null && i.ISBN.ToLower().Contains(@3)) || (i.IndexNumber != null && i.IndexNumber.ToLower().Contains(@4))",
//                FilterParameters = new object[] { search, search, search, search, search }
//            });
           
//            literatures = result; 
//        }

//        //<History Author = 'Hassan Abbas' Date='2022-03-18' Version="1.0" Branch="master"> Redirect to Add book wizard</History>
//        protected async Task ButtonAddClick(MouseEventArgs args)
//        {
//            navigationManager.NavigateTo("/lmsliterature-add");
//        }

//        protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
//        {
//            if (args?.Value == "csv")
//            {
//                await FatwaAPI.ExportLmsLiteraturesToCSV(new Query()
//                {
//                    Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
//                    OrderBy = $"{grid.Query.OrderBy}",
//                    Expand = "",
//                    Select = " BookName, LiteratureType, ISBN, AuthorName, IndexNumber, PurchaseDate"
//                }, $"Lms Literatures");
//            } 
//            if (args == null || args.Value == "xlsx")
//            {
//                await FatwaAPI.ExportLmsLiteraturesToExcel(new Query()
//                {
//                    Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
//                    OrderBy = $"{grid.Query.OrderBy}",
//                    Expand = "",
//                    Select = " BookName, LiteratureType, ISBN, AuthorName, IndexNumber, PurchaseDate"
//                }, $"Lms Literatures");
//            }
//        }
//        protected async Task EditLiterature(LiteratureDetailVM args)
//        {
//            navigationManager.NavigateTo("/lmsliterature-edit/" + args.LiteratureId);
//        }
//        //<History Author = 'Hassan Abbas' Date='2022-03-18' Version="1.0" Branch="master"> Redirect to Edit book wizard</History>
//        protected async Task ViewLiteratureDetail(LiteratureDetailVM args)
//        {
//            navigationManager.NavigateTo("/lmsliterature-detail/" + args.LiteratureId);
//        }

//        //protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
//        //{
//        //    try
//        //    {
//        //        if (await dialogService.Confirm("هل أنت متأكد أنك تريد حذف؟") == true)
//        //        {
//        //            var fatwaDbDeleteLmsLiteratureResult = await FatwaAPI.DeleteLmsLiterature(data.LiteratureId);
//        //            if (fatwaDbDeleteLmsLiteratureResult != null)
//        //            {
//        //                await Load();
//        //                await grid.Reload();
//        //            }
//        //        }
//        //    }
//        //    catch (System.Exception fatwaDbDeleteLmsLiteratureException)
//        //    {
//        //        notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = translationState.Translate("Error") $"تعذر حذف الأدب" });
//        //    }
//        //}

//        protected bool isChecked = false;
//        protected void OnSelectLiterature(bool value, string name, LiteratureDetailVM? obj)
//        {
//            if (name == "allChecked")
//            {
//                if (value == true)
//                {
//                    selectedLiteratures = literatures.ToList();
//                    isChecked = true;
//                }
//                else
//                { 
//                    selectedLiteratures = null;
//                    isChecked = false; 
//                }
//            }
//            //else
//            //{
//            //    grid.SelectRow(obj);
//            //}
//        }

//        protected async Task ButtonDeleteClick(MouseEventArgs args)
//        {
//            try
//            {
//                if(selectedLiteratures != null && selectedLiteratures.Any())
//                {
//                    bool? dialogResponse = await dialogService.Confirm(
//                        translationState.Translate("Are_you_sure_you_want_to_delete_this_record"),
//                        translationState.Translate("Confirm"),
//                        new ConfirmOptions()
//                        {
//                            OkButtonText = translationState.Translate("OK"),
//                            CancelButtonText = translationState.Translate("Cancel")
//                        });

//                    if (dialogResponse == true)
//                    {
//                        spinnerService.Show();
//                        var response = await FatwaAPI.DeleteLiterature(selectedLiteratures);
//                        if (response.IsSuccessStatusCode)
//                        {
//                            notificationService.Notify(new NotificationMessage()
//                            {
//                                Severity = NotificationSeverity.Success,
//                                Detail = translationState.Translate("Literature_Deleted_Successfully"),
//                                //Summary = $"!نجاح", 
//                                Style = "position: fixed !important; left: 0; margin: auto; "
//                            });
//                            StateHasChanged();
//                        }
//                        else
//                        {
//                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
//                        }
//                        if (response != null)
//                        {
                           

//                            await Load();
//                            await grid.Reload();
//                            spinnerService.Hide();
//                        }
//                    }
//                }
//                else
//                {
//                    bool? dialogResponse = await dialogService.Confirm(
//                       translationState.Translate("Select_Record_For_Delete"),
//                       translationState.Translate("warning"),
//                       new ConfirmOptions()
//                       {
//                           OkButtonText = translationState.Translate("OK"),
//                           CancelButtonText = translationState.Translate("Cancel")
//                       }); 
//                } 
//            }
//            catch (Exception)
//            {
//                notificationService.Notify(new NotificationMessage() { 
//                    Severity = NotificationSeverity.Error,  
//                    Detail = translationState.Translate("Literature_Delete_Failed"),
//                    //Summary = $"خطأ!",
//                    Style = "position: fixed !important; left: 0; margin: auto; "
//                });
//            }
//        }
       
//        #region AdvanceSearch
//        protected IEnumerable<LmsLiteratureClassification> lmsLiteratureClassificationsGrid { get; set; }
//        public class AdvancedSearchEnumTypes
//        {
//            public AdvancedSearchVM.AdvancedSearchDropDownEnum advancedSearchEnumValue { get; set; }
//            public string advancedSearchEnumName { get; set; }
//        }

//        protected bool Keywords = false;

//        protected List<object> AdvancedSearchOptions { get; set; } = new List<object>();

//        protected bool CheckKeywords = false;

//        protected async Task SubmitAdvanceSearch()
//        {
//            if(advancedSearchVM.FromDate > advancedSearchVM.ToDate)
//            {
//                notificationService.Notify(new NotificationMessage()
//                {
//                    Severity = NotificationSeverity.Error,
//                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
//                    //Summary = $"خطأ!",
//                    Style = "position: fixed !important; left: 0; margin: auto; "
//                });
//                return;
//            }

//            spinnerService.Show();
//            if (advancedSearchVM.EnumSearchValue != 0 && (advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Book_Name || advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Character ||
//                                                        advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Author_Name || advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Index_Name))
//            {
//                if (!string.IsNullOrEmpty(advancedSearchVM.KeywordsType))
//                {
//                    await Submitform();
//                }
//                else 
//                {
//                    CheckKeywords = true;
//                    Keywords = true;
//                }
//            }
//            else if (advancedSearchVM.EnumSearchValue != 0 && (advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Index_Number || advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Division_Number || advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Purchase_Date
//                                                                || advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Aisle_Number || advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Barcode || advancedSearchVM.EnumSearchValue == AdvancedSearchDropDownEnum.Price))
//            {
//                await Submitform();
//            }
//            else
//            {
//                await Submitform();
//            }
//            spinnerService.Hide();
//        }
//        public async void ResetForm()
//        {
//            advancedSearchVM.ClassificationId = 0;
//            advancedSearchVM.EnumSearchValue = 0;
//            advancedSearchVM.KeywordsType = null;
//            advancedSearchVM.PurchaseDateKeyword = null;
//            advancedSearchVM.GenericsIntergerKeyword = 0;
//            advancedSearchVM.FromDate = null;
//            advancedSearchVM.ToDate = null;
//            CheckKeywords = false;

//            await Task.Delay(100);

//            if (string.IsNullOrEmpty(search))
//            {
//                search = "";
//            }
//            //isLoading = true;
//            var fatwaDbGetLmsLiteraturesResult = await FatwaAPI.GetLmsLiteratures(new Query()
//            {
//                Filter = $@"i => i.LiteratureName.Contains(@0) || i.LiteratureType.Contains(@1) || i.AuthorName.Contains(@2)",
//                FilterParameters = new object[] { search, search, search }
//            });
//            getLmsLiteraturesResult = fatwaDbGetLmsLiteraturesResult;
//            literatures = getLmsLiteraturesResult;
//            AdvancedSearchResultGrid = false;
//            //isLoading = false;
//            grid.Reset(); 
//            await grid.Reload();
//            Keywords = false;
//            StateHasChanged();
//        }

//        private async Task Submitform()
//        {
//            CheckKeywords = false;
//            if (advancedSearchVM.ClassificationId == 0 && advancedSearchVM.EnumSearchValue == 0 && advancedSearchVM.KeywordsType == null
//                && !advancedSearchVM.FromDate.HasValue && !advancedSearchVM.ToDate.HasValue)
//            { }
//            else
//            {
//                Keywords = true;
//                args.ClassificationId = advancedSearchVM.ClassificationId;
//                args.EnumSearchValue = advancedSearchVM.EnumSearchValue;
//                args.KeywordsType = advancedSearchVM.KeywordsType;
//                args.PurchaseDateKeyword = advancedSearchVM.PurchaseDateKeyword;
//                args.GenericsIntergerKeyword = advancedSearchVM.GenericsIntergerKeyword;
//                args.FromDate = advancedSearchVM.FromDate;
//                args.ToDate = advancedSearchVM.ToDate;

//                await Task.Delay(100); 
                
//                var fatwaDbGetLmsLiteraturesResult = await FatwaAPI.GetLmsLiteraturesAdvanceSearch(args); 
//                literatures = fatwaDbGetLmsLiteraturesResult;
//                AdvancedSearchResultGrid = true; 

//                grid.Reset();
//                await grid.Reload();
//            }
//        }
         

//        //<History Author = 'Hassan Abbas' Date='2022-09-09' Version="1.0" Branch="master">Open Advance search Popup </History>
//        protected async Task ToggleAdvanceSearch()
//        {
//            isVisible = !isVisible;
//            if (!isVisible)
//            {
//                ResetForm();
//            }
//        }
//        #endregion

//        #region Badrequest Notiication

//        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Handle bad request and display error messages in whole class</History>
//        protected async Task ReturnBadRequestNotification(ApiCallResponse response)
//        {
//            try
//            {
//                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//                {
//                    notificationService.Notify(new NotificationMessage()
//                    {
//                        Severity = NotificationSeverity.Error,
//                        Detail = translationState.Translate("Token_Expired"),
//                        Style = "position: fixed !important; left: 0; margin: auto; "
//                    });
//                    await Task.Delay(5000);
//                    await BrowserStorage.RemoveItemAsync("User");
//                    await BrowserStorage.RemoveItemAsync("Token");
//                    await BrowserStorage.RemoveItemAsync("RefreshToken");
//                    await BrowserStorage.RemoveItemAsync("UserDetail");
//                    await BrowserStorage.RemoveItemAsync("SecurityStamp");

//                }
//                else
//                {
//                    var badRequestResponse = (BadRequestResponse)response.ResultData;
//                    if (badRequestResponse.InnerException != null && badRequestResponse.InnerException.ToLower().Contains("violation of unique key"))
//                    {
//                        notificationService.Notify(new NotificationMessage()
//                        {
//                            Severity = NotificationSeverity.Error,
//                            Detail = translationState.Translate("Record_Already_Exists"),
//                            Style = "position: fixed !important; left: 0; margin: auto; "
//                        });
//                    }
//                    else
//                    {
//                        notificationService.Notify(new NotificationMessage()
//                        {
//                            Severity = NotificationSeverity.Error,
//                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
//                            Style = "position: fixed !important; left: 0; margin: auto; "
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                notificationService.Notify(new NotificationMessage()
//                {
//                    Severity = NotificationSeverity.Error,
//                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
//                    Style = "position: fixed !important; left: 0; margin: auto; "
//                });
//            }
//        }
//        #endregion
//    }
//}

using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.FileSelect;

namespace FATWA_WEB.Pages.Lms
{
    public partial class ImportStockTakingReportDialog : ComponentBase
    {
        #region Parameter
        [Parameter]
        public List<LmsStockTakingBooksReportListVm> StockTakingBooksList { get; set; }
        [Parameter]
        public List<string> SelectedPerformerIds { get; set; }
        #endregion
        #region Variable Declaration
        protected List<string> FileTypes = new List<string> { ".xlsx" };
        protected FileSelectFileInfo selectedfile { get; set; }
        protected Syncfusion.Blazor.Inputs.UploadFiles selectedSFfile { get; set; }
        protected List<string> errorlist = new List<string>();
        protected string errorMessages;
        protected List<string> HeaderValues = new List<string> { "RFIdValue", "TotalCopiesCounted" };
        public TelerikFileSelect FileSelectRef { get; set; }
        public TelerikFileSelect FileSelectRef1 { get; set; }
        public bool isShowFileSelect { get; set; } = true;
        protected IEnumerable<UserVM> EmployeesList = new List<UserVM>();
        protected UserListAdvanceSearchVM AdvanceSearchVM { get; set; } = new UserListAdvanceSearchVM();
        public SaveStockTakingVm saveStockTaking { get; set; } = new SaveStockTakingVm();
        public string? MobileNumber { get; set; } = "";
        public List<StockTakingImportTemplate> stockList { get; set; } = new List<StockTakingImportTemplate>();
        public RadzenDataGrid<StockTakingImportTemplate> grid0;
        Regex rgx = new Regex("[^a-zA-Z?-?]");
        public string PerformerValidationMessage { get; set; } = " ";

        #endregion

        #region Load Component 
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(grid0);
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await GetUsersList(string.Empty);
            if (SelectedPerformerIds != null && SelectedPerformerIds.Count > 0)
            {
                saveStockTaking.StockTakingPerformerIds = SelectedPerformerIds;
            }
        }
        #endregion
        #region Get users List 
        public async void OnLoadUserData(LoadDataArgs args)
        {
            try
            {
                string str = args.Filter;
                bool hasNumSpecialChars = rgx.IsMatch(str);

                if (!hasNumSpecialChars)
                {
                    await GetUsersList(str);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task GetUsersList(string Filter)
        {
            try
            {
                var getUserDetail = await userService.GetLegalCulturalCenterUsers(Filter);
                if (getUserDetail.IsSuccessStatusCode)
                {
                    EmployeesList = (List<UserVM>)getUserDetail.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(getUserDetail);
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Select Functions
        protected async Task HandleFileSelect(FileSelectEventArgs args)
        {
            selectedfile = args.Files[0];
            spinnerService.Show();
            await ImportFile();
            spinnerService.Hide();
        }
        protected void HandleFileRemove(FileSelectEventArgs args)
        {
            selectedfile = null;
        }
        #endregion
        #region Import File
        protected async Task ImportFile()
        {
            try
            {
                if (selectedfile != null)
                {
                    isShowFileSelect = true;
                    var result = await excelImportService.StockTakingImportFromExcel(selectedfile.Stream, HeaderValues);
                    if (result.IsStatusCode)
                    {
                        stockList = result.stockTakingImports;
                        foreach (var item in StockTakingBooksList)
                        {
                            stockList
                                .Where(x => x.RFIdValue == item.RFIDValue)
                                .ToList()
                                .ForEach(y =>
                                {
                                    y.BookName = item.BookName;
                                    y.AuthorNameEn = item.AuthorNameEn;
                                    y.AuthorNameAr = item.AuthorNameAr;
                                    y.IndexNameEn = item.IndexNameEn;
                                    y.IndexNameAr = item.IndexNameAr;
                                });
                        }
                        stockList.Where(x => (string.IsNullOrEmpty(x.RFIdValue) && string.IsNullOrEmpty(x.BarcodeNumber)) || ( !StockTakingBooksList.Any(y => !string.IsNullOrEmpty(y.RFIDValue) && y.RFIDValue == x.RFIdValue) &&
                                    !StockTakingBooksList.Any(y => !string.IsNullOrEmpty(x.BarcodeNumber) && y.BarCodeNumber == x.BarcodeNumber)))
                                         .ToList()
                                         .ForEach(val => val.IsExist = false);
                        await grid0.Reload();
                        isShowFileSelect = false;
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Please_Select_Correct_File"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        selectedfile = null;
                        isShowFileSelect = false;
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Select_Any_File"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region On Cell Render
        protected void CellRender(RowRenderEventArgs<StockTakingImportTemplate> args)
        {
            if (args.Data.IsExist == false)
            {
                args.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }
        #endregion
        #region Compare
        protected async Task Compare()
        {
            try
            {
                if (!saveStockTaking.StockTakingPerformerIds.IsNullOrEmpty() && saveStockTaking.StockTakingPerformerIds.Any())
                {
                    foreach (var importList in stockList.Where(x => x.IsExist == true).ToList())
                    {
                        LmsStockTakingBooksReportListVm stockValue = new LmsStockTakingBooksReportListVm();
                        if (!string.IsNullOrEmpty(importList.RFIdValue))
                        {
                            stockValue = StockTakingBooksList.Where(x => x.RFIDValue == importList.RFIdValue).FirstOrDefault();
                            if (!string.IsNullOrEmpty(importList.TotalCopiesCounted) && stockValue != null)
                            {
                                int calculatedValue = (int)(Convert.ToInt32(importList.TotalCopiesCounted) - (stockValue.CopiesNotBorrowed + stockValue.CopiesBorrowed));
                                if (calculatedValue > 0)
                                {
                                    StockTakingBooksList
                                        .Where(x => x.RFIDValue == importList.RFIdValue)
                                        .ToList()
                                        .ForEach(x => { x.Excess = calculatedValue; x.Shortage = null; });

                                }
                                else if (calculatedValue < 0)
                                {
                                    StockTakingBooksList
                                        .Where(x => x.RFIDValue == importList.RFIdValue)
                                        .ToList()
                                        .ForEach(x => { x.Shortage = calculatedValue; x.Excess = null; });
                                }
                                else if (calculatedValue == 0)
                                {
                                    StockTakingBooksList
                                        .Where(x => x.RFIDValue == importList.RFIdValue)
                                        .ToList()
                                        .ForEach(x => { x.Excess = calculatedValue; x.Shortage = calculatedValue; });
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(importList.BarcodeNumber))
                        {
                            stockValue = StockTakingBooksList.Where(x => x.BarCodeNumber == importList.BarcodeNumber).FirstOrDefault();
                            if (!string.IsNullOrEmpty(importList.TotalCopiesCounted) && stockValue != null)
                            {
                                int calculatedBarcodeValue = (int)(Convert.ToInt32(importList.TotalCopiesCounted) - (stockValue.CopiesNotBorrowed + stockValue.CopiesBorrowed));
                                if (calculatedBarcodeValue > 0)
                                {
                                    StockTakingBooksList
                                        .Where(x => x.BarCodeNumber == importList.BarcodeNumber)
                                        .ToList()
                                        .ForEach(x => { x.Excess = calculatedBarcodeValue; x.Shortage = null; });

                                }
                                else if (calculatedBarcodeValue < 0)
                                {
                                    StockTakingBooksList
                                        .Where(x => x.BarCodeNumber == importList.BarcodeNumber)
                                        .ToList()
                                        .ForEach(x => { x.Shortage = calculatedBarcodeValue; x.Excess = null; });
                                }
                                else if (calculatedBarcodeValue == 0)
                                {
                                    StockTakingBooksList
                                        .Where(x => x.BarCodeNumber == importList.BarcodeNumber)
                                        .ToList()
                                        .ForEach(x => { x.Excess = calculatedBarcodeValue; x.Shortage = calculatedBarcodeValue; });
                                }
                            }
                        }
                        
                    }
                    StockTakingBooksList
                                      .Where(x => x.Excess == null && x.Shortage == null)
                                      .ToList()
                                      .ForEach(x => { x.IsRfIdNotMatch = true; });
                    saveStockTaking.lmsStockTakingBooksReportListVms = StockTakingBooksList;
                    dialogService.Close(saveStockTaking);
                }
                else
                {
                    PerformerValidationMessage = saveStockTaking.StockTakingPerformerIds.IsNullOrEmpty() ? translationState.Translate("Required_Field") : !saveStockTaking.StockTakingPerformerIds.Any() ? translationState.Translate("Required_Field") : " ";
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Dialog Close
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}

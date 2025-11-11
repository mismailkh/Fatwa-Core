using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class LegalPrinciple : ComponentBase
    {
        #region Variables Declaration 
        protected RadzenDataGrid<LLSLegalPrincipleDocumentVM> AppealJudgementDocumentGrid { get; set; } = new RadzenDataGrid<LLSLegalPrincipleDocumentVM>();
        protected RadzenDataGrid<LLSLegalPrincipleDocumentVM> SupremeJudgementDocumentGrid { get; set; } = new RadzenDataGrid<LLSLegalPrincipleDocumentVM>();
        protected RadzenDataGrid<LLSLegalPrinciplLegalAdviceDocumentVM> LegalAdviceDocumentGrid { get; set; } = new RadzenDataGrid<LLSLegalPrinciplLegalAdviceDocumentVM>();
        public RadzenDataGrid<LLSLegalPrincipleKuwaitAlYoumDocuments> KayFileGrid { get; set; } = new RadzenDataGrid<LLSLegalPrincipleKuwaitAlYoumDocuments>();
        public RadzenDataGrid<LLSLegalPrinciplOtherDocumentVM> OthersFileGrid { get; set; } = new RadzenDataGrid<LLSLegalPrinciplOtherDocumentVM>();
        public List<LLSLegalPrincipleDocumentVM> LLSLegalPrincipleDocuments { get; set; } = new List<LLSLegalPrincipleDocumentVM>();
        public List<LLSLegalPrincipleDocumentVM> AppealJudgementDocuments { get; set; } = new List<LLSLegalPrincipleDocumentVM>();
        public List<LLSLegalPrincipleDocumentVM> SupremeJudgementDocuments { get; set; } = new List<LLSLegalPrincipleDocumentVM>();
        public List<LLSLegalPrinciplLegalAdviceDocumentVM> LegalAdviceJudgementDocuments { get; set; } = new List<LLSLegalPrinciplLegalAdviceDocumentVM>();
        public List<LLSLegalPrinciplOtherDocumentVM> OthersJudgementDocuments { get; set; } = new List<LLSLegalPrinciplOtherDocumentVM>();
        public IList<LLSLegalPrincipleKuwaitAlYoumDocuments> kayselectedDocuments { get; set; }
        public LLSLegalPrincipleDocumentVM selectedDocuments { get; set; } = new LLSLegalPrincipleDocumentVM();
        IEnumerable<LLSLegalPrincipleKuwaitAlYoumDocuments> KayDocumentsList = new List<LLSLegalPrincipleKuwaitAlYoumDocuments>();
        public List<LLSLegalPrincipleDocumentVM> FilteredAppealJudgementDocuments { get; set; }
        public List<LLSLegalPrincipleDocumentVM> FilteredSupremeJudgementDocuments { get; set; }
        public List<LLSLegalPrinciplLegalAdviceDocumentVM> FilteredLegalAdviceDocuments { get; set; }
        public List<LLSLegalPrinciplOtherDocumentVM> FilteredOthersJudgementDocuments { get; set; }
        protected IEnumerable<LLSLegalPrincipleKuwaitAlYoumDocuments> FilteredKayDocumentList { get; set; }

        protected LLSLegalPrincipalDocumentSearchVM advanceSearch = new LLSLegalPrincipalDocumentSearchVM();
        public IList<Court> courts { get; set; } = new List<Court>();
        public IList<Chamber> chambers { get; set; } = new List<Chamber>();
        public List<ChamberNumber> ChamberNumbers { get; set; } = new List<ChamberNumber>();
        public List<FileTypeEnumTemp> FileTypes { get; set; } = new List<FileTypeEnumTemp>();
        protected List<JudgementStatus> judgementStatuses { get; set; } = new List<JudgementStatus>();
        public bool DisplayDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        public int? PreviewedDocumentId { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public bool allowRowSelectOnRowClick = false;
        public bool isVisible { get; set; } = false;
        protected bool keywords = false;
        protected int courtTypeId;
        protected int JudmentDocumentId = 0;
        protected int selectedIndex { get; set; } = 0;
        public List<string> validFiles { get; set; } = new List<string>() { ".pdf" };
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();

        protected string search { get; set; }
        public class FileTypeEnumTemp
        {
            public int FileTypeEnumValue { get; set; }
            public string FileTypeEnumName { get; set; }
        }

        //Encryption/Descyption Key
        public string password;
        System.Text.UnicodeEncoding UE;
        public byte[] key;
        RijndaelManaged RMCrypto;
        MemoryStream fsOut;
        public int data = 0;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage { get; set; }
        private int CurrentPageSize { get; set; }
        public RadzenTabs tabRef { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateFileTypes();
            translationState.TranslateGridFilterLabels(AppealJudgementDocumentGrid);
            translationState.TranslateGridFilterLabels(SupremeJudgementDocumentGrid);
            translationState.TranslateGridFilterLabels(LegalAdviceDocumentGrid);
            translationState.TranslateGridFilterLabels(KayFileGrid);
            translationState.TranslateGridFilterLabels(OthersFileGrid);
            spinnerService.Hide();
        }

        #endregion

        #region Reload
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region On Load Data 
        private async Task GetLLSLegalPrincipleSourceDocuments(LoadDataArgs args)
        {
            try
            {
                SetGridPageValues();

                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearch.PageNumber || CurrentPageSize != advanceSearch.PageSize || (keywords && advanceSearch.isDataSorted))
                {
                    if (advanceSearch.isGridLoaded && advanceSearch.PageSize == CurrentPageSize && !advanceSearch.isPageSizeChangeOnFirstLastPage)
                    {
                        AppealJudgementDocumentGrid.CurrentPage = (int)advanceSearch.PageNumber - 1;
                        advanceSearch.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args, AppealJudgementDocuments.Any() ? (AppealJudgementDocuments.First().TotalCount) : 0, AppealJudgementDocumentGrid);
                    spinnerService.Show();
                    advanceSearch.CourtTypeId = (int)CourtTypeEnum.Appeal;
                    var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleSourceDocuments(advanceSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        LLSLegalPrincipleDocuments = (List<LLSLegalPrincipleDocumentVM>)response.ResultData;

                        LLSLegalPrincipleDocuments = LLSLegalPrincipleDocuments.OrderByDescending(x => x.JudgementDate).ToList();
                        AppealJudgementDocuments = LLSLegalPrincipleDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Appeal).ToList();
                        FilteredAppealJudgementDocuments = AppealJudgementDocuments;
                        if (!(string.IsNullOrEmpty(search))) await OnAppealJudgementSearchInput(search);
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredAppealJudgementDocuments = await gridSearchExtension.Sort(FilteredAppealJudgementDocuments, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    await InvokeAsync(StateHasChanged);
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
            spinnerService.Hide();
        }

        private async Task GetLLSLegalPrincipleSourceDocumentsSupreme(LoadDataArgs dataArgs)
        {
            try
            {
                SetGridPageValues();

                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearch.PageNumber || CurrentPageSize != advanceSearch.PageSize || (keywords && advanceSearch.isDataSorted))
                {
                    if (advanceSearch.isGridLoaded && advanceSearch.PageSize == CurrentPageSize && !advanceSearch.isPageSizeChangeOnFirstLastPage)
                    {
                        SupremeJudgementDocumentGrid.CurrentPage = (int)advanceSearch.PageNumber - 1;
                        advanceSearch.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, SupremeJudgementDocuments.Any() ? (SupremeJudgementDocuments.First().TotalCount) : 0, SupremeJudgementDocumentGrid);
                    spinnerService.Show();
                    advanceSearch.CourtTypeId = (int)CourtTypeEnum.Supreme;
                    var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleSourceDocuments(advanceSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        LLSLegalPrincipleDocuments = (List<LLSLegalPrincipleDocumentVM>)response.ResultData;

                        SupremeJudgementDocuments = LLSLegalPrincipleDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Supreme).ToList();
                        FilteredSupremeJudgementDocuments = SupremeJudgementDocuments;

                        if (!(string.IsNullOrEmpty(search))) await OnSupremeJudgementSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredSupremeJudgementDocuments = await gridSearchExtension.Sort(FilteredSupremeJudgementDocuments, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    await InvokeAsync(StateHasChanged);
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
            spinnerService.Hide();
        }
        private async Task GetLLSLegalPrincipleLegalAdviceSourceDocuments(LoadDataArgs dataArgs)
        {
            try
            {
                SetGridPageValues();

                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearch.PageNumber || CurrentPageSize != advanceSearch.PageSize || (keywords && advanceSearch.isDataSorted))
                {
                    if (advanceSearch.isGridLoaded && advanceSearch.PageSize == CurrentPageSize && !advanceSearch.isPageSizeChangeOnFirstLastPage)
                    {
                        LegalAdviceDocumentGrid.CurrentPage = (int)advanceSearch.PageNumber - 1;
                        advanceSearch.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, LegalAdviceJudgementDocuments.Any() ? (LegalAdviceJudgementDocuments.First().TotalCount) : 0, LegalAdviceDocumentGrid);
                    spinnerService.Show();
                    var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleLegalAdviceSourceDocuments(advanceSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        LegalAdviceJudgementDocuments = (List<LLSLegalPrinciplLegalAdviceDocumentVM>)response.ResultData;
                        FilteredLegalAdviceDocuments = LegalAdviceJudgementDocuments;
                        if (!(string.IsNullOrEmpty(search))) await OnLegalAdviceJudgementSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredLegalAdviceDocuments = await gridSearchExtension.Sort(FilteredLegalAdviceDocuments, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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
            spinnerService.Hide();
        }

        protected async Task PopulateKayDocumentsGrid(LoadDataArgs dataArgs)
        {
            try
            {
                SetGridPageValues();

                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearch.PageNumber || CurrentPageSize != advanceSearch.PageSize || (keywords && advanceSearch.isDataSorted))
                {
                    if (advanceSearch.isGridLoaded && advanceSearch.PageSize == CurrentPageSize && !advanceSearch.isPageSizeChangeOnFirstLastPage)
                    {
                        KayFileGrid.CurrentPage = (int)advanceSearch.PageNumber - 1;
                        advanceSearch.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, KayDocumentsList.Any() ? (KayDocumentsList.First().TotalCount) : 0, KayFileGrid);
                    spinnerService.Show();
                    var response = await lLSLegalPrincipleService.GetKayDocumentsListForLLSLegalPrinciple(advanceSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        KayDocumentsList = (response.ResultData) as IEnumerable<LLSLegalPrincipleKuwaitAlYoumDocuments>;
                        FilteredKayDocumentList = KayDocumentsList;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchKayDocumentsInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredKayDocumentList = await gridSearchExtension.Sort(FilteredKayDocumentList, ColumnName, SortOrder);
                        }

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    await InvokeAsync(StateHasChanged);
                    spinnerService.Hide();
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
            spinnerService.Hide();
        }

        private async Task GetLLSLegalPrincipleOtherSourceDocuments(LoadDataArgs dataArgs)
        {
            try
            {
                SetGridPageValues();
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearch.PageNumber || CurrentPageSize != advanceSearch.PageSize || (keywords && advanceSearch.isDataSorted))
                {
                    if (advanceSearch.isGridLoaded && advanceSearch.PageSize == CurrentPageSize && !advanceSearch.isPageSizeChangeOnFirstLastPage)
                    {
                        OthersFileGrid.CurrentPage = (int)advanceSearch.PageNumber - 1;
                        advanceSearch.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, OthersJudgementDocuments.Any() ? (OthersJudgementDocuments.First().TotalCount) : 0, OthersFileGrid);
                    spinnerService.Show();
                    var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleOtherSourceDocuments(advanceSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        OthersJudgementDocuments = (List<LLSLegalPrinciplOtherDocumentVM>)response.ResultData;
                        FilteredOthersJudgementDocuments = OthersJudgementDocuments;
                        if (!(string.IsNullOrEmpty(search))) await OnOtherJudgementSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredOthersJudgementDocuments = await gridSearchExtension.Sort(FilteredOthersJudgementDocuments, ColumnName, SortOrder);
                        }

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    await InvokeAsync(StateHasChanged);
                    spinnerService.Hide();
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
            spinnerService.Hide();
        }

        #endregion

        #region On Search Input
        protected async Task OnAppealJudgementSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredAppealJudgementDocuments = await gridSearchExtension.Filter(AppealJudgementDocuments, new Query()
                    {
                        Filter = $@"i => (i.JudgementTypeEn != null && i.JudgementTypeEn.ToString().ToLower().Contains(@0)) || 
                                         (i.CANNumber != null && i.CANNumber.ToString().ToLower().Contains(@0)) || 
                                         (i.CourtEn != null && i.CourtEn.ToString().ToLower().Contains(@0)) || 
                                         (i.CourtAr != null && i.CourtAr.ToString().ToLower().Contains(@0)) || 
                                         (i.ChamberNameEn != null && i.ChamberNameEn.ToString().ToLower().Contains(@0)) || 
                                         (i.ChamberNameAr != null && i.ChamberNameAr.ToString().ToLower().Contains(@0)) || 
                                         (i.NumberOfPrinciple != null && i.NumberOfPrinciple.ToString().Contains(@0)) || 
                                         (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@0)) || 
                                         (i.JudgementDate.HasValue && i.JudgementDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) ||
                                         (i.JudgementTypeAr != null && i.JudgementTypeAr.ToString().ToLower().Contains(@0)) || 
                                         (i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredAppealJudgementDocuments = await gridSearchExtension.Sort(FilteredAppealJudgementDocuments, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        protected async Task OnSupremeJudgementSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredSupremeJudgementDocuments = await gridSearchExtension.Filter(SupremeJudgementDocuments, new Query()
                    {
                        Filter = $@"i => (i.JudgementTypeEn != null && i.JudgementTypeEn.ToString().ToLower().Contains(@0)) || 
                                         (i.CANNumber != null && i.CANNumber.ToString().ToLower().Contains(@0)) || 
                                         (i.CourtEn != null && i.CourtEn.ToString().ToLower().Contains(@0)) || 
                                         (i.CourtAr != null && i.CourtAr.ToString().ToLower().Contains(@0)) || 
                                         (i.ChamberNameEn != null && i.ChamberNameEn.ToString().ToLower().Contains(@0)) || 
                                         (i.ChamberNameAr != null && i.ChamberNameAr.ToString().ToLower().Contains(@0)) || 
                                         (i.NumberOfPrinciple != null && i.NumberOfPrinciple.ToString().Contains(@0)) || 
                                         (i.ChamberNumber != null && i.ChamberNumber.ToString().Contains(@0)) || 
                                         (i.JudgementDate.HasValue && i.JudgementDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) ||
                                         (i.JudgementTypeAr != null && i.JudgementTypeAr.ToString().ToLower().Contains(@0)) || 
                                         (i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search, search, search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredSupremeJudgementDocuments = await gridSearchExtension.Sort(FilteredSupremeJudgementDocuments, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        protected async Task OnOtherJudgementSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredOthersJudgementDocuments = await gridSearchExtension.Filter(OthersJudgementDocuments, new Query()
                    {
                        Filter = $@"i => (i.OtherAttachmentType != null && i.OtherAttachmentType.ToString().ToLower().Contains(@0)) || 
                             (i.NoOfPrinciples != null && i.NoOfPrinciples.ToString().ToLower().Contains(@0)) || 
                             (i.DocumentDate.HasValue && i.DocumentDate.Value.ToString(""dd/MM/yyyy"").Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredOthersJudgementDocuments = await gridSearchExtension.Sort(FilteredOthersJudgementDocuments, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        protected async Task OnLegalAdviceJudgementSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredLegalAdviceDocuments = await gridSearchExtension.Filter(LegalAdviceJudgementDocuments, new Query()
                    {
                        Filter = $@"i => (i.FinalDocFileName != null && i.FinalDocFileName.ToString().ToLower().Contains(@0)) || 
                                         (i.NoOfPrinciples != null && i.NoOfPrinciples.ToString().ToLower().Contains(@0)) || 
                                         (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) ||    
                                         (i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredLegalAdviceDocuments = await gridSearchExtension.Sort(FilteredLegalAdviceDocuments, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        #region Grid Pagination Calculation
        private void SetPagingProperties<T>(LoadDataArgs args, int ListTotalCount, RadzenDataGrid<T> grid)
        {
            if (advanceSearch.PageSize != null && advanceSearch.PageSize != CurrentPageSize)
            {
                int oldPageCount = ListTotalCount > 0 ? (ListTotalCount) / ((int)advanceSearch.PageSize) : 1;
                int oldPageNumber = (int)advanceSearch.PageNumber - 1;
                advanceSearch.isGridLoaded = true;
                advanceSearch.PageNumber = CurrentPage;
                advanceSearch.PageSize = args.Top;
                int TotalPages = ListTotalCount > 0 ? (ListTotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearch.PageNumber = TotalPages + 1;
                    advanceSearch.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((advanceSearch.PageNumber == 1 || (advanceSearch.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearch.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearch.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearch.PageNumber = CurrentPage;
            advanceSearch.PageSize = args.Top;
        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        protected async Task PopulateCourts()
        {
            var response = await lookupService.GetCourt();
            if (response.IsSuccessStatusCode)
            {
                courts = (List<Court>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task OnChangeChamberName(int? chamberId)
        {
            if (chamberId is not null)
            {
                var response = await lookupService.GetChamberNumbersByChamberId((int)chamberId);
                if (response.IsSuccessStatusCode)
                {
                    ChamberNumbers = (List<ChamberNumber>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
            }
            else
            {
                ChamberNumbers = new List<ChamberNumber>();
                advanceSearch.ChamberNumberId = null;
            }
        }

        protected async Task PopulateChambers()
        {
            var response = await lookupService.GetChamber();
            if (response.IsSuccessStatusCode)
            {
                chambers = (List<Chamber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }


        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        #endregion

        #region Kay Document List & Search/Advance Search


        protected async Task OnSearchKayDocumentsInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredKayDocumentList = await gridSearchExtension.Filter(KayDocumentsList, new Radzen.Query()
                    {
                        Filter = $@"i => (i.EditionNumber != null && i.EditionNumber.ToString().ToLower().Contains(@0)) 
                    || (i.PublicationDate.HasValue && i.PublicationDate.Value.ToString(""dd/MM/yyyy"").Contains(@0))    
                    || (i.PublicationDateHijri != null && i.PublicationDateHijri.ToString().ToLower().Contains(@0)) 
                    || (i.NoOfPrinciples != null && i.NoOfPrinciples.ToString().ToLower().Contains(@0)) 
                    || (i.DocumentTitle != null && i.DocumentTitle.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredKayDocumentList = await gridSearchExtension.Sort(FilteredKayDocumentList, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        #region Advance Search Latest

        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearch.FromDate > advanceSearch.ToDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                keywords = true;
                return;
            }
            if (advanceSearch.JudgementTypeId == null && advanceSearch.ChamberId == null && advanceSearch.ChamberNumberId == null && advanceSearch.CourtId == null && string.IsNullOrWhiteSpace(advanceSearch.CaseNumber) && string.IsNullOrWhiteSpace(advanceSearch.CANNumber)
                && advanceSearch.DocumentTitle == null && advanceSearch.EditionNumber == null
                && !advanceSearch.FromDate.HasValue && !advanceSearch.ToDate.HasValue)
            {
            }
            else
            {
                keywords = advanceSearch.isDataSorted = true;
                switch (selectedIndex)
                {
                    case (int)JudgementsTabsEnums.AppealJudgements:
                        if (AppealJudgementDocumentGrid.CurrentPage > 0)
                            await AppealJudgementDocumentGrid.FirstPage();
                        else
                        {
                            advanceSearch.isGridLoaded = false;
                            await AppealJudgementDocumentGrid.Reload();
                        }
                        break;

                    case (int)JudgementsTabsEnums.SupremeJudgements:
                        if (SupremeJudgementDocumentGrid.CurrentPage > 0)
                            await SupremeJudgementDocumentGrid.FirstPage();
                        else
                        {
                            advanceSearch.isGridLoaded = false;
                            await SupremeJudgementDocumentGrid.Reload();
                        }
                        break;

                    case (int)JudgementsTabsEnums.LegalAdvice:
                        if (LegalAdviceDocumentGrid.CurrentPage > 0)
                            await LegalAdviceDocumentGrid.FirstPage();
                        else
                        {
                            advanceSearch.isGridLoaded = false;
                            await LegalAdviceDocumentGrid.Reload();
                        }
                        break;

                    case (int)JudgementsTabsEnums.KuwaitAlYawm:
                        if (KayFileGrid.CurrentPage > 0)
                            await KayFileGrid.FirstPage();
                        else
                        {
                            advanceSearch.isGridLoaded = false;
                            await KayFileGrid.Reload();
                        }
                        break;

                    case (int)JudgementsTabsEnums.Others:
                        if (OthersFileGrid.CurrentPage > 0)
                            await OthersFileGrid.FirstPage();
                        else
                        {
                            advanceSearch.isGridLoaded = false;
                            await OthersFileGrid.Reload();
                        }
                        break;

                    default:
                        await AppealJudgementDocumentGrid.Reload();
                        await KayFileGrid.Reload();
                        break;
                }
                StateHasChanged();
            }
        }
        protected async void ToggleAdvanceSearch()
        {
            if (selectedIndex == (int)JudgementsTabsEnums.AppealJudgements || selectedIndex == (int)JudgementsTabsEnums.SupremeJudgements)
            {
                spinnerService.Show();
                await PopulateCourts();
                await PopulateChambers();
                await PopulateJudgementStatuses();
                spinnerService.Hide();
            }
            InitializeCourtTypeId();
            isVisible = !isVisible;

            if (!isVisible)
            {
                ResetForm();
            }
            StateHasChanged();
        }
        private async void ResetForm()
        {

            advanceSearch = new LLSLegalPrincipalDocumentSearchVM();
            keywords = advanceSearch.isDataSorted = false;
            switch (selectedIndex)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    advanceSearch.PageSize = AppealJudgementDocumentGrid.PageSize;
                    AppealJudgementDocumentGrid.Reset();
                    await AppealJudgementDocumentGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.SupremeJudgements:
                    advanceSearch.PageSize = SupremeJudgementDocumentGrid.PageSize;
                    SupremeJudgementDocumentGrid.Reset();
                    await SupremeJudgementDocumentGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.LegalAdvice:
                    advanceSearch.PageSize = LegalAdviceDocumentGrid.PageSize;
                    LegalAdviceDocumentGrid.Reset();
                    await LegalAdviceDocumentGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    advanceSearch.PageSize = KayFileGrid.PageSize;
                    KayFileGrid.Reset();
                    await KayFileGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.Others:
                    advanceSearch.PageSize = OthersFileGrid.PageSize;
                    OthersFileGrid.Reset();
                    await OthersFileGrid.Reload();
                    break;
            }
            StateHasChanged();
        }
        protected async Task PopulateFileTypes()
        {
            try
            {
                foreach (LLSLegalPrincipleSourceFileTypeEnum item in System.Enum.GetValues(typeof(LLSLegalPrincipleSourceFileTypeEnum)))
                {
                    FileTypes.Add(new FileTypeEnumTemp { FileTypeEnumName = translationState.Translate(item.ToString()), FileTypeEnumValue = (int)item });
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
        protected async Task PopulateJudgementStatuses()
        {
            var response = await lookupService.GetCaseJudgementStatuses();
            if (response.IsSuccessStatusCode)
            {
                judgementStatuses = (List<JudgementStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public void InitializeCourtTypeId()
        {
            switch (selectedIndex)
            {
                case 0:
                    courtTypeId = (int)CourtTypeEnum.Appeal;
                    break;
                case 1:
                    courtTypeId = (int)CourtTypeEnum.Supreme;
                    break;
                default:
                    break;
            }
            StateHasChanged();
        }
        public void OnChangeCourt()
        {
            advanceSearch.ChamberId = null;
            advanceSearch.ChamberNumberId = null;
            ChamberNumbers = new List<ChamberNumber>();

        }
        #endregion
        #region  Set current Page

        private void SetGridPageValues()
        {
            switch (selectedIndex)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    CurrentPageSize = AppealJudgementDocumentGrid.PageSize;
                    CurrentPage = AppealJudgementDocumentGrid.CurrentPage + 1;
                    break;

                case (int)JudgementsTabsEnums.SupremeJudgements:
                    CurrentPageSize = SupremeJudgementDocumentGrid.PageSize;
                    CurrentPage = SupremeJudgementDocumentGrid.CurrentPage + 1;

                    break;

                case (int)JudgementsTabsEnums.LegalAdvice:
                    CurrentPageSize = LegalAdviceDocumentGrid.PageSize;
                    CurrentPage = LegalAdviceDocumentGrid.CurrentPage + 1;
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    CurrentPageSize = KayFileGrid.PageSize;
                    CurrentPage = KayFileGrid.CurrentPage + 1;
                    break;

                case (int)JudgementsTabsEnums.Others:
                    CurrentPageSize = OthersFileGrid.PageSize;
                    CurrentPage = OthersFileGrid.CurrentPage + 1;
                    break;
            }
        }

        #endregion
        #region On Change Tab
        public async void OnChangeTab(int index)
        {
            if (index == selectedIndex) { return; }
            search = ColumnName = string.Empty;
            selectedIndex = index;
            advanceSearch = new LLSLegalPrincipalDocumentSearchVM();
            isVisible = keywords = false;
            await Task.Delay(100);

            switch (selectedIndex)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    advanceSearch.PageSize = AppealJudgementDocumentGrid.PageSize;
                    AppealJudgementDocumentGrid.Reset();
                    await AppealJudgementDocumentGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.SupremeJudgements:
                    advanceSearch.PageSize = SupremeJudgementDocumentGrid.PageSize;
                    SupremeJudgementDocumentGrid.Reset();
                    await SupremeJudgementDocumentGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.LegalAdvice:
                    advanceSearch.PageSize = LegalAdviceDocumentGrid.PageSize;
                    LegalAdviceDocumentGrid.Reset();
                    await LegalAdviceDocumentGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    advanceSearch.PageSize = KayFileGrid.PageSize;
                    KayFileGrid.Reset();
                    await KayFileGrid.Reload();
                    break;

                case (int)JudgementsTabsEnums.Others:
                    advanceSearch.PageSize = OthersFileGrid.PageSize;
                    OthersFileGrid.Reset();
                    await OthersFileGrid.Reload();
                    break;
            }

            if (selectedDocuments != null)
                selectedDocuments = new LLSLegalPrincipleDocumentVM();
            if (kayselectedDocuments != null && kayselectedDocuments.Any())
                kayselectedDocuments = new List<LLSLegalPrincipleKuwaitAlYoumDocuments>();
        }

        #endregion

        #region View Document

        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }

        //<History Author = 'Abu Zar' Date='2024-03-21' Version="1.0" Branch="master"> Open Document in New Window</History>
        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            await JsInterop.InvokeVoidAsync("openNewWindow", "/preview-document/" + PreviewedDocumentId);
        }
        List<int> abc = new List<int> { 1, 2, 3 };
        protected void AddPrinicipleBtn()
        {
            try
            {
                if (dataCommunicationService.selectedJudmentDocumentsList != null)
                {
                    navigationManager.NavigateTo("add-llslegalprinciple/" + PreviewedDocumentId + "/" + selectedIndex);
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
        protected async Task LinkPrinicipalContentBtn()
        {
            navigationManager.NavigateTo($"/link-llslegalprinciplecontent/" + PreviewedDocumentId + "/" + selectedIndex);
        }
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            DisplayDocumentViewer = false;
            StateHasChanged();
        }
        protected async Task ViewAttachement(dynamic theUpdatedItem)
        {
            try
            {
                if (theUpdatedItem != null)
                {
                    dataCommunicationService.selectedLegalPrincipleDocumentTab = selectedIndex;
                    switch (dataCommunicationService.selectedLegalPrincipleDocumentTab)
                    {
                        case (int)JudgementsTabsEnums.AppealJudgements:
                            dataCommunicationService.selectedJudmentDocumentsList = new LLSLegalPrincipleDocumentVM();
                            dataCommunicationService.selectedJudmentDocumentsList = theUpdatedItem;
                            break;

                        case (int)JudgementsTabsEnums.SupremeJudgements:
                            dataCommunicationService.selectedJudmentDocumentsList = new LLSLegalPrincipleDocumentVM();
                            dataCommunicationService.selectedJudmentDocumentsList = theUpdatedItem;
                            break;

                        case (int)JudgementsTabsEnums.LegalAdvice:
                            dataCommunicationService.selectedLegalAdviceDocument = new LLSLegalPrinciplLegalAdviceDocumentVM();
                            dataCommunicationService.selectedLegalAdviceDocument = theUpdatedItem;
                            break;

                        case (int)JudgementsTabsEnums.KuwaitAlYawm:
                            dataCommunicationService.selectedKuwaitAlYawmDocument = new LLSLegalPrincipleKuwaitAlYoumDocuments();
                            dataCommunicationService.selectedKuwaitAlYawmDocument = theUpdatedItem;
                            break;

                        case (int)JudgementsTabsEnums.Others:
                            dataCommunicationService.selectedOtherDocument = new LLSLegalPrinciplOtherDocumentVM();
                            dataCommunicationService.selectedOtherDocument = theUpdatedItem;
                            break;
                        default:
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Judgment_Document_Not_Loaded"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            navigationManager.NavigateTo("llslegalprincipledocuments-list");
                            return;
                    }

                    PreviewedDocumentId = theUpdatedItem.UploadedDocumentId != null ? theUpdatedItem.UploadedDocumentId : theUpdatedItem.AttachmentTypeId;
                }
                ToolbarItems = new List<ToolbarItem>()
                {
                    ToolbarItem.PageNavigationTool,
                    ToolbarItem.MagnificationTool,
                    ToolbarItem.SelectionTool,
                    ToolbarItem.PanTool,
                    ToolbarItem.SearchOption,
                    ToolbarItem.SearchOption,
                    ToolbarItem.PrintOption,
                    ToolbarItem.DownloadOption
                };
                DisplayDocumentViewer = false;
                StateHasChanged();
                var physicalPath = string.Empty;
#if DEBUG
                {
                    if (selectedIndex == (int)JudgementsTabsEnums.KuwaitAlYawm)
                        physicalPath = Path.Combine(theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    else
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");


                }
#else
                {
                    if (selectedIndex == (int)JudgementsTabsEnums.KuwaitAlYawm)
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("kay_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                        physicalPath = physicalPath.Replace(_config.GetValue<string>("KayPublicationsPath"), "");
                    }
                    else
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                        physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                    }
					
				}
#endif
                if (!string.IsNullOrEmpty(physicalPath))
                {

                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                    StateHasChanged();

                }
                else
                {

                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
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

        #region On Sort Grid Data
        private async Task OnSortDataAppealJudgements(DataGridColumnSortEventArgs<LLSLegalPrincipleDocumentVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredAppealJudgementDocuments = await gridSearchExtension.Sort(FilteredAppealJudgementDocuments, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearch.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }

        private async Task OnSortDataSupremeJudgements(DataGridColumnSortEventArgs<LLSLegalPrincipleDocumentVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredSupremeJudgementDocuments = await gridSearchExtension.Sort(FilteredSupremeJudgementDocuments, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearch.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }

        private async Task OnSortDataLegalAdvice(DataGridColumnSortEventArgs<LLSLegalPrinciplLegalAdviceDocumentVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredLegalAdviceDocuments = await gridSearchExtension.Sort(FilteredLegalAdviceDocuments, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearch.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }

        private async Task OnSortDataKuwaitAlYoum(DataGridColumnSortEventArgs<LLSLegalPrincipleKuwaitAlYoumDocuments> args)
        {
            if (args.SortOrder != null)
            {
                FilteredKayDocumentList = await gridSearchExtension.Sort(FilteredKayDocumentList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearch.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        private async Task OnSortDataOthers(DataGridColumnSortEventArgs<LLSLegalPrinciplOtherDocumentVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredOthersJudgementDocuments = await gridSearchExtension.Sort(FilteredOthersJudgementDocuments, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearch.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion
        protected async void ReloadOthersDocumentList(bool isUploaded)
        {
            if (isUploaded)
                await OthersFileGrid.Reload();
        }
    }
}

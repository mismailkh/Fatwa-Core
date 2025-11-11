using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_WEB.Pages.Shared;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Globalization;
using System.Text;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.Consultation.ConsultationFiles
{
    public partial class ListConsultationFile : ComponentBase
    {

        #region Varriables Declaration
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected AdvanceSearchConsultationRequestVM advanceSearchCOMSVM { get; set; } = new AdvanceSearchConsultationRequestVM();
        protected AdvanceSearchConsultationCaseFile advanceSearchVM = new AdvanceSearchConsultationCaseFile();
        protected RadzenDataGrid<ConsultationFileListVM>? consultationFileListGrid = new RadzenDataGrid<ConsultationFileListVM>();
        protected RadzenDataGrid<ConsultationDraftedRequestListVM>? consultationDraftedRequestListGrid = new RadzenDataGrid<ConsultationDraftedRequestListVM>();
        protected int pendingRequestsCount { get; set; }
        IEnumerable<ConsultationFileListVM> consultationFileListVM { get; set; } = new List<ConsultationFileListVM>();
        protected IEnumerable<ConsultationDraftedRequestListVM> comsDraftedRequestList { get; set; } = new List<ConsultationDraftedRequestListVM>();

        IEnumerable<ConsultationDraftedRequestListVM> filteredDraftedRequestList { get; set; }
        protected IEnumerable<ConsultationFileListVM> filteredconsultationFileListVM { get; set; }
        public int TypeId { get; set; } = 0;
        public bool allowRowSelectOnRowClick = true;
        protected RadzenDataGrid<ConsultationFileHistoryVM> HistoryGrid;
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();
        protected RadzenDataGrid<ConsultationDraftedRequestListVM>? gridDraftedRequests;
        public bool isdraftedVisible { get; set; }
        private int SelectedTabIndex { get; set; } = 0;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        /*private int CurrentPage => SelectedTabIndex == 0 ? (consultationFileListGrid.CurrentPage + 1) : (consultationDraftedRequestListGrid.CurrentPage + 1);
        private int CurrentPageSize => consultationFileListGrid.PageSize;*/

        private int CurrentPage { get; set; }
        private int CurrentPageSize { get; set; }

        protected string search { get; set; }
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateGovtEntities();
            translationState.TranslateGridFilterLabels(consultationFileListGrid);
            translationState.TranslateGridFilterLabels(consultationDraftedRequestListGrid);
            spinnerService.Hide();
        }
        #endregion

        #region Consultation Files

        #region On Load Consultation Files Data
        protected async Task OnLoadConsultationFilesData(LoadDataArgs args)
        {
            try
            {
                CurrentPage = consultationFileListGrid.CurrentPage + 1;
                CurrentPageSize = consultationFileListGrid.PageSize;
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        consultationFileListGrid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args, consultationFileListVM.Any() ? consultationFileListVM.First().TotalCount : 0, consultationFileListGrid);
                    spinnerService.Show();
                    advanceSearchVM.UserId = loginState.UserDetail.UserId;
                    advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;

                    var response = await consultationFileService.GetConsultationFileList(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        filteredconsultationFileListVM = consultationFileListVM = (IEnumerable<ConsultationFileListVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchConsultationFilesData();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            filteredconsultationFileListVM = await gridSearchExtension.Sort(filteredconsultationFileListVM, ColumnName, SortOrder);
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
        #endregion

        #region On Sort Consultation Files Data
        private async Task OnSortConsultationFilesData(DataGridColumnSortEventArgs<ConsultationFileListVM> args)
        {
            if (args.SortOrder != null)
            {
                filteredconsultationFileListVM = await gridSearchExtension.Sort(filteredconsultationFileListVM, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Consultation Files Grid Search
        protected async Task OnSearchConsultationFilesData()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                filteredconsultationFileListVM = await gridSearchExtension.Filter(consultationFileListVM, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                        ? $@"i => (i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovernmentEntityNameEn != null && i.GovernmentEntityNameEn.ToString().ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.ComplainantName != null && i.ComplainantName.ToString().ToLower().Contains(@4))
                            || (i.StatusEn != null && i.StatusEn.ToString().ToLower().Contains(@5))
                            || (i.LastActionEn != null && i.LastActionEn.ToString().ToLower().Contains(@6))
                            || (i.LawyerNameEn != null && i.LawyerNameEn.ToString().ToLower().Contains(@7))"

                        : $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovernmentEntityNameAr != null && i.GovernmentEntityNameAr.ToString().ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.ComplainantName != null && i.ComplainantName.ToString().ToLower().Contains(@4))
                            || (i.StatusAr != null && i.StatusAr.ToString().ToLower().Contains(@5))
                            || (i.LastActionAr != null && i.LastActionAr.ToString().ToLower().Contains(@6))
                            || (i.LawyerNameAr != null && i.LawyerNameAr.ToString().ToLower().Contains(@7))",
                    FilterParameters = new object[] { RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search) }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    filteredconsultationFileListVM = await gridSearchExtension.Sort(filteredconsultationFileListVM, ColumnName, SortOrder);
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

        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties<T>(LoadDataArgs args, int ListTotalCount, RadzenDataGrid<T> grid)
        {
            if (SelectedTabIndex == 0)
            {
                if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
                {
                    int oldPageCount = ListTotalCount > 0 ? (ListTotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                    int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                    advanceSearchVM.isGridLoaded = true;
                    advanceSearchVM.PageNumber = CurrentPage;
                    advanceSearchVM.PageSize = args.Top;
                    int TotalPages = ListTotalCount > 0 ? (ListTotalCount) / (grid.PageSize) : 1;
                    if (CurrentPage > TotalPages)
                    {
                        advanceSearchVM.PageNumber = TotalPages + 1;
                        advanceSearchVM.PageSize = args.Top;
                        grid.CurrentPage = TotalPages;
                    }
                    if ((advanceSearchVM.PageNumber == 1 || (advanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                    {
                        advanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                    }
                    else
                    {
                        advanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                    }
                    return;
                }
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
            }
            else
            {
                if (advanceSearchCOMSVM.PageSize != null && advanceSearchCOMSVM.PageSize != CurrentPageSize)
                {
                    int oldPageCount = ListTotalCount > 0 ? (ListTotalCount) / ((int)advanceSearchCOMSVM.PageSize) : 1;
                    int oldPageNumber = (int)advanceSearchCOMSVM.PageNumber - 1;
                    advanceSearchCOMSVM.isGridLoaded = true;
                    advanceSearchCOMSVM.PageNumber = CurrentPage;
                    advanceSearchCOMSVM.PageSize = args.Top;
                    int TotalPages = ListTotalCount > 0 ? (ListTotalCount) / (grid.PageSize) : 1;
                    if (CurrentPage > TotalPages)
                    {
                        advanceSearchCOMSVM.PageNumber = TotalPages + 1;
                        advanceSearchCOMSVM.PageSize = args.Top;
                        grid.CurrentPage = TotalPages;
                    }
                    if ((advanceSearchCOMSVM.PageNumber == 1 || (advanceSearchCOMSVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                    {
                        advanceSearchCOMSVM.isPageSizeChangeOnFirstLastPage = true;
                    }
                    else
                    {
                        advanceSearchCOMSVM.isPageSizeChangeOnFirstLastPage = false;
                    }
                    return;
                }
                advanceSearchCOMSVM.PageNumber = CurrentPage;
                advanceSearchCOMSVM.PageSize = args.Top;
            }
        }
        #endregion

        #region Drafted Consultation Files

        #region On Load Drafted Consultation Request Data
        protected async Task OnLoadDataDraftedRequest(LoadDataArgs args)
        {
            try
            {
                CurrentPage = consultationDraftedRequestListGrid.CurrentPage + 1;
                CurrentPageSize = consultationDraftedRequestListGrid.PageSize;
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchCOMSVM.PageNumber || CurrentPageSize != advanceSearchCOMSVM.PageSize || (Keywords && advanceSearchCOMSVM.isDataSorted))
                {
                    if (advanceSearchCOMSVM.isGridLoaded && advanceSearchCOMSVM.PageSize == CurrentPageSize && !advanceSearchCOMSVM.isPageSizeChangeOnFirstLastPage)
                    {
                        consultationDraftedRequestListGrid.CurrentPage = (int)advanceSearchCOMSVM.PageNumber - 1;
                        advanceSearchCOMSVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args, comsDraftedRequestList.Any() ? comsDraftedRequestList.First().TotalCount : 0, consultationDraftedRequestListGrid);
                    spinnerService.Show();
                    advanceSearchCOMSVM.userDetail = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                    advanceSearchCOMSVM.StatusId = (int)CaseRequestStatusEnum.Draft;

                    var response = await consultationFileService.GetDraftedConsultationRequestList(advanceSearchCOMSVM);
                    if (response.IsSuccessStatusCode)
                    {
                        comsDraftedRequestList = (List<ConsultationDraftedRequestListVM>)response.ResultData;
                        filteredDraftedRequestList = (List<ConsultationDraftedRequestListVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInputDraftedFiles();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            filteredDraftedRequestList = await gridSearchExtension.Sort(filteredDraftedRequestList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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
            spinnerService.Hide();
        }
        #endregion

        #region On Sort Drafted Request Data
        private async Task OnSortDraftedRequest(DataGridColumnSortEventArgs<ConsultationDraftedRequestListVM> args)
        {
            if (args.SortOrder != null)
            {
                filteredDraftedRequestList = await gridSearchExtension.Sort(filteredDraftedRequestList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Drafted Consultation Files Grid Search
        protected async Task OnSearchInputDraftedFiles()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                filteredDraftedRequestList = await gridSearchExtension.Filter(comsDraftedRequestList, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                        ? $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovtEntityEn != null && i.GovtEntityEn.ToString().ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.ComplainantName != null && i.ComplainantName.ToString().ToLower().Contains(@4))
                            || (i.Status_Name_En != null && i.Status_Name_En.ToString().ToLower().Contains(@5))
                            || (i.LastActionEn != null && i.LastActionEn.ToString().ToLower().Contains(@6))"

                        : $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovtEntityAr != null && i.GovtEntityAr.ToString().ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.ComplainantName != null && i.ComplainantName.ToString().ToLower().Contains(@4))
                            || (i.Status_Name_Ar != null && i.Status_Name_Ar.ToString().ToLower().Contains(@5))
                            || (i.LastActionAr != null && i.LastActionAr.ToString().ToLower().Contains(@6))",
                    FilterParameters = new object[] { RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search) }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    filteredDraftedRequestList = await gridSearchExtension.Sort(filteredDraftedRequestList, ColumnName, SortOrder);
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

        #endregion

        #region Advance Searches

        #region Consultation Files Advance Search
        protected async Task OnSubmitConsultationFileAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (string.IsNullOrEmpty(advanceSearchVM.FileNumber)
                && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue) { }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (consultationFileListGrid.CurrentPage > 0)
                    await consultationFileListGrid.FirstPage();
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await consultationFileListGrid.Reload();
                }
                StateHasChanged();
            }
        }
        public async void OnResetComsFileListAdvanceSearch()
        {
            advanceSearchVM = new AdvanceSearchConsultationCaseFile { PageSize = consultationFileListGrid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            consultationFileListGrid.Reset();
            await consultationFileListGrid.Reload();
        }
        protected async Task OnToggleComsFileListAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                OnResetComsFileListAdvanceSearch();
            }
        }
        #endregion

        #region Drafted Coms Files Advance Search
        protected async Task SubmitDraftedRequestAdvanceSearch()
        {
            if (advanceSearchCOMSVM.RequestFrom > advanceSearchCOMSVM.RequestTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }

            if (string.IsNullOrEmpty(advanceSearchCOMSVM.RequestNumber) && !advanceSearchCOMSVM.StatusId.HasValue
                && !advanceSearchCOMSVM.RequestFrom.HasValue && !advanceSearchCOMSVM.RequestTo.HasValue) { }
            else
            {
                Keywords = advanceSearchCOMSVM.isDataSorted = true;
                if (consultationDraftedRequestListGrid.CurrentPage > 0)
                    await consultationDraftedRequestListGrid.FirstPage();
                else
                {
                    advanceSearchCOMSVM.isGridLoaded = false;
                    await consultationDraftedRequestListGrid.Reload();
                }
                StateHasChanged();
            }
        }
        public async void ResetDraftedListForm()
        {
            advanceSearchCOMSVM = new AdvanceSearchConsultationRequestVM { PageSize = consultationDraftedRequestListGrid.PageSize };

            await PopulateGovtEntities();
            Keywords = advanceSearchCOMSVM.isDataSorted = false;
            if (consultationDraftedRequestListGrid.CurrentPage > 0)
                await consultationDraftedRequestListGrid.FirstPage();
            else
            {
                advanceSearchCOMSVM.isGridLoaded = false;
                await consultationDraftedRequestListGrid.Reload();
            }
        }
        protected async Task ToggleDraftedListAdvanceSearch()
        {
            isdraftedVisible = !isdraftedVisible;
            if (!isdraftedVisible)
            {
                ResetDraftedListForm();

            }
        }
        #endregion

        #endregion

        #region Remove Diacraite
        protected string RemoveDiacritics(string text)
        {
            // Use CultureInfo.InvariantCulture for consistent comparison
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Remove diacritic marks from the text
            return new string(
                text.Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray()
            ).Normalize(NormalizationForm.FormC);
        }
        #endregion

        #region Grid Buttons
        protected async Task DetailConsultationFile(ConsultationFileListVM args)
        {
            args.SectorTypeId = advanceSearchVM.SectorTypeId;
            navigationManager.NavigateTo("consultationfile-view/" + args.FileId + "/" + args.SectorTypeId);
        }
        protected async Task DetailConsultationRequest(ConsultationDraftedRequestListVM args)
        {
            args.SectorTypeId = advanceSearchVM.SectorTypeId;
            navigationManager.NavigateTo("consultationrequest-detail/" + args.ConsultationRequestId + "/" + args.SectorTypeId);
        }
        protected async Task EditConsultationRequest(ConsultationDraftedRequestListVM args)
        {
            navigationManager.NavigateTo("consultationrequest-add/" + args.ConsultationRequestId);
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

        #region Populate Govt Entities
        //<History Author = 'Muhammad Zaeem' Date='2024-24-10' Version="1.0" Branch="master">Populate Govt Entites</History>
        protected async Task PopulateGovtEntities()
        {
            try
            {
                var response = await lookupService.GetGovernmentEntities();
                if (response.IsSuccessStatusCode)
                {
                    GovtEntities = (List<GovernmentEntity>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
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

        #region Load Hisory for a File
        //<History Author = 'Muhammad Zaeem ' Date='2023-13-04' Version="1.0" Branch="master"> Load Hsitory for a File </History>

        protected async Task ExpandHistory(ConsultationFileListVM file)
        {
            var response = await consultationFileService.GetConslutationFileStatusHistoryForList((Guid)file.FileId);
            if (response.IsSuccessStatusCode)
            {
                file.LatestHistory = (List<ConsultationFileHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }


        }
        #endregion

        #region Add Consultation Request

        //<History Author = 'Hassan Abbas' Date='2022-02-28' Version="1.0" Branch="master"> Redirect to Add case request wizard</History>
        protected async Task AddConsultationRequest(MouseEventArgs args)
        {
            var RequestTypeId = await SelectRequestTypeIdBySectorId();
            if (RequestTypeId > 0)
            {
                var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Select_Request_Type"),
                      new Dictionary<string, object>()
                      {
                           { "IsViewOnly", false },
                           { "IsUploadPopup", true },
                           { "FileTypes", new List<string>() { ".pdf" } },
                           { "MaxFileSize", systemSettingState.File_Maximum_Size },
                           { "Multiple", false },
                           { "IsCaseRequest", true },
                           { "RequestTypeId", RequestTypeId },
                           { "IsRequestTypeSelection", true },
                      }
                      ,
                      new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            }

        }

        #endregion

        #region Set Request Type Id
        protected async Task<int> SelectRequestTypeIdBySectorId()
        {
            try
            {
                int requestTypeId = 0;

                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
                {
                    requestTypeId = (int)RequestTypeEnum.LegalAdvice;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
                {
                    requestTypeId = (int)RequestTypeEnum.Legislations;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
                {
                    requestTypeId = (int)RequestTypeEnum.Contracts;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
                {
                    requestTypeId = (int)RequestTypeEnum.InternationalArbitration;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                {
                    requestTypeId = (int)RequestTypeEnum.AdministrativeComplaints;
                }
                return requestTypeId;
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return 0;

            }

        }
        #endregion

        #region On Tab Change
        protected async Task OnTabChange(int index)
        {
            if (index == SelectedTabIndex) { return; }
            search = ColumnName = string.Empty;
            SelectedTabIndex = index;
            isVisible = isdraftedVisible = Keywords = false;
            await Task.Delay(100);
            search = string.Empty;

            if (SelectedTabIndex == 0)
            {
                advanceSearchVM = new AdvanceSearchConsultationCaseFile { PageSize = consultationFileListGrid.PageSize };
                consultationFileListGrid.Reset();
                await consultationFileListGrid.Reload();
            }
            else
            {
                advanceSearchCOMSVM = new AdvanceSearchConsultationRequestVM { PageSize = consultationDraftedRequestListGrid.PageSize };
                consultationDraftedRequestListGrid.Reset();
                await consultationDraftedRequestListGrid.Reload();
            }
            StateHasChanged();
        }
        #endregion
    }
}

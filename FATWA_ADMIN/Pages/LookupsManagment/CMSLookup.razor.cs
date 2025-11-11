using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

//< History Author = 'Ammaar Naveed' Date = '2024-06-04' Version = "1.0" Branch = "master"> UI fixations in pages and dialogs</History>

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class CMSLookup : ComponentBase
    {
        #region Service Injection
        [Inject]
        protected RoleService roleService { get; set; }
        #endregion

        #region Variable Declaration
        public int count { get; set; }
        protected IList<ClaimVM> selectedClaimsList;
        public bool allowRowSelectOnRowClick = true;
        public IEnumerable<UserVM> User = new List<UserVM>();
        public IList<UserVM> SelectUsers;
        public IList<Group> SelectUserGroups;
        public IEnumerable<Group> Grouplist = new List<Group>();
        protected bool isCheckedUser = false;
        IEnumerable<UserVM> _getUmsUserResult;

        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;

        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();
        protected RadzenDataGrid<CourtDetailVM>? CourtDetailGrid = new RadzenDataGrid<CourtDetailVM>();
        protected RadzenDataGrid<ChamberDetailVM>? ChamberDetailGrid = new RadzenDataGrid<ChamberDetailVM>();
        protected RadzenDataGrid<CourtDetailVM>? grid1 = new RadzenDataGrid<CourtDetailVM>();
        protected RadzenDataGrid<RequestTypeVM>? gridRequesttype = new RadzenDataGrid<RequestTypeVM>();
        protected RadzenDataGrid<ChamberDetailVM>? ChamberGrid = new RadzenDataGrid<ChamberDetailVM>();
        protected RadzenDataGrid<DepartmentDetailVM>? DepartmentGrid = new RadzenDataGrid<DepartmentDetailVM>();
        protected RadzenDataGrid<SubTypeVM>? gridSubtype = new RadzenDataGrid<SubTypeVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();
        protected RadzenDataGrid<CaseFileStatusVM>? CaseFileStatusGrid = new RadzenDataGrid<CaseFileStatusVM>();
        protected RadzenDataGrid<CaseRequestStatusVM>? CaseRequestStatusGrid = new RadzenDataGrid<CaseRequestStatusVM>();
        protected RadzenDataGrid<CmsRegisteredCaseStatusVM>? CaseStatusGrid = new RadzenDataGrid<CmsRegisteredCaseStatusVM>();
        protected RadzenDataGrid<ChambersNumberDetailVM>? ChambersNumberGrid = new RadzenDataGrid<ChambersNumberDetailVM>();
        protected RadzenDataGrid<ChamberNumberHearingDetailVM>? ChambersNumberHearingGrid = new RadzenDataGrid<ChamberNumberHearingDetailVM>();
        protected string search { get; set; }

        protected string searchHistory;
        protected string searchSubType;

        private Timer debouncer;
        private const int debouncerDelay = 500;
        protected IEnumerable<SubTypeVM> SubTypeVM = new List<SubTypeVM>();
        protected IEnumerable<UserVM> getUmsUserResult = new List<UserVM>();
        protected IEnumerable<ClaimVM> getGroupClaimsResult = new List<ClaimVM>();

        #endregion

        #region Fuctions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            await GetCourtTypeList();
            await GetChamberList();
            await GetRequestTypeList();
            await GetChamberNumberList();
            await GetChamberNumberHearingList();
            StateHasChanged();
            spinnerService.Hide();
        }
        #endregion

        #region Court type lookup list 
        IEnumerable<CourtDetailVM> CmsCourtG2GLKP;
        protected IEnumerable<CourtDetailVM> FilteredCmsCourtG2GLKP = new List<CourtDetailVM>();


        protected async Task GetCourtTypeList()
        {
            var result = await lookupService.GetCourtTypeList();
            if (result.IsSuccessStatusCode)
            {

                CmsCourtG2GLKP = (IEnumerable<CourtDetailVM>)result.ResultData;
                FilteredCmsCourtG2GLKP = (IEnumerable<CourtDetailVM>)result.ResultData;
                count = FilteredCmsCourtG2GLKP.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredCmsCourtG2GLKP = await gridSearchExtension.Filter(CmsCourtG2GLKP, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US"
                    ? $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) 
                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@1))
                    || (i.Description != null && i.Description.ToLower().Contains(@2))
                    || (i.CourtCode != null && i.CourtCode.ToLower().Contains(@3))
                    || (i.CourtTypeEn != null && i.CourtTypeEn.ToLower().Contains(@4)) 
                    || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@5)))"
                    : $@"i => ((i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@0)) 
                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@1))
                    || (i.Description != null && i.Description.ToLower().Contains(@2))
                    || (i.CourtCode != null && i.CourtCode.ToLower().Contains(@3))
                    || (i.CourtTypeAr != null && i.CourtTypeAr.ToLower().Contains(@4)) 
                    || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@5)))",
                        FilterParameters = new object[] { search, search, search, search, search, search }
                    });await InvokeAsync(StateHasChanged);
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

        #region Chamber list 
        public IList<ChamberDetailVM> selectedChambers;
        IEnumerable<ChamberDetailVM> CmsChamberG2GLKP;
        protected IEnumerable<ChamberDetailVM> FilteredCmsChamberG2GLKP = new List<ChamberDetailVM>();
        protected async Task GetChamberList()
        {

            var result = await lookupService.GetChamberList();
            if (result.IsSuccessStatusCode)
            {

                CmsChamberG2GLKP = (IEnumerable<ChamberDetailVM>)result.ResultData;
                FilteredCmsChamberG2GLKP = (IEnumerable<ChamberDetailVM>)result.ResultData;
                count = CmsChamberG2GLKP.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnChamberSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredCmsChamberG2GLKP = await gridSearchExtension.Filter(CmsChamberG2GLKP, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US"
                    ? $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) 
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@1))
                    || (i.Description != null && i.Description.ToLower().Contains(@2))
                    || (i.ChamberCode != null && i.ChamberCode.ToLower().Contains(@3))
                    || (i.CourtNameEn != null && i.CourtNameEn.ToLower().Contains(@4)) 
                    || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@5)))"
                    : $@"i => ((i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@0)) 
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@1))
                    || (i.Description != null && i.Description.ToLower().Contains(@2))
                    || (i.ChamberCode != null && i.ChamberCode.ToLower().Contains(@3))
                    || (i.CourtNameAr != null && i.CourtNameAr.ToLower().Contains(@4)) 
                    || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@5)))",
                        FilterParameters = new object[] { search, search, search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
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

        #region History Function 
        protected IEnumerable<LookupsHistory> LookupHistoryVM = new List<LookupsHistory>();
        #endregion

        #region Lookups History
        IEnumerable<LookupsHistory> RequestTypeLookupHistory { get; set; } //for request type history
        protected async Task CourtDetailHistoryList(CourtDetailVM courtDetailVM)
        {
            await GetLookupHistoryListByRefernceId(courtDetailVM.Id, (int)LookupsTablesEnum.CMS_COURT_G2G_LKP);
        }
        protected async Task ExpendChamberHistoryList(ChamberDetailVM chamberDetailVM)
        {
            await GetLookupHistoryListByRefernceId(chamberDetailVM.Id, (int)LookupsTablesEnum.CMS_CHAMBER_G2G_LKP);
        }
        protected async Task ExpendDepartmentHistoryList(DepartmentDetailVM departmentDetailVM)
        {
            await GetLookupHistoryListByRefernceId(departmentDetailVM.Id, (int)LookupsTablesEnum.Department);
        }
        protected async Task ExpendCaseFileStatusHistoryList(CaseFileStatusVM caseFileStatusVM)
        {
            await GetLookupHistoryListByRefernceId(caseFileStatusVM.Id, (int)LookupsTablesEnum.CMS_CASE_FILE_STATUS_G2G_LKP);
        }

        protected async Task ExpandRequestTypeHistory(SubTypeVM subTypeVM)
        {
            await GetRequestTypeLookupHistoryListByReferncedId(subTypeVM.RequestTypeId, (int)LookupsTablesEnum.CMS_REQUEST_TYPE_G2G_LKP);
        }
        protected async Task ExpandSubTypeRequestHistory(SubTypeVM subTypeVM)
        {
            await GetLookupHistoryListByRefernceId(subTypeVM.Id, (int)LookupsTablesEnum.CMS_SUBTYPE_G2G_LKP);
        }

        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId) // general
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);
            await GetRequestTypeLookupHistoryListByReferncedId(Id, LookupstableId);

            LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }
        protected async Task GetRequestTypeLookupHistoryListByReferncedId(int Id, int LookupstableId) // specific for request type
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

            RequestTypeLookupHistory = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Chambers Number  List 
        IEnumerable<ChambersNumberDetailVM> ChamberNumberList;
        protected IEnumerable<ChambersNumberDetailVM> FilteredChamberNumberList = new List<ChambersNumberDetailVM>();


        protected async Task GetChamberNumberList()
        {
            var result = await lookupService.GetChamberNumberList();
            if (result.IsSuccessStatusCode)
            {
                ChamberNumberList = (IEnumerable<ChambersNumberDetailVM>)result.ResultData;
                FilteredChamberNumberList = (IEnumerable<ChambersNumberDetailVM>)result.ResultData;
                count = FilteredChamberNumberList.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnChamberNumberSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredChamberNumberList = await gridSearchExtension.Filter(ChamberNumberList, new Query()
                    {

                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US"
                    ? $@"i => ((i.ChamberNamesEn != null && i.ChamberNamesEn.ToLower().Contains(@0)) 
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@1))
                    || (i.Number != null && i.Number.ToLower().Contains(@2))
                    || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@3))
                    || (i.ShiftNameEn != null && i.ShiftNameEn.ToLower().Contains(@4)))"
                    : $@"i => ((i.ChamberNamesAr != null && i.ChamberNamesAr.ToLower().Contains(@0)) 
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@1))
                    || (i.Number != null && i.Number.ToLower().Contains(@2))
                    || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3))
                    || (i.ShiftNameAr != null && i.ShiftNameAr.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    }); await InvokeAsync(StateHasChanged);
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

        #region Chambers Number Hearing Days List 
        IEnumerable<ChamberNumberHearingDetailVM> ChamberHearingDaysList;
        protected IEnumerable<ChamberNumberHearingDetailVM> FilteredChamberHearingDaysList = new List<ChamberNumberHearingDetailVM>();


        protected async Task GetChamberNumberHearingList()
        {
            var result = await lookupService.GetChamberNumberHearingList();
            if (result.IsSuccessStatusCode)
            {
                ChamberHearingDaysList = (IEnumerable<ChamberNumberHearingDetailVM>)result.ResultData;
                FilteredChamberHearingDaysList = (IEnumerable<ChamberNumberHearingDetailVM>)result.ResultData;
                count = FilteredChamberHearingDaysList.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnChamberHearingDaysSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredChamberHearingDaysList = await gridSearchExtension.Filter(ChamberHearingDaysList, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US"
                    ? $@"i => ((i.ChamberNameEn != null && i.ChamberNameEn.ToLower().Contains(@0)) 
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@1))
                    || (i.ChamberNumber != null && i.ChamberNumber.ToLower().Contains(@2))
                    || (i.HearingDaysEn != null && i.HearingDaysEn.ToLower().Contains(@3))
                    || (i.CourtTypeEn != null && i.CourtTypeEn.ToLower().Contains(@4)) 
                    || (i.CourtNameEn != null && i.CourtNameEn.ToLower().Contains(@5)) 
                    || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@6)))"
                    : $@"i => ((i.ChamberNameAr != null && i.ChamberNameAr.ToLower().Contains(@0)) 
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@1))
                    || (i.ChamberNumber != null && i.ChamberNumber.ToLower().Contains(@2))
                    || (i.HearingDaysAr != null && i.HearingDaysAr.ToLower().Contains(@3))
                    || (i.CourtTypeAr != null && i.CourtTypeAr.ToLower().Contains(@4)) 
                    || (i.CourtNameAr != null && i.CourtNameAr.ToLower().Contains(@5)) 
                    || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@6)))",
                        FilterParameters = new object[] { search, search, search, search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
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

        #region Request Type Lists
        protected IEnumerable<RequestTypeVM> FilteredRequestTypeVM { get; set; } = new List<RequestTypeVM>();
        protected IEnumerable<RequestTypeVM> RequestTypeVM = new List<RequestTypeVM>();
        protected async Task OnSearchInputRequestType(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredRequestTypeVM = await gridSearchExtension.Filter(RequestTypeVM, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    });await InvokeAsync(StateHasChanged);
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
        protected async Task GetRequestTypeList()
        {
            var result = await lookupService.GetRequestTypeList();
            if (result.IsSuccessStatusCode)
            {
                RequestTypeVM = (IEnumerable<RequestTypeVM>)result.ResultData;
                FilteredRequestTypeVM = (IEnumerable<RequestTypeVM>)result.ResultData;
                count = RequestTypeVM.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }



        #endregion

        #region Sub Type Lists
        protected IEnumerable<SubTypeVM> FilteredSubTypeVM = new List<SubTypeVM>();

        protected async Task OnFileTypeSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    searchSubType = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredSubTypeVM = await gridSearchExtension.Filter(SubTypeVM, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { searchSubType, searchSubType, searchSubType, searchSubType, searchSubType }
                    });  await InvokeAsync(StateHasChanged);
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
        protected async Task ExpandDraftVersions(RequestTypeVM requestTypeVM)
        {
            await GetSubTypeList(requestTypeVM.Id);
            selectedRequestType = requestTypeVM;
        }
        protected async Task GetSubTypeList(int RequestTypeId)
        {
            var result = await lookupService.GetSubTypeList(RequestTypeId);
            SubTypeVM = (IEnumerable<SubTypeVM>)result.ResultData;
            FilteredSubTypeVM = SubTypeVM;
            count = CmsCourtG2GLKP.Count();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Edit Request Sub type 
        protected async Task EditRequestSubTypeItem(SubTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<RequestSubTypeDetailAdd>(
                translationState.Translate("Edit_Sub_type_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30% !important",
                }) == true)
                {
                    await Task.Delay(100);
                    await GetSubTypeList(selectedRequestType.Id);
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Edit Request  type 
        protected async Task EditRequestTypeItem(RequestTypeVM args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<RequestTypeDetailAdd>(
                translationState.Translate("Edit_Request_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30% !important",
                });
                await Task.Delay(100);
                await Load();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Court, Chamber, Chamber Number and Chamber Number Hearing Action Details and Add SubType 

        #region Court Detail
        #region Add Court 
        protected async Task AddCourtType(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<CourtTypeDetailAdd>(
                   translationState.Translate("Add_Court_Detail"),
                    null,
                   new DialogOptions()
                   {
                       CloseDialogOnOverlayClick = false,
                       CloseDialogOnEsc = false
                   }) == true)
                {
                    await Task.Delay(100);
                    await GetCourtTypeList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #region Edit Court 
        protected async Task EditItem(CourtDetailVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<CourtTypeDetailAdd>(
                translationState.Translate("Edit_Court_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GetCourtTypeList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Delete Court
        protected async Task DeleteItem(CourtDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Court_Type"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteCourtType(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetCourtTypeList();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #region Active court
        protected async Task GridActiveCourtsButtonClicked(CourtDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"),
                translationState.Translate("Status"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    args.IsActive = args.IsActive ? false : true;
                    spinnerService.Show();
                    var response = await lookupService.ActiveCourtTypes(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetCourtTypeList();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion

        #endregion

        #region Chamber Detail
        #region Add Chamber
        protected async Task AddChamber(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<ChamberDetailAdd>(
                    translationState.Translate("Add_Chamber_Detail"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }) == true)
                {
                    await Task.Delay(200);
                    await GetChamberList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #region Assign Sector 
        protected async Task AssignSector(MouseEventArgs args)
        {
            try
            {
                var asdas = selectedChambers.Select(x => x.Id);
                var dialogResult = await dialogService.OpenAsync<ChamberOperatingSectorAdd>(translationState.Translate("Assign_Sector"),
                      new Dictionary<string, object>()
                      {
                            { "SelectedChambers", selectedChambers },
                      },
                      new DialogOptions() { CloseDialogOnOverlayClick = false, Width = "30%" }
                  );
                selectedChambers = null;
                await GetChamberList();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #region Edit Chamber
        protected async Task EditItem(ChamberDetailVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<ChamberDetailAdd>(
                translationState.Translate("Edit_Chamber_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GetChamberList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Delete Chamber 
        protected async Task DeleteItem(ChamberDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Chamber"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteChamber(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetChamberList();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }
        #endregion
        #region Active chamber
        protected async Task GridActiveChamberButtonClicked(ChamberDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"),
                    translationState.Translate("Status"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("Yes"),
                        CancelButtonText = translationState.Translate("No")
                    }) == true)
                {
                    selectedChambers = null;
                    args.IsActive = args.IsActive ? false : true;
                    spinnerService.Show();
                    var response = await lookupService.ActiveChamber(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetChamberList();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
                else
                {
                    selectedChambers = null;
                }
            }
            catch (Exception ex)
            {

                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #endregion

        #region  Chambers Number Detail
        #region Add Chambers Number
        protected async Task AddChambersNumber(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<ChambersNumberAdd>(
                    translationState.Translate("Add_Chamber_Number_Detail"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetChamberNumberList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #region Edit Chamber Number 
        protected async Task EditChambersNumber(ChambersNumberDetailVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<ChambersNumberAdd>(
                translationState.Translate("Edit_Chamber_Number_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GetChamberNumberList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Delete Chambers Number 
        protected async Task DeleteChambersNumber(ChambersNumberDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Chamber_Number"),
                    translationState.Translate("Delete"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteChambersNumber(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetChamberNumberList();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }
        #endregion
        #region Active Chamber Number
        protected async Task ActiveButtonChambersNumber(ChambersNumberDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"),
            translationState.Translate("Status"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
                {
                    args.IsActive = args.IsActive ? false : true;
                    spinnerService.Show();
                    var response = await lookupService.ActiveChambersNumber(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetChamberNumberList();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion

        #endregion

        #region  Chambers Number Hearing Detail
        #region Add Chambers Number Hearing
        protected async Task AddChambersNumberHearing(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<ChamberNumberHearingDetailAdd>(
                    translationState.Translate("Add_Chamber_Number_Hearing_Detail"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetChamberNumberHearingList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #region Edit Chamber Number 
        protected async Task EditChambersNumberHearing(ChamberNumberHearingDetailVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<ChamberNumberHearingDetailAdd>(
                translationState.Translate("Edit_Chamber_Number_Hearing_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GetChamberNumberHearingList();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Delete Chambers Number 
        protected async Task DeleteChambersNumberHearing(ChamberNumberHearingDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Chamber_Number_Hearing"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteChambersNumberHearing(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetChamberNumberHearingList();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #endregion

        #region Add Sub Types
        private RequestTypeVM selectedRequestType;
        protected async Task AddSubtype(RequestTypeVM selectedRequestType)
        {
            try
            {

                if (await dialogService.OpenAsync<SubTypesAdd>(
                    translationState.Translate("Add_Sub_Types"),
                     new Dictionary<string, object>()
                        {
                            { "SelectedRequestType", selectedRequestType }
                        },
                    new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetSubTypeList(selectedRequestType.Id);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion

        #endregion

        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

    }
}

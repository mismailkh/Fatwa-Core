using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class EnumsLists : ComponentBase
    {
        #region Service Injection
        [Inject]
        protected RoleService roleService { get; set; }
        #endregion

        #region Variable Declaration
        public int count { get; set; }
        public bool allowRowSelectOnRowClick = true;
        public bool allowRowSelectOnRowClick1 = true;
        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;
        protected bool isCheckedUser = false;
        public IEnumerable<UserVM> User = new List<UserVM>();
        public IList<UserVM> SelectUsers;
        public IList<Group> SelectUserGroups;
        protected IList<ClaimVM> selectedClaimsList;
        public IEnumerable<Group> Grouplist = new List<Group>();


        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();
        public Group Group = new Group();
        private RequestTypeVM selectedRequestType;

        protected RadzenDataGrid<CommunicationTypeVM>? CommunicationGrid = new RadzenDataGrid<CommunicationTypeVM>();
        protected RadzenDataGrid<CaseFileStatusVM>? CaseFileStatusGrid = new RadzenDataGrid<CaseFileStatusVM>();
        protected RadzenDataGrid<CaseRequestStatusVM>? CaseRequestStatusGrid = new RadzenDataGrid<CaseRequestStatusVM>();
        protected RadzenDataGrid<CmsRegisteredCaseStatusVM>? CaseStatusGrid = new RadzenDataGrid<CmsRegisteredCaseStatusVM>();
        protected RadzenDataGrid<TaskTypeVM>? TaskTypeGrid = new RadzenDataGrid<TaskTypeVM>();
        protected RadzenDataGrid<SubTypeVM>? gridSubtype = new RadzenDataGrid<SubTypeVM>();
        protected RadzenDataGrid<RequestTypeVM>? gridRequesttype = new RadzenDataGrid<RequestTypeVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();

        protected string search { get; set; }


        string _searchSubType;
        protected string searchSubType
        {
            get
            {
                return _searchSubType;
            }
            set
            {
                if (!object.Equals(_searchSubType, value))
                {
                    var args = new FATWA_ADMIN.Services.General.PropertyChangedEventArgs() { Name = "searchSubType", NewValue = value, OldValue = _searchSubType };
                    _searchSubType = value;

                    Reload();
                }
            }
        }

        string _searchHistory;
        protected string searchHistory
        {
            get
            {
                return _searchHistory;
            }
            set
            {
                if (!object.Equals(_searchHistory, value))
                {
                    var args = new FATWA_ADMIN.Services.General.PropertyChangedEventArgs() { Name = "searchHistory", NewValue = value, OldValue = _searchHistory };
                    _searchHistory = value;

                    Reload();
                }
            }
        }

        IEnumerable<UserVM> _getUmsUserResult;
        protected IEnumerable<UserVM> getUmsUserResult
        {
            get
            {
                return _getUmsUserResult;
            }
            set
            {
                if (!object.Equals(_getUmsUserResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getUmsUserResult", NewValue = value, OldValue = _getUmsUserResult };
                    _getUmsUserResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<ClaimVM> _getGroupClaimsResult;
        protected IEnumerable<ClaimVM> getGroupClaimsResult
        {
            get
            {
                return _getGroupClaimsResult;
            }
            set
            {
                if (!object.Equals(_getGroupClaimsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getGroupClaimsResult", NewValue = value, OldValue = _getGroupClaimsResult };
                    _getGroupClaimsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
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

            await GetCommunicationTypeList();
            await GetTaskTypeList();
            await GetRequestTypeList();
            await GetCaseFileStatusList();
            await GetCaseRequestStatusList();
            await GetCaseStatusList();

            StateHasChanged();
            spinnerService.Hide();
        }
        #endregion

        #region Communication Type list 
        IEnumerable<CommunicationTypeVM> _communicationtypelist;
        protected IEnumerable<CommunicationTypeVM> Filteredcommunicationtypelist { get; set; } = new List<CommunicationTypeVM>();
        protected IEnumerable<CommunicationTypeVM> communicationtypelist
        {
            get
            {
                return _communicationtypelist;
            }
            set
            {
                if (!Equals(_communicationtypelist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "communicationtypelist", NewValue = value, OldValue = _communicationtypelist };
                    _communicationtypelist = value;

                    Reload();
                }
            }
        }

        protected async Task GetCommunicationTypeList()
        {
            var result = await lookupService.GetCommunicationTypeList();
            if (result.IsSuccessStatusCode)
            {
                communicationtypelist = (IEnumerable<CommunicationTypeVM>)result.ResultData;
                Filteredcommunicationtypelist = (IEnumerable<CommunicationTypeVM>)result.ResultData;
                count = communicationtypelist.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    Filteredcommunicationtypelist = await gridSearchExtension.Filter(communicationtypelist, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => (i.CommunicationTypeId != null && i.CommunicationTypeId.ToString().ToLower().Contains(@0)) || (i.NameEn != null && i.NameEn.ToLower().Contains(@1)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))" : $@"i => (i.CommunicationTypeId != null && i.CommunicationTypeId.ToString().ToLower().Contains(@0)) || (i.NameAr != null && i.NameAr.ToLower().Contains(@1)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        #endregion 

        #region Case File Status List 
        IEnumerable<CaseFileStatusVM> _casefilestatuslist;
        protected IEnumerable<CaseFileStatusVM> Filteredcasefilestatuslist { get; set; } = new List<CaseFileStatusVM>();
        protected IEnumerable<CaseFileStatusVM> casefilestatuslist
        {
            get
            {
                return _casefilestatuslist;
            }
            set
            {
                if (!Equals(_casefilestatuslist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "casefilestatuslist", NewValue = value, OldValue = _casefilestatuslist };
                    _casefilestatuslist = value;

                    Reload();
                }
            }
        }

        protected async Task GetCaseFileStatusList()
        {
            var result = await lookupService.GetCaseFileStatusList();
            if (result.IsSuccessStatusCode)
            {
                casefilestatuslist = (IEnumerable<CaseFileStatusVM>)result.ResultData;
                Filteredcasefilestatuslist = (IEnumerable<CaseFileStatusVM>)result.ResultData;
                count = casefilestatuslist.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInputCaseFileStatus(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    Filteredcasefilestatuslist = await gridSearchExtension.Filter(casefilestatuslist, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => (i.Id != null && i.Id.ToString().ToLower().Contains(@0)) || (i.Name_En != null && i.Name_En.ToLower().Contains(@1)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@2)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@3) || (i.CreatedDate != null && i.CreatedDate.HasValue != null && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@4)))" : $@"i => (i.Id != null && i.Id.ToString().ToLower().Contains(@0)) || (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@2)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3) || (i.CreatedDate != null && i.CreatedDate.HasValue != null && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        #endregion

        #region Case Request Status List 
        IEnumerable<CaseRequestStatusVM> _caserequeststatuslist;
        protected IEnumerable<CaseRequestStatusVM> Filteredcaserequeststatuslist { get; set; } = new List<CaseRequestStatusVM>();
        protected IEnumerable<CaseRequestStatusVM> caserequeststatuslist
        {
            get
            {
                return _caserequeststatuslist;
            }
            set
            {
                if (!Equals(_caserequeststatuslist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "caserequeststatuslist", NewValue = value, OldValue = _caserequeststatuslist };
                    _caserequeststatuslist = value;

                    Reload();
                }
            }
        }

        protected async Task GetCaseRequestStatusList()
        {
            var result = await lookupService.GetCaseRequestStatusList();
            if (result.IsSuccessStatusCode)
            {
                caserequeststatuslist = (IEnumerable<CaseRequestStatusVM>)result.ResultData;
                Filteredcaserequeststatuslist = (IEnumerable<CaseRequestStatusVM>)result.ResultData;
                count = caserequeststatuslist.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInputCaseRequestStatus(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();

                    Filteredcaserequeststatuslist = await gridSearchExtension.Filter(caserequeststatuslist, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => (i.Id != null && i.Id.ToString().ToLower().Contains(@0)) || (i.Name_En != null && i.Name_En.ToLower().Contains(@1)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))" : $@"i => (i.Id != null && i.Id.ToString().ToLower().Contains(@0)) || (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        #endregion

        #region Case  Status List 
        IEnumerable<CmsRegisteredCaseStatusVM> _casestatuslist;
        protected IEnumerable<CmsRegisteredCaseStatusVM> Filteredcasestatuslist { get; set; } = new List<CmsRegisteredCaseStatusVM>();
        protected IEnumerable<CmsRegisteredCaseStatusVM> casestatuslist
        {
            get
            {
                return _casestatuslist;
            }
            set
            {
                if (!Equals(_casestatuslist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "casestatuslist", NewValue = value, OldValue = _casestatuslist };
                    _casestatuslist = value;

                    Reload();
                }
            }
        }

        protected async Task GetCaseStatusList()
        {
            var result = await lookupService.GetCaseStatusList();
            if (result.IsSuccessStatusCode)
            {
                casestatuslist = (IEnumerable<CmsRegisteredCaseStatusVM>)result.ResultData;
                Filteredcasestatuslist = (IEnumerable<CmsRegisteredCaseStatusVM>)result.ResultData;
                count = casestatuslist.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInputCaseStatus(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    Filteredcasestatuslist = await gridSearchExtension.Filter(casestatuslist, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => (i.Id != null && i.Id.ToString().ToLower().Contains(@0)) || (i.Name_En != null && i.Name_En.ToLower().Contains(@1)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))" : $@"i => (i.Id != null && i.Id.ToString().ToLower().Contains(@0)) || (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        #endregion 

        #region Task Type List
        IEnumerable<TaskTypeVM> _TaskTypeLkp;
        protected IEnumerable<TaskTypeVM> FilteredTaskTypeLkp { get; set; } = new List<TaskTypeVM>();
        protected IEnumerable<TaskTypeVM> TaskTypeLkp
        {
            get
            {
                return _TaskTypeLkp;
            }
            set
            {
                if (!Equals(_TaskTypeLkp, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "TaskTypeLkp", NewValue = value, OldValue = _TaskTypeLkp };
                    _TaskTypeLkp = value;

                    Reload();
                }
            }
        }

        protected async Task GetTaskTypeList()
        {
            var result = await lookupService.GetTaskTypeList();
            if (result.IsSuccessStatusCode)
            {
                TaskTypeLkp = (IEnumerable<TaskTypeVM>)result.ResultData;
                FilteredTaskTypeLkp = (IEnumerable<TaskTypeVM>)result.ResultData;
                count = TaskTypeLkp.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInputTaskType(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredTaskTypeLkp = await gridSearchExtension.Filter(TaskTypeLkp, new Query()
                    {
                        Filter = $@"i => ((i.NameEn != null && i.NameEn.ToLower().Contains(@0)) ||(i.NameAr != null && i.NameAr.ToLower().Contains(@1))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.Description != null && i.Description.ToLower().Contains(@3))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@4)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@5)))",
                        FilterParameters = new object[] { search, search, search, search, search, search }
                    }); await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        #endregion

        #region Request Type List
        IEnumerable<RequestTypeVM> _RequestTypeVM;
        protected IEnumerable<RequestTypeVM> FilteredRequestTypeVM { get; set; } = new List<RequestTypeVM>();
        protected IEnumerable<RequestTypeVM> RequestTypeVM
        {
            get
            {
                return _RequestTypeVM;
            }
            set
            {
                if (!Equals(_RequestTypeVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "RequestTypeVM", NewValue = value, OldValue = _RequestTypeVM };
                    _RequestTypeVM = value;
                    Reload();
                }
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
        protected async Task OnSearchInputRequestType()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.ToLower();

                FilteredRequestTypeVM = await gridSearchExtension.Filter(RequestTypeVM, new Query()
                {
                    Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                    FilterParameters = new object[] { search, search, search, search, search }
                });

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        #endregion

        #region Sub Type Lists
        IEnumerable<SubTypeVM> SubTypeVM;
        IEnumerable<SubTypeVM> _FilteredSubTypeVM;
        protected IEnumerable<SubTypeVM> FilteredSubTypeVM
        {
            get
            {
                return _FilteredSubTypeVM;
            }
            set
            {
                if (!Equals(_FilteredSubTypeVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredSubTypeVM", NewValue = value, OldValue = _FilteredSubTypeVM };
                    _FilteredSubTypeVM = value;
                    Reload();
                }
            }
        }
        protected async Task GetSubTypeList(int RequestTypeId)
        {
            var result = await lookupService.GetSubTypeList(RequestTypeId);
            SubTypeVM = (IEnumerable<SubTypeVM>)result.ResultData;
            count = RequestTypeVM.Count();
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnFileTypeSearchInput()
        {
            try
            {
                searchSubType = string.IsNullOrEmpty(searchSubType) ? "" : searchSubType.ToLower();
                FilteredSubTypeVM = await gridSearchExtension.Filter(SubTypeVM, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => (i.Name_En != null && i.Name_En.ToLower().Contains(@0))" : $@"i => (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@0))",
                    FilterParameters = new object[] { searchSubType, searchSubType }
                });
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
        #endregion

        #region Edit Communicaiton Type
        protected async Task EditCommunication(CommunicationTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<CommunicationTypeAdd>(
                translationState.Translate("Edit_Communication_Detail"),
                new Dictionary<string, object>() { { "CommunicationTypeId", args.CommunicationTypeId } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetCommunicationTypeList();
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

        #region Edit Task Type
        protected async Task EditTaskType(TaskTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<TaskTypeAdd>(
                translationState.Translate("Edit_Task_Type"),
                new Dictionary<string, object>() { { "TypeId", args.TypeId } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetTaskTypeList();
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

        #region Edit Case File Status
        protected async Task EditCaseFileStatus(CaseFileStatusVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<CaseFileStatusAdd>(
                translationState.Translate("Edit_Case_File_Status_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(400);
                    await GetCaseFileStatusList();
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

        #region Edit Case Request Status
        protected async Task UpdateCaseRequestStatus(CaseRequestStatusVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<CaseRequestStatusAdd>(
                translationState.Translate("Edit_Case_Request_Status_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetCaseRequestStatusList();
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

        #region Edit Case  Status
        protected async Task EditCaseStatus(CmsRegisteredCaseStatusVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<CaseStatusAdd>(
                translationState.Translate("Edit_Case_Status_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetCaseStatusList();
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

        #region Lookup History 
        IEnumerable<LookupsHistory> RequestTypeLookupHistory { get; set; } //for request type history
        IEnumerable<LookupsHistory> _LookupHistoryVM;
        protected IEnumerable<LookupsHistory> LookupHistoryVM
        {
            get
            {
                return _LookupHistoryVM;
            }
            set
            {
                if (!Equals(_LookupHistoryVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SubTypeVM", NewValue = value, OldValue = _LookupHistoryVM };
                    _LookupHistoryVM = value;
                    Reload();
                }
            }
        }
        protected async Task GetRequestTypeLookupHistoryListByReferncedId(int Id, int LookupstableId) // specific for request type
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

            RequestTypeLookupHistory = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }
        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId) // general
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);
            await GetRequestTypeLookupHistoryListByReferncedId(Id, LookupstableId);

            LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }
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

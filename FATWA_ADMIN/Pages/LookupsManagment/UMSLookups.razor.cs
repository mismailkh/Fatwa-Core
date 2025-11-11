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

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class UMSLookups : ComponentBase
    {
        #region Service Injection
        [Inject]
        protected RoleService roleService { get; set; }
        #endregion

        #region Variable Declaration
        public int count { get; set; }
        protected IList<ClaimVM> selectedClaimsList;
        public bool allowRowSelectOnRowClick = true;
        public bool allowRowSelectOnRowClick1 = true;
        public IEnumerable<UserVM> User = new List<UserVM>();
        public IList<UserVM> SelectUsers;
        public IList<Group> SelectUserGroups;
        public IEnumerable<Group> Grouplist = new List<Group>();
        protected bool isCheckedUser = false;
        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;

        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();
        protected RadzenDataGrid<DepartmentDetailVM>? DepartmentGrid = new RadzenDataGrid<DepartmentDetailVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();
        protected RadzenDataGrid<EpNationalityVM>? EPNationalityGrid = new RadzenDataGrid<EpNationalityVM>();
        protected RadzenDataGrid<EpGradeVM>? EPGradeGrid = new RadzenDataGrid<EpGradeVM>();
        protected RadzenDataGrid<EpGradeTypeVM>? EPGradeGridType = new RadzenDataGrid<EpGradeTypeVM>();
        protected RadzenDataGrid<EpDesignationVM>? EPDesignationGrid = new RadzenDataGrid<EpDesignationVM>();
        protected RadzenDataGrid<EpContractTypeVM>? ContractTypeGrid = new RadzenDataGrid<EpContractTypeVM>();


        protected string search { get; set; }

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
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

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
        #endregion

        #region Fuctions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            await GetEpGradeTypeList();
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            await GetDepartmentList();
            await GetEpNationalityList();
            await GetEpGradeList();
            await GetEpDesignationList();
            await GetEpGradeTypeList();
            await GetEpContractTypeList();
            StateHasChanged();
            spinnerService.Hide();
        }

        #endregion

        #region Department list 
        IEnumerable<DepartmentDetailVM> _DepartmentDetailLkp;
        protected IEnumerable<DepartmentDetailVM> FilteredDepartmentDetailLkp { get; set; } = new List<DepartmentDetailVM>();
        protected IEnumerable<DepartmentDetailVM> DepartmentDetailLkp
        {
            get
            {
                return _DepartmentDetailLkp;
            }
            set
            {
                if (!Equals(_DepartmentDetailLkp, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "DepartmentDetailLkp", NewValue = value, OldValue = _DepartmentDetailLkp };
                    _DepartmentDetailLkp = value;

                    Reload();
                }
            }
        }

        protected async Task GetDepartmentList()
        {
            var result = await lookupService.GetDepartmentList();
            if (result.IsSuccessStatusCode)
            {
                DepartmentDetailLkp = (IEnumerable<DepartmentDetailVM>)result.ResultData;
                FilteredDepartmentDetailLkp = (IEnumerable<DepartmentDetailVM>)result.ResultData;
                count = DepartmentDetailLkp.Count();
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
                    FilteredDepartmentDetailLkp = await gridSearchExtension.Filter(DepartmentDetailLkp, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) || (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)) || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@1)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@3)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    });

                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        #endregion

        #region  Ep Nationality  List 
        IEnumerable<EpNationalityVM> _EpNationalities;
        protected IEnumerable<EpNationalityVM> FilteredEpNationalities { get; set; } = new List<EpNationalityVM>();
        protected IEnumerable<EpNationalityVM> EpNationalities
        {
            get
            {
                return _EpNationalities;
            }
            set
            {
                if (!Equals(_EpNationalities, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "EpNationalities", NewValue = value, OldValue = _EpNationalities };
                    _EpNationalities = value;



                    Reload();
                }
            }
        }
        protected async Task GetEpNationalityList()
        {
            var result = await lookupService.GetEpNationalityList();
            if (result.IsSuccessStatusCode)
            {
                EpNationalities = (IEnumerable<EpNationalityVM>)result.ResultData;
                FilteredEpNationalities = (IEnumerable<EpNationalityVM>)result.ResultData;
                count = EpNationalities.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

        }
        protected async Task OnSearchEpNationality(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredEpNationalities = await gridSearchExtension.Filter(EpNationalities, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    }); await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }

        #endregion

        #region  Ep Grade  List 
        IEnumerable<EpGradeVM> _EpGrades;
        IEnumerable<EpGradeTypeVM> _EpGradesType;
        protected IEnumerable<EpGradeVM> FilteredEpGrades { get; set; } = new List<EpGradeVM>();
        protected IEnumerable<EpGradeTypeVM> FilteredEpGradeTypes { get; set; } = new List<EpGradeTypeVM>();
        protected IEnumerable<EpGradeVM> EpGrades
        {
            get
            {
                return _EpGrades;
            }
            set
            {
                if (!Equals(_EpGrades, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "EpGrades", NewValue = value, OldValue = _EpGrades };
                    _EpGrades = value;



                    Reload();
                }
            }
        }
        protected IEnumerable<EpGradeTypeVM> EpGradeType
        {
            get
            {
                return _EpGradesType;
            }
            set
            {
                if (!object.Equals(_EpGradesType, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "EpGradeType", NewValue = value, OldValue = _EpGradesType };
                    _EpGradesType = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async Task GetEpGradeList()
        {
            var result = await lookupService.GetEpGradeList();
            if (result.IsSuccessStatusCode)
            {
                EpGrades = (IEnumerable<EpGradeVM>)result.ResultData;
                FilteredEpGrades = (IEnumerable<EpGradeVM>)result.ResultData;
                count = EpGrades.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

        }
        protected async Task GetEpGradeTypeList()
        {
            var result = await lookupService.GetEpGradeTypeList();
            if (result.IsSuccessStatusCode)
            {
                EpGradeType = (IEnumerable<EpGradeTypeVM>)result.ResultData;
                FilteredEpGradeTypes = (IEnumerable<EpGradeTypeVM>)result.ResultData;
                count = EpGradeType.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

        }
        protected async Task OnSearchEpGrades(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredEpGrades = await gridSearchExtension.Filter(EpGrades, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) || 
                   (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)) || 
                   (i.GradeTypeEN != null && i.GradeTypeEN.ToLower().Contains(@2)) || 
                   (i.GradeTypeAr != null && i.GradeTypeAr.ToLower().Contains(@3)) || 
                   (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)) || 
                   (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@5)) || 
                   (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)))",
                        FilterParameters = new object[] { search, search, search, search, search, search, search }
                    }); await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Detail = translationState.Translate("Something_went_wrong_Please_try_again"), Style = "position: fixed !important; left: 0; margin: auto; " });
            }
        }
        protected async Task OnSearchEpGradeTypes(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredEpGradeTypes = await gridSearchExtension.Filter(EpGradeType, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
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

        #region  Ep Designation List 
        IEnumerable<EpDesignationVM> _EpDesignation;
        protected IEnumerable<EpDesignationVM> FilteredEpDesignation { get; set; } = new List<EpDesignationVM>();
        protected IEnumerable<EpDesignationVM> EpDesignation
        {
            get
            {
                return _EpDesignation;
            }
            set
            {
                if (!Equals(_EpDesignation, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "EpDesignation", NewValue = value, OldValue = _EpDesignation };
                    _EpDesignation = value;

                    Reload();
                }
            }
        }
        protected async Task GetEpDesignationList()
        {
            var result = await lookupService.GetEpDesignationList();
            if (result.IsSuccessStatusCode)
            {
                EpDesignation = (IEnumerable<EpDesignationVM>)result.ResultData;
                FilteredEpDesignation = (IEnumerable<EpDesignationVM>)result.ResultData;
                count = EpDesignation.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

        }
        protected async Task OnSearchEpDesignation(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredEpDesignation = await gridSearchExtension.Filter(EpDesignation, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
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

        #region  Ep Contract Type  List 
        IEnumerable<EpContractTypeVM> _EpContractType;
        protected IEnumerable<EpContractTypeVM> FilteredEpContractType { get; set; } = new List<EpContractTypeVM>();
        protected IEnumerable<EpContractTypeVM> EpContractType
        {
            get
            {
                return _EpContractType;
            }
            set
            {
                if (!Equals(_EpContractType, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "EpContractType", NewValue = value, OldValue = _EpContractType };
                    _EpContractType = value;

                    Reload();
                }
            }
        }
        protected async Task GetEpContractTypeList()
        {
            var result = await lookupService.GetEpContractTypeList();
            if (result.IsSuccessStatusCode)
            {
                EpContractType = (IEnumerable<EpContractTypeVM>)result.ResultData;
                FilteredEpContractType = (IEnumerable<EpContractTypeVM>)result.ResultData;
                count = EpContractType.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

        }
        protected async Task OnSearchEpContractType(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredEpContractType = await gridSearchExtension.Filter(EpContractType, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
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

        #region History Function 
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
        #endregion

        #region Lookups History
        protected async Task ExpendDepartmentHistoryList(DepartmentDetailVM departmentDetailVM)
        {
            await GetLookupHistoryListByRefernceId(departmentDetailVM.Id, (int)LookupsTablesEnum.Department);
        }
        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId) // general
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);
            LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

        }
        #endregion

        #region Department Action Detail
        #region Add Department
        protected async Task AddDepartment(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<DepartmentAdd>(
                    translationState.Translate("Add_Department_Detail"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetDepartmentList();
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
        #region Edit Department
        protected async Task EditDepartment(DepartmentDetailVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<DepartmentAdd>(
                translationState.Translate("Edit_Department_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetDepartmentList();
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
        #region Delete Department
        protected async Task DeleteDepartment(DepartmentDetailVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Department"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteDepartment(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetDepartmentList();
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
        #region Active Department
        protected async Task GridActiveButtonClick1(DepartmentDetailVM args)
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
                    var response = await lookupService.ActiveDepartment(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetDepartmentList();
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

        #region  Ep Nationality Action Button Detail
        #region Add EP Nationality  
        protected async Task AddEpNationality(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpNationalityAdd>(
                    translationState.Translate("Add_Nationality"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetEpNationalityList();
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
        #region Edit Ep Nationality
        protected async Task EditEpNationality(EpNationalityVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpNationalityAdd>(
                translationState.Translate("Edit_Nationality"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetEpNationalityList();
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
        #region Delete EP Nationality
        protected async Task DeleteEpNationality(EpNationalityVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Ep_Nationality"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteEpNationality(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetEpNationalityList();
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
        #region Active Ep Nationality
        protected async Task ActiveEpNationality(EpNationalityVM args)
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
                    var response = await lookupService.ActiveEpNationality(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetEpNationalityList();
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

        #region  Ep Grade Action Button Detail
        #region Add EP Grade
        protected async Task EpGradeAdd(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpGradeAdd>(
                    translationState.Translate("Add_Grade"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetEpGradeList();
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
        #region Edit Ep Grade
        protected async Task EditEpGrade(EpGradeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpGradeAdd>(
                translationState.Translate("Edit_Grade"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetEpGradeList();
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
        #region Delete Ep Grade
        protected async Task DeleteEpGrade(EpGradeVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Ep_Grade"),
                    translationState.Translate("Delete"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteEpGrade(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetEpGradeList();
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
        #region Active Ep Grade
        protected async Task ActiveEpGrade(EpGradeVM args)
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
                    var response = await lookupService.ActiveEpGrade(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetEpGradeList();
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

        #region  Ep Grade Type Action Button Detail
        #region Add EP Grade Type
        protected async Task EpGradeTypeAdd(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpGradeTypeAdd>(
                    translationState.Translate("Add_Grade_Type"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetEpGradeTypeList();
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
        #region Edit Ep Grade Type
        protected async Task EditEpGradeType(EpGradeTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpGradeTypeAdd>(
                translationState.Translate("Edit_Grade_Type"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetEpGradeTypeList();
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
        #region Delete Ep Grade Type
        protected async Task DeleteEpGradeType(EpGradeTypeVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Ep_Grade_Type"),
                    translationState.Translate("Delete"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteEpGradeType(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetEpGradeTypeList();
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

        #region  Ep Designation Action Button Detail
        #region Add EP Designation  
        protected async Task EpDesignationAdd(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpDesignationAdd>(
                    translationState.Translate("Add_Designation"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetEpDesignationList();
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
        #region Edit Ep Designation
        protected async Task EditEpDesignation(EpDesignationVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpDesignationAdd>(
                translationState.Translate("Edit_Ep_Designation"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetEpDesignationList();
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
        #region Delete Ep Designation
        protected async Task DeleteEpDesignation(EpDesignationVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Ep_Designation?"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteEpDesignation(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetEpDesignationList();
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

        #region  Ep Contract Type  Action Button Detail
        #region Add EP Contract Type  
        protected async Task EpContractAdd(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpContractTypeAdd>(
                    translationState.Translate("Add_Ep_Contract_Type"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetEpContractTypeList();
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
        #region Edit Ep Contract Type
        protected async Task EditEpContractType(EpContractTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<EpContractTypeAdd>(
                translationState.Translate("Edit_Ep_Contract_Type"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetEpContractTypeList();
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
        #region Delete Ep Contract Type 
        protected async Task DeleteEpContractType(EpContractTypeVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Ep_Contract_Type?"),
                    translationState.Translate("Delete"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteEpContractType(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetEpContractTypeList();
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

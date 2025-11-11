using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Net;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

//< History Author = 'Ammaar Naveed' Date = '2024-06-04' Version = "1.0" Branch = "master" > UI fixations in pages and dialogs</History>*@

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class CommonEnumsLists : ComponentBase
    {
        #region Service Injection

        [Inject]
        protected RoleService roleService { get; set; }




        #endregion

        #region Variable Declaration
        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();
        protected RadzenDataGrid<CommunicationTypeVM>? CommunicationGrid = new RadzenDataGrid<CommunicationTypeVM>();
        protected RadzenDataGrid<CaseFileStatusVM>? CaseFileStatusGrid = new RadzenDataGrid<CaseFileStatusVM>();
        protected RadzenDataGrid<CaseRequestStatusVM>? CaseRequestStatusGrid = new RadzenDataGrid<CaseRequestStatusVM>();
        protected RadzenDataGrid<CmsRegisteredCaseStatusVM>? CaseStatusGrid = new RadzenDataGrid<CmsRegisteredCaseStatusVM>();
        protected RadzenDataGrid<TaskTypeVM>? TaskTypeGrid = new RadzenDataGrid<TaskTypeVM>();
        protected RadzenDataGrid<SectorTypeVM>? SectorTypeGrid = new RadzenDataGrid<SectorTypeVM>();
        protected RadzenDataGrid<GovernmentEntitiesVM>? GovernmentEntityGrid = new RadzenDataGrid<GovernmentEntitiesVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();
        protected RadzenDataGrid<GovernmentEntitiesSectorsVM>? GovernmentEntitySectorGrid = new RadzenDataGrid<GovernmentEntitiesSectorsVM>();
        protected RadzenDataGrid<GovernmentEntitiesRepresentativeVM>? GovernmentEntityRepresentativeGrid = new RadzenDataGrid<GovernmentEntitiesRepresentativeVM>();
        protected RadzenDataGrid<CmsSectorTypeGEDepartmentVM>? G2GCorrespondencesReceiverGrid = new RadzenDataGrid<CmsSectorTypeGEDepartmentVM>();

        public Group Group = new Group();
        public GovermentEntityAndDepartmentSyncLog gEsAndDepartmentsSyncLog { get; set; } = new GovermentEntityAndDepartmentSyncLog();



        public int count { get; set; }
        public int lookup { get; set; }
        public bool lookup1 { get; set; } = true;
        public bool lookup2 { get; set; } = false;
        public bool lookup3 { get; set; } = false;
        public bool lookup4 { get; set; } = false;
        string _searchHistory;
        public string search { get; set; }
        public string searchDepartment { get; set; }
        public string GERepresentative { get; set; }
        public string GECorrespondenceReceiver { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
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

        protected IList<ClaimVM> selectedClaimsList;
        public bool allowRowSelectOnRowClick = true;
        public bool allowRowSelectOnRowClick1 = true;
        public IEnumerable<UserVM> User = new List<UserVM>();
        public IList<UserVM> SelectUsers;
        public IList<Group> SelectUserGroups;
        public IEnumerable<Group> Grouplist = new List<Group>();
        protected bool isCheckedUser = false;
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
        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;

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
            await GovernmentEntitylist();
            await GetGovernmentEntiteSectorList();
            await GetGovernmentEntiteRepresentativesList();
            await GetG2GCorrespondencesReceiverList();
            await GetLatestGEsAndDepartmentsSyncLog();
            StateHasChanged();
            spinnerService.Hide();
        }

        #endregion

        #region Lookup History 
        IEnumerable<LookupsHistory> RequestTypeLookupHistory { get; set; } //for request type history
        protected async Task GovernmentEntityHistoryList(GovernmentEntitiesVM governmentEntitiesVM)
        {
            await GetLookupHistoryListByRefernceId(governmentEntitiesVM.EntityId, (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_G2G_LKP);
        }
        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId) // general
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);
            await GetRequestTypeLookupHistoryListByReferncedId(Id, LookupstableId);

            FilteredLookupHistoryVM = LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }
        protected async Task GetRequestTypeLookupHistoryListByReferncedId(int Id, int LookupstableId) // specific for request type
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

            RequestTypeLookupHistory = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }
        protected async Task GovernmentEntityDepartmentHistoryList(GovernmentEntitiesSectorsVM governmentEntitiesVM)
        {
            await GetLookupHistoryListByRefernceId(governmentEntitiesVM.Id, (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP);
        }
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

        #region Governmnet Entity List
        IEnumerable<GovernmentEntitiesVM> GovernmentEntities;
        IEnumerable<GovernmentEntitiesVM> _FilteredGovernmentEntities;
        protected IEnumerable<LookupsHistory> FilteredLookupHistoryVM = new List<LookupsHistory>();
        protected IEnumerable<GovernmentEntitiesVM> FilteredGovernmentEntities
        {
            get
            {
                return _FilteredGovernmentEntities;
            }
            set
            {
                if (!Equals(_FilteredGovernmentEntities, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredGovernmentEntities", NewValue = value, OldValue = _FilteredGovernmentEntities };
                    _FilteredGovernmentEntities = value;



                    Reload();
                }
            }
        }
        protected async Task GovernmentEntitylist()
        {
            var result = await lookupService.GetGovernmentEntiteslist();
            if (result.IsSuccessStatusCode)
            {
                FilteredGovernmentEntities = GovernmentEntities = (IEnumerable<GovernmentEntitiesVM>)result.ResultData;
                count = FilteredGovernmentEntities.Count();
                await InvokeAsync(StateHasChanged);
                StateHasChanged();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnGovEntitiesSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();

                    FilteredGovernmentEntities = await gridSearchExtension.Filter(GovernmentEntities, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@0))||(i.GECode != null && i.GECode.ToLower().Contains(@0))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0))|| (i.G2GSiteId != null && i.G2GSiteId.ToString().ToLower().Contains(@0))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@0)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@0)))",
                        FilterParameters = new object[] { search }
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



        protected async Task OngridLookupHistoryVMSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    searchHistory = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredLookupHistoryVM = await gridSearchExtension.Filter(LookupHistoryVM, new Query()
                    {
                        Filter = $@"i => ((i.NameEn != null && i.NameEn.ToLower().Contains(@0)) ||(i.NameAr != null && i.NameAr.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3))|| (i.Description != null && i.Description.ToLower().Contains(@4)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@5)))",
                        FilterParameters = new object[] { searchHistory, searchHistory, searchHistory, searchHistory, searchHistory, searchHistory }
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

        #region  Government Entites Sectors List 
        IEnumerable<GovernmentEntitiesSectorsVM> GovernmentEntitySector;
        IEnumerable<GovernmentEntitiesSectorsVM> _FilteredGovernmentEntitySector;
        protected IEnumerable<GovernmentEntitiesSectorsVM> FilteredGovernmentEntitySector
        {
            get
            {
                return _FilteredGovernmentEntitySector;
            }
            set
            {
                if (!Equals(_FilteredGovernmentEntitySector, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredGovernmentEntitySector", NewValue = value, OldValue = _FilteredGovernmentEntitySector };
                    _FilteredGovernmentEntitySector = value;

                    Reload();
                }
            }
        }

        protected async Task GetGovernmentEntiteSectorList()
        {
            var result = await lookupService.GetGovernmentEntityDepartmentList();
            if (result.IsSuccessStatusCode && result.StatusCode == HttpStatusCode.OK)
            {
                GovernmentEntitySector = (IEnumerable<GovernmentEntitiesSectorsVM>)result.ResultData;
                FilteredGovernmentEntitySector = (IEnumerable<GovernmentEntitiesSectorsVM>)result.ResultData;
                count = FilteredGovernmentEntitySector.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }

        protected async Task OnGEDepartmentInputSearch(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    searchDepartment = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredGovernmentEntitySector = await gridSearchExtension.Filter(GovernmentEntitySector, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0))
                    || (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))
                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2))
                    || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) 
                    || (i.GovernmentEntityEn != null && i.GovernmentEntityEn.ToLower().Contains(@4))
                    || (i.GovernmentEntityAr != null && i.GovernmentEntityAr.ToLower().Contains(@5))
                    || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@6)))",
                        FilterParameters = new object[] { searchDepartment, searchDepartment, searchDepartment, searchDepartment, searchDepartment, searchDepartment, searchDepartment }
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

        #region  Government Entites Representative List 
        IEnumerable<GovernmentEntitiesRepresentativeVM> GovernmentEntitiesRepresentatives;
        IEnumerable<GovernmentEntitiesRepresentativeVM> _FilteredGovernmentEntitiesRepresentatives;
        protected IEnumerable<GovernmentEntitiesRepresentativeVM> FilteredGovernmentEntitiesRepresentatives
        {
            get
            {
                return _FilteredGovernmentEntitiesRepresentatives;
            }
            set
            {
                if (!Equals(_FilteredGovernmentEntitiesRepresentatives, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredGovernmentEntitiesRepresentatives", NewValue = value, OldValue = _FilteredGovernmentEntitiesRepresentatives };
                    _FilteredGovernmentEntitiesRepresentatives = value;



                    Reload();
                }
            }
        }

        protected async Task GetGovernmentEntiteRepresentativesList()
        {
            var result = await lookupService.GetGovernmentEntiteRepresentativesList();
            if (result.IsSuccessStatusCode)
            {
                GovernmentEntitiesRepresentatives = (IEnumerable<GovernmentEntitiesRepresentativeVM>)result.ResultData;
                FilteredGovernmentEntitiesRepresentatives = (IEnumerable<GovernmentEntitiesRepresentativeVM>)result.ResultData;
                count = GovernmentEntitiesRepresentatives.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnGeRepresentativeSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    GERepresentative = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredGovernmentEntitiesRepresentatives = await gridSearchExtension.Filter(GovernmentEntitiesRepresentatives, new Query()
                    {
                        Filter = $@"i => ((i.NameEn != null && i.NameEn.ToLower().Contains(@0)) ||(i.NameAr != null && i.NameAr.ToLower().Contains(@0))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0))|| (i.GovernmentEntityEn != null && i.GovernmentEntityEn.ToLower().Contains(@0))|| (i.GovernmentEntityAr != null && i.GovernmentEntityAr.ToLower().Contains(@0))|| (i.RepresentativeCode != null && i.RepresentativeCode.ToLower().Contains(@0))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@0)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@0)))",
                        FilterParameters = new object[] { GERepresentative }
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

        #region  Government G2G Correspondences Receiver List 
        IEnumerable<CmsSectorTypeGEDepartmentVM> G2GCorrespondencesReceiver;
        IEnumerable<CmsSectorTypeGEDepartmentVM> _FilteredG2GCorrespondencesReceiver;
        protected IEnumerable<CmsSectorTypeGEDepartmentVM> FilteredG2GCorrespondencesReceiver
        {
            get
            {
                return _FilteredG2GCorrespondencesReceiver;
            }
            set
            {
                if (!Equals(_FilteredG2GCorrespondencesReceiver, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredG2GCorrespondencesReceiver", NewValue = value, OldValue = _FilteredG2GCorrespondencesReceiver };
                    _FilteredG2GCorrespondencesReceiver = value;
                    Reload();
                }
            }
        }

        protected async Task GetG2GCorrespondencesReceiverList()
        {
            var result = await lookupService.GetG2GCorrespondencesReceiverList();
            if (result.IsSuccessStatusCode)
            {
                G2GCorrespondencesReceiver = (IEnumerable<CmsSectorTypeGEDepartmentVM>)result.ResultData;
                _FilteredG2GCorrespondencesReceiver = (IEnumerable<CmsSectorTypeGEDepartmentVM>)result.ResultData;
                count = _FilteredG2GCorrespondencesReceiver.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnG2GCorrespondencesReceiverSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    GECorrespondenceReceiver = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    _FilteredG2GCorrespondencesReceiver = await gridSearchExtension.Filter(G2GCorrespondencesReceiver, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
                        @"i => (i.GEDepartmentEN != null && i.GEDepartmentEN.ToLower().Contains(@0)) || 
                    (i.GovernmentEntityEn != null && i.GovernmentEntityEn.ToLower().Contains(@0)) || 
                    (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@0)) || 
                    (i.GovernmentEntityEn != null && i.GovernmentEntityEn.ToLower().Contains(@0)) || 
                    (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) || 
                    (i.CmsFatwaSectorType.ToString().ToLower().Contains(@0))" :
                        @"i => (i.GEDepartmentAr != null && i.GEDepartmentAr.ToLower().Contains(@0)) || 
                    (i.GovernmentEntityAr != null && i.GovernmentEntityAr.ToLower().Contains(@0)) || 
                    (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@0)) || 
                    (i.GovernmentEntityAr != null && i.GovernmentEntityAr.ToLower().Contains(@0)) || 
                    (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) || 
                    (i.CmsFatwaSectorType.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { GECorrespondenceReceiver }

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

        #region Government Entities Department, Representative, and G2G Correspondences Receiver Action Details

        #region Government Entities Action Button Detail
        #region Edit Government Entities
        protected async Task EditRowItem(GovernmentEntitiesVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<GovernmentEntitesAdd>(
                translationState.Translate("Edit_Government_Entity"),
                new Dictionary<string, object>() { { "EntityId", args.EntityId } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GovernmentEntitylist();
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
        #region Delete Government Entities
        protected async Task DeleteRowItem(GovernmentEntitiesVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Government_Entity"),
                    translationState.Translate("Delete"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel"),
                        Width = "25% !important"
                    }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteGovernmentEntity(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GovernmentEntitylist();
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
        #region Active Government Entities
        protected async Task GridActiveButtonClick(GovernmentEntitiesVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    args.IsActive = (bool)args.IsActive ? false : true;
                    spinnerService.Show();

                    var response = await lookupService.ActiveGovernmentEntities(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GovernmentEntitylist();
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
        #region Synced GE and Dept
        protected async Task GetLatestGEsAndDepartmentsSyncLog()
        {
            var result = await lookupService.GetLatestGEsAndDepartmentsSyncLog();
            if (result.IsSuccessStatusCode && result.StatusCode == HttpStatusCode.OK)
            {
                gEsAndDepartmentsSyncLog = (GovermentEntityAndDepartmentSyncLog)result.ResultData;
            }
            else if (!result.IsSuccessStatusCode)
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }

        protected async Task SyncGEsAndDepartments()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.SyncGEsAndDepartments(loginState.UserDetail.UserName);
                    if (response.IsSuccessStatusCode)
                    {
                        spinnerService.Hide();
                        await GetLatestGEsAndDepartmentsSyncLog();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("GEs_And_Departments_Synced_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        spinnerService.Hide();
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
        }
        #endregion
        #endregion

        #region  Government Entities Department Action Button Detail
        #region Add Government Entity Department 
        protected async Task AddGovernmentEntityDepartment(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<GovernmentEntitySectorAdd>(
                    translationState.Translate("Add_Government_Entity_Department_Detail"),
                    null,
                   new DialogOptions()
                   {
                       CloseDialogOnEsc = false,
                       CloseDialogOnOverlayClick = false
                   }) == true)
                {
                    await Task.Delay(100);
                    await GetGovernmentEntiteSectorList();
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
        #region Edit Government Entities Sectors 
        protected async Task EditGovernmentEntityDepartment(GovernmentEntitiesSectorsVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<GovernmentEntitySectorAdd>(
                translationState.Translate("Edit_Government_Entity_Department_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GetGovernmentEntiteSectorList();
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
        #region Delete Government Entities Sectors
        protected async Task DeleteGovernmentEntityDepartment(GovernmentEntitiesSectorsVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Government_Entity_Department"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel"),
                    Width = "25% !important"
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteGovernmentEntityDepartment(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetGovernmentEntiteSectorList();
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
        #region Active Government Entity Department 
        protected async Task ActiveGovernmentEntityDepartment(GovernmentEntitiesSectorsVM args)
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
                    args.IsActive = (bool)args.IsActive ? false : true;
                    spinnerService.Show();

                    var response = await lookupService.ActiveGovernmentEntityDepartment(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetGovernmentEntiteSectorList();
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

        #region  Government Entities Representative  Action Button Detail
        #region Add Government Entity Representative 
        protected async Task AddGovernmentEntityRepresentative(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<GovernmentEntityRepresentativeAdd>(
                    translationState.Translate("Add_GE_Representative_Detail"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetGovernmentEntiteRepresentativesList();
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
        #region Edit  Government Entity Representative 
        protected async Task EditGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<GovernmentEntityRepresentativeAdd>(
                translationState.Translate("Edit_GE_Representative_Detail"),
                new Dictionary<string, object>() { { "Id", args.id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await GetGovernmentEntiteRepresentativesList();
                    StateHasChanged();
                }
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
        #region Delete  Government Entity Representative 
        protected async Task DeleteGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Government_Entity_Representative"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteGovernmentEntityRepresentative(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetGovernmentEntiteRepresentativesList();
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
        #region Active Government Entity Representative
        protected async Task ActiveGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM args)
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

                    var response = await lookupService.ActiveGovernmentEntityRepresentative(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetGovernmentEntiteRepresentativesList();
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

        #region   G2G Correspondences Receiver Action Button Detail
        #region Add G2G Correspondences Receiver
        protected async Task AddG2GCorrespondencesReceiver(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<G2GCorrespondencesReceiverAdd>(
                    translationState.Translate("Add_G2G_Correspondences_Receiver_Detail"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnEsc = false,
                        CloseDialogOnOverlayClick = false,
                        Style = "width: 30% !important;"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetG2GCorrespondencesReceiverList();
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
        #region Delete  G2G Correspondences Receiver  
        protected async Task DeleteG2GCorrespondencesReceiver(CmsSectorTypeGEDepartmentVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_G2G_Correspondences_Receiver?"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteG2GCorrespondencesReceiver(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetG2GCorrespondencesReceiverList();
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
        #endregion

    }
}

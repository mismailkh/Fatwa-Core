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
    public partial class UMSEnums : ComponentBase
    {
        #region Service Injection

        [Inject]
        protected RoleService roleService { get; set; }




        #endregion

        #region Variable Declaration
        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();
        protected RadzenDataGrid<EpGenderVM>? GenderGrid = new RadzenDataGrid<EpGenderVM>();
        protected RadzenDataGrid<SectorTypeVM>? SectorTypeGrid = new RadzenDataGrid<SectorTypeVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();

        public Group Group = new Group();
        public int count { get; set; }
        public int lookup { get; set; }
        public bool lookup1 { get; set; } = true;
        public bool lookup2 { get; set; } = false;
        public bool lookup3 { get; set; } = false;
        public bool lookup4 { get; set; } = false;
        string _searchHistory;
        string _search;
        protected string search { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;

        protected IEnumerable<SectorRolesVM> sectorRolesList = new List<SectorRolesVM>();
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

        IEnumerable<SectorTypeVM> _SectorTypeLkp;
        protected IEnumerable<SectorTypeVM> FilteredSectorTypeLkp { get; set; } = new List<SectorTypeVM>();
        protected IEnumerable<SectorTypeVM> SectorTypeLkp
        {
            get
            {
                return _SectorTypeLkp;
            }
            set
            {
                if (!Equals(_SectorTypeLkp, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SectorTypeLkp", NewValue = value, OldValue = _SectorTypeLkp };
                    _SectorTypeLkp = value;

                    Reload();
                }
            }
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
            await GetGenderlist();
            await GetSectorTypeList();

            StateHasChanged();
            spinnerService.Hide();
        }

        #endregion

        #region Gender  list 
        IEnumerable<EpGenderVM> _Genderlist;
        protected IEnumerable<EpGenderVM> FilteredGenderlist { get; set; } = new List<EpGenderVM>();
        protected IEnumerable<EpGenderVM> Genderlist
        {
            get
            {
                return _Genderlist;
            }
            set
            {
                if (!Equals(_Genderlist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "Genderlist", NewValue = value, OldValue = _Genderlist };
                    _Genderlist = value;

                    Reload();
                }
            }
        }

        protected async Task GetGenderlist()
        {
            var result = await lookupService.GetGenderList();
            if (result.IsSuccessStatusCode)
            {
                Genderlist = (IEnumerable<EpGenderVM>)result.ResultData;
                FilteredGenderlist = (IEnumerable<EpGenderVM>)result.ResultData;
                count = Genderlist.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);

        }
        protected async Task OnSearchEpGender(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredGenderlist = await gridSearchExtension.Filter(Genderlist, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)))",
                        FilterParameters = new object[] { search, search }
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

        #region Update User Group

        #endregion

        #region button Event
        protected async Task Cencel(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Cancel"),
            translationState.Translate("Confirm"),
            new ConfirmOptions()
            {
                OkButtonText = @translationState.Translate("OK"),
                CancelButtonText = @translationState.Translate("Cancel")
            });

            if (dialogResponse == true)
            {
                navigationManager.NavigateTo("/groups");
            }
        }
        protected async Task Submitform()
        {
            grid0.Reset();
            await grid0.Reload();

            StateHasChanged();

        }
        #endregion

        #region Buttons  

        #region Gender Action Detail

        #region Edit Gender  
        protected async Task EditGender(EpGenderVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<GenderAdd>(
                translationState.Translate("Edit_Gender_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetGenderlist();
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
        #endregion

        #region Redirect Functions
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Sector Type Action Details
        #region Edit Sector Type
        protected async Task EditSectorType(SectorTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<SectorTypeAdd>(
                translationState.Translate("Edit_Sector_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions() { Width = "50%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetSectorTypeList();
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
        #region Sector Type List

        protected async Task GetSectorTypeList()
        {
            var result = await lookupService.GetSectorTypeList();
            if (result.IsSuccessStatusCode)
            {
                SectorTypeLkp = (IEnumerable<SectorTypeVM>)result.ResultData;
                FilteredSectorTypeLkp = (IEnumerable<SectorTypeVM>)result.ResultData;
                count = SectorTypeLkp.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInputSectorType(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredSectorTypeLkp = await gridSearchExtension.Filter(SectorTypeLkp, new Query()
                    {
                        Filter = $@"i => ((i.Id != null && i.Id.ToString().ToLower().Contains(@0)) || (i.Name_En != null && i.Name_En.ToLower().Contains(@1)) || (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@2))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@5)))",
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
        #region Lookup History 
        IEnumerable<LookupsHistory> RequestTypeLookupHistory { get; set; } //for request type history
        protected async Task GetSectorTypeLookupHistory(SectorTypeVM operatingSectorType)
        {
            await GetLookupHistoryListByRefernceId(operatingSectorType.Id, (int)LookupsTablesEnum.CMS_OPERATING_SECTOR_TYPE_G2G_LKP);
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

        protected async Task GetSectorRolesBySectorID(int sectorId)
        {
            List<int> SectorIds = new List<int>();
            SectorIds.Add(sectorId);
            var result = await lookupService.GetRolesBySectorTypeIds(SectorIds);
            if (result.IsSuccessStatusCode)
            {
                sectorRolesList = (IEnumerable<SectorRolesVM>)result.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
    }
}

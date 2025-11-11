using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class LMSLockups : ComponentBase
    {
       

        #region Service Injection

        [Inject]
        protected RoleService roleService { get; set; }

       

        #endregion

        #region Variable Declaration
        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();
        public Group Group = new Group();
        //public LmsLiteratureType lmsLiteratureType = new LmsLiteratureType();
     

        public int count { get; set; }
        public int lookup { get; set; }
        public bool lookup1 { get; set; } = true;
        public bool lookup2 { get; set; } = false;
        public bool lookup3 { get; set; } = false;
        public bool lookup4 { get; set; } = false;
        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new FATWA_ADMIN.Services.General.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }
        LmsLiteratureType _lmsLiteratureType;
        protected LmsLiteratureType lmsLiteratureType
        {
            get
            {
                return _lmsLiteratureType;
            }
            set
            {
                if (!object.Equals(_lmsLiteratureType, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "legalPrinciplePublicationSourceName", NewValue = value, OldValue = _lmsLiteratureType };
                    _lmsLiteratureType = value;
                    OnPropertyChanged(args);
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
            
            //translationState.TranslateGridFilterLabels(grid0);
            //translationState.TranslateGridFilterLabels(supervisorsAndManagersGrid);
            //spinnerService.Hide();

        }
        protected async Task Load()
        {
          
        }
        #endregion

        #region Save User Group
        protected async Task SaveChanges()
        {
            if (!String.IsNullOrEmpty(lmsLiteratureType.Name_En) && !String.IsNullOrEmpty(lmsLiteratureType.Name_Ar))
            {
                
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Group_Name_Required"),
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

     
    }
}

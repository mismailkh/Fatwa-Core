using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static SkiaSharp.HarfBuzz.SKShaper;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class DMSEnums : ComponentBase
    {
        #region Service Injection

        [Inject]
        protected RoleService roleService { get; set; }


        #endregion

        #region Variable Declaration
        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();
        protected RadzenDataGrid<AttachmentTypeVM>? DocumentTypeGrid = new RadzenDataGrid<AttachmentTypeVM>();
        public Group Group = new Group();

        private Timer debouncer;
        private const int debouncerDelay = 500;

        public int count { get; set; }
        public int lookup { get; set; }
        public bool lookup1 { get; set; } = true;
        public bool lookup2 { get; set; } = false;
        public bool lookup3 { get; set; } = false;
        public bool lookup4 { get; set; } = false;
        protected string search { get; set; }
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
        public IList<AttachmentTypeVM> SelectedAttachment { get; set; } = new List<AttachmentTypeVM>();


        #endregion

        #region Fuctions
        void TogglePageClaims(bool isChecked)
        {
            var currentPageData = DocumentTypeGrid.PagedView.ToList();

            if (isChecked)
            {
                foreach (var employee in currentPageData)
                {
                    if (!SelectedAttachment.Contains(employee))
                    {
                        SelectedAttachment.Add(employee);
                        DocumentTypeGrid.SelectRow(employee);
                    }
                }
            }
            else
            {
                foreach (var employee in currentPageData)
                {
                    if (SelectedAttachment.Contains(employee))
                    {
                        SelectedAttachment.Remove(employee);
                        DocumentTypeGrid.SelectRow(employee);
                    }
                }
            }
            DocumentTypeGrid.Reload();
        }
        void ToggleRowSelection(bool isChecked, AttachmentTypeVM data)
        {
            if (isChecked)
            {
                if (!SelectedAttachment.Contains(data))
                {
                    SelectedAttachment.Add(data);
                    //DocumentTypeGrid.SelectRow(data);
                }
            }
            else
            {
                if (SelectedAttachment.Contains(data))
                {
                    SelectedAttachment.Remove(data);
                }
            }
            DocumentTypeGrid.Reload();
        }
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
            await GetDocumentTypeList();
            StateHasChanged();
            spinnerService.Hide();
        }
        protected async Task GridActiveButtonClick(AttachmentTypeVM args)
        {
            if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"),
            translationState.Translate("Status"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel"),
            }) == true)
            {
                args.IsActive = (bool)args.IsActive ? false : true;
                spinnerService.Show();
                var response = await lookupService.ActiveDocumentTypes(args);
                spinnerService.Hide();

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Updated_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await Load();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }
        #endregion

        #region Communication Type list 
        IEnumerable<CommunicationTypeVM> _communicationtypelist;
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
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            var result = await lookupService.GetCommunicationTypeList(new Query()
            {
                Filter = $@"i => ((i.NameEn != null && i.NameEn.ToLower().Contains(@0)) ||(i.NameAr != null && i.NameAr.ToLower().Contains(@1)))",
                FilterParameters = new object[] { search, search }
            });
            communicationtypelist = (IEnumerable<CommunicationTypeVM>)result.ResultData;
            count = communicationtypelist.Count();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Document  Type list 
        IEnumerable<AttachmentTypeVM> documenttypelist;
        IEnumerable<AttachmentTypeVM> _FilteredDocumentTypelist;
        protected IEnumerable<AttachmentTypeVM> FilteredDocumentTypelist
        {
            get
            {
                return _FilteredDocumentTypelist;
            }
            set
            {
                if (!Equals(_FilteredDocumentTypelist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredDocumentTypelist", NewValue = value, OldValue = _FilteredDocumentTypelist };
                    _FilteredDocumentTypelist = value;

                    Reload();
                }
            }
        }

        protected async Task GetDocumentTypeList()
        {
            var result = await lookupService.GetDocumentTypeList();
            if (result.IsSuccessStatusCode)
            {
                documenttypelist = (IEnumerable<AttachmentTypeVM>)result.ResultData;
                FilteredDocumentTypelist = (IEnumerable<AttachmentTypeVM>)result.ResultData;
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
                    FilteredDocumentTypelist = await gridSearchExtension.Filter(documenttypelist, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => (i.AttachmentTypeId != null && i.AttachmentTypeId.ToString().ToLower().Contains(@0)) || (i.Type_En != null && i.Type_En.ToLower().Contains(@1)) || (i.Description != null && i.Description.ToLower().Contains(@2)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@3)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@4))" : $@"i => (i.AttachmentTypeId != null && i.AttachmentTypeId.ToString().ToLower().Contains(@0)) || (i.Type_Ar != null && i.Type_Ar.ToLower().Contains(@1)) || (i.Description != null && i.Description.ToLower().Contains(@2)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@3)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@4))",
                        FilterParameters = new object[] { search, search, search, search, search }
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

        #region Update User Group

        #endregion

        #region Buttons  
        #region Document Type Action Detail
        #region Add Document Type
        protected async Task AddDocumentType(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<DocumentTypeAdd>(
                   translationState.Translate("Add_Document_Detail"),
                    null,
                    new DialogOptions()
                    {
                        Width = "50%",
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }
                    ) == true)
                {
                    await Task.Delay(100);
                    await GetDocumentTypeList();
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
        protected async Task AddDigitalSignatureMethods(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<DocumentTypeDsSigningMethodAdd>(
                   translationState.Translate("Assign_Signing_Methods"),
                    new Dictionary<string, object>() { { "AttachmentTypeList", SelectedAttachment } },
                    new DialogOptions()
                    {
                        Width = "35%",
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }
                    ) == true)
                {
                    await Task.Delay(100);
                    await GetDocumentTypeList();
                    SelectedAttachment = new List<AttachmentTypeVM>();
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
        #region Edit Document Type
        protected async Task EditDocument(AttachmentTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<DocumentTypeAdd>(
                translationState.Translate("Edit_Document_Detail"),
                new Dictionary<string, object>() { { "AttachmentTypeId", args.AttachmentTypeId } },
                new DialogOptions() { Width = "50%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetDocumentTypeList();
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

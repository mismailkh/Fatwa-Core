using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class LPSLockups : ComponentBase
    {
        #region Service Injection
        [Inject]
        protected RoleService roleService { get; set; }
        #endregion

        #region Variable Declaration
        public int count { get; set; }
        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;
        protected IList<ClaimVM> selectedClaimsList;
        public bool allowRowSelectOnRowClick = true;
        public bool allowRowSelectOnRowClick1 = true;
        public IEnumerable<UserVM> User = new List<UserVM>();
        public IList<UserVM> SelectUsers;
        public IList<Group> SelectUserGroups;
        public IEnumerable<Group> Grouplist = new List<Group>();
        protected bool isCheckedUser = false;

        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserVM>? grid0 = new RadzenDataGrid<UserVM>();

        protected RadzenDataGrid<LegalPrincipleTypeVM>? LegalPrinciplegrid = new RadzenDataGrid<LegalPrincipleTypeVM>();
        protected RadzenDataGrid<LegalPublicationSourceNameVM>? grid1 = new RadzenDataGrid<LegalPublicationSourceNameVM>();
        public Group Group = new Group();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();
        protected string search;
        protected string searchHistory;
        protected IEnumerable<LegalPrincipleTypeVM> FilteredlegalPrincipleTypes = new List<LegalPrincipleTypeVM>();
        protected IEnumerable<LegalPrincipleTypeVM> legalPrincipleTypes = new List<LegalPrincipleTypeVM>();
        protected IEnumerable<UserVM> getUmsUserResult = new List<UserVM>();
        protected IEnumerable<ClaimVM> getGroupClaimsResult = new List<ClaimVM>();
        protected IEnumerable<LookupsHistory> LookupHistoryVM = new List<LookupsHistory>();
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
            spinnerService.Show();
            await Load();
            spinnerService.Hide();

        }
        protected async Task Load()
        {
            await LegalPrincipleTypeslist();
            StateHasChanged();
        }
        #endregion

        #region Legal Principle Publication Source Lists

        protected async Task LegalPrincipleTypeslist()
        {
            var result = await lookupService.GetLegalPrincipleTypes();
            if (result.IsSuccessStatusCode)
            {
                legalPrincipleTypes = (IEnumerable<LegalPrincipleTypeVM>)result.ResultData;
                FilteredlegalPrincipleTypes = (IEnumerable<LegalPrincipleTypeVM>)result.ResultData;
                count = legalPrincipleTypes.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
            StateHasChanged();
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredlegalPrincipleTypes = await gridSearchExtension.Filter(legalPrincipleTypes, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
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

        #region Legal Principle Type Action Details
        #region Add Legal Principle Type
        protected async Task Button1Click()
        {
            try
            {
                if (await dialogService.OpenAsync<LegalPrincipleTypeAdd>(
                    translationState.Translate("Add_Legal_Principle_Type"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await LegalPrincipleTypeslist();
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
        #region Edit Legal Principle Type
        protected async Task EditRowItem(LegalPrincipleTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<LegalPrincipleTypeAdd>(
                translationState.Translate("Edit_Legal_Principle_Type"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await LegalPrincipleTypeslist();
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
        #region Delete Legal Principle Type
        protected async Task DeleteRowItem(LegalPrincipleTypeVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Legal_Principle_Type"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeletelegalPrincipleType(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await LegalPrincipleTypeslist();
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
        #region Active Legal Principle Type
        protected async Task GridActiveButtonClick(LegalPrincipleTypeVM args)
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
                    var response = await lookupService.ActivelegalPrincipleTypes(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await LegalPrincipleTypeslist();
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

        #region Redirect events lpps and lps
        protected async Task Expendlegalprinciplehistorylist(LegalPrincipleTypeVM legalPrincipleTypes)
        {
            await GetLookupHistoryListByRefernceId(legalPrincipleTypes.Id, (int)LookupsTablesEnum.LEGAL_PRINCIPLE_TYPE);
        }
        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId)
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

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

using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class LLSLockups : ComponentBase
    {
        #region Service Injection

        [Inject]
        protected RoleService roleService { get; set; }
        #endregion

        #region Variable Declaration
        protected bool isLoading { get; set; }
        protected int count { get; set; }
        protected bool isCheckedRole = false;

        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<LegallegislationtypesVM>? grid0 = new RadzenDataGrid<LegallegislationtypesVM>();
        protected RadzenDataGrid<LegalPublicationSourceNameVM>? grid1 = new RadzenDataGrid<LegalPublicationSourceNameVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();
        //  public LegalLegislationType legislationType = new LegalLegislationType();

        protected string search;
        protected string searchHistory;

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
            await GetlegallegislationType();
            await GetLegalPublicationSourceNamelist();
        }
        #endregion

        #region Legislation Type lists

        protected IEnumerable<LegallegislationtypesVM> FilteredlegalLegislationType { get; set; } = new List<LegallegislationtypesVM>();
        protected IEnumerable<LegallegislationtypesVM> legalLegislationType = new List<LegallegislationtypesVM>();
        public async Task GetlegallegislationType()
        {
            var response = await lookupService.GetLegalLegislationTypes();
            if (response.IsSuccessStatusCode)
            {
                legalLegislationType = (IEnumerable<LegallegislationtypesVM>)response.ResultData;
                FilteredlegalLegislationType = (IEnumerable<LegallegislationtypesVM>)response.ResultData;
                count = legalLegislationType.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInputlegallegislationType(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredlegalLegislationType = await gridSearchExtension.Filter(legalLegislationType, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
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

        #region Legal Publication Source Name list 
        protected IEnumerable<LegalPublicationSourceNameVM> FilteredlegalPublicationSourceNames { get; set; } = new List<LegalPublicationSourceNameVM>();

        protected IEnumerable<LegalPublicationSourceNameVM> legalPublicationSourceNames = new List<LegalPublicationSourceNameVM>();

        protected async Task GetLegalPublicationSourceNamelist()
        {
            var result = await lookupService.GetLegalPublicationSourceName();
            if (result.IsSuccessStatusCode)
            {
                legalPublicationSourceNames = (IEnumerable<LegalPublicationSourceNameVM>)result.ResultData;
                FilteredlegalPublicationSourceNames = (IEnumerable<LegalPublicationSourceNameVM>)result.ResultData;
                count = legalPublicationSourceNames.Count();
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
                    FilteredlegalPublicationSourceNames = await gridSearchExtension.Filter(legalPublicationSourceNames, new Query()
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

        #region Legal Publication Source Action Button Detail
        #region Add Legal Publication Source
        protected async Task Button1Click(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<LegalPrincipleSourceNameAdd>(
                   translationState.Translate("Add_Publication_Source"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetLegalPublicationSourceNamelist();
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
        #region Edit Legal Publication Source
        protected async Task EditItem(LegalPublicationSourceNameVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<LegalPrincipleSourceNameAdd>(
                translationState.Translate("Edit_Publication_Source"),
                new Dictionary<string, object>() { { "Id", args.PublicationNameId } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetLegalPublicationSourceNamelist();
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
        #region Delete Legal Publication Source
        protected async Task DeleteItem(LegalPublicationSourceNameVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Legal_Publication_Source_Name"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeletelegalPublicationSourceNames(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetLegalPublicationSourceNamelist();
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
        #region Active Legal Publication Source
        protected async Task GridActiveButtonClick1(LegalPublicationSourceNameVM args)
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
                    var response = await lookupService.ActivePublicationSourceNames(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetLegalPublicationSourceNamelist();
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

        #region Legislation Type Action Button Detail
        #region Add Legislation Type
        protected async Task Button0Click()
        {
            try
            {
                if (await dialogService.OpenAsync<LegallegislationtypeAdd>(
                   translationState.Translate("Add_Legal_Legislation_Type"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetlegallegislationType();
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
        #region Edit Legislation Type
        protected async Task GridEditButtonClick(LegallegislationtypesVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<LegallegislationtypeAdd>(
                translationState.Translate("Edit_Legal_Legislation_Type"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(400);
                    await GetlegallegislationType();
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
        #region Delete Legislation Type
        protected async Task GridDeleteButtonClick(LegallegislationtypesVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_You_Sure_You_Want_To_Delete_Legislation_Type"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeletelegislationType(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetlegallegislationType();
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
        #region Active Legislation Type
        protected async Task GridActiveButtonClick(LegallegislationtypesVM args)
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
                    var response = await lookupService.IsActivelegislationType(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetlegallegislationType();
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

        #region Redirect events

        //<History Author = 'Aqeel Altaf' Date='2022-07-22' Version="1.0" Branch="master"> Handle bad request and delete specific user</History>

        protected async Task ExpandDraftVersionslegalPublicationSourceNames(LegalPublicationSourceNameVM legalPublicationSourceNameVM)
        {
            await GetLookupHistoryListByRefernceId(legalPublicationSourceNameVM.PublicationNameId, (int)LookupsTablesEnum.LEGAL_PUBLICATION_SOURCE_NAME);
        }
        protected async Task ExpandDraftVersionslegalLegislationType(LegallegislationtypesVM lmsLiteratureTagVM)
        {
            await GetLookupHistoryListByRefernceId(lmsLiteratureTagVM.Id, (int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE);
        }
        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId)
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

            LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
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
    }
}

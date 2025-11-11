using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.ContactManagment
{
    public partial class ListContact : ComponentBase
    {
        #region Parameters
        [Parameter]
        public bool ContactListForFileLink { get; set; }
        [Parameter]
        public Guid? FileId { get; set; }
        [Parameter]
        public int? FileModule { get; set; }
        #endregion

        #region Variables Declarations
        protected RadzenDataGrid<ContactListVM>? grid = new RadzenDataGrid<ContactListVM>();
        protected ContactAdvanceSearchVM advanceSearchVM { get; set; } = new ContactAdvanceSearchVM();
        protected List<Role> Roles { get; set; } = new List<Role>();
        protected List<Department> Departments { get; set; } = new List<Department>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        public List<CntContactType> CntContactTypeList { get; set; } = new List<CntContactType>();
        public IList<ContactListVM> selectedContact { get; set; } = new List<ContactListVM>();
        public bool allowRowSelectOnRowClick = true;
        public IEnumerable<ContactListVM> FilteredContactList { get; set; }
        IEnumerable<ContactListVM> ContactsList { get; set; } = new List<ContactListVM>();
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        private Timer debouncer;
        private const int debouncerDelay = 500;

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            await PopulateDepartments();
            await PopulateJobRoles();
            await GetContactType();
            spinnerService.Hide();
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Populate Data Functions
        protected async Task PopulateJobRoles()
        {
            var response = await lookupService.GetContactJobRole();
            if (response.IsSuccessStatusCode)
            {
                Roles = (List<Role>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateDepartments()
        {
            var response = await lookupService.GetDepartments();
            if (response.IsSuccessStatusCode)
            {
                Departments = (List<Department>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        private async Task GetContactType()
        {
            var response = await lookupService.GetContactType();
            if (response.IsSuccessStatusCode)
            {
                CntContactTypeList = (List<CntContactType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region On Load Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-03' Version="1.0" Branch="master">Fetch grid data based on pagination</History>*/
        private async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var response = await CntContactService.GetContactList(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredContactList = ContactsList = (IEnumerable<ContactListVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredContactList = await gridSearchExtension.Sort(FilteredContactList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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
        }
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = ContactsList.Any() ? (ContactsList.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = ContactsList.Any() ? (ContactsList.First().TotalCount) / (grid.PageSize) : 1;
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
        #endregion

        #region On Sort Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-03' Version="1.0" Branch="master">Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<ContactListVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredContactList = await gridSearchExtension.Sort(FilteredContactList, args.Column.Property, (SortOrder)args.SortOrder);
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

        #region Grid Search
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
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    Reload();
                }
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
                    FilteredContactList = await gridSearchExtension.Filter(ContactsList, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ?
                    $@"i =>(i.ContactTypeNameEn != null && i.ContactTypeNameEn.ToString().ToLower().Contains(@0)) 
                    || (i.Name != null && i.Name.ToString().ToLower().Contains(@1))
                    || (i.JobRoleEn != null && i.JobRoleEn.ToString().ToLower().Contains(@2))
                    || (i.DesignationNameEn != null && i.DesignationNameEn.ToString().ToLower().Contains(@3))
                    || (i.WorkplaceNameEn != null && i.WorkplaceNameEn.ToString().ToLower().Contains(@4)) 
                    || (i.PhoneNumber != null && i.PhoneNumber.ToString().ToLower().Contains(@5))"
                    :
                    $@"i =>(i.ContactTypeNameAr != null && i.ContactTypeNameAr.ToString().ToLower().Contains(@0))
                    || (i.Name != null && i.Name.ToString().ToLower().Contains(@1))
                    || (i.JobRoleAr != null && i.JobRoleAr.ToString().ToLower().Contains(@2))
                    || (i.DesignationNameEn != null && i.DesignationNameEn.ToString().ToLower().Contains(@3))
                    || (i.WorkplaceNameAr != null && i.WorkplaceNameAr.ToString().ToLower().Contains(@4))
                    || (i.PhoneNumber != null && i.PhoneNumber.ToString().ToLower().Contains(@5))",
                    FilterParameters = new object[] { search, search, search, search, search, search }
                });

                if (!string.IsNullOrEmpty(ColumnName))

                    FilteredContactList = await gridSearchExtension.Sort(FilteredContactList, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
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

        #region Grid Buttons
        //<History Author = 'Umer Zaman' Date='2022-03-04' Version="1.0" Branch="master"> Redirect to Add contact</History>
        protected async Task ButtonAddClick(MouseEventArgs args)
        {
            var result = await dialogService.OpenAsync<SelectContactTypePopup>(translationState.Translate("Select_Contact_Type"),
                null,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            //navigationManager.NavigateTo("/contact-add");
        }
        protected async Task ButtonAddContactWithFileClick(MouseEventArgs args)
        {
            var result = await dialogService.OpenAsync<SelectContactTypePopup>(translationState.Translate("Select_Contact_Type"),
                new Dictionary<string, object>()
                {
                    {"FileId", FileId },
                    {"FileModule", FileModule }
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            //navigationManager.NavigateTo("/contact-add");
        }
        protected async Task EditContact(ContactListVM args)
        {
            navigationManager.NavigateTo("/contact-add/" + args.ContactId);
        }

        protected async Task DetailContact(ContactListVM args)
        {
            int Module = (int)WorkflowModuleEnum.CNTContactManagement;
            Guid File = Guid.Empty;
            int SectorId = 0;
            navigationManager.NavigateTo("contact-view/" + args.ContactId + "/" + Module + "/" + File + "/" + SectorId);
        }
        protected async Task DeleteContact(ContactListVM contact)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await CntContactService.DeleteContact(contact);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Deleted_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    grid.Reset();
                    await grid.Reload();
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
                }
            }
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

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
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
            if (!advanceSearchVM.ContactTypeId.HasValue && string.IsNullOrEmpty(advanceSearchVM.Name) && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
            {

            }
            else
            {
                spinnerService.Show();
                Keywords = advanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                {
                    await grid.FirstPage();
                }
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                spinnerService.Hide();
                StateHasChanged();
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new ContactAdvanceSearchVM { PageSize = grid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }

        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        #endregion

        #region Form closed
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

        #region Add Selected Contact to file

        protected async Task AddSelectedContactToFile(MouseEventArgs args)
        {
            try
            {
                if (selectedContact.Count() != 0)
                {
                    bool? dialogResponse = await dialogService.Confirm(
                        translationState.Translate("Sure_Add_The_File_Contact_Record"),
                        translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = translationState.Translate("OK"),
                            CancelButtonText = translationState.Translate("Cancel")
                        });

                    if (dialogResponse == true)
                    {
                        spinnerService.Show();
                        var response = await lookupService.AddSelectedContactToFile(selectedContact, FileId, FileModule);
                        spinnerService.Hide();
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Contact_File_Remove_Success"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close("Done");
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Select_File_For_Contact"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Literature_Delete_Failed"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}

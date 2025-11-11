using DocumentFormat.OpenXml.Drawing.Diagrams;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.UserManagement.Groups
{
    public partial class AddGroup : ComponentBase
    {
        #region Parameters

        [Parameter]
        public string GroupId { get; set; }

        [Parameter]
        public bool IsView { get; set; }

        #endregion Parameters

        #region Variable Declaration
        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<UserListGroupVM>? grid0 = new RadzenDataGrid<UserListGroupVM>();
        protected RadzenDataGrid<UserListGroupVM>? grid1 = new RadzenDataGrid<UserListGroupVM>();
        public Group Group { get; set; } = new Group();
        public RadzenTemplateForm<Group> groupForm { get; set; }
        protected IEnumerable<GroupTypeWebSystemVM> groupAccessTypes { get; set; }
        public int count { get; set; }
        protected string search;
        protected IList<ClaimVM> selectedClaimsList;
        public bool allowRowSelectOnRowClick;
        public IEnumerable<UserListGroupVM> GroupUsersList = new List<UserListGroupVM>();
        public IEnumerable<UserListGroupVM> UMSUsersFilteredList = new List<UserListGroupVM>();
        public IEnumerable<UserListGroupVM> GroupUsersFilteredList = new List<UserListGroupVM>();
        public IList<UserListGroupVM> SelectUsers;
        public IList<UserListGroupVM> UsersWithExistingGroups;
        public IList<Group> SelectUserGroups;
        public IEnumerable<Group> Grouplist = new List<Group>();
        protected bool isCheckedUser = false;
        private IEnumerable<ClaimVM> claimsResults;
        protected IEnumerable<ClaimVM> getGroupClaimsResults;
        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;
        protected bool visible = false;
        public bool isRTL = false;
        private bool? IsUserHasAnyTask = false; 
        private static readonly string ArabicRegex = @"^[\u0600-\u06FF\s]+$";
        private static readonly string EnglishRegex = @"^[A-Za-z\s]+$";
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion Variable Declaration

        #region Oninitialized & Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (Thread.CurrentThread.CurrentUICulture.Name != "en-US")
            {
                isRTL = true;
            }

            await PopulateGroupTypes();
            if (GroupId != null)
            {
                await GetGroupDetailsById();
                await ShowPermission();
            }
            await Load();

            translationState.TranslateGridFilterLabels(grid0);
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        private async Task GetGroupDetailsById()
        {
            Group.GroupId = Guid.Parse(GroupId);
            var response = await groupService.GetUserGroupById(Guid.Parse(GroupId));
            if (response.IsSuccessStatusCode)
            {
                Group = response.ResultData as Group;
                GroupUsersFilteredList = UsersWithExistingGroups = Group.Users;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task Load()
        {
            try
            {
                spinnerService.Show();
                isLoading = true;
                allowRowSelectOnRowClick = !IsView;
                search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
                var response = await groupService.GetUmsUserList(GroupId, IsView);
                if (response.IsSuccessStatusCode)
                {
                    UMSUsersFilteredList = GroupUsersList = (IEnumerable<UserListGroupVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

                var response2 = await groupService.GetAllUserClaims(GroupId);
                if (response2.IsSuccessStatusCode)
                {
                    claimsResults = (IEnumerable<ClaimVM>)response2.ResultData;
                    getGroupClaimsResults = claimsResults;
                    Group.GroupClaims = getGroupClaimsResults.Where(c => c.IsAssigned).ToList();
                    if (IsView)
                        claimsResults = Group.GroupClaims;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response2);
                }
                isLoading = false;
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Fuctions

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        #region Redirect Function

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        #endregion Redirect Functions        

        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    {
                        UMSUsersFilteredList = await gridSearchExtension.Filter(GroupUsersList, new Query()
                        {
                            Filter = $@"i => (i.FullName_En != null && i.FullName_En.ToLower().Contains(@0)) || (i.SectorTypeName_En != null && i.SectorTypeName_En.ToLower().Contains(@1)) || (i.UserTypeEnglish != null && i.UserTypeEnglish.ToLower().Contains(@2))|| (i.Role_En != null && i.Role_En.ToLower().Contains(@3))",
                            FilterParameters = new object[] { search, search, search, search }
                        });
                    }
                    else
                    {
                        UMSUsersFilteredList = await gridSearchExtension.Filter(GroupUsersList, new Query()
                        {
                            Filter = $@"i => (i.FullName_Ar != null && i.FullName_Ar.ToLower().Contains(@0)) || (i.SectorTypeName_Ar != null && i.SectorTypeName_Ar.ToLower().Contains(@1)) || (i.UserTypeArabic != null && i.UserTypeArabic.ToLower().Contains(@2))|| (i.Role_En != null && i.Role_En.ToLower().Contains(@3))",
                            FilterParameters = new object[] { search, search, search, search }
                        });
                    }
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        protected async Task SearchGroupUsers(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    {
                        GroupUsersFilteredList = await gridSearchExtension.Filter(UsersWithExistingGroups, new Query()
                        {
                            Filter = $@"i => (i.FullName_En != null && i.FullName_En.ToLower().Contains(@0)) || (i.SectorTypeName_En != null && i.SectorTypeName_En.ToLower().Contains(@1)) || (i.Designation_En != null && i.Designation_En.ToLower().Contains(@2)) || (i.Role_En != null && i.Role_En.ToLower().Contains(@3)) || (i.ADUsername != null && i.ADUsername.ToLower().Contains(@4))",
                            FilterParameters = new object[] { search, search, search, search, search }
                        });
                    }
                    else
                    {
                        GroupUsersFilteredList = await gridSearchExtension.Filter(UsersWithExistingGroups, new Query()
                        {
                            Filter = $@"i => (i.FullName_Ar != null && i.FullName_Ar.ToLower().Contains(@0)) || (i.SectorTypeName_Ar != null && i.SectorTypeName_Ar.ToLower().Contains(@1)) || (i.Designation_Ar != null && i.Designation_Ar.ToLower().Contains(@2)) || (i.Role_Ar != null && i.Role_Ar.ToLower().Contains(@3)) || (i.ADUsername != null && i.ADUsername.ToLower().Contains(@4))",
                            FilterParameters = new object[] { search, search, search, search, search }
                        });
                    }
                   
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);

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

        protected void OnSelectUsers(bool value, string name, UserListGroupVM? obj)
        {
            if (name == "allChecked")
            {
                if (value == true)
                {
                    SelectUsers = GroupUsersList.ToList();
                    isCheckedUser = true;
                }
                else
                {
                    SelectUsers = null;
                    isCheckedUser = false;
                }
            }
        }

        protected void OnSelectGroup(bool value, string name, Group? obj)
        {
            if (name == "allChecked")
            {
                if (value == true)
                {
                    SelectUserGroups = Grouplist.ToList();
                    isCheckedRole = true;
                }
                else
                {
                    SelectUsers = null;
                    isCheckedRole = false;
                }
            }
        }

        private async Task PopulateGroupTypes()
        {
            var response = await groupService.GetGroupAccessTypes();
            if (response.IsSuccessStatusCode)
            {
                groupAccessTypes = (IEnumerable<GroupTypeWebSystemVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion Fuctions

        #region Create New Group

        protected async Task SaveUmsUserGroup()
        {
            if (string.IsNullOrEmpty(Group.Name_En) || string.IsNullOrEmpty(Group.Name_Ar) || Group.GroupTypeId == 0)
            {
                groupForm.EditContext.Validate();
                return;
            }

            if (!Group.GroupClaims.Any() && visible == true)
            {
                ShowError("Please_Assign_Permissions");
                return;
            }

            bool? dialogResponse = await dialogService.Confirm(
                GroupId == null
                    ? translationState.Translate("Add_UMS_Group")
                    : translationState.Translate("Update_UMS_Group"),
                translationState.Translate("Confirm"),
                new ConfirmOptions
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                });

            if (dialogResponse != true)
                return;

            try
            {
                spinnerService.Show();

                Group.Users = SelectUsers;
                Group.CreatedDate = DateTime.Now;
                Group.CreatedBy = loginState.Username;
                Group.IsDeleted = false;

                if (GroupId == null)
                {
                    Group.GroupId = Guid.NewGuid();
                }

                var response = await groupService.SubmitUmsGroup(Group);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Group_Create_Success_Message"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    navigationManager.NavigateTo("groups");
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            finally
            {
                spinnerService.Hide();
            }
        }
        #endregion Save User Group

        #region Shared Permissions Dialog

        //<History Author = 'Ammaar Naveed' Date='2024-04-15' Version="1.0" Branch="master"> Added dialog for ensurity of shared permissions while creating a group.</History>
        private async Task SharedPermissionsDialog()
        {
            await dialogService.OpenAsync<PermissionsDialog>((Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "Note" : "ملاحظة"),
                 new Dictionary<string, object>()
                 {
                 },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });
        }

        #endregion Shared Permissions Dialog

        #region Update User Group

        protected async Task UpdateUmsUserGroup()
        {
            if (string.IsNullOrEmpty(Group.Name_En) || string.IsNullOrEmpty(Group.Name_Ar) || Group.GroupTypeId == 0 || string.IsNullOrEmpty(Group.Description_En) || string.IsNullOrEmpty(Group.Description_Ar))
            {
                groupForm.EditContext.Validate();
                return;
            }

            if (GroupId == null)
            {
                return;
            }

            bool? dialogResponse = await dialogService.Confirm(
                translationState.Translate("Group_Update_Dialog_Message"),
                translationState.Translate("Confirm"),
                new ConfirmOptions
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                });

            if (dialogResponse != true)
                return;

            try
            {
                spinnerService.Show();

                Group.Users = SelectUsers;
                Group.ModifiedBy = loginState.Username;
                Group.ModifiedDate = DateTime.Now;
                Group.UsersWithExistingGroups = UsersWithExistingGroups;

                var response = await groupService.UpdateGroup(Group);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Group_Update_Success_Message"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    navigationManager.NavigateTo("/groups");
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            finally
            {
                spinnerService.Hide();
            }
        }
        #endregion Update User Group

        #region Helper Methods To Check English & Arabic Validation
        private bool IsValidArabic(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(input, ArabicRegex);
        }
        private bool IsValidEnglish(string input)
        {
            return !string.IsNullOrWhiteSpace(input) &&
                   System.Text.RegularExpressions.Regex.IsMatch(input, EnglishRegex);
        }
        private void ShowError(string messageKey)
        {
            notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Detail = translationState.Translate(messageKey),
                Style = "position: fixed !important; left: 0; margin: auto;"
            });
        }
        #endregion

        #region button Events

        protected async Task CancelChanges(MouseEventArgs args)
        {
            if (IsView)
            {
                navigationManager.NavigateTo("/groups");
            }
            else
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
        }

        protected async Task Submitform()
        {
            grid0.Reset();
            await grid0.Reload();

            StateHasChanged();
        }

        #endregion button Event

        #region Fuction Render

        protected void OnRender(DataGridRenderEventArgs<ClaimVM> args)
        {
            if (args.FirstRender)
            {
                args.Grid.Groups.Add(new GroupDescriptor() { Title = translationState.Translate("Module"), Property = "Module", SortOrder = SortOrder.Descending });
                args.Grid.Groups.Add(new GroupDescriptor() { Title = translationState.Translate("Submodule"), Property = "SubModule", SortOrder = SortOrder.Descending });
                StateHasChanged();
            }
        }

        protected void OnGroupRowRender(GroupRowRenderEventArgs args)
        {
            if (args.FirstRender) { args.Expanded = false; }
        }

        protected async Task ShowPermission()
        {
            var webSystemsIds = groupAccessTypes.Where(x => x.GroupTypeId == Group.GroupTypeId).Select(x => x.WebSystemsIds).FirstOrDefault();
            if (webSystemsIds != null)
            {
                visible = webSystemsIds.Any(value => value == (int)WebSystemEnum.CoreApp);
                if (visible && GroupId == null)
                {
                    await SharedPermissionsDialog();
                }
                StateHasChanged();
            }
        }
        #endregion Fuction Render

        #region Select All

        //<History Author = 'Hassan Abbas' Date='2022-07-25' Version="1.0" Branch="master"> Select all permissions based on type i.e, All, Module, Submodule</History>
        protected async Task SelectAllPermissions(bool args, string? key, SelectPermissionType type)
        {
            var list = new List<ClaimVM>();

            if (type == SelectPermissionType.All)
            {
                list = grid.View.ToList();
            }
            else if (type == SelectPermissionType.Module)
            {
                list = grid.View.Where(x => x.Module == key).ToList();
            }
            else
            {
                list = grid.View.Where(x => x.SubModule == key).ToList();
            }

            foreach (var claim in list)
            {
                if (args)
                {
                    if (!Group.GroupClaims.Contains(claim))
                    {
                        Group.GroupClaims.Add(claim);
                        grid.SelectRow(claim);
                    }
                }
                else
                {
                    if (Group.GroupClaims.Contains(claim))
                    {
                        Group.GroupClaims.Remove(claim);
                        grid.SelectRow(claim);
                    }
                }
            }
        }

        #endregion Select All

    }
}

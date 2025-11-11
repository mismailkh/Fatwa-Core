using FATWA_ADMIN.Pages.LookupsManagment;
using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.UserManagement.GroupAccessType
{
    public partial class ListGroupType : ComponentBase
    {
        #region Variables
        protected IEnumerable<GroupTypeWebSystemVM> groupAccessTypes { get; set; }
        protected RadzenDataGrid<GroupTypeWebSystemVM>? groupAccessTypesGrid { get; set; }
        protected bool isLoading { get; set; }
        protected List<GroupTypeWebSystemVM> _FilteredGroupAccessTypes;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region GroupAccessTypeList Filtration for SearchFunctionality
        protected List<GroupTypeWebSystemVM> FilteredGroupAccessTypeList { get; set; }

        protected string search { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region On Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        public async Task Load()
        {
            isLoading = true;
            var response = await groupService.GetGroupAccessTypes();
            if (response.IsSuccessStatusCode)
            {
                groupAccessTypes = (IEnumerable<GroupTypeWebSystemVM>)response.ResultData;
                FilteredGroupAccessTypeList = (List<GroupTypeWebSystemVM>)groupAccessTypes;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Add GroupAccess Type
        protected async Task Button0Click(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<AddGroupType>(
                    translationState.Translate("Create_Group_Type"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await Load();
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

        protected async Task EditGroupType(MouseEventArgs args, int Id)
        {
            try
            {
                if (await dialogService.OpenAsync<AddGroupType>(
                translationState.Translate("Update_Group_Type"),
                new Dictionary<string, object>() { { "Id", Id.ToString() } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await Load();
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

        #region GroupAccessTypes List Filtration -> SearchFunctionality
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {
                        FilteredGroupAccessTypeList = await gridSearchExtension.Filter(groupAccessTypes, new Query()
                        {
                            Filter = $@"i => (i.Name != null && i.Name.ToLower().Contains(@0)) || (i.WebSystemsEn != null && i.WebSystemsEn.ToLower().Contains(@1))",
                            FilterParameters = new object[] { search, search }
                        });
                    }
                    else
                    {
                        FilteredGroupAccessTypeList = await gridSearchExtension.Filter(groupAccessTypes, new Query()
                        {
                            Filter = $@"i => (i.Name != null && i.Name.ToLower().Contains(@0)) || (i.WebSystemsAr != null && i.WebSystemsAr.ToLower().Contains(@1))",
                            FilterParameters = new object[] { search, search }
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

    }
}

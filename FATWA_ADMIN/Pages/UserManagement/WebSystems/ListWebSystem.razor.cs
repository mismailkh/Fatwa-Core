using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.UserManagement.WebSystems
{
    public partial class ListWebSystem : ComponentBase
    {
        protected IEnumerable<WebSystem> websystemList { get; set; }

        protected RadzenDataGrid<WebSystem>? websystemGrid;
        public WebSystem WebSystem { get; set; }
        protected bool isLoading { get; set; }
        public int count { get; set; }
        protected List<WebSystem> _FilteredWebSystem;

        private Timer debouncer;
        private const int debouncerDelay = 500;
        protected List<WebSystem> FilteredWebSystemList
        {
            get
            {
                return _FilteredWebSystem;
            }
            set
            {
                if (!object.Equals(_FilteredWebSystem, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _FilteredWebSystem };
                    _FilteredWebSystem = value;

                    Reload();
                }

            }
        }
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
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }


        public async Task Load()
        {
            #region Get Websystems list
            isLoading = true;

            var response = await groupService.GetWebSystems();
            if (response.IsSuccessStatusCode)
            {
                websystemList = (IEnumerable<WebSystem>)response.ResultData;
                FilteredWebSystemList = (List<WebSystem>)websystemList;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            #endregion

            StateHasChanged();
        }

        #region WebSystems List Filtration -> SearchFunctionality
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
                        FilteredWebSystemList = await gridSearchExtension.Filter(websystemList, new Query()
                        {
                            Filter = $@"i => (i.WebSystemId != null && i.WebSystemId.ToString().ToLower().Contains(@0)) || (i.NameEn != null && i.NameEn.ToLower().Contains(@1))",
                            FilterParameters = new object[] { search, search }
                        });
                    }
                    else
                    {
                        FilteredWebSystemList = await gridSearchExtension.Filter(websystemList, new Query()
                        {
                            Filter = $@"i => (i.WebSystemId != null && i.WebSystemId.ToString().ToLower().Contains(@0)) || (i.NameAr != null && i.NameAr.ToLower().Contains(@0))",
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


        #region Add Web Systems
        protected async Task AddWebsystem(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/add-websystem");
        }
        #endregion

        #region Update WebSystems
        protected async Task EditWebSystem(MouseEventArgs args, int Id)
        {
            try
            {

                if (await dialogService.OpenAsync<AddWebSystems>(translationState.Translate("Update_Web_systems"),
                new Dictionary<string, object>() { { "Id", Id } },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Load();
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

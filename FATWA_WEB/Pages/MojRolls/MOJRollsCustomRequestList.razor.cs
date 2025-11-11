using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
namespace FATWA_WEB.Pages.MojRolls
{
    public partial class MOJRollsCustomRequestList : ComponentBase
    {
        #region Variables 
        protected RadzenDataGrid<MOJRollsRequestListVM>? grid;
        #endregion
        #region on change
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion
        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {

            try
            {
                spinnerService.Show();
                await Load();
                translationState.TranslateGridFilterLabels(grid);
                System.Timers.Timer t_loadpending = new System.Timers.Timer();
                t_loadpending.Elapsed += async (s, e) =>
                {
                    await Load();
                    await InvokeAsync(StateHasChanged);
                };
                t_loadpending.Interval = 1000 * 60 * 5; // Adjusted interval to 5 minutes
                t_loadpending.Start();
                spinnerService.Hide();
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
        #region Get
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

        IEnumerable<MOJRollsRequestListVM> _getMojRollesRequestList;
        protected IEnumerable<MOJRollsRequestListVM> getMojRollesRequestList
        {
            get
            {
                return _getMojRollesRequestList;
            }
            set
            {
                if (!object.Equals(_getMojRollesRequestList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getMojRollesRequestList", NewValue = value, OldValue = _getMojRollesRequestList };
                    _getMojRollesRequestList = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected async Task Load()
        {
           
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            else
                search = search.ToLower();

            var response = await mojRollsService.GetMojRequestListbyUserId(loginState.UserDetail.ActiveDirectoryUserName);
            if (response.IsSuccessStatusCode)
            {
                getMojRollesRequestList = (IEnumerable<MOJRollsRequestListVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        #endregion
        #region View File
        //<History Author = 'ijaz Ahmad' Date='2023-05-23' Version="1.0" Branch="master"> File view</History>
        protected async Task ViewPDF(MouseEventArgs args, dynamic data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<MojRollsDocumentView>(
                 translationState.Translate("Request_Details"),
                 new Dictionary<string, object>() { { "Id", data.Id } },//requestId
               new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true, });
                await Task.Delay(200);
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
        #region Exception View
        //<History Author = 'ijaz Ahmad' Date='2023-05-23' Version="1.0" Branch="master"> Exception View</History>
        protected async Task ExceptionView(MouseEventArgs args, dynamic data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<MojRollsExceptionView>(

                translationState.Translate("Request_Details"),
                 new Dictionary<string, object>() { { "Id", data.Id } },//RequestId
               new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true, });
                await Task.Delay(200);
                await Load();
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

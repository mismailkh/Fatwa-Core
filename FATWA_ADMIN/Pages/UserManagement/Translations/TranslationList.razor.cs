using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;


namespace FATWA_ADMIN.Pages.UserManagement.Translations
{
    public partial class TranslationList : ComponentBase
    {
        #region Radzen DataGrid Variables and Settings
        protected RadzenDataGrid<Translation>? grid = new RadzenDataGrid<Translation>();
        string _search;
        private Timer debouncer;
        private const int debouncerDelay = 500;
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
        IEnumerable<Translation> _getTransaltionResult;
        protected IEnumerable<Translation> FilteredGetTransaltionResult { get; set; } = new List<Translation>();
        protected IEnumerable<Translation> getTransaltionResult
        {
            get
            {
                return _getTransaltionResult;
            }
            set
            {
                if (!object.Equals(_getTransaltionResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTransaltionResult", NewValue = value, OldValue = _getTransaltionResult };
                    _getTransaltionResult = value;
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

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            translationState.TranslateGridFilterLabels(grid);

            spinnerService.Hide();

        }
        protected async Task Load()
        {
            var response = await translationService.GetTranslation();
            if (response.IsSuccessStatusCode)
            {
                getTransaltionResult = (IEnumerable<Translation>)response.ResultData;
                FilteredGetTransaltionResult = (IEnumerable<Translation>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
                    FilteredGetTransaltionResult = await gridSearchExtension.Filter(getTransaltionResult, new Query()
                    {
                        Filter = $@"i => ( (i.TranslationKey != null && i.TranslationKey.ToLower().Contains(@0)) || (i.Value_En != null && i.Value_En.ToLower().Contains(@1)) || (i.Value_Ar != null && i.Value_Ar.ToLower().Contains(@2)) )",
                        FilterParameters = new object[] { search, search, search }
                    });  await InvokeAsync(StateHasChanged);
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

        #region Grid Actions
        protected async Task GridEditButtonClick(Translation data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<SaveSystemTranslation>(
                                  translationState.Translate("Update_Translation"),
                                  new Dictionary<string, object>() {
                                      { "Translation", data }
                                  },
                                  new DialogOptions()
                                  {
                                      Width = "30% !important",
                                      CloseDialogOnOverlayClick = false,
                                  });
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
        protected async Task GridDeleteButtonClick(Translation translation)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_sure_you_want_to_delete_this_record"), translationState.Translate("delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var response = await translationService.DeleteTranslation(translation.TranslationId);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Delete_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await Load();
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

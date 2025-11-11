using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using Radzen;
using System.Net;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_ADMIN.Services.General
{
    /// <summary>
    ///   History  <<< AttiqueRehman <<<<  05/FEB/2025   
    ///   Service for handling Api response and alerting bad requests.
    /// </summary> 
    public class InvalidRequestHandlerService
    {
        #region Variables
        private readonly NotificationService _notificationService;
        private readonly TranslationState _translationState;
        private readonly LoginState _loginState;
        private readonly ILocalStorageService _browserStorage;

        #endregion

        #region Injection thru Constructor
        public InvalidRequestHandlerService(NotificationService notificationService, TranslationState translationState, LoginState loginState, ILocalStorageService browserStorage)
        {
            _notificationService = notificationService;
            _translationState = translationState;
            _loginState = loginState;
            _browserStorage = browserStorage;
        }
        #endregion

        #region Badrequest Notiication
        /// <summary>
        /// Handles the API BadRequset response and provides appropriate msg
        /// </summary>
        /// <param name="response">The received API response</param> 
        /// <param name="alreadyDataExistMessage">Translation key for custom messages (e,g "UserName_Already_Exist"), when you are expecting exception of dupilcate data enry</param> 
        public async Task ReturnBadRequestNotification(ApiCallResponse response, string alreadyExistMessage = null)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = _translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(5000);
                    await _browserStorage.RemoveItemAsync("User");
                    await _browserStorage.RemoveItemAsync("Token");
                    await _browserStorage.RemoveItemAsync("RefreshToken");
                    await _browserStorage.RemoveItemAsync("UserDetail");
                    await _browserStorage.RemoveItemAsync("SecurityStamp");
                    _loginState.IsLoggedIn = false;
                    _loginState.IsStateChecked = true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.NoContent)
                {
                    _notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = _translationState.Translate("No_record_found"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    var badRequestResponse = (BadRequestResponse)response.ResultData;
                    if (badRequestResponse?.InnerException != null && badRequestResponse.InnerException.ToLower().Contains("violation of unique key"))
                    {
                        _notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = string.IsNullOrEmpty(alreadyExistMessage) ? "Save_failed_duplicate_data_detected" : _translationState.Translate(alreadyExistMessage),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        _notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = _translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = _translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}

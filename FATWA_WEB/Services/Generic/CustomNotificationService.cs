using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services.Generic
{
    //<History Author = 'Hassan Abbas' Date='2023-10-02' Version="1.0" Branch="master"> Added Generic Bad Request Handler Notification Service which can be extanded later</History>
    public class CustomNotificationService
    {
        public event Action<ApiCallResponse> OnNotification;

        public void DisplayNotification(ApiCallResponse response)
        {
            OnNotification?.Invoke(response);
        }
    }
}

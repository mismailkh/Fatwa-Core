using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_WEB.Data;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Notifications
{
    public partial class NotificationViewDetailComponent : ComponentBase
    {
        #region Service Injection

        [Inject]
        protected NotificationDetailService notificationDetailService { get; set; }

        [Inject]
        protected LoginState loginState { get; set; }

        [Inject]
        protected TranslationState translationState { get; set; }

        [Inject]
        protected SpinnerService spinnerService { get; set; }

        [Inject]
        protected NotificationService notificationService { get; set; }

        [Inject]
        protected DialogService dialogService { get; set; }
     

        [Inject]
        protected IConfiguration _config { get; set; }

        #endregion

        #region Variable

        [Parameter]
        public Guid NotficationDetailId { get; set; }

        NotificationDetailVM _notificationDetailVM;
        protected NotificationDetailVM notificationDetailVM
        {
            get
            {
                return _notificationDetailVM;
            }
            set
            {
                if (!object.Equals(_notificationDetailVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "notificationDetailVM", NewValue = value, OldValue = _notificationDetailVM };
                    _notificationDetailVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected string direction;

        #endregion

        #region Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            if (Thread.CurrentThread.CurrentUICulture.Name != "en-US")
            {
                direction = "direction: rtl; width: 100%;";
            }
            else
            {
                direction = "direction: ltr; width: 100%;";
            }

            await Load();

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            try
            {
                var getNotification = await notificationDetailService.GetNotificationDetailView(NotficationDetailId);
                if (getNotification.IsSuccessStatusCode)
                {
                    notificationDetailVM = (NotificationDetailVM)getNotification.ResultData; 
                    CreateNotificationMessage();
                }
                else
                {
                    notificationDetailVM = new NotificationDetailVM();
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

        #region FUNCTIONS

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        protected string fullUrl;
        protected string NotifMessage;
        protected void CreateNotificationMessage()
        {
            string entity = notificationDetailVM.Url.Split('/')[1].Split('-')[0];
            entity = translationState.Translate("Click_Here");

            string entityId = notificationDetailVM.Url.Split('/')[2];

            //For Notification With Some Number in their Entity 
            if (translationState.Translate(notificationDetailVM.NotificationMessageEn).Contains("#entityId#"))
            {
                if (notificationDetailVM.Url.Contains("delete") || notificationDetailVM.LinkIsDeleted == true)
                    fullUrl = $"{entityId}";
                else if (notificationDetailVM.Url.Contains("list"))
                    fullUrl = $"<a href='/{notificationDetailVM.Url.Split('/')[1]}'>{entityId}</a>";
                else
                    fullUrl = $"<a href='{notificationDetailVM.Url}' target='_parent'>{entityId}</a>";

                notificationDetailVM.NotificationMessageEn = translationState.Translate(notificationDetailVM.NotificationMessageEn).Replace("#entityId#", fullUrl);
            }
            //For Notification With Name as their Entity  
            else
            {
                if (notificationDetailVM.Url.Contains("delete") || notificationDetailVM.LinkIsDeleted == true)
                    fullUrl = $"{entity}";
                else if (notificationDetailVM.Url.Contains("list"))
                    fullUrl = $"<a href='/{notificationDetailVM.Url.Split('/')[1]}'>{entity}</a>";
                else
                    fullUrl = $"<a href='{notificationDetailVM.Url}' target='_parent'>{entity}</a>";
                

                notificationDetailVM.NotificationMessageEn = translationState.Translate(notificationDetailVM.NotificationMessageEn).Replace("#entity#", fullUrl); 
            }
            NotifMessage = $"<p>{notificationDetailVM.NotificationMessageEn}</p>";
        }

        protected async Task DeleteNotification(Guid NotficationDetailId)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Delete_Notification"), translationState.Translate("delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)  
                {

                    NotificationVM notification = new NotificationVM()
                    {
                        NotificationId = NotficationDetailId
                    };

                    await notificationDetailService.DeleteNotification(notification);
                    dialogService.Close();

                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Delete_Notification_Success"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
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

        protected async Task UnreadNotification(Guid notficationId)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Mark_Unread_Notification"), translationState.Translate("Mark_Unread"), new ConfirmOptions()
				{
					OkButtonText = translationState.Translate("OK"),
					CancelButtonText = translationState.Translate("Cancel")
				}) == true)
                {

                    CheckNotificationVM notification = new CheckNotificationVM()
                    {
                        notificationStatus = (int)NotificationStatusEnum.Unread,
                        NotificationReadDate = null,
                        notificationIds = new List<Guid>()
                        {
                            notficationId
                        }
                    };
                    //ApiCallResponse response = await notificationDetailService.MarkNotificationAsRead(notification);
                    //if (response.ResultData == null)
                    //{
                    //    notificationService.Notify(new NotificationMessage()
                    //    {
                    //        Severity = NotificationSeverity.Success,
                    //        Detail = translationState.Translate("Contact_Administrator"),
                    //        Style = "position: fixed !important; left: 0; margin: auto;"
                    //    });
                    //}
                    //else
                    //{
                    //    notificationService.Notify(new NotificationMessage()
                    //    {
                    //        Severity = NotificationSeverity.Success,
                    //        Detail = translationState.Translate("Mark_Unread_Notification_Success"),
                    //        Style = "position: fixed !important; left: 0; margin: auto;"
                    //    });
                    //}
                    dialogService.Close();
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
    }
}

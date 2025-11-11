using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.Notification
{
    public partial class ListNotificationEvents : ComponentBase
    {
        #region Varriables
        protected RadzenDataGrid<NotificationEventListVM>? EventGrid;
        protected IEnumerable<NotificationEventListVM> EventList { get; set; }

        public NotificationEventListVM NotificationEventListVM { get; set; }
        protected bool isLoading { get; set; }
        public int count { get; set; }
        protected List<NotificationEventListVM> _FilteredEvent;
        private Timer debouncer;
        private const int debouncerDelay = 500;

        protected List<NotificationEventListVM> FilteredEvent
        {
            get
            {
                return _FilteredEvent;
            }
            set
            {
                if (!object.Equals(_FilteredEvent, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _FilteredEvent };
                    _FilteredEvent = value;

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
        #endregion
        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;

            var response = await notificationsService.GetEventList();
            if (response.IsSuccessStatusCode)
            {
                EventList = (IEnumerable<NotificationEventListVM>)response.ResultData;
                FilteredEvent = (List<NotificationEventListVM>)EventList;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;



            StateHasChanged();
        }
        #endregion

        protected RadzenDataGrid<NotificationTemplateListVM>? TemplateGrid;
        IEnumerable<NotificationTemplateListVM> _getNotificationTemplateList;
        protected IEnumerable<NotificationTemplateListVM> getNotificationTemplateList
        {
            get
            {
                return _getNotificationTemplateList;
            }
            set
            {
                if (!Equals(_getNotificationTemplateList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getNotificationTemplateList", NewValue = value, OldValue = _getNotificationTemplateList };
                    _getNotificationTemplateList = value;
                    Reload();
                }
            }
        }
        public int? countTemplate { get; set; }
        IEnumerable<NotificationTemplateListVM> FilteredNotificationTemplateList { get; set; } = new List<NotificationTemplateListVM>();
        protected async Task ExpandFileTimelog(int? args)
        {

            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<NotificationTemplateListVM> timeTrackingVMs = new List<NotificationTemplateListVM>();
            var result = await notificationsService.GetTemplateListByEventId(args);
            if (result.IsSuccessStatusCode)
            {
                getNotificationTemplateList = (IEnumerable<NotificationTemplateListVM>)result.ResultData;

                getNotificationTemplateList = timeTrackingVMs;
                FilteredNotificationTemplateList = (IEnumerable<NotificationTemplateListVM>)result.ResultData;
                countTemplate = getNotificationTemplateList.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }

        protected async Task ExpandFileTimelog(RowRenderEventArgs<NotificationEventListVM> args)
        {

            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<NotificationTemplateListVM> timeTrackingVMs = new List<NotificationTemplateListVM>();
            var result = await notificationsService.GetTemplateListByEventId(args.Data.EventId);
            if (result.IsSuccessStatusCode)
            {
                getNotificationTemplateList = (IEnumerable<NotificationTemplateListVM>)result.ResultData;

                getNotificationTemplateList = timeTrackingVMs;
                FilteredNotificationTemplateList = (IEnumerable<NotificationTemplateListVM>)result.ResultData;
                countTemplate = getNotificationTemplateList.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }

        #region  List Filtration -> SearchFunctionality
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredEvent = await gridSearchExtension.Filter(EventList, new Query()
                    {
                        Filter = @"i => 
                      (i.NameEn != null && i.NameEn.ToLower().Contains(@0)) || 
                      (i.ReceiverTypeName != null && i.ReceiverTypeName.ToLower().Contains(@1)) || 
                      (i.DescriptionAr != null && i.DescriptionAr.ToLower().Contains(@2)) || 
                      (i.DescriptionEn != null && i.DescriptionEn.ToLower().Contains(@3)) || 
                      (i.NameAr != null && i.NameAr.ToLower().Contains(@4))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
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

        #region Edit Template
        protected async Task EditEventTemplate(MouseEventArgs args, Guid? Id)
        {
            navigationManager.NavigateTo("/add-Event/" + Id);
        }
        #endregion
        #region Edit Event
        protected async Task EditEvent(int EventId)
        {
            navigationManager.NavigateTo("/edit-Event/" + EventId);
        }
        #endregion
        #region Delete
        protected async Task UpdateTemplateStatus(bool value, NotificationTemplateListVM args)
        {

            if (await dialogService.Confirm(value ? translationState.Translate("Sure_Want_To_Activate_Template") : translationState.Translate("Sure_Want_To_Deactivate_Template"),
                translationState.Translate("Status"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
            {
                args.isActive = value;
                var response = await notificationsService.DeleteEventTemplate(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Update_Successfully"),
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
        protected async Task UpdateEventStatus(bool value, NotificationEventListVM args)
        {
            if (await dialogService.Confirm(value ? translationState.Translate("Sure_Want_To_Activate_Event") : translationState.Translate("Sure_Want_To_Deactivate_Event"),
                translationState.Translate("Status"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
            {
                args.IsActive = value;
                var response = await notificationsService.UpdateEventStatus(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Update_Successfully"),
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
        #endregion
        #region Add Template Event
        protected async Task AddTemplate(int eventId, int chanelId)
        {
            navigationManager.NavigateTo("/add-event-template/" + eventId + "/" + chanelId);
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

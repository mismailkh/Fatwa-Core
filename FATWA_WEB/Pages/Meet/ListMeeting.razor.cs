using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.MeetingEnums;

namespace FATWA_WEB.Pages.Meet
{
    public partial class ListMeeting : ComponentBase
    {
        #region Variables Declarations
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        private int? PreviousPageSize { get; set; }
        private int? PreviousPageNumber { get; set; } = 1;
        public bool isGridLoaded { get; set; }
        public bool isPageSizeChangeOnFirstLastPage { get; set; }
        protected string search { get; set; }
        protected RadzenDataGrid<MeetingVM>? grid = new RadzenDataGrid<MeetingVM>();
        protected IEnumerable<MeetingVM> getMeetingList { get; set; } = new List<MeetingVM>();
        protected IEnumerable<MeetingVM> FilteredGetMeetingList { get; set; }
        private Meeting meetingdetail = new Meeting();
        protected MeetingStatusVM meetingStatusVM = new MeetingStatusVM();
        protected SaveMomVM meetingStatus = new();
        bool IsHeld = true;
        protected bool IsHeldErrorShow { get; set; } = false;

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data
        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != PreviousPageNumber || CurrentPageSize != PreviousPageSize)
                {
                    if (isGridLoaded && PreviousPageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)PreviousPageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var result = await meetingService.GetMeetingsList((int)PreviousPageSize, (int)PreviousPageNumber);
                    if (result.IsSuccessStatusCode)
                    {
                        getMeetingList = (IEnumerable<MeetingVM>)result.ResultData;
                        FilteredGetMeetingList = (IEnumerable<MeetingVM>)result.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredGetMeetingList = await gridSearchExtension.Sort(FilteredGetMeetingList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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
            spinnerService.Hide();
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PreviousPageSize != CurrentPageSize)
            {
                int oldPageCount = getMeetingList.Any() ? (getMeetingList.First().TotalCount) / ((int)PreviousPageSize) : 1;
                int oldPageNumber = (int)PreviousPageNumber - 1;
                isGridLoaded = true;
                PreviousPageNumber = CurrentPage;
                PreviousPageSize = args.Top;
                int TotalPages = getMeetingList.Any() ? (getMeetingList.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PreviousPageNumber = TotalPages + 1;
                    PreviousPageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((PreviousPageNumber == 1 || (PreviousPageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            PreviousPageNumber = CurrentPage;
            PreviousPageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSort(DataGridColumnSortEventArgs<MeetingVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredGetMeetingList = await gridSearchExtension.Sort(FilteredGetMeetingList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Grid Search
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                FilteredGetMeetingList = await gridSearchExtension.Filter(getMeetingList, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                        ? $@"i => (i.Subject != null && i.Subject.ToLower().Contains(@0)) 
                            || (i.TypeEn != null && i.TypeEn.ToLower().Contains(@1)) 
                            || (i.UserNameEn != null && i.UserNameEn.ToLower().Contains(@2))
                            || (i.StatusEn != null && i.StatusEn.ToLower().Contains(@3))
                            || (i.DateTime != null && i.DateTime.ToString(""dd/MM/yyyy h:mm tt"").Contains(@4))"

                        : $@"i => (i.Subject != null && i.Subject.ToLower().Contains(@0)) 
                            || (i.TypeAr != null && i.TypeAr.ToLower().Contains(@1)) 
                            || (i.UserNameAr != null && i.UserNameAr.ToLower().Contains(@2))
                            || (i.StatusAr != null && i.StatusAr.ToLower().Contains(@3))
                            || (i.DateTime != null && i.DateTime.ToString(""dd/MM/yyyy h:mm tt"").Contains(@4))",
                    FilterParameters = new object[] { search, search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredGetMeetingList = await gridSearchExtension.Sort(FilteredGetMeetingList, ColumnName, SortOrder);
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

        #region Meeting Functions
        protected async Task AddMeeting(MouseEventArgs args)
        {
            try
            {
                Guid MeetingId = Guid.Empty;
                Guid CommunicationId = Guid.Empty;
                Guid ReferenceId = Guid.Empty;
                int SubModuleId = 0;
                int SectorTypeId = 0;
                navigationManager.NavigateTo("/meeting-add");
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

        protected async Task EditMeeting(MeetingVM item)
        {
            try
            {
                navigationManager.NavigateTo("/meeting-add/" + item.MeetingId);

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

        protected async Task AddMOM(MeetingVM item)
        {
            try
            {
                navigationManager.NavigateTo("/meetingmom-save/" + item.MeetingId + "/" + item.ReferenceGuid + "/" + false + "/" + false);
                //  await dialogService.OpenAsync<SaveMOM>(translationState.Translate("Meeting_MOM"),
                //	new Dictionary<string, object>()
                //	{
                //		{ "MeetingId", item.MeetingId },
                //      { "ReferenceGuid", item.ReferenceGuid },
                //  },
                //	new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "900px" });
                //await Task.Delay(200);
                //await Load();
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


        protected async Task EditMOM(MeetingVM item)
        {
            try
            {
                navigationManager.NavigateTo("/meetingmom-save/" + item.MeetingId + "/" + item.ReferenceGuid + "/" + true + "/" + false);
                //await dialogService.OpenAsync<SaveMOM>(translationState.Translate("Meeting_MOM"),
                //    new Dictionary<string, object>()
                //    {
                //        { "MeetingId", item.MeetingId },
                //        { "ReferenceGuid", item.ReferenceGuid },
                //        { "isEdit", true },
                //    },
                //    new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "900px" });
                //await Task.Delay(200);
                //await Load();
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

        protected async Task ReviewMOM(MeetingVM item)
        {
            try
            {
                navigationManager.NavigateTo("/meetingmom-save/" + item.MeetingId + "/" + item.ReferenceGuid + "/" + true + "/" + true);
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

        protected async Task DecisionMeeting(MeetingVM meeting)
        {
            navigationManager.NavigateTo($"/meeting-decision/{meeting.MeetingId}");
        }

        protected async Task DetailMeeting(MeetingVM item)
        {
            try
            {
                Guid MeetingId = (Guid)item.MeetingId;
                bool IsView = true;
                navigationManager.NavigateTo("/meeting-view/" + MeetingId + "/" + IsView);
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

        public async Task PopulateMeetingDetail(Guid MeetingId)
        {

            var response = await meetingService.GetMeetingDetailById(MeetingId);
            if (response.IsSuccessStatusCode)
            {
                meetingStatus = (SaveMomVM)response.ResultData;
                meetingStatusVM.MeetingId = meetingStatus.Meeting.MeetingId;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }


        protected async Task DecisionHeld(MeetingVM item)
        {


            try
            {
                await PopulateMeetingDetail((Guid)item.MeetingId);

                if (meetingStatus.Meeting.StartTime <= DateTime.Now && meetingStatus.Meeting.Date <= DateTime.Now)
                {
                    meetingStatusVM.MeetingStatusId = (int)MeetingStatusEnum.Held;
                    meetingStatusVM.MeetingId = (Guid)item.MeetingId;

                }
                else
                {
                    IsHeld = false;
                }
                bool? dialogResponse = false;
                string successMessage = translationState.Translate("Meeting_Held");

                if (IsHeld == false)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Cant_Held_Meeting"),
                        Style = "position: fixed !important; left: 0; margin: auto; ",
                        Duration = 5000
                    });
                    return;
                }
                else
                {
                    IsHeldErrorShow = false;
                }
                dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Held_Meeting"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                if (dialogResponse == true)
                {
                    var response = await meetingService.EditMeetingStatus(meetingStatusVM);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = successMessage,
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        //navigationManager.NavigateTo("/meeting-list", true);
                        item.MeetingStatusId = (int)MeetingStatusEnum.Held;

                    }
                }
                else
                {
                    dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Cancel"),
                    translationState.Translate("Confirm_Cancel"),
                    new ConfirmOptions()
                    {
                        CloseDialogOnOverlayClick = true,
                        OkButtonText = @translationState.Translate("OK"),
                        CancelButtonText = @translationState.Translate("Cancel")
                    });
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

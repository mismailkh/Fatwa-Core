using DocumentFormat.OpenXml.Vml.Spreadsheet;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages
{
    //<History Author = 'Hassan Abbas' Date='2023-03-22' Version="1.0" Branch="master"> Inbox Outbox List</History>
    public partial class ListInboxOutbox : ComponentBase
    {
        #region Variables Declaration
        protected string RedirectURL { get; set; }
        protected IEnumerable<CommunicationInboxOutboxVM> inboxOutboxList { get; set; }
        protected RadzenDataGrid<CommunicationInboxOutboxVM> grid = new RadzenDataGrid<CommunicationInboxOutboxVM>();
        public ViewConsultationVM consultationRequestVM = new ViewConsultationVM();
        protected int selectedTabIndex { get; set; } = 0;
        protected int correspondenceType { get; set; } = 0;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage { get; set; }
        private int CurrentPageSize { get; set; }


        private int? PreviousPageSize { get; set; }
        private int? PreviousPageNumber { get; set; } = 1;
        public bool isGridLoaded { get; set; }
        public bool isPageSizeChangeOnFirstLastPage { get; set; }

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion
        protected async Task OnChangeTab(int index)
        {
            if (index == selectedTabIndex) { return; }
            selectedTabIndex = index;
            ColumnName = string.Empty;
            PreviousPageSize = grid.PageSize;
            PreviousPageNumber = 1;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }

        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                CurrentPage = grid.CurrentPage + 1;
                CurrentPageSize = grid.PageSize;
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

                    if (selectedTabIndex == 0)
                        correspondenceType = (int)CommunicationCorrespondenceTypeEnum.Inbox;
                    else
                        correspondenceType = (int)CommunicationCorrespondenceTypeEnum.Outbox;
                    var CommunicationResponse = await communicationService.GetInboxOutboxList(correspondenceType, loginState.Username, (int)PreviousPageSize, (int)PreviousPageNumber);
                    if (CommunicationResponse.IsSuccessStatusCode)
                    {
                        inboxOutboxList = JsonConvert.DeserializeObject<List<CommunicationInboxOutboxVM>>(CommunicationResponse.ResultData.ToString());
                        if (!(string.IsNullOrEmpty(args.OrderBy)))
                        {
                            inboxOutboxList = await gridSearchExtension.Sort(inboxOutboxList, ColumnName, SortOrder);
                        }
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(CommunicationResponse);
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

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PreviousPageSize != CurrentPageSize)
            {
                int oldPageCount = inboxOutboxList != null && inboxOutboxList.Any() ? (inboxOutboxList.First().TotalCount) / ((int)PreviousPageSize) : 1;
                int oldPageNumber = (int)PreviousPageNumber - 1;
                isGridLoaded = true;
                PreviousPageNumber = CurrentPage;
                PreviousPageSize = args.Top;
                int TotalPages = inboxOutboxList != null && inboxOutboxList.Any() ? (inboxOutboxList.First().TotalCount) / (grid.PageSize) : 1;
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
        private async Task OnSort(DataGridColumnSortEventArgs<CommunicationInboxOutboxVM> args)
        {
            if (args.SortOrder != null)
            {
                inboxOutboxList = await gridSearchExtension.Sort(inboxOutboxList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Action Events

        protected async void ViewResponse(CommunicationInboxOutboxVM item)
        {
            if (item.CommunicationTypeId == (int)CommunicationTypeEnum.CaseRequest)
            {
                navigationManager.NavigateTo("/caserequest-view/" + item.ReferenceId);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.ContractRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.LegislationRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.LegalAdviceRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.AdministrativeComplaintRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.InternationalArbitrationRequest)
            {

                var result = await consultationRequestService.GetConsultationDetailById((Guid)item.ReferenceId);


                if (result.IsSuccessStatusCode)
                {
                    consultationRequestVM = (ViewConsultationVM)result.ResultData;
                    navigationManager.NavigateTo("/consultationrequest-detail/" + item.ReferenceId + "/" + consultationRequestVM.RequestTypeId);

                }
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
            {
                navigationManager.NavigateTo("/meeting-view/" + item.CommunicationId + "/" + item.ReferenceId + "/" + item.CommunicationTypeId + "/" + true + "/" + item.LinkTargetTypeId);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.MeetingScheduled)
            {
                navigationManager.NavigateTo("/meeting-view/" + item.CommunicationId + "/" + item.CommunicationTypeId + "/" + true);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.WithdrawRequested)
            {
                RedirectURL = "/detail-withdraw-request/" + item.ReferenceId + "/" + (int)CommunicationTypeEnum.WithdrawRequested;
                navigationManager.NavigateTo(RedirectURL);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.GeneralUpdate)
            {
                RedirectURL = "/request-need-more-detail/" + item.CommunicationId + "/" + item.LinkTargetTypeId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.G2GTarasolCorrespondence)
            {
                RedirectURL = "/request-need-more-detail/" + Guid.Empty + "/" + item.CommunicationId + "/" + item.LinkTargetTypeId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }

            else
            {
                RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.LinkTargetTypeId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
        }

        protected async Task AddMeeting(CommunicationInboxOutboxVM item)
        {
            try
            {
                Guid MeetingId = Guid.Empty;
                Guid CommunicationId = item.CommunicationId;
                Guid? ReferenceId = item.ReferenceId;
                int SubModuleId = (int)SubModuleEnum.CaseFile;
                int SectorTypeId = 0;
                string ReceivedBy = "";
                navigationManager.NavigateTo("/meeting-add/" + MeetingId + "/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + SectorTypeId + "/" + ReceivedBy);
                //await dialogService.OpenAsync<SaveMeeting>(
                //    translationState.Translate("Schedule_Meeting"),
                //    new Dictionary<string, object>()
                //    {
                //        { "CommunicationId", item.CommunicationId },
                //        { "ReferenceId", Guid.Parse(item.ReferenceId) },
                //        { "SubModuleId", item.LinkTargetTypeId },
                //    },
                //    new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "900px" });

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

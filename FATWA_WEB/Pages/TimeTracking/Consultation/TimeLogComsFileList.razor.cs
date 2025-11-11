using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.Consultation.ConsultationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.TimeTracking.Consultation
{
    public partial class TimeLogComsFileList : ComponentBase
    {
        #region Varriables Declaration
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected AdvanceSearchConsultationCaseFile advanceSearchVM = new AdvanceSearchConsultationCaseFile();
        protected RadzenDataGrid<ConsultationFileListVM>? consultationFileListGrid = new RadzenDataGrid<ConsultationFileListVM>();
        protected int pendingRequestsCount { get; set; }
        public int TypeId { get; set; } = 0;
        public bool allowRowSelectOnRowClick = true;
        protected RadzenDataGrid<ConsultationFileHistoryVM> HistoryGrid;
        IEnumerable<ConsultationFileListVM> consultationFileListVM = new List<ConsultationFileListVM>();
        protected IEnumerable<ConsultationFileListVM> filteredconsultationFileListVM { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => consultationFileListGrid.CurrentPage + 1;
        private int CurrentPageSize => consultationFileListGrid.PageSize;
        protected string? search { get; set; }
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateGovtEntities();
            await PopulateFileStatus();
            //await PopulateTaskCount();
            translationState.TranslateGridFilterLabels(consultationFileListGrid);
            spinnerService.Hide();
        }
        #endregion        

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        consultationFileListGrid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    advanceSearchVM.UserId = loginState.UserDetail.UserId;
                    advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;

                    var response = await consultationFileService.GetConsultationFileList(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        consultationFileListVM = (IEnumerable<ConsultationFileListVM>)response.ResultData;
                        filteredconsultationFileListVM = (IEnumerable<ConsultationFileListVM>)response.ResultData;
                        //if (loginState.UserDetail.RoleId == SystemRoles.ComsLawyer)
                        //{
                        //    consultationFileListVM = consultationFileListVM.Where(x => x.StatusId != (int)CaseFileStatusEnum.AssignToLawyer).ToList();
                        //    filteredconsultationFileListVM = filteredconsultationFileListVM.Where(x => x.StatusId != (int)CaseFileStatusEnum.AssignToLawyer).ToList();
                        //}
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            filteredconsultationFileListVM = await gridSearchExtension.Sort(filteredconsultationFileListVM, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
            spinnerService.Hide();
        }
        #endregion


        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = consultationFileListVM.Any() ? (consultationFileListVM.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = consultationFileListVM.Any() ? (consultationFileListVM.First().TotalCount) / (consultationFileListGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    consultationFileListGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchVM.PageNumber == 1 || (advanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchVM.PageNumber = CurrentPage;
            advanceSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSort(DataGridColumnSortEventArgs<ConsultationFileListVM> args)
        {
            if (args.SortOrder != null)
            {
                filteredconsultationFileListVM = await gridSearchExtension.Sort(filteredconsultationFileListVM, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
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

                filteredconsultationFileListVM = await gridSearchExtension.Filter(consultationFileListVM, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? @"i => ( i.FileNumber != null && i.FileNumber.ToLower().Contains(@0) ) 
                        || ( i.StatusEn != null && i.StatusEn.Replace(""  "","" "").ToLower().Contains(@1)) 
                        || ( i.GovernmentEntityNameEn != null && i.GovernmentEntityNameEn.ToLower().Contains(@2))
                        || ( i.LastActionEn != null && i.LastActionEn.ToLower().Contains(@3))"

                    : @"i => ( i.FileNumber != null && i.FileNumber.ToLower().Contains(@0) ) 
                        || ( i.StatusAr != null && i.StatusAr.Replace(""  "","" "").ToLower().Contains(@1)) 
                        || ( i.GovernmentEntityNameAr != null && i.GovernmentEntityNameAr.ToLower().Contains(@2))
                        || ( i.LastActionAr != null && i.LastActionAr.ToLower().Contains(@3))",
                    FilterParameters = new object[] { search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    filteredconsultationFileListVM = await gridSearchExtension.Sort(filteredconsultationFileListVM, ColumnName, SortOrder);
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

        #region GRid Buttons
        protected async Task DetailConsultationFile(ConsultationFileListVM args)
        {
            args.SectorTypeId = advanceSearchVM.SectorTypeId;
            navigationManager.NavigateTo("consultationfile-view/" + args.FileId + "/" + args.SectorTypeId);
        }
        #endregion

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (string.IsNullOrEmpty(advanceSearchVM.FileNumber)
                && !advanceSearchVM.StatusId.HasValue
                && !advanceSearchVM.GovEntityId.HasValue) { }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (consultationFileListGrid.CurrentPage > 0)
                    await consultationFileListGrid.FirstPage();
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await consultationFileListGrid.Reload();
                }
            }
        }

        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchConsultationCaseFile { PageSize = consultationFileListGrid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            consultationFileListGrid.Reset();
            await consultationFileListGrid.Reload();
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

        #region Populate Task Count
        //<History Author = 'Muhammad Zaeem' Date='2023-03-14' Version="1.0" Branch="master">Populate Count data</History>
        protected async Task PopulateTaskCount()
        {
            ApiCallResponse response;
            response = await taskService.GetCountComsTasksList(new AdvanceSearchTaskVM { SubModuleId = (int)SubModuleEnum.ConsultationRequest, ScreenId = (int)ConsultationScreensEnum.ListConsultationRequestPendingtransferRequests, TaskStatusId = (int)TaskStatusEnum.Pending });
            if (response.IsSuccessStatusCode)
            {
                pendingRequestsCount = (int)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Load Hisory for a File
        //<History Author = 'Muhammad Zaeem ' Date='2023-13-04' Version="1.0" Branch="master"> Load Hsitory for a File </History>

        protected async Task ExpandHistory(ConsultationFileListVM file)
        {
            var response = await consultationFileService.GetConslutationFileStatusHistoryForList((Guid)file.FileId);
            if (response.IsSuccessStatusCode)
            {
                file.LatestHistory = (List<ConsultationFileHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion 

        #region Populate GovtEntities
        protected List<GovernmentEntity> GovtEntities = new List<GovernmentEntity>();
        protected List<CaseFileStatus> FileStatuses = new List<CaseFileStatus>();
        protected async Task PopulateGovtEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task PopulateFileStatus()
        {
            var response = await lookupService.GetCaseFileStatuses();
            if (response.IsSuccessStatusCode)
            {
                FileStatuses = (List<CaseFileStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion
    }
}
